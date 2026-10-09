# Level 6: Strategy Development and Backtesting

**Rank:** Strategist  ·  **Time:** 6-8 weeks  ·  **Pass mark:** 70% on the in-app quiz

> **Goal:** Turn ideas into precise rules, test them honestly on historical data, and know how to tell a real edge from luck.

[← Back to the roadmap](../ROADMAP.md)

## How to study this level

| Week | What to do |
|---|---|
| 1 | Lesson 6.1. Write a complete strategy (filter, entry, stop, exit, sizing). |
| 2 | Lesson 6.2. Manual backtest: 30 trades. |
| 3 | Continue backtest to 100+ trades across bull, bear and sideways periods. |
| 4 | Lesson 6.3. Robustness checks: vary parameters, test another market, keep out-of-sample data. |
| 5 | Lesson 6.4. Start forward testing on paper. |
| 6 | Forward test continues. |
| 7 | Compare forward test with backtest. |
| 8 | Decide: refine, drop, or go small live. Quiz. |

In the app: **Learn → Level 6** to read the lessons, **Quiz → Level 6** to test yourself.

## Lessons

### 6.1 Anatomy of a strategy

A complete strategy has: a market and timeframe; a setup filter (when the strategy is allowed to trade, e.g. only above the 200 SMA); an entry trigger; an initial stop; exit rules (target, trailing stop or time exit); and position sizing.

Every strategy should have a reason it might work. Common edges come from trend persistence (momentum), mean reversion (prices returning to an average after extreme moves), volatility expansion after contraction, or behavioural patterns around events.

Simple strategies tend to be more robust than complex ones. A system with three rules that works across many markets is more trustworthy than one with fifteen rules tuned to one stock.

Example trend strategy: on daily charts, if the close is above the 50 SMA and makes a new 20-day high, buy; initial stop 2 ATR below entry; exit on a close below the 20 SMA; risk 1% per trade.

**Key takeaways**

- Strategy = filter + entry + stop + exit + sizing.
- Know why the edge should exist.
- Simple and robust beats complex and fragile.

**Practice:** Write the example trend strategy above into your trading plan and trade it in the simulator for 3 sessions.

**Read more:** Andreas Clenow, 'Following the Trend'; Robert Carver, 'Systematic Trading'

### 6.2 Backtesting properly

A backtest applies your exact rules to historical data to see how they would have performed. It can be manual (scrolling a chart bar by bar) or automated in code.

Include realistic costs: commission, spread and slippage. Many strategies that look great without costs lose money with them, especially on short timeframes.

You need enough trades for the result to mean something: at least 100 is a common rule of thumb, across different market conditions (bull, bear, sideways).

Key output metrics: total return, max drawdown, win rate, average win/loss, profit factor, expectancy, number of trades and the Sharpe ratio (return divided by volatility).

**Key takeaways**

- Test exact rules, include all costs.
- 100+ trades across different regimes.
- Look at drawdown, not only return.

**Practice:** Manually backtest your strategy on one stock: scroll the daily chart from 3 years ago and record 30 trades in a spreadsheet.

**Read more:** Ernest P. Chan, 'Quantitative Trading'; QuantConnect documentation and Bootcamp (quantconnect.com)

### 6.3 Overfitting and other traps

Overfitting (curve fitting) means tuning parameters until a strategy fits past noise. It looks perfect on history and fails live. Warning signs: many parameters, results that collapse if a parameter changes slightly, and very few trades.

Look-ahead bias means using information that would not have been available at the time, for example using today's close to decide to buy at today's open.

Survivorship bias means testing only on companies that still exist today, ignoring those that went bankrupt or were delisted. It makes results look better than reality.

Defences: keep an out-of-sample period you never touch while developing; use walk-forward testing; test on several markets; prefer parameters in a stable 'plateau' rather than one perfect value.

**Key takeaways**

- If small parameter changes destroy results, it is overfit.
- Never use future information in a test.
- Always keep untouched out-of-sample data.

**Practice:** Take any indicator setting you like (e.g. SMA 20). Test 15, 20 and 25. If results change drastically, your edge is fragile.

**Read more:** Marcos López de Prado, 'Advances in Financial Machine Learning'; David Aronson, 'Evidence-Based Technical Analysis'

### 6.4 From paper to live

After a backtest, forward test (paper trade) the strategy in real time. This tests whether you can execute it and whether the edge still exists in current markets.

Compare forward results with the backtest. Similar win rate, average R and drawdown are a good sign. A big gap means something is wrong: costs, execution or overfitting.

Go live with small size, for example a quarter of your planned risk. The goal is to experience real emotions with real money while mistakes are cheap. Increase size step by step only after a set number of trades that match expectations.

Define in advance when you will stop trading a strategy: for example if drawdown exceeds 1.5 times the worst backtest drawdown.

**Key takeaways**

- Backtest, then paper trade, then small live, then scale.
- Compare live results to the backtest.
- Pre-define when to switch a strategy off.

**Practice:** Write your scaling plan: how many trades at each size before increasing, and your 'switch off' drawdown.

**Read more:** Alpaca paper trading API docs (alpaca.markets); Interactive Brokers paper trading account (interactivebrokers.com)

## Milestones (tick all before moving on)

- [ ] Write one complete strategy with entry, exit, stop, sizing and filter rules.
- [ ] Backtest it on at least 100 trades of history (manually or in code).
- [ ] Forward test it on paper for at least 1-2 months and compare with the backtest.
- [ ] Passed the Level 6 quiz in the app (70% or higher)

---

[Level 7 →](level-7.md)
