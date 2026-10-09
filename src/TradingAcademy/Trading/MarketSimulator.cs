namespace TradingAcademy.Trading;

public enum MarketRegime { Bull, Bear, Sideways }

/// <summary>
/// Generates a realistic-looking but fake price series so you can practise without real money.
/// Prices follow geometric Brownian motion whose drift and volatility switch between bull,
/// bear and sideways regimes, so trends, pullbacks and choppy ranges all appear.
/// The same seed always produces the same market, which makes sessions repeatable.
/// </summary>
public sealed class MarketSimulator
{
    private readonly Random _rng;
    private readonly List<Candle> _candles = new();
    private MarketRegime _regime;
    private int _regimeBarsLeft;
    private double _price;

    public MarketSimulator(int seed, decimal startPrice = 100m, string symbol = "SIM")
    {
        if (startPrice <= 0) throw new ArgumentOutOfRangeException(nameof(startPrice));
        _rng = new Random(seed);
        _price = (double)startPrice;
        Symbol = symbol;
        Seed = seed;
        PickRegime();
    }

    public string Symbol { get; }
    public int Seed { get; }
    public MarketRegime Regime => _regime;
    public IReadOnlyList<Candle> Candles => _candles;
    public Candle Last => _candles.Count > 0 ? _candles[^1] : throw new InvalidOperationException("No candles yet.");

    /// <summary>Creates the history shown before the player's first decision.</summary>
    public void Warmup(int bars)
    {
        for (var i = 0; i < bars; i++) Next();
    }

    public Candle Next()
    {
        if (_regimeBarsLeft-- <= 0) PickRegime();

        var (drift, vol) = _regime switch
        {
            MarketRegime.Bull => (0.0012, 0.014),
            MarketRegime.Bear => (-0.0014, 0.020),
            _ => (0.0, 0.010),
        };

        // Split each bar into small steps so the high and low are reached naturally.
        const int steps = 8;
        var open = _price;
        double high = open, low = open, price = open;
        for (var s = 0; s < steps; s++)
        {
            var shock = NextGaussian() * vol / Math.Sqrt(steps);
            price *= Math.Exp(drift / steps - 0.5 * vol * vol / steps + shock);
            high = Math.Max(high, price);
            low = Math.Min(low, price);
        }
        _price = Math.Max(price, 0.5);

        var volume = (long)(100_000 * (0.6 + _rng.NextDouble()) * (1 + 25 * Math.Abs(price / open - 1)));
        var candle = new Candle(_candles.Count, Round(open), Round(high), Round(Math.Max(low, 0.01)), Round(_price), volume);
        _candles.Add(candle);
        return candle;
    }

    private void PickRegime()
    {
        var roll = _rng.NextDouble();
        _regime = roll < 0.4 ? MarketRegime.Bull : roll < 0.7 ? MarketRegime.Sideways : MarketRegime.Bear;
        _regimeBarsLeft = _rng.Next(15, 60);
    }

    // Box-Muller transform: turns two uniform numbers into one normally distributed number.
    private double NextGaussian()
    {
        var u1 = 1.0 - _rng.NextDouble();
        var u2 = _rng.NextDouble();
        return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);
    }

    private static decimal Round(double value) => Math.Round((decimal)value, 2);
}
