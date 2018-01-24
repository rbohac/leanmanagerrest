using QuantConnect.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using QuantConnect.Securities;
using QuantConnect.Securities.Option;


namespace QuantConnect.Algorithm.CSharp
{
    public class Position
    {

        public string Ticker;

        public Symbol Symbol;

        public Option Security;

        public decimal UnderlyingPrice;

        public decimal OptionPrice;

        public decimal Return
        {
            get
            {
                return 0;
            }
        }
    }

    public class GrapeAlgorithm : QCAlgorithm
    {
        public Dictionary<string, Position> Manager = new Dictionary<string, Position>();
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

                var specificContracts = (from contract in contracts
                                         where contract.ID.OptionStyle == OptionStyle.American &&
                                                 contract.ID.OptionRight == OptionRight.Call &&
                                                 contract.ID.StrikePrice > underlyingPrice &&
                                                 contract.ID.Date > Time.AddDays(15) &&
                                                 contract.ID.Date < Time.AddDays(60) &&
                                                 contract.ID.Date == new DateTime(2017,10,20)
                                         select contract).OrderBy(c => c.ID.StrikePrice).FirstOrDefault();

                if (specificContracts != null)
                {
                    var optionContract = AddOptionContract(specificContracts, Resolution.Minute);
                    Manager[underlying] = new Position()
                    {
                        Ticker = underlying,
                        Security = optionContract,
                        OptionPrice = optionContract.Close,
                        Symbol = optionContract.Symbol,
                        UnderlyingPrice = underlyingPrice
                    };
                }
            }
        }

        public override void OnData(Slice data)
        {
            // Do nothing.
        }
    }
}