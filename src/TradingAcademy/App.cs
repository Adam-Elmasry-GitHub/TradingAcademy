using TradingAcademy.Learning;
using TradingAcademy.UI;

namespace TradingAcademy;

public sealed class App
{
    private readonly IReadOnlyList<Level> _levels;
    private readonly ProgressStore _store;
    private readonly Progress _progress;

    public App(IReadOnlyList<Level> levels, ProgressStore store)
    {
        _levels = levels;
        _store = store;
        _progress = store.Load();
    }

    public void Run()
    {
        while (true)
        {
            ShowMenu();
            switch (ConsoleUi.Ask("\nChoose: ").ToLowerInvariant())
            {
                case "1": RoadmapView.Show(_levels, _progress); break;
                case "2": LessonView.ChooseLevel(_levels, _progress, Save); break;
                case "3": QuizView.ChooseQuiz(_levels, _progress, Save); break;
                case "4": SimulatorView.Run(_progress, Save); break;
                case "5": ToolsView.Show(); break;
                case "6": JournalView.Show(_progress, Save); break;
                case "7": ShowProgress(); break;
                case "0" or "q" or "exit":
                    ConsoleUi.Success("Good trading! Remember: protect your capital first.");
                    return;
            }
        }
    }

    private void ShowMenu()
    {
        ConsoleUi.Header("TRADING ACADEMY · beginner to pro");
        var current = _levels.LastOrDefault(l => _progress.IsLevelUnlocked(l.Number));
        if (current != null)
            ConsoleUi.WriteLine($"  Current level: {current.Number} · {current.Name} ({current.Rank})\n", ConsoleColor.Yellow);

        ConsoleUi.Info("  [1] Roadmap             see the path from beginner to pro");
        ConsoleUi.Info("  [2] Learn               lessons for every level");
        ConsoleUi.Info("  [3] Quiz                pass to unlock the next level");
        ConsoleUi.Info("  [4] Paper trading       practise on a simulated market");
        ConsoleUi.Info("  [5] Trader's toolkit    position size, risk/reward, expectancy");
        ConsoleUi.Info("  [6] Trading journal     log and review your trades");
        ConsoleUi.Info("  [7] Progress            your stats");
        ConsoleUi.Info("  [0] Exit");
        ConsoleUi.WriteLine("\n  Educational only. Not financial advice. Most retail traders lose money.", ConsoleColor.DarkGray);
    }

    private void ShowProgress()
    {
        ConsoleUi.Header("YOUR PROGRESS");
        var totalLessons = _levels.Sum(l => l.Lessons.Count);
        ConsoleUi.Info($"  Lessons completed: {_progress.CompletedLessons.Count}/{totalLessons}");
        ConsoleUi.Info($"  Levels passed:     {_levels.Count(l => _progress.IsLevelPassed(l.Number))}/{_levels.Count}");
        foreach (var level in _levels)
        {
            var score = _progress.BestQuizScores.TryGetValue(level.Number, out var s) ? $"{s:0}%" : "-";
            ConsoleUi.WriteLine($"    Level {level.Number} {level.Name,-36} best quiz {score}", _progress.IsLevelPassed(level.Number) ? ConsoleColor.Green : ConsoleColor.Gray);
        }
        ConsoleUi.Info($"  Simulator sessions: {_progress.SimulatorSessions} (best return {_progress.BestSimulatorReturnPercent:+0.00;-0.00}%)");
        ConsoleUi.Info($"  Journal entries:    {_progress.Journal.Count}");
        ConsoleUi.WriteLine($"\n  Progress file: {_store.FilePath}", ConsoleColor.DarkGray);

        if (ConsoleUi.Ask("\nType RESET to delete all progress, or Enter to go back: ") == "RESET")
        {
            _progress.CompletedLessons.Clear();
            _progress.BestQuizScores.Clear();
            _progress.Journal.Clear();
            _progress.SimulatorSessions = 0;
            _progress.BestSimulatorReturnPercent = 0;
            Save();
            ConsoleUi.Warning("Progress reset.");
            ConsoleUi.Pause();
        }
    }

    private void Save() => _store.Save(_progress);
}
