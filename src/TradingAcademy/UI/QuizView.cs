using TradingAcademy.Learning;

namespace TradingAcademy.UI;

public static class QuizView
{
    public static void ChooseQuiz(IReadOnlyList<Level> levels, Progress progress, Action save)
    {
        ConsoleUi.Header("QUIZ: prove it and unlock the next level");
        foreach (var level in levels)
        {
            var best = progress.BestQuizScores.TryGetValue(level.Number, out var s) ? $"best {s:0}%" : "not taken";
            var color = progress.IsLevelPassed(level.Number) ? ConsoleColor.Green
                : progress.IsLevelUnlocked(level.Number) ? ConsoleColor.White : ConsoleColor.DarkGray;
            ConsoleUi.WriteLine($"  [{level.Number}] {level.Name,-36} {level.Quiz.Count} questions  ({best})", color);
        }

        var choice = ConsoleUi.AskInt($"\nLevel to test (pass mark {QuizEngine.PassPercent}%, q = back): ", levels.Min(l => l.Number), levels.Max(l => l.Number));
        if (choice is null) return;

        var chosen = levels.First(l => l.Number == choice);
        if (!progress.IsLevelUnlocked(chosen.Number))
        {
            ConsoleUi.Warning($"Level {chosen.Number} is locked. Pass the level {chosen.Number - 1} quiz first.");
            ConsoleUi.Pause();
            return;
        }
        Run(chosen, progress, save);
    }

    private static void Run(Level level, Progress progress, Action save)
    {
        var questions = QuizEngine.Shuffle(level.Quiz, Random.Shared);
        var answers = new List<int>();

        for (var i = 0; i < questions.Count; i++)
        {
            var q = questions[i];
            ConsoleUi.Header($"Level {level.Number} quiz · question {i + 1} of {questions.Count}");
            ConsoleUi.Paragraph(q.Prompt, 2, ConsoleColor.White);
            Console.WriteLine();
            for (var o = 0; o < q.Options.Count; o++)
                ConsoleUi.Paragraph($"{(char)('A' + o)}) {q.Options[o]}", 4, ConsoleColor.Gray);

            var answer = AskLetter(q.Options.Count);
            if (answer is null) return;
            answers.Add(answer.Value);

            if (answer == q.Answer) ConsoleUi.Success("\n  ✔ Correct!");
            else ConsoleUi.Error($"\n  ✖ Not quite. The answer is {(char)('A' + q.Answer)}.");
            if (!string.IsNullOrWhiteSpace(q.Explanation)) ConsoleUi.Paragraph(q.Explanation, 4, ConsoleColor.Yellow);
            ConsoleUi.Pause();
        }

        var result = QuizEngine.Score(questions, answers);
        var wasPassed = progress.IsLevelPassed(level.Number);
        progress.RecordQuiz(level.Number, result.Percent);
        save();

        ConsoleUi.Header("QUIZ RESULT");
        ConsoleUi.WriteLine($"  You scored {result.Correct}/{result.Total} ({result.Percent:0}%).", result.Passed ? ConsoleColor.Green : ConsoleColor.Red);
        if (result.Passed && !wasPassed)
            ConsoleUi.Success($"  🎉 Level {level.Number} complete! The next level is now unlocked.");
        else if (!result.Passed)
            ConsoleUi.Warning($"  You need {QuizEngine.PassPercent}% to pass. Re-read the lessons and try again; questions are shuffled each time.");
        ConsoleUi.Pause();
    }

    private static int? AskLetter(int optionCount)
    {
        while (true)
        {
            var input = ConsoleUi.Ask($"\nYour answer (A-{(char)('A' + optionCount - 1)}, q = quit): ").ToUpperInvariant();
            if (input == "Q") return null;
            if (input.Length == 1 && input[0] >= 'A' && input[0] < 'A' + optionCount) return input[0] - 'A';
            ConsoleUi.Error("Please type one of the letters shown.");
        }
    }
}
