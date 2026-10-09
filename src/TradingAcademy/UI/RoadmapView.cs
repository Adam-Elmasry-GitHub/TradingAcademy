using TradingAcademy.Learning;

namespace TradingAcademy.UI;

/// <summary>Draws the learning roadmap in the terminal and marks each level as done, open or locked.</summary>
public static class RoadmapView
{
    public static void Show(IReadOnlyList<Level> levels, Progress progress)
    {
        ConsoleUi.Header("ROADMAP: from beginner to professional trader");

        for (var i = 0; i < levels.Count; i++)
        {
            var level = levels[i];
            var (tag, color) = progress.IsLevelPassed(level.Number) ? ("✔ DONE  ", ConsoleColor.Green)
                : progress.IsLevelUnlocked(level.Number) ? ("▶ OPEN  ", ConsoleColor.Yellow)
                : ("✖ LOCKED", ConsoleColor.DarkGray);

            var title = $"LEVEL {level.Number} · {level.Name}";
            var sub = $"{level.Rank} · {level.Duration}";
            var width = Math.Max(title.Length, sub.Length) + 4;
            var indent = new string(' ', 2 + (i % 2) * 6);

            ConsoleUi.WriteLine($"{indent}┌{new string('─', width)}┐", color);
            ConsoleUi.Write($"{indent}│  {title.PadRight(width - 2)}│", color);
            ConsoleUi.WriteLine($"  {tag}", color);
            ConsoleUi.WriteLine($"{indent}│  {sub.PadRight(width - 2)}│", color);
            ConsoleUi.WriteLine($"{indent}└{new string('─', width / 2)}┬{new string('─', width - width / 2 - 1)}┘", color);

            if (i < levels.Count - 1)
            {
                var arrowIndent = new string(' ', indent.Length + width / 2 + 1);
                ConsoleUi.WriteLine($"{arrowIndent}│  pass quiz ≥ {QuizEngine.PassPercent}%", ConsoleColor.DarkGray);
                ConsoleUi.WriteLine($"{arrowIndent}▼", ConsoleColor.DarkGray);
            }
        }

        ConsoleUi.WriteLine("\n  ★ PRO TRADER: consistent, rule-based, risk-managed, journaled.\n", ConsoleColor.Magenta);
        ConsoleUi.Info("  The full roadmap with diagrams and study plans is in docs/ROADMAP.md.");
        ConsoleUi.Pause();
    }
}
