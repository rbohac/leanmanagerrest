using QuantConnect.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using QuantConnect.Securities;
using QuantConnect.Securities.Option;


namespace QuantConnect.Algorithm.CSharp
{
    public class GrapeAlgorithm : QCAlgorithm
    {
        public Dictionary<string, CoveredCallPosition> Manager = new Dictionary<string, CoveredCallPosition>();
        string[] _underlyings = new[] { "IBM", "AAPL", "SPY" };

        public override void Initialize()
        {
            SetStartDate(2013, 10, 07);
            SetEndDate(2013, 10, 11);
            SetCash(100000);

            foreach (var underlying in _underlyings)
            {
                AddEquity(underlying, Resolution.Minute);
            }

            // Every 15 min scan.
            Schedule.On(DateRules.EveryDay("SPY"), TimeRules.Every(TimeSpan.FromMinutes(15)), Scan);
        }

        public void Scan()
        {
            foreach (var underlying in _underlyings)
            {
                var contracts = OptionChainProvider.GetOptionContractList(underlying, Time);

                var underlyingPrice = Portfolio[underlying].Price;

                var specificContract = (from contract in contracts
                                         where contract.ID.OptionStyle == OptionStyle.American &&
                                                 contract.ID.OptionRight == OptionRight.Call &&
                                                 contract.ID.StrikePrice > underlyingPrice &&
                                                 contract.ID.Date > Time.AddDays(15) &&
                                                 contract.ID.Date < Time.AddDays(60) &&
                                                 contract.ID.Date == new DateTime(2017,10,20)
                                         select contract).OrderBy(c => c.ID.StrikePrice).FirstOrDefault();

                if (specificContract != null)
                {
                    Option optionContract = null;
                    if ((Securities.ContainsKey(specificContract) && (Securities[specificContract].Invested)))
                    {
                        optionContract = (Option) Securities[specificContract];
                    }
                    else
                    {
                        optionContract = AddOptionContract(specificContract, Resolution.Minute);
                    }
                    Manager[underlying] = new CoveredCallPosition()
                    {
                        Ticker = underlying,
                        OptionSecurity = optionContract,
                        OptionPrice = optionContract.Close,
                        UnderlyingSymbol = optionContract.Symbol,
                        UnderlyingPrice = underlyingPrice
                    };


                    // Do Stuff
                    if (!optionContract.Invested)
                      RemoveSecurity(optionContract.Symbol);

                }
            }
        }

        public override void OnData(Slice data)
        {
            // Do nothing.
        }
    }
}