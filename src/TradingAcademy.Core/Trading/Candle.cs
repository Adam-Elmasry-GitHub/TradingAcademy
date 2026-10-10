namespace TradingAcademy.Trading;

/// <summary>One bar of price data (OHLCV).</summary>
public readonly record struct Candle(int Index, decimal Open, decimal High, decimal Low, decimal Close, long Volume)
{
    public bool IsBullish => Close >= Open;
    public decimal Range => High - Low;
}
