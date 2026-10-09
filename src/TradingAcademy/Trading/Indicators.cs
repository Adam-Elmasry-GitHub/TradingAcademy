namespace TradingAcademy.Trading;

/// <summary>Classic technical indicators. Each returns one value per input bar, null until enough data exists.</summary>
public static class Indicators
{
    public static double?[] Sma(IReadOnlyList<double> values, int period)
    {
        Guard(period);
        var result = new double?[values.Count];
        double sum = 0;
        for (var i = 0; i < values.Count; i++)
        {
            sum += values[i];
            if (i >= period) sum -= values[i - period];
            if (i >= period - 1) result[i] = sum / period;
        }
        return result;
    }

    public static double?[] Ema(IReadOnlyList<double> values, int period)
    {
        Guard(period);
        var result = new double?[values.Count];
        if (values.Count < period) return result;

        var k = 2.0 / (period + 1);
        var ema = values.Take(period).Average();
        result[period - 1] = ema;
        for (var i = period; i < values.Count; i++)
        {
            ema = values[i] * k + ema * (1 - k);
            result[i] = ema;
        }
        return result;
    }

    /// <summary>Relative Strength Index using Wilder's smoothing. Above 70 is often called overbought, below 30 oversold.</summary>
    public static double?[] Rsi(IReadOnlyList<double> values, int period = 14)
    {
        Guard(period);
        var result = new double?[values.Count];
        if (values.Count <= period) return result;

        double gain = 0, loss = 0;
        for (var i = 1; i <= period; i++)
        {
            var change = values[i] - values[i - 1];
            if (change > 0) gain += change; else loss -= change;
        }
        gain /= period;
        loss /= period;
        result[period] = ToRsi(gain, loss);

        for (var i = period + 1; i < values.Count; i++)
        {
            var change = values[i] - values[i - 1];
            gain = (gain * (period - 1) + Math.Max(change, 0)) / period;
            loss = (loss * (period - 1) + Math.Max(-change, 0)) / period;
            result[i] = ToRsi(gain, loss);
        }
        return result;
    }

    /// <summary>MACD line (fast EMA minus slow EMA) and its signal line.</summary>
    public static (double?[] Macd, double?[] Signal) Macd(IReadOnlyList<double> values, int fast = 12, int slow = 26, int signal = 9)
    {
        var fastEma = Ema(values, fast);
        var slowEma = Ema(values, slow);
        var macd = new double?[values.Count];
        for (var i = 0; i < values.Count; i++)
            if (fastEma[i] is { } f && slowEma[i] is { } s) macd[i] = f - s;

        var start = Array.FindIndex(macd, v => v.HasValue);
        var signalLine = new double?[values.Count];
        if (start >= 0)
        {
            var tail = Ema(macd.Skip(start).Select(v => v!.Value).ToList(), signal);
            for (var i = 0; i < tail.Length; i++) signalLine[start + i] = tail[i];
        }
        return (macd, signalLine);
    }

    /// <summary>Average True Range: how far price typically moves per bar. Useful for placing stops.</summary>
    public static double?[] Atr(IReadOnlyList<Candle> candles, int period = 14)
    {
        Guard(period);
        var result = new double?[candles.Count];
        if (candles.Count <= period) return result;

        var tr = new double[candles.Count];
        for (var i = 1; i < candles.Count; i++)
        {
            var c = candles[i];
            var prevClose = (double)candles[i - 1].Close;
            tr[i] = Math.Max((double)(c.High - c.Low), Math.Max(Math.Abs((double)c.High - prevClose), Math.Abs((double)c.Low - prevClose)));
        }

        var atr = tr.Skip(1).Take(period).Average();
        result[period] = atr;
        for (var i = period + 1; i < candles.Count; i++)
        {
            atr = (atr * (period - 1) + tr[i]) / period;
            result[i] = atr;
        }
        return result;
    }

    public static List<double> Closes(IEnumerable<Candle> candles) => candles.Select(c => (double)c.Close).ToList();

    private static double ToRsi(double avgGain, double avgLoss) =>
        avgLoss == 0 ? 100 : 100 - 100 / (1 + avgGain / avgLoss);

    private static void Guard(int period)
    {
        if (period < 1) throw new ArgumentOutOfRangeException(nameof(period), "Period must be at least 1.");
    }
}
