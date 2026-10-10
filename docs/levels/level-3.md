# Level 3: Risk Management

**Rank:** Risk Manager  ·  **Time:** 3-4 weeks  ·  **Pass mark:** 70% on the in-app quiz

> **Goal:** Size every trade so that no single loss, or losing streak, can knock you out of the game.

[← Back to the roadmap](../ROADMAP.md)

## How to study this level

| Week | What to do |
|---|---|
| 1 | Lesson 3.1. Study the drawdown table. Write your max loss per day/week. |
| 2 | Lesson 3.2. Use the position size calculator before every trade. |
| 3 | Lesson 3.3. Calculate expectancy of your journal trades in R. |
| 4 | Lesson 3.4. 5 simulator sessions with max drawdown under 10%. Quiz. |

In the app: **Learn → Level 3** to read the lessons, **Quiz → Level 3** to test yourself.

## Lessons

### 3.1 Why risk comes first

Losses and gains are not symmetrical. A 10% loss needs an 11% gain to recover, a 50% loss needs 100%, and a 90% loss needs 900%. Open the Drawdown recovery table in the Trader's toolkit to see this.

Even a good strategy has losing streaks. With a 50% win rate, 8 losses in a row will happen sooner or later over a few hundred trades. If you risk 10% per trade, that streak costs over 55% of your account. At 1% per trade it costs about 8%.

This is why professional traders talk about risk before they talk about returns. Your job is to stay in the game long enough for your edge to play out.

Professional firms enforce hard limits: maximum loss per trade, per day and per month. When a limit is hit, trading stops. Set your own limits and treat them as unbreakable.

**Key takeaways**

- Big losses need much bigger gains to recover.
- Losing streaks are normal; plan for them.
- Hard daily and weekly loss limits protect you from yourself.

**Practice:** Calculate what 10 losses in a row would cost you at 1%, 2% and 5% risk per trade.

**Read more:** Van K. Tharp, 'Trade Your Way to Financial Freedom' (position sizing chapters); Ralph Vince, 'The Mathematics of Money Management'

### 3.2 Stop-losses and position sizing

First decide where your trade idea is wrong: that is your stop. Then calculate the size. Never pick the size first and squeeze the stop to fit.

Fixed-fractional sizing: units = (account x risk%) / |entry - stop|. With 10,000, 1% risk, entry 100 and stop 95, you risk 100, and each unit risks 5, so you buy 20 units. This formula is implemented in RiskCalculator.PositionSize.

Good stop locations are beyond a support/resistance level, beyond a swing high/low, or a multiple of ATR from entry. Bad stop locations are round numbers everyone uses, or a fixed amount that ignores volatility.

Moving a stop further away to avoid a loss is one of the most destructive habits in trading. You may move a stop only in the direction that reduces risk (a trailing stop).

**Key takeaways**

- Stop first, size second.
- Units = account x risk% / distance to stop.
- Never widen a stop once in a trade.

**Practice:** Use the simulator's risk-based position size (1% risk) before every trade in a full session.

**Read more:** Investopedia: 'Position Sizing in Trading'; Curtis Faith, 'Way of the Turtle' (volatility-based sizing)

### 3.3 R-multiples and expectancy

R is the amount you risk on a trade. A trade that makes twice the risk is +2R; one that hits the stop is -1R. Measuring results in R lets you compare trades of different sizes and markets.

Expectancy = (win rate x average win) - (loss rate x average loss). A strategy that wins only 40% of the time but averages +2.5R winners and -1R losers has expectancy of 0.4 x 2.5 - 0.6 x 1 = +0.4R per trade. That is a good strategy.

Break-even win rate = 1 / (1 + reward-to-risk). At 1:1 you need over 50%; at 2:1 you need over 33%; at 3:1 over 25%. The Reward-to-risk tool in the toolkit calculates this.

Win rate alone means nothing. Many beginners love high win rates and then lose everything on one huge loss. Track the whole distribution.

**Key takeaways**

- Think in R: risk = 1R.
- Expectancy decides if a strategy is worth trading.
- High win rate with big losses is a trap.

**Practice:** Enter your last 10 simulator trades into the Expectancy tool. Is your expectancy positive?

**Read more:** Van K. Tharp, 'Definitive Guide to Position Sizing'; Investopedia: 'Expectancy'

### 3.4 Leverage, correlation and the Kelly criterion

Leverage does not change your edge; it only changes the size of your bets. Used with proper position sizing, it can be managed. Used to 'make more money faster', it is the most common way traders blow up.

Correlation: five long positions in five Norwegian oil stocks are almost one big oil bet. If oil falls, all five lose together. Count correlated positions as one when you add up risk.

The Kelly criterion gives the bet size that maximises long-run growth: f = p - (1 - p) / R, where p is the win rate and R the reward-to-risk ratio. Because your real win rate is uncertain, professionals use a quarter or half of Kelly, or simply cap risk at 0.5-2%.

Keep a cash buffer, avoid holding large positions through major news if your plan does not include it, and know your total open risk at all times.

**Key takeaways**

- Leverage magnifies mistakes as much as wins.
- Correlated positions are one risk, not many.
- Use a fraction of Kelly, never full Kelly.

**Practice:** Use the Kelly tool with a 45% win rate and 2:1 reward-to-risk. Compare full, half and quarter Kelly.

**Read more:** William Poundstone, 'Fortune's Formula' (history of Kelly); Edward O. Thorp, 'A Man for All Markets'

## Milestones (tick all before moving on)

- [ ] Use the position size calculator before every simulator trade for 10 sessions.
- [ ] Finish 5 simulator sessions in a row with max drawdown under 10%.
- [ ] Write your personal risk rules: max risk per trade, per day and per week.
- [ ] Passed the Level 3 quiz in the app (70% or higher)

---

[Level 4 →](level-4.md)
