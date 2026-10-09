# Level 7: Professional Trading

**Rank:** Pro  ·  **Time:** 6-12 months and beyond  ·  **Pass mark:** 70% on the in-app quiz

> **Goal:** Operate like a professional: automate with code, understand derivatives and execution, manage a portfolio of strategies and run trading as a business.

[← Back to the roadmap](../ROADMAP.md)

## How to study this level

| Week | What to do |
|---|---|
| 1 | Month 1: Lesson 7.1. Learn QuantConnect LEAN in C#; code your Level 6 strategy. |
| 2 | Month 2: Automate a strategy against this app's MarketSimulator on 100 seeds. |
| 3 | Month 3: Lesson 7.2. Options theory; paper trade defined-risk spreads. |
| 4 | Month 4: Lesson 7.3. Add a second, uncorrelated strategy; set a total risk budget. |
| 5 | Month 5: Small live trading with strict limits. |
| 6 | Month 6+: Lesson 7.4. Write the business plan; scale only on 6+ months of journaled results. |

In the app: **Learn → Level 7** to read the lessons, **Quiz → Level 7** to test yourself.

## Lessons

### 7.1 Algorithmic trading with C#

Algorithmic trading means a program executes your rules. Benefits: no emotions, perfect consistency, fast backtesting and the ability to watch many markets. Risks: bugs, data errors and over-optimisation.

C# is a first-class language for this. QuantConnect's open-source LEAN engine is written in C# and lets you backtest and live trade stocks, futures, forex, options and crypto with many brokers. NinjaTrader (NinjaScript) and cTrader (cAlgo) also use C#.

This app's code is your starting point: Trading/Indicators.cs shows how SMA, EMA, RSI, MACD and ATR are calculated; Trading/PaperAccount.cs shows how fills, stops, commissions and P/L work; Trading/MarketSimulator.cs shows how synthetic prices are generated. Try writing an automated strategy that trades the simulator.

Production essentials: logging every decision, kill switches, maximum position and loss limits in code, reconciliation of your positions with the broker, and testing with paper accounts before live money.

**Key takeaways**

- Algorithms remove emotion but add technology risk.
- QuantConnect LEAN, NinjaTrader and cTrader all use C#.
- Always build kill switches and hard limits into code.

**Practice:** Fork this repository and write a class that trades MarketSimulator automatically using the Level 6 trend strategy. Run it on 100 seeds and report average return and drawdown.

**Read more:** QuantConnect LEAN on GitHub (github.com/QuantConnect/Lean); Ernest P. Chan, 'Algorithmic Trading: Winning Strategies and Their Rationale'; Stefan Jansen, 'Machine Learning for Algorithmic Trading'

### 7.2 Options and derivatives

A call option gives the right to buy at the strike price before expiry; a put gives the right to sell. Buyers pay a premium and can lose at most that premium. Sellers collect the premium but can face large losses.

Option prices depend on the underlying price, strike, time to expiry, interest rates and implied volatility. The Black-Scholes model is the classic pricing model.

The greeks measure sensitivity: delta (to price), gamma (how delta changes), theta (time decay), vega (to volatility). Professionals manage these exposures rather than just direction.

Defined-risk strategies such as vertical spreads, covered calls and protective puts are the sensible place to start. Never sell naked options until you fully understand the worst case.

**Key takeaways**

- Calls = right to buy, puts = right to sell.
- Know your greeks: delta, gamma, theta, vega.
- Start with defined-risk strategies.

**Practice:** Paper trade a bull call spread. Write down the max profit, max loss and break-even before entering.

**Read more:** Lawrence G. McMillan, 'Options as a Strategic Investment'; Sheldon Natenberg, 'Option Volatility and Pricing'; The Options Industry Council (optionseducation.org)

### 7.3 Portfolio of strategies and execution

Professionals rarely depend on one strategy. Combining uncorrelated strategies (for example trend following and mean reversion, on different markets) smooths the equity curve and reduces drawdowns.

Set a total risk budget: for example, maximum 5% of capital at risk across all open positions, and allocate risk by volatility so each strategy contributes similarly.

Market microstructure is how orders actually meet: the order book, liquidity, market makers and price impact. Large orders move price, so professionals split them (VWAP, TWAP) and prefer limit orders when possible.

Measure execution quality: compare your fill prices with the price when you decided to trade. Slippage is a cost you can reduce.

**Key takeaways**

- Diversify across uncorrelated strategies.
- Use a total risk budget.
- Execution quality is part of your edge.

**Practice:** Run two different strategies in the simulator on the same seed. Would combining them have reduced drawdown?

**Read more:** Larry Harris, 'Trading and Exchanges: Market Microstructure for Practitioners'; Robert Carver, 'Leveraged Trading'

### 7.4 Trading as a business

Treat trading like a business with a plan, a budget, records and performance reviews. Track costs: data, software, platform fees, taxes and your time.

Be realistic about returns. Very good professional traders and funds often target 10-30% per year with controlled drawdowns. Promises of 10% per month are marketing or fraud.

Routes to trading professionally include working at a bank or fund, joining a proprietary trading firm, or trading your own capital. Read the rules and costs of funded-account 'challenges' carefully; many are designed so most participants fail.

Know the regulation: market abuse rules (insider trading, manipulation) apply to everyone. Keep tax records of every trade. In Norway, check Finanstilsynet and Skatteetaten guidance.

**Key takeaways**

- Run it like a business with records and reviews.
- Realistic returns are measured per year, not per day.
- Know the regulations and tax rules.

**Practice:** Write a one-page business plan: capital, monthly costs, target return, max drawdown, and what you will do if you hit it.

**Read more:** Jack D. Schwager, 'Market Wizards' series; Edwin Lefèvre, 'Reminiscences of a Stock Operator'; Finanstilsynet: rules on market abuse (finanstilsynet.no)

## Milestones (tick all before moving on)

- [ ] Code one strategy in C# on QuantConnect LEAN and run a backtest with costs.
- [ ] Paper trade an options strategy and explain each greek in your position.
- [ ] Run at least two uncorrelated strategies with a combined risk budget.
- [ ] Have 6+ months of journaled, plan-following results before scaling capital.
- [ ] Passed the Level 7 quiz in the app (70% or higher)

---

[You made it: read the Pro checklist in the roadmap →](../ROADMAP.md#pro-checklist)
