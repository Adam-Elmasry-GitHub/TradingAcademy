using TradingAcademy.Trading;

namespace TradingAcademy.Tests;

public class IndicatorTests
{
    private static readonly double[] Prices = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    [Fact]
    public void Sma_AveragesTheLastNValues()
    {
        var sma = Indicators.Sma(Prices, 3);
        Assert.Null(sma[1]);
        Assert.Equal(2.0, sma[2]);
        Assert.Equal(9.0, sma[9]);
    }

    [Fact]
    public void Ema_StartsAtTheSmaAndFollowsTrend()
    {
        var ema = Indicators.Ema(Prices, 3);
        Assert.Equal(2.0, ema[2]);
        Assert.True(ema[9] > ema[8]);
    }

    [Fact]
    public void Rsi_Is100WhenPriceOnlyRises()
    {
        var rsi = Indicators.Rsi(Prices, 5);
        Assert.Equal(100, rsi[9]);
    }

    [Fact]
    public void Rsi_StaysBetween0And100()
    {
        var market = new MarketSimulator(7);
        market.Warmup(300);
        var rsi = Indicators.Rsi(Indicators.Closes(market.Candles));
        Assert.All(rsi.Where(v => v.HasValue), v => Assert.InRange(v!.Value, 0, 100));
    }

    [Fact]
    public void Atr_IsPositiveForMovingMarket()
    {
        var market = new MarketSimulator(3);
        market.Warmup(50);
        var atr = Indicators.Atr(market.Candles);
        Assert.True(atr[^1] > 0);
    }

    [Fact]
    public void Macd_SignalLineAppearsAfterWarmup()
    {
        var market = new MarketSimulator(11);
        market.Warmup(100);
        var (macd, signal) = Indicators.Macd(Indicators.Closes(market.Candles));
        Assert.NotNull(macd[^1]);
        Assert.NotNull(signal[^1]);
    }
}
