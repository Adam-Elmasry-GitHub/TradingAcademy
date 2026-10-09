using TradingAcademy.Learning;
using TradingAcademy.Trading;

namespace TradingAcademy.Tests;

public class SimulatorTests
{
    [Fact]
    public void SameSeedGivesSameMarket()
    {
        var a = new MarketSimulator(42);
        var b = new MarketSimulator(42);
        a.Warmup(100);
        b.Warmup(100);
        Assert.Equal(a.Candles, b.Candles);
    }

    [Fact]
    public void CandlesAreInternallyConsistent()
    {
        var market = new MarketSimulator(5);
        market.Warmup(500);
        Assert.All(market.Candles, c =>
        {
            Assert.True(c.High >= Math.Max(c.Open, c.Close));
            Assert.True(c.Low <= Math.Min(c.Open, c.Close));
            Assert.True(c.Low > 0);
        });
    }

    [Fact]
    public void EachCandleOpensAtPreviousClose()
    {
        var market = new MarketSimulator(9);
        market.Warmup(50);
        for (var i = 1; i < market.Candles.Count; i++)
            Assert.Equal(market.Candles[i - 1].Close, market.Candles[i].Open);
    }

    [Fact]
    public void ChartHasRequestedSize()
    {
        var market = new MarketSimulator(1);
        market.Warmup(40);
        var chart = AsciiChart.Render(market.Candles, 12);
        Assert.Equal(12, chart.Height);
        Assert.Equal(40, chart.Width);
    }
}

public class CurriculumTests
{
    private static readonly IReadOnlyList<Level> Levels = Curriculum.Load();

    [Fact]
    public void HasEightLevelsInOrder() =>
        Assert.Equal(Enumerable.Range(0, 8), Levels.Select(l => l.Number));

    [Fact]
    public void EveryLevelHasLessonsAndAQuiz() =>
        Assert.All(Levels, l =>
        {
            Assert.NotEmpty(l.Lessons);
            Assert.True(l.Quiz.Count >= 5, $"Level {l.Number} needs at least 5 questions");
        });

    [Fact]
    public void LessonIdsAreUnique()
    {
        var ids = Levels.SelectMany(l => l.Lessons).Select(l => l.Id).ToList();
        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public void QuizScoringAndUnlocking()
    {
        var quiz = Levels[0].Quiz;
        var perfect = QuizEngine.Score(quiz, quiz.Select(q => q.Answer).ToList());
        Assert.True(perfect.Passed);

        var progress = new Progress();
        Assert.True(progress.IsLevelUnlocked(0));
        Assert.False(progress.IsLevelUnlocked(1));
        progress.RecordQuiz(0, perfect.Percent);
        Assert.True(progress.IsLevelUnlocked(1));
    }

    [Fact]
    public void ProgressRoundTripsThroughDisk()
    {
        var path = Path.Combine(Path.GetTempPath(), $"ta-{Guid.NewGuid()}.json");
        try
        {
            var store = new ProgressStore(path);
            var progress = new Progress();
            progress.CompletedLessons.Add("0.1");
            progress.RecordQuiz(0, 87.5);
            store.Save(progress);

            var loaded = store.Load();
            Assert.Contains("0.1", loaded.CompletedLessons);
            Assert.Equal(87.5, loaded.BestQuizScores[0]);
        }
        finally
        {
            File.Delete(path);
        }
    }
}
