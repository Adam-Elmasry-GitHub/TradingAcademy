using TradingAcademy.Trading;

namespace TradingAcademy.Tests;

public class RiskCalculatorTests
{
    [Fact]
    public void PositionSize_RisksTheChosenPercent()
    {
        // 10,000 account, 1% risk = 100. Stop 5 away => 20 units.
        Assert.Equal(20m, RiskCalculator.PositionSize(10_000m, 1m, 100m, 95m));
    }

    [Fact]
    public void PositionSize_NeverExceedsWhatTheAccountCanBuy()
    {
        // Risk allows 1000 units, but the account can only afford 100.
        Assert.Equal(100m, RiskCalculator.PositionSize(10_000m, 1m, 100m, 99.9m));
    }

    [Fact]
    public void PositionSize_RejectsZeroDistanceStop() =>
        Assert.Throws<ArgumentException>(() => RiskCalculator.PositionSize(10_000m, 1m, 100m, 100m));

    [Theory]
    [InlineData(1.0, 50.0)]
    [InlineData(2.0, 33.333)]
    [InlineData(3.0, 25.0)]
    public void BreakEvenWinRate_MatchesFormula(double rr, double expected) =>
        Assert.Equal(expected, RiskCalculator.BreakEvenWinRate(rr), 2);

    [Fact]
    public void Expectancy_MatchesLessonExample() =>
        Assert.Equal(60, RiskCalculator.Expectancy(40, 300, 100), 6);

    [Theory]
    [InlineData(50, 100)]
    [InlineData(10, 11.111)]
    [InlineData(90, 900)]
    public void RecoveryNeeded_IsAsymmetric(double drawdown, double expected) =>
        Assert.Equal(expected, RiskCalculator.RecoveryNeeded(drawdown), 2);

    [Fact]
    public void Kelly_IsZeroWithoutAnEdge() =>
        Assert.Equal(0, RiskCalculator.KellyFraction(33, 2));

    [Fact]
    public void Kelly_MatchesFormula() =>
        Assert.Equal(0.175, RiskCalculator.KellyFraction(45, 2), 6);
}
