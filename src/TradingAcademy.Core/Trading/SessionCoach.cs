namespace TradingAcademy.Trading;

/// <summary>Turns session statistics into the feedback a trading coach would give.</summary>
public static class SessionCoach
{
    public static IEnumerable<string> Notes(SessionStats stats, PaperAccount account)
    {
        if (stats.TradeCount == 0) { yield return "No trades taken. Watching is fine, but practise executing a plan with small size."; yield break; }
        if (stats.TradeCount > 25) yield return "Many trades. Overtrading raises costs and is often driven by boredom or revenge. Wait for your setup.";
        if (stats.MaxDrawdownPercent > 20) yield return "Your drawdown was over 20%. Size smaller: risk 1% or less per trade using the size command.";
        if (stats.AverageLoss > stats.AverageWin && stats.WinRate < 60) yield return "Your average loss is bigger than your average win. Cut losers faster with a stop-loss and let winners run.";
        if (account.Trades.Count(t => t.Reason == "stop-loss") == 0 && stats.AverageLoss > 0) yield return "No trade exited at a stop-loss. Make sure every trade has one: pros always know where they are wrong before they enter.";
        if (stats.TradeCount >= 3 && stats.ProfitFactor >= 1.5m && stats.MaxDrawdownPercent < 10) yield return "Solid session: good profit factor with controlled risk. Repeat it on new seeds to check it was skill, not luck.";
        if (stats.ReturnPercent < 0 && stats.MaxDrawdownPercent < 5) yield return "A small, controlled loss. That is a professional outcome on a bad day.";
    }
}
