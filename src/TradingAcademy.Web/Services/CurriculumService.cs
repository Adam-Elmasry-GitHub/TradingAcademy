using TradingAcademy.Learning;

namespace TradingAcademy.Web.Services;

/// <summary>The curriculum is the same for every visitor, so it is loaded once and shared.</summary>
public sealed class CurriculumService
{
    public CurriculumService() => Levels = Curriculum.Load();

    public IReadOnlyList<Level> Levels { get; }

    public int TotalLessons => Levels.Sum(l => l.Lessons.Count);

    public Level? FindLevel(int number) => Levels.FirstOrDefault(l => l.Number == number);

    public (Level Level, Lesson Lesson, int Index)? FindLesson(string id)
    {
        foreach (var level in Levels)
            for (var i = 0; i < level.Lessons.Count; i++)
                if (level.Lessons[i].Id == id) return (level, level.Lessons[i], i);
        return null;
    }

    /// <summary>Lessons in roadmap order, used for "previous" and "next" links.</summary>
    public IReadOnlyList<Lesson> AllLessons => Levels.SelectMany(l => l.Lessons).ToList();

    public Lesson? NextLesson(Progress progress) =>
        AllLessons.FirstOrDefault(l => !progress.CompletedLessons.Contains(l.Id));
}
