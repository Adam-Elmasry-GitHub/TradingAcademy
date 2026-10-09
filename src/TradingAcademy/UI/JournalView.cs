using TradingAcademy.Learning;

namespace TradingAcademy.UI;

/// <summary>A trading journal. Reviewing your own trades is the fastest way to improve.</summary>
public static class JournalView
{
    public static void Show(Progress progress, Action save)
    {
        while (true)
        {
            ConsoleUi.Header("TRADING JOURNAL");
            var entries = progress.Journal;
            if (entries.Count == 0) ConsoleUi.Info("  No entries yet. Log your paper trades here.");
            foreach (var e in entries.TakeLast(15))
                ConsoleUi.WriteLine($"  {e.Date:yyyy-MM-dd} {e.Symbol,-8} {e.Side,-5} {e.Quantity,6} @ {e.Entry,8:0.00} → {e.Exit,8:0.00}  P/L {e.ProfitLoss,9:+0.00;-0.00}  {e.Setup}",
                    e.ProfitLoss >= 0 ? ConsoleColor.Green : ConsoleColor.Red);

            if (entries.Count > 0)
            {
                var wins = entries.Count(e => e.ProfitLoss > 0);
                ConsoleUi.Info($"\n  {entries.Count} trades · win rate {100.0 * wins / entries.Count:0}% · total P/L {entries.Sum(e => e.ProfitLoss):+0.00;-0.00}");
                var bestSetup = entries.GroupBy(e => e.Setup).OrderByDescending(g => g.Sum(e => e.ProfitLoss)).First();
                ConsoleUi.Info($"  Most profitable setup: {bestSetup.Key} ({bestSetup.Sum(e => e.ProfitLoss):+0.00;-0.00})");
            }

            ConsoleUi.Info("\n  [1] Add entry   [2] Read lessons learned   [q] Back");
            var choice = ConsoleUi.Ask("> ");
            if (choice == "1") Add(progress, save);
            else if (choice == "2") Lessons(progress);
            else return;
        }
    }

    private static void Add(Progress progress, Action save)
    {
        ConsoleUi.Header("NEW JOURNAL ENTRY");
        var symbol = ConsoleUi.Ask("Symbol (e.g. EQNR, AAPL, SIM): ").ToUpperInvariant();
        var side = ConsoleUi.Ask("Side (long/short): ").ToLowerInvariant() == "short" ? "short" : "long";
        if (ConsoleUi.AskDecimal("Quantity: ") is not { } qty) return;
        if (ConsoleUi.AskDecimal("Entry price: ") is not { } entry) return;
        if (ConsoleUi.AskDecimal("Exit price: ") is not { } exit) return;
        var setup = ConsoleUi.Ask("Setup (e.g. breakout, pullback to SMA20): ");
        var emotion = ConsoleUi.Ask("How did you feel (calm, FOMO, fear, revenge...): ");
        var lesson = ConsoleUi.Ask("What did you learn: ");

        progress.Journal.Add(new JournalEntry
        {
            Date = DateTime.Now, Symbol = symbol, Side = side, Quantity = qty, Entry = entry, Exit = exit,
            Setup = string.IsNullOrWhiteSpace(setup) ? "unspecified" : setup, Emotion = emotion, Lesson = lesson,
        });
        save();
        ConsoleUi.Success("Saved.");
        ConsoleUi.Pause();
    }

    private static void Lessons(Progress progress)
    {
        ConsoleUi.Header("LESSONS LEARNED");
        foreach (var e in progress.Journal.Where(e => !string.IsNullOrWhiteSpace(e.Lesson)))
            ConsoleUi.Paragraph($"{e.Date:yyyy-MM-dd} {e.Symbol} ({e.Emotion}): {e.Lesson}");
        ConsoleUi.Pause();
    }
}
