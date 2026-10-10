using System.Text.Json;

namespace TradingAcademy.Learning;

/// <summary>One step on the roadmap: a group of lessons plus a quiz that unlocks the next level.</summary>
public sealed class Level
{
    public int Number { get; set; }
    public string Name { get; set; } = "";
    public string Rank { get; set; } = "";
    public string Goal { get; set; } = "";
    public string Duration { get; set; } = "";
    public List<Lesson> Lessons { get; set; } = new();
    public List<QuizQuestion> Quiz { get; set; } = new();
    public List<string> Milestones { get; set; } = new();
}

public sealed class Lesson
{
    public string Id { get; set; } = "";
    public string Title { get; set; } = "";
    public List<string> Body { get; set; } = new();
    public List<string> KeyTakeaways { get; set; } = new();
    public string Exercise { get; set; } = "";
    public List<string> ReadMore { get; set; } = new();
}

public sealed class QuizQuestion
{
    public string Prompt { get; set; } = "";
    public List<string> Options { get; set; } = new();
    public int Answer { get; set; }
    public string Explanation { get; set; } = "";
}

/// <summary>Loads the curriculum (lessons and quizzes) from the level JSON files.</summary>
public static class Curriculum
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    /// <summary>
    /// Loads every level. With no folder given, the curriculum embedded in this assembly is used;
    /// pass a folder to load edited JSON files from disk instead.
    /// </summary>
    public static IReadOnlyList<Level> Load(string? directory = null)
    {
        var sources = directory is null ? EmbeddedLevels() : FileLevels(directory);
        var levels = sources
            .Select(source => JsonSerializer.Deserialize<Level>(source.Json, Options)
                              ?? throw new InvalidDataException($"Empty level file: {source.Name}"))
            .OrderBy(level => level.Number)
            .ToList();

        foreach (var level in levels)
            Validate(level);

        return levels;
    }

    private static IEnumerable<(string Name, string Json)> EmbeddedLevels()
    {
        var assembly = typeof(Curriculum).Assembly;
        foreach (var name in assembly.GetManifestResourceNames().Where(n => n.StartsWith("TradingAcademy.Content.levels.level-")))
        {
            using var reader = new StreamReader(assembly.GetManifestResourceStream(name)!);
            yield return (name, reader.ReadToEnd());
        }
    }

    private static IEnumerable<(string Name, string Json)> FileLevels(string directory)
    {
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException($"Curriculum folder not found: {directory}");
        return Directory.GetFiles(directory, "level-*.json").Select(file => (file, File.ReadAllText(file)));
    }

    private static void Validate(Level level)
    {
        foreach (var q in level.Quiz)
        {
            if (q.Options.Count < 2 || q.Answer < 0 || q.Answer >= q.Options.Count)
                throw new InvalidDataException($"Level {level.Number}: bad quiz question \"{q.Prompt}\"");
        }
    }
}
