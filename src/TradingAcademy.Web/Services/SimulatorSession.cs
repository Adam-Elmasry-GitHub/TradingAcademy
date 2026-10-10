using TradingAcademy.Trading;

namespace TradingAcademy.Web.Services;

/// <summary>One paper-trading game. Kept per visitor so leaving the page does not lose the session.</summary>
public sealed class SimulatorSession
{
    public const int WarmupBars = 60;
    public const int SessionBars = 150;

    public MarketSimulator? Market { get; private set; }
    public PaperAccount? Account { get; private set; }
    public List<(string Text, bool Good)> Log { get; } = new();
    public bool IsFinished { get; private set; }
    public bool Recorded { get; set; }
    public bool IsRunning => Market != null && !IsFinished;
    public SessionStats? Stats { get; private set; }
    public int EndBar => WarmupBars + SessionBars;
    public int BarsLeft => Market == null ? 0 : EndBar - Market.Candles.Count;
    public decimal Price => Market?.Last.Close ?? 0;

    public void Start(int seed)
    {
        Market = new MarketSimulator(seed, 100m, "SIM");
        Market.Warmup(WarmupBars);
        Account = new PaperAccount();
        Log.Clear();
        Stats = null;
        IsFinished = false;
        Recorded = false;
        Note($"New session on market #{seed}. You have 10,000 in paper money.", true);
    }

    public void Advance(int bars)
    {
        if (!IsRunning) return;
        for (var i = 0; i < bars && Market!.Candles.Count < EndBar; i++)
        {
            var fill = Account!.OnCandle(Market.Next());
            if (fill != null) { Note(fill, !fill.StartsWith("stop")); break; }
        }
        if (Market!.Candles.Count >= EndBar) Finish();
    }

    public void Buy(decimal qty) => Record(Account!.Buy(qty, Price, Market!.Last.Index));
    public void Sell(decimal qty) => Record(Account!.Sell(qty, Price, Market!.Last.Index));
    public void Close() => Record(Account!.ClosePosition(Price, Market!.Last.Index));
    public void SetStop(decimal? price) => Record(Account!.SetStopLoss(price, Price));
    public void SetTarget(decimal? price) => Record(Account!.SetTakeProfit(price, Price));

    public void Finish()
    {
        if (Market == null || Account == null || IsFinished) return;
        if (!Account.IsFlat) Account.ClosePosition(Price, Market.Last.Index, "session end");
        Stats = SessionStats.From(Account, Price);
        IsFinished = true;
    }

    public decimal BuyAndHoldPercent =>
        Market == null ? 0 : (Price / Market.Candles[WarmupBars - 1].Close - 1) * 100;

    private void Record(OrderResult result) => Note(result.Message, result.Success);

    private void Note(string text, bool good)
    {
        Log.Insert(0, (text, good));
        if (Log.Count > 30) Log.RemoveAt(Log.Count - 1);
    }
}
