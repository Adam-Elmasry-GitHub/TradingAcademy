# Level 2: Technical Analysis

**Rank:** Analyst  ·  **Time:** 4-6 weeks  ·  **Pass mark:** 70% on the in-app quiz

> **Goal:** Use indicators correctly: moving averages, RSI, MACD, ATR and chart patterns, and understand their limits.

[← Back to the roadmap](../ROADMAP.md)

## How to study this level

| Week | What to do |
|---|---|
| 1 | Lesson 2.1. Add SMA 20/50/200 to your charts. Trade the simulator with an SMA20 filter. |
| 2 | Lesson 2.2. Study RSI behaviour in trends vs ranges. Find 5 divergences. |
| 3 | Lesson 2.3. Use ATR to place every simulator stop. |
| 4 | Lesson 2.4. Write one rule-based setup with max two indicators. |
| 5 | Trade the setup in 5 simulator sessions; log every trade. |
| 6 | Review journal, adjust setup, take the quiz. |

In the app: **Learn → Level 2** to read the lessons, **Quiz → Level 2** to test yourself.

## Lessons

### 2.1 Moving averages

A simple moving average (SMA) is the average closing price of the last N bars. An exponential moving average (EMA) gives more weight to recent prices, so it reacts faster. Common periods are 20 (short-term), 50 (medium) and 200 (long-term).

Moving averages smooth noise and show trend direction: price above a rising 50 SMA is a simple definition of an uptrend. The 20 SMA is often used as a pullback zone in strong trends.

A crossover happens when a faster average crosses a slower one. The 50/200 'golden cross' and 'death cross' are famous, but crossovers lag and give many false signals in sideways markets.

All moving averages lag price, because they are built from past data. Use them as a filter (only take longs above the 200 SMA) rather than as precise entry triggers.

**Key takeaways**

- SMA = equal weights, EMA = more weight on recent prices.
- Moving averages show direction and act as dynamic support/resistance.
- They lag, so they work best as filters.

**Practice:** In the simulator, the yellow dots are the SMA20. Only take long trades when price is above it and short trades when below. Compare results with a session where you ignore it.

**Read more:** Investopedia: 'Moving Average (MA): Purpose, Uses, Formula'; Perry Kaufman, 'Trading Systems and Methods'

### 2.2 RSI and momentum

The Relative Strength Index (RSI), created by J. Welles Wilder, compares the size of recent gains to recent losses on a scale from 0 to 100. The standard period is 14.

Above 70 is often called overbought and below 30 oversold. But in a strong uptrend RSI can stay above 70 for weeks. Selling only because RSI is high is a classic beginner mistake.

More robust uses: in an uptrend, buy pullbacks when RSI dips to 40-50 and turns up; watch for divergence, when price makes a new high but RSI makes a lower high, a sign that momentum is fading.

Momentum indicators measure speed, not direction. Combine them with trend and levels.

**Key takeaways**

- RSI ranges 0-100; 70/30 are common thresholds.
- Overbought can stay overbought in a strong trend.
- Divergence warns that momentum is fading.

**Practice:** Find one bearish divergence in the simulator (higher price high, lower RSI high). What happened next?

**Read more:** J. Welles Wilder, 'New Concepts in Technical Trading Systems'; Andrew Cardwell's RSI work (summaries on Investopedia)

### 2.3 MACD, ATR and volatility

MACD is the 12-period EMA minus the 26-period EMA. Its 9-period EMA is the signal line. MACD above the signal line suggests rising momentum; the histogram shows the gap between them.

Average True Range (ATR) measures how far price typically moves per bar, including gaps. It does not show direction. ATR is one of the most useful tools for risk: a stop placed 1.5 to 2 ATR away is less likely to be hit by normal noise.

Bollinger Bands put bands two standard deviations around a 20 SMA. Narrow bands (a squeeze) often come before a big move; touching a band is not on its own a signal to buy or sell.

Volatility changes over time. Professionals size positions smaller when volatility is high, so each trade risks the same amount of money.

**Key takeaways**

- MACD = fast EMA - slow EMA, with a signal line.
- ATR measures typical movement: use it for stops and sizing.
- Smaller size when volatility is high.

**Practice:** In the simulator, use the ATR14 shown under the chart. Place your stop 2 x ATR from entry and use the risk-based position size (1% risk).

**Read more:** Gerald Appel, 'Technical Analysis: Power Tools for Active Investors' (MACD); John Bollinger, 'Bollinger on Bollinger Bands'

### 2.4 Chart patterns and their limits

Classic continuation patterns include flags, pennants and triangles: short pauses in a trend before it continues. Reversal patterns include head and shoulders and double tops/bottoms.

Patterns are easy to see in hindsight and hard to see in real time. Research such as Thomas Bulkowski's pattern statistics shows that many patterns work only slightly better than chance, and failure rates are high.

The real value of a pattern is that it defines risk: a flag gives a clear entry (breakout), a clear stop (below the flag) and a measured target. That structure is what makes a trade plan.

Too many indicators create 'analysis paralysis' and contradictory signals. Pros usually use price, one trend filter and one volatility measure.

**Key takeaways**

- Patterns are probabilities, not promises.
- Their main value is a clear entry, stop and target.
- Fewer indicators, clearer decisions.

**Practice:** Write a one-sentence rule for a flag breakout trade: entry, stop, target and filter.

**Read more:** Thomas Bulkowski, 'Encyclopedia of Chart Patterns'; David Aronson, 'Evidence-Based Technical Analysis'

## Milestones (tick all before moving on)

- [ ] Explain how SMA, EMA, RSI, MACD and ATR are calculated (the code in Trading/Indicators.cs shows you).
- [ ] Define one simple rule-based setup using at most two indicators.
- [ ] Track that setup in 5 simulator sessions and log every trade in the journal.
- [ ] Passed the Level 2 quiz in the app (70% or higher)

---

[Level 3 →](level-3.md)
