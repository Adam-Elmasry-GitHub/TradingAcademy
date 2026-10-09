using TradingAcademy.Trading;

namespace TradingAcademy.Tests;

public class PaperAccountTests
{
    private static Candle Bar(int i, decimal o, decimal h, decimal l, decimal c) => new(i, o, h, l, c, 1000);

    [Fact]
    public void LongTrade_RecordsProfitNetOfExitFee()
    {
        var account = new PaperAccount(10_000m, 0m);
        account.Buy(10, 100m, 0);
        account.Sell(10, 110m, 1);

        Assert.True(account.IsFlat);
        Assert.Equal(100m, account.Trades.Single().ProfitLoss);
        Assert.Equal(10_100m, account.Cash);
    }

    [Fact]
    public void ShortTrade_ProfitsWhenPriceFalls()
    {
        var account = new PaperAccount(10_000m, 0m);
        account.Sell(100, 50m, 0);
        Assert.Equal("short", account.Side);
        account.Buy(100, 45m, 1);
        Assert.Equal(500m, account.Trades.Single().ProfitLoss);
        Assert.Equal(10_500m, account.Equity(45m));
    }

    [Fact]
    public void CommissionIsCharged()
    {
        var account = new PaperAccount(10_000m, 0.001m);
        account.Buy(10, 100m, 0);
        Assert.Equal(1m, account.TotalCommission);
        Assert.Equal(9_999m, account.Equity(100m));
    }

    [Fact]
    public void RejectsOrdersBiggerThanEquity()
    {
        var account = new PaperAccount(1_000m, 0m);
        var result = account.Buy(11, 100m, 0);
        Assert.False(result.Success);
        Assert.True(account.IsFlat);
    }

    [Fact]
    public void AveragePriceUpdatesWhenAddingToPosition()
    {
        var account = new PaperAccount(10_000m, 0m);
        account.Buy(10, 100m, 0);
        account.Buy(10, 110m, 1);
        Assert.Equal(105m, account.AveragePrice);
    }

    [Fact]
    public void StopLossFillsWhenTouched()
    {
        var account = new PaperAccount(10_000m, 0m);
        account.Buy(10, 100m, 0);
        account.SetStopLoss(95m, 100m);
        var message = account.OnCandle(Bar(1, 99m, 99m, 94m, 96m));

        Assert.NotNull(message);
        Assert.True(account.IsFlat);
        Assert.Equal(-50m, account.Trades.Single().ProfitLoss);
        Assert.Equal("stop-loss", account.Trades.Single().Reason);
    }

    [Fact]
    public void StopLossFillsAtOpenOnGap()
    {
        var account = new PaperAccount(10_000m, 0m);
        account.Buy(10, 100m, 0);
        account.SetStopLoss(95m, 100m);
        account.OnCandle(Bar(1, 90m, 92m, 89m, 91m));
        Assert.Equal(90m, account.Trades.Single().ExitPrice);
    }

    [Fact]
    public void TakeProfitFillsForShort()
    {
        var account = new PaperAccount(10_000m, 0m);
        account.Sell(10, 100m, 0);
        account.SetTakeProfit(90m, 100m);
        account.OnCandle(Bar(1, 95m, 96m, 89m, 92m));
        Assert.Equal(100m, account.Trades.Single().ProfitLoss);
    }

    [Fact]
    public void RejectsStopOnWrongSide()
    {
        var account = new PaperAccount(10_000m, 0m);
        account.Buy(10, 100m, 0);
        Assert.False(account.SetStopLoss(105m, 100m).Success);
    }

    [Fact]
    public void SellingMoreThanLongFlipsToShort()
    {
        var account = new PaperAccount(10_000m, 0m);
        account.Buy(10, 100m, 0);
        account.Sell(15, 100m, 1);
        Assert.Equal(-5m, account.Position);
        Assert.Equal(100m, account.AveragePrice);
    }

    [Fact]
    public void StatsCalculateWinRateAndDrawdown()
    {
        var account = new PaperAccount(10_000m, 0m);
        account.Buy(10, 100m, 0);
        account.OnCandle(Bar(1, 100m, 100m, 80m, 80m));
        account.Sell(10, 80m, 1);
        account.Buy(10, 80m, 2);
        account.Sell(10, 120m, 3);

        var stats = SessionStats.From(account, 120m);
        Assert.Equal(2, stats.TradeCount);
        Assert.Equal(50, stats.WinRate);
        Assert.Equal(2m, stats.ProfitFactor);
        Assert.Equal(2m, stats.MaxDrawdownPercent);
    }
}
