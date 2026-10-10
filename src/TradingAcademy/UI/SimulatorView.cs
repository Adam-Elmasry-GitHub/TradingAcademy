using TradingAcademy.Learning;
using TradingAcademy.Trading;

namespace TradingAcademy.UI;

/// <summary>Interactive paper-trading game: trade a simulated market candle by candle with fake money.</summary>
public static class SimulatorView
{
    private const int WarmupBars = 60;
    private const int SessionBars = 150;
    private const int ChartWidth = 60;
    private const int ChartHeight = 16;

    public static void Run(Progress progress, Action save)
    {
        ConsoleUi.Header("PAPER TRADING SIMULATOR");
        ConsoleUi.Paragraph($"You start with 10,000 in fake money and trade a simulated stock for {SessionBars} candles. " +
                            "The market switches between bull, bear and sideways phases, just like real markets. " +
                            "Commission is 0.1% per trade. Your goal is not just profit: keep your drawdown small and follow your rules.");
        Console.WriteLine();
        var seedInput = ConsoleUi.Ask("Market seed (Enter = random; share a seed so friends get the same market): ");
        var seed = int.TryParse(seedInput, out var s) ? s : Random.Shared.Next(1, 100_000);

        var market = new MarketSimulator(seed, 100m, "SIM");
        market.Warmup(WarmupBars);
        var account = new PaperAccount();
        var endBar = WarmupBars + SessionBars;
        var messages = new List<string> { $"Market seed {seed}. Type h for help." };

        while (true)
        {
            Draw(market, account, endBar, messages);
            messages.Clear();
            if (market.Candles.Count >= endBar) break;

            var input = ConsoleUi.Ask("\ncommand> ").ToLowerInvariant();
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) parts = new[] { "n" };
            var price = market.Last.Close;
            var bar = market.Last.Index;

            switch (parts[0])
            {
                case "n" or "next":
                    var count = parts.Length > 1 && int.TryParse(parts[1], out var k) ? Math.Clamp(k, 1, 50) : 1;
                    for (var i = 0; i < count && market.Candles.Count < endBar; i++)
                    {
                        var fill = account.OnCandle(market.Next());
                        if (fill != null) { messages.Add(fill); break; }
                    }
                    break;
                case "b" or "buy" when TryQty(parts, out var qty):
                    messages.Add(account.Buy(qty, price, bar).Message);
                    break;
                case "s" or "sell" or "short" when TryQty(parts, out var qty):
                    messages.Add(account.Sell(qty, price, bar).Message);
                    break;
                case "c" or "close":
                    messages.Add(account.ClosePosition(price, bar).Message);
                    break;
                case "sl" or "tp" when parts.Length > 1:
                    decimal? level = parts[1] == "off" ? null : ConsoleUi.TryDecimal(parts[1], out var p) ? p : -1;
                    if (level == -1) { messages.Add("Give a price, e.g. sl 97.50, or 'off'."); break; }
                    messages.Add((parts[0] == "sl" ? account.SetStopLoss(level, price) : account.SetTakeProfit(level, price)).Message);
                    break;
                case "size" when parts.Length > 2 && ConsoleUi.TryDecimal(parts[1], out var risk) && ConsoleUi.TryDecimal(parts[2], out var stop):
                    try
                    {
                        var units = RiskCalculator.PositionSize(account.Equity(price), risk, price, stop);
                        messages.Add($"Risking {risk}% with entry {price:0.00} and stop {stop:0.00}: trade {units} units (max loss ≈ {units * Math.Abs(price - stop):0.00}).");
                    }
                    catch (ArgumentException e) { messages.Add(e.Message); }
                    break;
                case "h" or "help" or "?":
                    ShowHelp();
                    break;
                case "q" or "quit":
                    endBar = market.Candles.Count;
                    break;
                default:
                    messages.Add("Unknown command. Type h for help.");
                    break;
            }
        }

