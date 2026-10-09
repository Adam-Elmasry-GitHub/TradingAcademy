namespace TradingAcademy.Trading;

public sealed record ClosedTrade(string Side, decimal Quantity, decimal EntryPrice, decimal ExitPrice, decimal ProfitLoss, int ExitBar, string Reason);

public sealed record OrderResult(bool Success, string Message);

/// <summary>
/// A simulated brokerage account. Supports long and short positions, commissions,
/// stop-loss and take-profit orders. No leverage: total exposure may not exceed equity.
/// Position is signed: positive = long, negative = short.
/// </summary>
public sealed class PaperAccount
{
    private readonly List<ClosedTrade> _trades = new();
    private readonly List<decimal> _equityCurve = new();

    public PaperAccount(decimal startingCash = 10_000m, decimal commissionRate = 0.001m)
    {
        if (startingCash <= 0) throw new ArgumentOutOfRangeException(nameof(startingCash));
        StartingCash = startingCash;
        Cash = startingCash;
        CommissionRate = commissionRate;
    }

    public decimal StartingCash { get; }
    public decimal CommissionRate { get; }
    public decimal Cash { get; private set; }
    public decimal Position { get; private set; }
    public decimal AveragePrice { get; private set; }
    public decimal? StopLoss { get; private set; }
    public decimal? TakeProfit { get; private set; }
    public decimal TotalCommission { get; private set; }
    public IReadOnlyList<ClosedTrade> Trades => _trades;
    public IReadOnlyList<decimal> EquityCurve => _equityCurve;

    public bool IsFlat => Position == 0;
    public string Side => Position > 0 ? "long" : Position < 0 ? "short" : "flat";

    public decimal Equity(decimal price) => Cash + Position * price;
    public decimal UnrealizedPnL(decimal price) => Position * (price - AveragePrice);

    public OrderResult Buy(decimal quantity, decimal price, int bar) => Execute(quantity, price, bar, "manual");
    public OrderResult Sell(decimal quantity, decimal price, int bar) => Execute(-quantity, price, bar, "manual");

    public OrderResult ClosePosition(decimal price, int bar, string reason = "manual") =>
        IsFlat ? new OrderResult(false, "You have no open position.") : Execute(-Position, price, bar, reason);

    public OrderResult SetStopLoss(decimal? price, decimal marketPrice)
    {
        if (price is null) { StopLoss = null; return new OrderResult(true, "Stop-loss removed."); }
        if (IsFlat) return new OrderResult(false, "Open a position before setting a stop-loss.");
        if (Position > 0 && price >= marketPrice) return new OrderResult(false, "A long stop-loss must be below the current price.");
        if (Position < 0 && price <= marketPrice) return new OrderResult(false, "A short stop-loss must be above the current price.");
        StopLoss = price;
        return new OrderResult(true, $"Stop-loss set at {price:0.00}.");
    }

    public OrderResult SetTakeProfit(decimal? price, decimal marketPrice)
    {
        if (price is null) { TakeProfit = null; return new OrderResult(true, "Take-profit removed."); }
        if (IsFlat) return new OrderResult(false, "Open a position before setting a take-profit.");
        if (Position > 0 && price <= marketPrice) return new OrderResult(false, "A long take-profit must be above the current price.");
        if (Position < 0 && price >= marketPrice) return new OrderResult(false, "A short take-profit must be below the current price.");
        TakeProfit = price;
        return new OrderResult(true, $"Take-profit set at {price:0.00}.");
    }

    /// <summary>
    /// Called for every new candle. Fills stop-loss or take-profit orders that the bar touched.
    /// If price gaps through the level, the fill is at the open (like a real market), which can be worse than planned.
    /// When both are touched in the same bar, the stop is assumed to fill first (the conservative choice).
    /// </summary>
    public string? OnCandle(Candle candle)
    {
        string? message = null;
        if (Position > 0)
        {
            if (StopLoss is { } sl && candle.Low <= sl)
                message = Exit(Math.Min(sl, candle.Open), candle.Index, "stop-loss");
            else if (TakeProfit is { } tp && candle.High >= tp)
                message = Exit(Math.Max(tp, candle.Open), candle.Index, "take-profit");
        }
        else if (Position < 0)
        {
            if (StopLoss is { } sl && candle.High >= sl)
                message = Exit(Math.Max(sl, candle.Open), candle.Index, "stop-loss");
            else if (TakeProfit is { } tp && candle.Low <= tp)
                message = Exit(Math.Min(tp, candle.Open), candle.Index, "take-profit");
        }

        _equityCurve.Add(Equity(candle.Close));
        return message;
    }

