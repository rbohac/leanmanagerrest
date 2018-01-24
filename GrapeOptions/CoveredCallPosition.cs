using System;
using QuantConnect;
using QuantConnect.Securities.Option;

public class CoveredCallPosition
{
    public string Ticker;

    public Symbol UnderlyingSymbol;

    public Option OptionSecurity;

    public decimal UnderlyingPrice;

    public decimal OptionPrice;

    public decimal Return
    {
        get { return 0; }

    }
    public CoveredCallPosition()
    {

    
    }
}