        Finish(market, account, progress, save);
    }

    private static bool TryQty(string[] parts, out decimal qty)
    {
        qty = 0;
        return parts.Length > 1 && ConsoleUi.TryDecimal(parts[1], out qty) && qty > 0;
    }

    private static void Draw(MarketSimulator market, PaperAccount account, int endBar, List<string> messages)
    {
        ConsoleUi.Clear();
        var candles = market.Candles;
        var closes = Indicators.Closes(candles);
        var sma20 = Indicators.Sma(closes, 20);
        var sma50 = Indicators.Sma(closes, 50);
        var rsi = Indicators.Rsi(closes);
        var atr = Indicators.Atr(candles);
        var (macd, signal) = Indicators.Macd(closes);

        var start = Math.Max(0, candles.Count - ChartWidth);
        var visible = candles.Skip(start).ToList();
        var levels = new List<decimal>();
        if (account.StopLoss is { } sl) levels.Add(sl);
        if (account.TakeProfit is { } tp) levels.Add(tp);
        var chart = AsciiChart.Render(visible, ChartHeight, sma20.Skip(start).ToList(), levels);

        var last = market.Last;
        var barsLeft = endBar - candles.Count;
        ConsoleUi.WriteLine($" {market.Symbol}  close {last.Close:0.00}   bars left {barsLeft}   (• = SMA20, --- = your stop/target)", ConsoleColor.Cyan);
        for (var r = 0; r < chart.Height; r++)
        {
            Console.Write(" ");
            for (var c = 0; c < chart.Width; c++)
                ConsoleUi.Write(chart.Cells[r, c].ToString(), ColorOf(chart.Kinds[r, c]));
            var label = r == 0 ? chart.Top : r == chart.Height - 1 ? chart.Bottom : r == chart.Height / 2 ? (chart.Top + chart.Bottom) / 2 : (decimal?)null;
            ConsoleUi.WriteLine(label is { } l ? $" {l:0.00}" : "", ConsoleColor.DarkGray);
        }

        var i = candles.Count - 1;
        ConsoleUi.WriteLine($" SMA20 {Fmt(sma20[i])}  SMA50 {Fmt(sma50[i])}  RSI14 {Fmt(rsi[i], "0")}  ATR14 {Fmt(atr[i])}  MACD {Fmt(macd[i])} / signal {Fmt(signal[i])}", ConsoleColor.Gray);

        var equity = account.Equity(last.Close);
        var change = (equity / account.StartingCash - 1) * 100;
        ConsoleUi.Write($" Equity {equity:0.00} ({change:+0.00;-0.00}%)  Cash {account.Cash:0.00}  ", change >= 0 ? ConsoleColor.Green : ConsoleColor.Red);
        if (account.IsFlat) ConsoleUi.WriteLine("Position: flat", ConsoleColor.Gray);
        else ConsoleUi.WriteLine($"Position: {Math.Abs(account.Position)} {account.Side} @ {account.AveragePrice:0.00}  open P/L {account.UnrealizedPnL(last.Close):+0.00;-0.00}  SL {account.StopLoss?.ToString("0.00") ?? "none"}  TP {account.TakeProfit?.ToString("0.00") ?? "none"}", ConsoleColor.White);

        foreach (var m in messages) ConsoleUi.WriteLine(" » " + m, ConsoleColor.Yellow);
        ConsoleUi.WriteLine(" b <qty> buy · s <qty> sell/short · c close · sl/tp <price|off> · size <risk%> <stop> · n [bars] next · h help · q end", ConsoleColor.DarkGray);
    }

    private static void ShowHelp()
    {
        ConsoleUi.Header("SIMULATOR COMMANDS");
        var lines = new[]
        {
            "n  or Enter      Advance one candle.  n 10 advances ten (stops early if your stop/target fills).",
            "b 50             Buy 50 units at the current close. When short, buying covers the short.",
            "s 50             Sell 50 units. When flat, this opens a SHORT position (profit if price falls).",
            "c                Close the whole position at the current price.",
            "sl 97.5          Place a stop-loss. It fills automatically if a later candle touches it.",
            "tp 110           Place a take-profit. Same idea, on the winning side.",
            "sl off / tp off  Remove the order.",
            "size 1 97.5      Position-size calculator: units to trade if you risk 1% of equity with a stop at 97.5.",
            "q                End the session early and see your statistics.",
            "",
            "Pro habit: before every entry decide your stop, your target and your size. Then use sl, tp and size.",
        };
        foreach (var line in lines) ConsoleUi.Info("  " + line);
        ConsoleUi.Pause();
    }

    private static void Finish(MarketSimulator market, PaperAccount account, Progress progress, Action save)
    {
        var last = market.Last;
        if (!account.IsFlat) account.ClosePosition(last.Close, last.Index, "session end");
        var stats = SessionStats.From(account, last.Close);

        progress.SimulatorSessions++;
        progress.BestSimulatorReturnPercent = progress.SimulatorSessions == 1
            ? (double)stats.ReturnPercent
            : Math.Max(progress.BestSimulatorReturnPercent, (double)stats.ReturnPercent);
        save();

        var buyHold = (last.Close / market.Candles[WarmupBars - 1].Close - 1) * 100;
        ConsoleUi.Header($"SESSION REPORT · seed {market.Seed}");
        ConsoleUi.WriteLine($"  Your return          {stats.ReturnPercent,8:+0.00;-0.00}%", stats.ReturnPercent >= 0 ? ConsoleColor.Green : ConsoleColor.Red);
        ConsoleUi.Info($"  Buy & hold return    {buyHold,8:+0.00;-0.00}%   (did you beat simply holding?)");
        ConsoleUi.Info($"  Trades               {stats.TradeCount,8}");
        ConsoleUi.Info($"  Win rate             {stats.WinRate,8:0.0}%");
        ConsoleUi.Info($"  Average win / loss   {stats.AverageWin,8:0.00} / {stats.AverageLoss:0.00}");
        ConsoleUi.Info($"  Profit factor        {stats.ProfitFactor,8:0.00}   (above 1.5 is good)");
        ConsoleUi.Info($"  Expectancy per trade {stats.Expectancy,8:0.00}");
        ConsoleUi.Info($"  Max drawdown         {stats.MaxDrawdownPercent,8:0.00}%   (pros try to keep this under 10-20%)");
        ConsoleUi.Info($"  Commission paid      {account.TotalCommission,8:0.00}");

        ConsoleUi.WriteLine("\n  COACH'S NOTES", ConsoleColor.Yellow);
        foreach (var note in SessionCoach.Notes(stats, account)) ConsoleUi.Paragraph("• " + note, 4, ConsoleColor.Yellow);
        ConsoleUi.Info("\n  Tip: write the best and worst trade of this session into the Trading Journal.");
        ConsoleUi.Pause();
    }

    private static string Fmt(double? value, string format = "0.00") => value?.ToString(format) ?? "n/a";

    private static ConsoleColor ColorOf(CellKind kind) => kind switch
    {
        CellKind.BullBody => ConsoleColor.Green,
        CellKind.BearBody => ConsoleColor.Red,
        CellKind.Wick => ConsoleColor.Gray,
        CellKind.Overlay => ConsoleColor.Yellow,
        CellKind.Marker => ConsoleColor.Magenta,
        _ => ConsoleColor.Black,
    };
}
