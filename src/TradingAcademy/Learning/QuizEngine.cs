namespace TradingAcademy.Learning;

public sealed record QuizResult(int Correct, int Total)
{
    public double Percent => Total == 0 ? 0 : 100.0 * Correct / Total;
    public bool Passed => Percent >= QuizEngine.PassPercent;
}

public static class QuizEngine
{
    /// <summary>Score needed to pass a level quiz and unlock the next level.</summary>
    public const double PassPercent = 70;

    public static QuizResult Score(IReadOnlyList<QuizQuestion> questions, IReadOnlyList<int> answers)
    {
        if (answers.Count != questions.Count)
            throw new ArgumentException("One answer is needed per question.", nameof(answers));

        var correct = questions.Where((q, i) => q.Answer == answers[i]).Count();
        return new QuizResult(correct, questions.Count);
    }

    /// <summary>Returns the questions in a random order (Fisher-Yates) so repeat attempts test understanding, not memory of positions.</summary>
    public static List<QuizQuestion> Shuffle(IEnumerable<QuizQuestion> questions, Random rng)
    {
        var list = questions.ToList();
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }
}
