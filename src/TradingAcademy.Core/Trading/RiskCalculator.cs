namespace TradingAcademy.Trading;

/// <summary>The maths that keeps a trader in the game. Every function here is taught in Level 3.</summary>
public static class RiskCalculator
{
    /// <summary>
    /// Fixed-fractional position sizing: how many units to buy so that hitting the stop loses
    /// exactly <paramref name="riskPercent"/> of the account.
    /// </summary>
    public static decimal PositionSize(decimal accountSize, decimal riskPercent, decimal entry, decimal stop)
    {
        if (accountSize <= 0) throw new ArgumentOutOfRangeException(nameof(accountSize));
        if (riskPercent <= 0 || riskPercent > 100) throw new ArgumentOutOfRangeException(nameof(riskPercent));
        var riskPerUnit = Math.Abs(entry - stop);
        if (riskPerUnit == 0) throw new ArgumentException("Entry and stop cannot be the same price.");

        var riskAmount = accountSize * riskPercent / 100m;
        var units = Math.Floor(riskAmount / riskPerUnit);
        // Never size beyond what the account can pay for (no leverage).
        return Math.Min(units, Math.Floor(accountSize / entry));
    }

    /// <summary>Reward-to-risk ratio (the "R multiple" of the target). 2.0 means you aim to make twice what you risk.</summary>
    public static decimal RewardToRisk(decimal entry, decimal stop, decimal target)
    {
        var risk = Math.Abs(entry - stop);
        if (risk == 0) throw new ArgumentException("Entry and stop cannot be the same price.");
        return Math.Abs(target - entry) / risk;
    }

    /// <summary>The win rate you need just to break even at a given reward-to-risk ratio (ignoring costs).</summary>
    public static double BreakEvenWinRate(double rewardToRisk)
    {
        if (rewardToRisk <= 0) throw new ArgumentOutOfRangeException(nameof(rewardToRisk));
        return 100.0 / (1 + rewardToRisk);
    }

    /// <summary>Average result per trade: (win% x average win) - (loss% x average loss).</summary>
    public static double Expectancy(double winRatePercent, double averageWin, double averageLoss)
    {
        var p = winRatePercent / 100.0;
        return p * averageWin - (1 - p) * Math.Abs(averageLoss);
    }

    /// <summary>The gain needed to recover from a drawdown. A 50% loss needs a 100% gain.</summary>
    public static double RecoveryNeeded(double drawdownPercent)
    {
        if (drawdownPercent < 0 || drawdownPercent >= 100) throw new ArgumentOutOfRangeException(nameof(drawdownPercent));
        return 100.0 * drawdownPercent / (100 - drawdownPercent);
    }

    /// <summary>
    /// Kelly criterion: the bet fraction that maximises long-run growth. Professionals usually use
    /// a quarter or half of it, because the inputs are only estimates.
    /// </summary>
    public static double KellyFraction(double winRatePercent, double rewardToRisk)
    {
        var p = winRatePercent / 100.0;
        return Math.Max(0, p - (1 - p) / rewardToRisk);
    }
}
