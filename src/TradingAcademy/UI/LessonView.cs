using TradingAcademy.Learning;

namespace TradingAcademy.UI;

public static class LessonView
{
    public static void ChooseLevel(IReadOnlyList<Level> levels, Progress progress, Action save)
    {
        while (true)
        {
            ConsoleUi.Header("LEARN: choose a level");
            foreach (var level in levels)
            {
                var done = level.Lessons.Count(l => progress.CompletedLessons.Contains(l.Id));
                var locked = !progress.IsLevelUnlocked(level.Number);
                var color = locked ? ConsoleColor.DarkGray : progress.IsLevelPassed(level.Number) ? ConsoleColor.Green : ConsoleColor.White;
                ConsoleUi.WriteLine($"  [{level.Number}] {level.Name,-36} {done}/{level.Lessons.Count} lessons{(locked ? "  (locked)" : "")}", color);
            }
            ConsoleUi.Info("\n  Locked levels can still be read, but the quiz for each level unlocks the next one.");

            var choice = ConsoleUi.AskInt("\nLevel number (q = back): ", levels.Min(l => l.Number), levels.Max(l => l.Number));
            if (choice is null) return;
            ChooseLesson(levels.First(l => l.Number == choice), progress, save);
        }
    }

    private static void ChooseLesson(Level level, Progress progress, Action save)
    {
        while (true)
        {
            ConsoleUi.Header($"LEVEL {level.Number}: {level.Name}");
            ConsoleUi.Paragraph($"Goal: {level.Goal}", 2, ConsoleColor.Yellow);
            Console.WriteLine();
            for (var i = 0; i < level.Lessons.Count; i++)
            {
                var lesson = level.Lessons[i];
                var mark = progress.CompletedLessons.Contains(lesson.Id) ? "✔" : " ";
                ConsoleUi.WriteLine($"  [{i + 1}] {mark} {lesson.Title}", mark == "✔" ? ConsoleColor.Green : ConsoleColor.White);
            }
            if (level.Milestones.Count > 0)
            {
                ConsoleUi.WriteLine("\n  Milestones before moving on:", ConsoleColor.Cyan);
                foreach (var m in level.Milestones) ConsoleUi.Paragraph("• " + m, 4, ConsoleColor.Gray);
            }

            var choice = ConsoleUi.AskInt("\nLesson number (q = back): ", 1, level.Lessons.Count);
            if (choice is null) return;
            Read(level.Lessons[choice.Value - 1], progress, save);
        }
    }

    public static void Read(Lesson lesson, Progress progress, Action save)
    {
        ConsoleUi.Header(lesson.Title);
        foreach (var paragraph in lesson.Body)
        {
            ConsoleUi.Paragraph(paragraph);
            Console.WriteLine();
        }

        if (lesson.KeyTakeaways.Count > 0)
        {
            ConsoleUi.WriteLine("  KEY TAKEAWAYS", ConsoleColor.Yellow);
            foreach (var t in lesson.KeyTakeaways) ConsoleUi.Paragraph("✔ " + t, 4, ConsoleColor.Yellow);
            Console.WriteLine();
        }
        if (!string.IsNullOrWhiteSpace(lesson.Exercise))
        {
            ConsoleUi.WriteLine("  PRACTICE", ConsoleColor.Green);
            ConsoleUi.Paragraph(lesson.Exercise, 4, ConsoleColor.Green);
            Console.WriteLine();
        }
        if (lesson.ReadMore.Count > 0)
        {
            ConsoleUi.WriteLine("  READ MORE", ConsoleColor.Cyan);
            foreach (var r in lesson.ReadMore) ConsoleUi.Paragraph("→ " + r, 4, ConsoleColor.Cyan);
        }

        if (progress.CompletedLessons.Add(lesson.Id))
        {
            save();
            ConsoleUi.Success("\n  Lesson marked as completed.");
        }
        ConsoleUi.Pause();
    }
}
