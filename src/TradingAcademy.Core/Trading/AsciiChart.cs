namespace TradingAcademy.Trading;

public enum CellKind { Empty, BullBody, BearBody, Wick, Overlay, Marker }

/// <summary>Draws candles as a grid of characters so a chart can be shown in any terminal.</summary>
public static class AsciiChart
{
    public sealed record Chart(char[,] Cells, CellKind[,] Kinds, decimal Top, decimal Bottom)
    {
        public int Height => Cells.GetLength(0);
        public int Width => Cells.GetLength(1);
    }

    public static Chart Render(IReadOnlyList<Candle> candles, int height, IReadOnlyList<double?>? overlay = null, IReadOnlyList<decimal>? levels = null)
    {
        if (height < 3) throw new ArgumentOutOfRangeException(nameof(height));
        var width = candles.Count;
        var cells = new char[height, width];
        var kinds = new CellKind[height, width];
        for (var r = 0; r < height; r++)
            for (var c = 0; c < width; c++) cells[r, c] = ' ';

        if (width == 0) return new Chart(cells, kinds, 0, 0);

        var top = candles.Max(c => c.High);
        var bottom = candles.Min(c => c.Low);
        if (overlay != null)
            foreach (var v in overlay.Where(v => v.HasValue))
            {
                top = Math.Max(top, (decimal)v!.Value);
                bottom = Math.Min(bottom, (decimal)v.Value);
            }
        if (levels != null)
            foreach (var level in levels)
            {
                top = Math.Max(top, level);
                bottom = Math.Min(bottom, level);
            }
        if (top == bottom) { top += 1; bottom -= 1; }

        int Row(decimal price) => (int)Math.Round((top - price) / (top - bottom) * (height - 1));

        if (levels != null)
            foreach (var level in levels)
            {
                var r = Row(level);
                for (var c = 0; c < width; c++) { cells[r, c] = '-'; kinds[r, c] = CellKind.Marker; }
            }

        for (var c = 0; c < width; c++)
        {
            var candle = candles[c];
            int hi = Row(candle.High), lo = Row(candle.Low);
            int bodyTop = Row(Math.Max(candle.Open, candle.Close)), bodyBottom = Row(Math.Min(candle.Open, candle.Close));
            for (var r = hi; r <= lo; r++) { cells[r, c] = '│'; kinds[r, c] = CellKind.Wick; }
            var kind = candle.IsBullish ? CellKind.BullBody : CellKind.BearBody;
            for (var r = bodyTop; r <= bodyBottom; r++) { cells[r, c] = '█'; kinds[r, c] = kind; }

            if (overlay != null && c < overlay.Count && overlay[c] is { } ma)
            {
                var r = Row((decimal)ma);
                if (kinds[r, c] is CellKind.Empty or CellKind.Marker) { cells[r, c] = '•'; kinds[r, c] = CellKind.Overlay; }
            }
        }

        return new Chart(cells, kinds, top, bottom);
    }
}
