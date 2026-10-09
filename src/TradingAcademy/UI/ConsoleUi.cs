using System.Globalization;

namespace TradingAcademy.UI;

/// <summary>Small helpers for consistent, colourful console output and safe input.</summary>
public static class ConsoleUi
{
    public static void Clear()
    {
        // Clear throws when output is redirected (for example in CI); skip it there.
        if (!Console.IsOutputRedirected)
        {
            try { Console.Clear(); } catch (IOException) { }
        }
    }

    public static void Header(string title)
    {
        Clear();
        var line = new string('═', Math.Max(title.Length + 4, 50));
        Write(line + "\n", ConsoleColor.DarkCyan);
        Write($"  {title}\n", ConsoleColor.Cyan);
        Write(line + "\n\n", ConsoleColor.DarkCyan);
    }

    public static void Write(string text, ConsoleColor color)
    {
        var old = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = old;
    }

    public static void WriteLine(string text, ConsoleColor color) => Write(text + "\n", color);
    public static void Info(string text) => WriteLine(text, ConsoleColor.Gray);
    public static void Success(string text) => WriteLine(text, ConsoleColor.Green);
    public static void Warning(string text) => WriteLine(text, ConsoleColor.Yellow);
    public static void Error(string text) => WriteLine(text, ConsoleColor.Red);

    /// <summary>Wraps long text to the console width with an optional indent.</summary>
    public static void Paragraph(string text, int indent = 2, ConsoleColor color = ConsoleColor.White)
    {
        var width = Math.Max(40, SafeWidth() - indent - 2);
        var pad = new string(' ', indent);
        var line = "";
        foreach (var word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.Length + word.Length + 1 > width)
            {
                WriteLine(pad + line, color);
                line = "";
            }
            line = line.Length == 0 ? word : line + " " + word;
        }
        if (line.Length > 0) WriteLine(pad + line, color);
    }

    public static string Ask(string prompt)
    {
        Write(prompt, ConsoleColor.Cyan);
        return Console.ReadLine()?.Trim() ?? "q";
    }

    public static int? AskInt(string prompt, int min, int max)
    {
        while (true)
        {
            var input = Ask(prompt);
            if (input.Length == 0 || input.Equals("q", StringComparison.OrdinalIgnoreCase)) return null;
            if (int.TryParse(input, out var value) && value >= min && value <= max) return value;
            Error($"Please enter a number from {min} to {max} (or q to go back).");
        }
    }

    public static decimal? AskDecimal(string prompt)
    {
        while (true)
        {
            var input = Ask(prompt);
            if (input.Length == 0 || input.Equals("q", StringComparison.OrdinalIgnoreCase)) return null;
            if (TryDecimal(input, out var value)) return value;
            Error("Please enter a number, for example 125.50 (or q to go back).");
        }
    }

    /// <summary>Accepts both "12.5" and "12,5" so Norwegian and English keyboards both work.</summary>
    public static bool TryDecimal(string input, out decimal value) =>
        decimal.TryParse(input.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out value);

    public static void Pause()
    {
        Write("\nPress Enter to continue...", ConsoleColor.DarkGray);
        Console.ReadLine();
    }

    public static int SafeWidth()
    {
        try { return Console.IsOutputRedirected ? 100 : Math.Max(60, Console.WindowWidth); }
        catch (IOException) { return 100; }
    }
}
