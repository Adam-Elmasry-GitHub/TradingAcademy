using System.Globalization;

namespace TradingAcademy.Web.Components.Shared;

/// <summary>Parsing and formatting helpers that accept both "12.5" and "12,5".</summary>
public static class Num
{
    public static decimal? Parse(string? text) =>
        decimal.TryParse(text?.Trim().Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out var v) ? v : null;

    public static string Money(decimal value) => value.ToString("#,0.00", CultureInfo.InvariantCulture);
    public static string Signed(decimal value) => value.ToString("+#,0.00;-#,0.00;0.00", CultureInfo.InvariantCulture);
    public static string SignedPct(decimal value) => value.ToString("+0.00;-0.00;0.00", CultureInfo.InvariantCulture) + "%";
    public static string Opt(double? value, string format = "0.00") => value?.ToString(format, CultureInfo.InvariantCulture) ?? "–";
    public static string Css(decimal value) => value > 0 ? "up" : value < 0 ? "down" : "";
}