    private string Exit(decimal price, int bar, string reason)
    {
        var result = Execute(-Position, price, bar, reason);
        return $"{reason} hit: {result.Message}";
    }

    private OrderResult Execute(decimal signedQty, decimal price, int bar, string reason)
    {
        if (signedQty == 0) return new OrderResult(false, "Quantity must be greater than zero.");
        if (price <= 0) return new OrderResult(false, "Price must be positive.");

        var newPosition = Position + signedQty;
        var commission = Math.Abs(signedQty) * price * CommissionRate;
        var equityAfter = Equity(price) - commission;
        var increasesExposure = Math.Abs(newPosition) > Math.Abs(Position);
        if (increasesExposure && Math.Abs(newPosition) * price > equityAfter)
            return new OrderResult(false, $"Not enough equity. Max size at {price:0.00} is about {Math.Floor(equityAfter / price)} units.");

        Cash -= signedQty * price + commission;
        TotalCommission += commission;

        if (Position == 0 || Math.Sign(Position) == Math.Sign(signedQty))
        {
            AveragePrice = (AveragePrice * Math.Abs(Position) + price * Math.Abs(signedQty)) / Math.Abs(newPosition);
            Position = newPosition;
            return new OrderResult(true, $"Filled {Describe(signedQty)} at {price:0.00} (fee {commission:0.00}). Position: {Position} {Side} @ {AveragePrice:0.00}.");
        }

        // Reducing, closing or flipping the position.
        var closingQty = Math.Min(Math.Abs(signedQty), Math.Abs(Position));
        var side = Position > 0 ? "long" : "short";
        var gross = closingQty * (price - AveragePrice) * Math.Sign(Position);
        var exitFee = commission * closingQty / Math.Abs(signedQty);
        var pnl = gross - exitFee;
        _trades.Add(new ClosedTrade(side, closingQty, AveragePrice, price, Math.Round(pnl, 2), bar, reason));

        Position = newPosition;
        if (Position == 0 || Math.Sign(Position) != Math.Sign(Position - signedQty))
        {
            StopLoss = null;
            TakeProfit = null;
            AveragePrice = Position == 0 ? 0 : price;
        }

        return new OrderResult(true, $"Filled {Describe(signedQty)} at {price:0.00}. Realised P/L {pnl:+0.00;-0.00}. Position: {(Position == 0 ? "flat" : $"{Position} {Side} @ {AveragePrice:0.00}")}.");
    }

    private static string Describe(decimal signedQty) => signedQty > 0 ? $"BUY {signedQty}" : $"SELL {-signedQty}";
}

/// <summary>Performance numbers a professional trader reviews after every session.</summary>
public sealed record SessionStats(
    decimal ReturnPercent, int TradeCount, double WinRate, decimal AverageWin, decimal AverageLoss,
    decimal ProfitFactor, decimal Expectancy, decimal MaxDrawdownPercent)
{
    public static SessionStats From(PaperAccount account, decimal lastPrice)
    {
        var trades = account.Trades;
        var wins = trades.Where(t => t.ProfitLoss > 0).ToList();
        var losses = trades.Where(t => t.ProfitLoss <= 0).ToList();
        var grossWin = wins.Sum(t => t.ProfitLoss);
        var grossLoss = -losses.Sum(t => t.ProfitLoss);

        decimal peak = account.StartingCash, maxDd = 0;
        foreach (var equity in account.EquityCurve)
        {
            peak = Math.Max(peak, equity);
            maxDd = Math.Max(maxDd, peak == 0 ? 0 : (peak - equity) / peak);
        }

        return new SessionStats(
            ReturnPercent: (account.Equity(lastPrice) / account.StartingCash - 1) * 100,
            TradeCount: trades.Count,
            WinRate: trades.Count == 0 ? 0 : 100.0 * wins.Count / trades.Count,
            AverageWin: wins.Count == 0 ? 0 : grossWin / wins.Count,
            AverageLoss: losses.Count == 0 ? 0 : grossLoss / losses.Count,
            ProfitFactor: grossLoss == 0 ? (grossWin > 0 ? 99.99m : 0) : grossWin / grossLoss,
            Expectancy: trades.Count == 0 ? 0 : trades.Sum(t => t.ProfitLoss) / trades.Count,
            MaxDrawdownPercent: maxDd * 100);
    }
}
