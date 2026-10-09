using TradingAcademy.Trading;

namespace TradingAcademy.UI;

/// <summary>The calculators a professional trader uses before every trade.</summary>
public static class ToolsView
{
    public static void Show()
    {
        while (true)
        {
            ConsoleUi.Header("TRADER'S TOOLKIT");
            ConsoleUi.Info("  [1] Position size calculator");
            ConsoleUi.Info("  [2] Reward-to-risk and break-even win rate");
            ConsoleUi.Info("  [3] Expectancy of a strategy");
            ConsoleUi.Info("  [4] Drawdown recovery table");
            ConsoleUi.Info("  [5] Kelly criterion (how much to bet)");
            var choice = ConsoleUi.AskInt("\nChoose a tool (q = back): ", 1, 5);
            if (choice is null) return;

            try
            {
                switch (choice)
                {
                    case 1: PositionSize(); break;
                    case 2: RewardRisk(); break;
                    case 3: Expectancy(); break;
                    case 4: Drawdown(); break;
                    case 5: Kelly(); break;
                }
            }
            catch (ArgumentException e)
            {
                ConsoleUi.Error("  " + e.Message);
            }
            ConsoleUi.Pause();
        }
    }

    private static void PositionSize()
    {
        ConsoleUi.Header("POSITION SIZE CALCULATOR");
        if (ConsoleUi.AskDecimal("Account size: ") is not { } account) return;
        if (ConsoleUi.AskDecimal("Risk per trade in % (1 is a common professional limit): ") is not { } risk) return;
        if (ConsoleUi.AskDecimal("Entry price: ") is not { } entry) return;
        if (ConsoleUi.AskDecimal("Stop-loss price: ") is not { } stop) return;

        var units = RiskCalculator.PositionSize(account, risk, entry, stop);
        ConsoleUi.Success($"\n  Buy/sell {units} units. Position value {units * entry:0.00}.");
        ConsoleUi.Info($"  If the stop is hit you lose about {units * Math.Abs(entry - stop):0.00} ({risk}% of the account), plus fees.");
    }

    private static void RewardRisk()
    {
        ConsoleUi.Header("REWARD-TO-RISK");
        if (ConsoleUi.AskDecimal("Entry price: ") is not { } entry) return;
        if (ConsoleUi.AskDecimal("Stop-loss price: ") is not { } stop) return;
        if (ConsoleUi.AskDecimal("Target price: ") is not { } target) return;

        var rr = RiskCalculator.RewardToRisk(entry, stop, target);
        ConsoleUi.Success($"\n  Reward-to-risk: {rr:0.00} : 1");
        ConsoleUi.Info($"  You must win more than {RiskCalculator.BreakEvenWinRate((double)rr):0.0}% of trades like this to make money (before costs).");
    }

    private static void Expectancy()
    {
        ConsoleUi.Header("EXPECTANCY");
        if (ConsoleUi.AskDecimal("Win rate in %: ") is not { } win) return;
        if (ConsoleUi.AskDecimal("Average win: ") is not { } avgWin) return;
        if (ConsoleUi.AskDecimal("Average loss: ") is not { } avgLoss) return;

        var e = RiskCalculator.Expectancy((double)win, (double)avgWin, (double)avgLoss);
        (e > 0 ? (Action<string>)ConsoleUi.Success : ConsoleUi.Error)($"\n  Expected result per trade: {e:+0.00;-0.00}");
        ConsoleUi.Info(e > 0 ? "  Positive edge: over many trades this strategy should make money." : "  Negative edge: this strategy loses money over time, however good it feels.");
    }

    private static void Drawdown()
    {
        ConsoleUi.Header("DRAWDOWN RECOVERY");
        ConsoleUi.Info("  Loss      Gain needed to get back to break-even");
        foreach (var dd in new[] { 5, 10, 20, 30, 40, 50, 60, 75, 90 })
            ConsoleUi.WriteLine($"  {dd,3}%      {RiskCalculator.RecoveryNeeded(dd),7:0.0}%", dd >= 40 ? ConsoleColor.Red : dd >= 20 ? ConsoleColor.Yellow : ConsoleColor.Green);
        ConsoleUi.Paragraph("\nThis is why risk management comes first: big losses need enormous gains to recover.", 2, ConsoleColor.Yellow);
    }

    private static void Kelly()
    {
        ConsoleUi.Header("KELLY CRITERION");
        if (ConsoleUi.AskDecimal("Win rate in %: ") is not { } win) return;
        if (ConsoleUi.AskDecimal("Reward-to-risk ratio (e.g. 2): ") is not { } rr) return;

        var k = RiskCalculator.KellyFraction((double)win, (double)rr);
        ConsoleUi.Success($"\n  Full Kelly: risk {k * 100:0.0}% per trade. Half Kelly: {k * 50:0.0}%. Quarter Kelly: {k * 25:0.0}%.");
        ConsoleUi.Paragraph("Full Kelly is very volatile and assumes your win rate is known exactly. Most professionals use a fraction of it, or simply cap risk at 0.5-2%.", 2, ConsoleColor.Gray);
    }
}
