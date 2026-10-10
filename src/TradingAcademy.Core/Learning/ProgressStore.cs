using System.Text.Json;

namespace TradingAcademy.Learning;

/// <summary>What the learner has done so far. Saved as JSON in the user's application data folder.</summary>
public sealed class Progress
{
    public HashSet<string> CompletedLessons { get; set; } = new();
    public Dictionary<int, double> BestQuizScores { get; set; } = new();
    public List<JournalEntry> Journal { get; set; } = new();
    public double BestSimulatorReturnPercent { get; set; }
    public int SimulatorSessions { get; set; }

    public bool IsLevelPassed(int level) =>
        BestQuizScores.TryGetValue(level, out var score) && score >= QuizEngine.PassPercent;

    /// <summary>Level 0 is always open; every other level opens when the previous quiz is passed.</summary>
    public bool IsLevelUnlocked(int level) => level == 0 || IsLevelPassed(level - 1);

    public void RecordQuiz(int level, double percent)
    {
        if (!BestQuizScores.TryGetValue(level, out var best) || percent > best)
            BestQuizScores[level] = percent;
    }
}

public sealed class JournalEntry
{
    public DateTime Date { get; set; }
    public string Symbol { get; set; } = "";
    public string Side { get; set; } = "";
    public string Setup { get; set; } = "";
    public decimal Entry { get; set; }
    public decimal Exit { get; set; }
    public decimal Quantity { get; set; }
    public string Emotion { get; set; } = "";
    public string Lesson { get; set; } = "";

    public decimal ProfitLoss => Side.Equals("short", StringComparison.OrdinalIgnoreCase)
        ? (Entry - Exit) * Quantity
        : (Exit - Entry) * Quantity;
}

public sealed class ProgressStore
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    private readonly string _path;

    public ProgressStore(string? path = null)
    {
        _path = path ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create),
            "TradingAcademy", "progress.json");
    }

    public string FilePath => _path;

    public Progress Load()
    {
        try
        {
            return File.Exists(_path)
                ? JsonSerializer.Deserialize<Progress>(File.ReadAllText(_path), Options) ?? new Progress()
                : new Progress();
        }
        catch (JsonException)
        {
            // A damaged file should never stop someone from learning; start fresh instead.
            return new Progress();
        }
    }

    public void Save(Progress progress)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        File.WriteAllText(_path, JsonSerializer.Serialize(progress, Options));
    }
}
