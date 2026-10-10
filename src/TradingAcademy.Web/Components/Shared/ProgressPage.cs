using Microsoft.AspNetCore.Components;
using TradingAcademy.Learning;
using TradingAcademy.Web.Services;

namespace TradingAcademy.Web.Components.Shared;

/// <summary>Base for pages that show the learner's progress: re-renders when progress loads or changes.</summary>
public abstract class ProgressPage : ComponentBase, IDisposable
{
    [Inject] protected ProgressService ProgressService { get; set; } = default!;
    [Inject] protected CurriculumService Curriculum { get; set; } = default!;

    protected Progress Progress => ProgressService.Progress;

    protected override void OnInitialized() => ProgressService.Changed += OnProgressChanged;

    private void OnProgressChanged() => InvokeAsync(StateHasChanged);

    public virtual void Dispose()
    {
        ProgressService.Changed -= OnProgressChanged;
        GC.SuppressFinalize(this);
    }

    protected int LessonsDone(Level level) => level.Lessons.Count(l => Progress.CompletedLessons.Contains(l.Id));

    protected string LevelStatus(Level level) =>
        Progress.IsLevelPassed(level.Number) ? "done" : Progress.IsLevelUnlocked(level.Number) ? "open" : "locked";

    protected static string Pct(double value) => value.ToString("0.#", System.Globalization.CultureInfo.InvariantCulture);
}
