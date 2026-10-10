# 📈 Trading Academy

[![build](https://github.com/Adam-Elmasry-GitHub/TradingAcademy/actions/workflows/build.yml/badge.svg)](https://github.com/Adam-Elmasry-GitHub/TradingAcademy/actions/workflows/build.yml)

**Learn trading from absolute beginner to professional, in a C# console app.**

Trading Academy is a free, open-source learning app written in C# (.NET 8). It comes as a **modern, responsive web app** (Blazor) and as a **console app**, both built on the same core library. It combines a drawn **8-level roadmap**, **32 lessons**, **60 quiz questions**, a **paper-trading simulator** with live charts and indicators, a **trader's toolkit** of risk calculators, and a **trading journal**. Next to the app, the [`docs`](docs) folder contains a study guide for every level, a reading list of the best books and free courses, a glossary and ready-to-use templates.

The idea is simple: **learn → test → practise → review**, one level at a time. You unlock the next level by passing the quiz for the current one, and you practise every skill with fake money before real money is ever involved.

> ⚠️ **Educational use only. Nothing here is financial advice.** Trading involves substantial risk, and most retail traders lose money. Never trade with money you cannot afford to lose.

![Trading Academy web app](docs/screenshots/home.png)

<p align="center">
  <img src="docs/screenshots/simulator.png" width="68%" alt="Paper trading simulator" />
  <img src="docs/screenshots/mobile-quiz.png" width="24%" alt="Quiz on a phone, light mode" />
</p>

---

## Contents

- [Features](#features)
- [Quick start](#quick-start)
- [The web app](#the-web-app)
- [The roadmap](#the-roadmap)
- [How to use the app](#how-to-use-the-app)
- [Paper trading simulator](#paper-trading-simulator)
- [Trader's toolkit](#traders-toolkit)
- [Documentation and sources](#documentation-and-sources)
- [Project structure](#project-structure)
- [How it works (for developers)](#how-it-works-for-developers)
- [Extending the course](#extending-the-course)
- [Running the tests](#running-the-tests)
- [Disclaimer](#disclaimer)

---

## Features

| Module | What it does |
|---|---|
| 🗺️ **Roadmap** | Draws the 8-level path in the terminal and shows which levels are done, open or locked. |
| 📚 **Learn** | 32 lessons from "what is a stock" to "algorithmic trading in C#", each with key takeaways, a practice task and further reading. Completed lessons are ticked off. |
| ❓ **Quiz** | 7-8 shuffled multiple-choice questions per level with explanations for every answer. Score **70%+** to unlock the next level. |
| 🕹️ **Paper trading simulator** | Trade a simulated stock candle by candle with 10,000 in fake money. Coloured candlestick chart, SMA20 overlay, SMA50, RSI, ATR and MACD readouts, long and short positions, stop-loss and take-profit orders, commission, gap fills, and an end-of-session report with coaching notes. |
| 🧮 **Trader's toolkit** | Position size calculator, reward-to-risk and break-even win rate, expectancy, drawdown recovery table and Kelly criterion. |
| 📓 **Trading journal** | Log trades with setup, emotion and lesson learned. Shows win rate, total P/L and your most profitable setup. |
| 📊 **Progress** | Lessons done, best quiz scores, simulator sessions and journal entries, saved automatically between runs. |

## Quick start

**Requirements:** the [.NET 8 SDK](https://dotnet.microsoft.com/download) (or newer) on Windows, macOS or Linux.

```bash
git clone https://github.com/Adam-Elmasry-GitHub/TradingAcademy.git
cd TradingAcademy

# Web app (recommended): then open the address it prints, e.g. http://localhost:5000
dotnet run --project src/TradingAcademy.Web

# Console app
dotnet run --project src/TradingAcademy
```

For the console app, use a terminal at least 120 characters wide so the charts fit.

To open it in Visual Studio or Rider, open `TradingAcademy.sln`, choose `TradingAcademy.Web` (or `TradingAcademy`) as the startup project and press Run.

## The web app

A Blazor Server app with a hand-made design system: no CSS framework, no JavaScript libraries.

| | |
|---|---|
| **Responsive** | Sidebar navigation on desktop, a top bar with a slide-in menu on phones and tablets. Cards, grids and the simulator layout reflow from 4K screens down to 360 px phones. On phones the price chart scrolls sideways and starts at the newest candles. |
| **Dark and light themes** | Follows your system setting, with a one-click toggle that is remembered. Every colour is a design token. |
| **Interactive SVG chart** | Candlesticks, SMA 20/50, an RSI panel, a live price tag, and your entry, stop and target drawn on the chart. |
| **Your progress stays in your browser** | Lessons, quiz scores, simulator stats and journal entries are saved in the browser's local storage. No account, nothing sent to a server. |
| **Pages** | Home dashboard · Roadmap timeline · Lessons and a lesson reader · Quizzes with instant feedback and a score ring · Paper-trading simulator with order ticket, risk-based sizing and session report · Toolkit with live calculators · Trading journal · Progress |

## The roadmap

| Level | Rank | Time | You learn |
|---|---|---|---|
| **0 · Foundations** | Novice | 2-3 weeks | Trading vs investing, stocks/ETFs/forex/futures/options/crypto, order types, bid/ask/spread, brokers, costs, leverage, taxes |
| **1 · Charts and Price Action** | Apprentice | 3-4 weeks | Candlesticks, trend structure, support and resistance, timeframes, volume |
| **2 · Technical Analysis** | Analyst | 4-6 weeks | SMA/EMA, RSI, MACD, ATR, Bollinger Bands, chart patterns and their limits |
| **3 · Risk Management** | Risk Manager | 3-4 weeks | Drawdown maths, stop placement, position sizing, R-multiples, expectancy, correlation, Kelly |
| **4 · Fundamental and Macro** | Market Analyst | 4-6 weeks | Earnings, P/E, central banks, inflation, economic calendar, sentiment, oil and NOK |
| **5 · Trading Psychology** | Disciplined Trader | ongoing | Biases, fear/greed/FOMO, revenge trading, writing a trading plan, journaling |
| **6 · Strategy and Backtesting** | Strategist | 6-8 weeks | Building rule-based strategies, honest backtesting, overfitting, forward testing |
| **7 · Professional** | Pro | 6-12 months+ | Algorithmic trading in C# (QuantConnect LEAN), options and greeks, portfolios, execution, trading as a business |

The full roadmap, with a Mermaid diagram, the three phases, a weekly routine, rules for day one and the pro checklist, is in **[docs/ROADMAP.md](docs/ROADMAP.md)**. Each level has its own week-by-week study guide in **[docs/levels](docs/levels)**.

## How to use the app

The web app has the same sections as the console menu below, in the sidebar.

```
══════════════════════════════════════════════════
  TRADING ACADEMY · beginner to pro
══════════════════════════════════════════════════

  Current level: 0 · Foundations (Novice)

  [1] Roadmap             see the path from beginner to pro
  [2] Learn               lessons for every level
  [3] Quiz                pass to unlock the next level
  [4] Paper trading       practise on a simulated market
  [5] Trader's toolkit    position size, risk/reward, expectancy
  [6] Trading journal     log and review your trades
  [7] Progress            your stats
  [0] Exit
```

A good study loop for each level:

1. Open the level's study guide in `docs/levels` and follow the week-by-week plan.
2. Read the lessons in **Learn** and do each practice task.
3. Play at least three **Paper trading** sessions using what you learned.
4. Log your best and worst trade in the **Trading journal**.
5. Take the **Quiz**. Pass with 70% to unlock the next level; otherwise review and retry (questions are shuffled).

In the web app, progress is saved in your browser. In the console app, progress is saved automatically to `progress.json` in your user application data folder (`%APPDATA%\TradingAcademy` on Windows, `~/.config/TradingAcademy` on Linux and macOS). You can reset it from the Progress screen.

## Paper trading simulator

```
 SIM  close 102.03   bars left 87   (• = SMA20, --- = your stop/target)
                                  │                           116.74
                                ████
                           ████ █││███
                          │██████  ███
                       │ ███││██    •█••••••
                       ███ │  ██ ••• ███    ••••
 ████          ││███  █      ••        ██│   ││   ••          107.64
 █││██         │██ ██│█    ••           ███ ███ │   ••
     █████••••█•    │•                     ││ ████      ••
         ██ █│                                   ██████ ││█ │
 -------------------------------------------------------------  98.53
 SMA20 102.26  SMA50 105.97  RSI14 45  ATR14 1.69  MACD -1.66 / signal -1.83
 Equity 10005.37 (+0.05%)  Cash 7964.77  Position: 20 long @ 101.66  open P/L +7.40  SL 98.50  TP none
```

You start with **10,000** in fake money and trade **150 candles**. The market is generated by geometric Brownian motion that switches between **bull, bear and sideways regimes**, so you meet trends, pullbacks and choppy ranges. Enter a **seed** to replay the same market or challenge a friend on identical prices.

In the web app these are buttons and fields in the order ticket. In the console app you type commands:

| Command | Action |
|---|---|
| `n` or Enter | Next candle (`n 10` = ten candles; stops early if a stop or target fills) |
| `b 20` | Buy 20 units (covers a short if you have one) |
| `s 20` | Sell 20 units; when flat this opens a **short** |
| `c` | Close the whole position |
| `sl 98.5` / `tp 110` | Place a stop-loss / take-profit (`sl off` removes it) |
| `size 1 98.5` | Units to trade if you risk 1% of equity with a stop at 98.5 |
| `h` | Help |
| `q` | End the session and see the report |

Realistic details: 0.1% commission per trade, no leverage (exposure cannot exceed equity), stops that gap fill at the open (worse than planned, like in real markets), and the stop wins if a candle touches both stop and target.

At the end you get a **session report**: your return versus buy-and-hold, number of trades, win rate, average win and loss, profit factor, expectancy, maximum drawdown and commission paid, plus **coach's notes** pointing out overtrading, missing stops, oversized risk and losses larger than wins.

## Trader's toolkit

| Tool | Formula |
|---|---|
| Position size | `units = account × risk% ÷ |entry − stop|` (capped at what the account can buy) |
| Reward-to-risk | `|target − entry| ÷ |entry − stop|` |
| Break-even win rate | `1 ÷ (1 + reward-to-risk)` |
| Expectancy | `win% × average win − loss% × average loss` |
| Drawdown recovery | `drawdown ÷ (1 − drawdown)` (a 50% loss needs +100%) |
| Kelly criterion | `p − (1 − p) ÷ R`, shown as full, half and quarter Kelly |

## Documentation and sources

| Document | Contents |
|---|---|
| [docs/ROADMAP.md](docs/ROADMAP.md) | Drawn roadmap, phases, level overview, weekly routine, day-one rules, pro checklist |
| [docs/levels/level-0.md](docs/levels/level-0.md) … [level-7.md](docs/levels/level-7.md) | Study guide for each level: weekly plan, full lessons, practice, reading, milestones |
| [docs/SOURCES.md](docs/SOURCES.md) | Curated books, free courses (Investopedia, BabyPips, Yale/Coursera, CME, Norges Bank, Finanstilsynet), tools (TradingView, QuantConnect LEAN, Alpaca, Interactive Brokers) |
| [docs/GLOSSARY.md](docs/GLOSSARY.md) | 34 trading terms explained in one line each |
| [docs/templates](docs/templates) | Trading plan, weekly review and CSV trade journal templates |

Core reading list: Murphy, *Technical Analysis of the Financial Markets* · Tharp, *Trade Your Way to Financial Freedom* · Douglas, *Trading in the Zone* · Kahneman, *Thinking, Fast and Slow* · Elder, *The New Trading for a Living* · Chan, *Quantitative Trading* · Schwager, *Market Wizards*.

## Project structure

```
TradingAcademy/
├── TradingAcademy.sln
├── src/TradingAcademy.Web/        modern responsive web app (Blazor Server)
│   ├── Components/Layout/         app shell: sidebar, mobile menu, theme toggle
│   ├── Components/Pages/          home, roadmap, lessons, quiz, simulator, tools, journal, progress
│   ├── Components/Shared/         icons, SVG candlestick chart, helpers
│   ├── Services/                  curriculum, per-visitor progress (localStorage), simulator session
│   └── wwwroot/app.css            the design system (tokens, dark/light themes, responsive layout)
├── src/TradingAcademy/            console app
│   ├── Program.cs                 entry point
│   ├── App.cs                     main menu and navigation
│   └── UI/                        console screens (roadmap, lessons, quiz, simulator, tools, journal)
├── src/TradingAcademy.Core/       shared logic used by both apps and the tests
│   ├── Content/levels/            the curriculum: one JSON file per level (lessons + quiz), embedded in the assembly
│   ├── Learning/
│   │   ├── Curriculum.cs          level/lesson/quiz models and JSON loader
│   │   ├── QuizEngine.cs          scoring, pass mark, shuffling
│   │   └── ProgressStore.cs       saved progress and journal
│   ├── Trading/
│   │   ├── Candle.cs              OHLCV bar
│   │   ├── MarketSimulator.cs     regime-switching GBM price generator
│   │   ├── Indicators.cs          SMA, EMA, RSI, MACD, ATR
│   │   ├── PaperAccount.cs        orders, fills, stops, P/L and session statistics
│   │   ├── RiskCalculator.cs      position sizing, expectancy, Kelly, drawdown maths
│   │   ├── SessionCoach.cs        coaching notes for the session report
│   │   └── AsciiChart.cs          candlestick chart for the terminal
├── tests/TradingAcademy.Tests/    xUnit tests for the maths, simulator, account and content
├── docs/                          roadmap, study guides, sources, glossary, templates
└── .github/workflows/build.yml    CI: build and test on every push
```

## How it works (for developers)

- **No external dependencies**: only .NET and ASP.NET Core. Tests use xUnit.
- **One core, two front ends.** All trading maths, the simulator and the curriculum live in `TradingAcademy.Core`. The web and console apps only handle presentation.
- **Content is data.** Lessons and quizzes live in `src/TradingAcademy.Core/Content/levels/level-N.json`, so you can add or fix material without touching code. The loader validates every quiz answer index on startup.
- **The trading engine is plain, testable C#.** `PaperAccount` uses `decimal` for money and keeps a signed position (positive = long, negative = short), a volume-weighted average price, realised trades and an equity curve. `MarketSimulator` is deterministic for a given seed.
- **Indicators** are implemented from their textbook definitions (Wilder smoothing for RSI and ATR), and are a good way to learn how they really work. Level 7 asks you to build an automated strategy on top of them.

## Extending the course

- **Add a lesson:** add an object to the `lessons` array of a level JSON file (`id`, `title`, `body`, `keyTakeaways`, `exercise`, `readMore`).
- **Add a quiz question:** add to `quiz` with `prompt`, `options`, the zero-based `answer`, and an `explanation`.
- **Add a level:** create `level-8.json`; it appears automatically on the roadmap and menus.
- **Automate a strategy:** create a class that reads `MarketSimulator.Candles`, calls `Indicators`, and places orders on `PaperAccount`. Run it over many seeds and compare `SessionStats`.
- **Translate:** the content files are plain text, so a Norwegian (`nb`) version is just a copy of the JSON files.

## Running the tests

```bash
dotnet test
```

38 tests cover position sizing, expectancy, Kelly and drawdown maths, every indicator, long/short P/L, commissions, stop-loss and take-profit fills (including gaps), position flipping, session statistics, simulator determinism and candle consistency, curriculum integrity, quiz unlocking and saving progress. GitHub Actions runs them on every push.

## Disclaimer

This project is for **education only** and is **not financial, investment or tax advice**. The simulator uses synthetic prices; results there say nothing about how you would do in real markets. Past performance does not guarantee future results. Check the rules that apply to you with your local regulator (in Norway: [Finanstilsynet](https://www.finanstilsynet.no)) and tax authority ([Skatteetaten](https://www.skatteetaten.no)).

## License

[MIT](LICENSE) © 2026 Adam Elmasry
