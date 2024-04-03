using System;
using System.Collections.Generic;

namespace Punk.SP500OptionModels;

public partial class Price
{
    public int Duration { get; set; }

    public DateOnly? MaturityDate { get; set; }

    public double Open { get; set; }

    public double? Close { get; set; }

    public double? Adjclose { get; set; }

    public double High { get; set; }

    public double Low { get; set; }

    public int Volume { get; set; }

    public double Vwap { get; set; }

    public string Code { get; set; } = null!;

    public virtual OptionCode CodeNavigation { get; set; } = null!;

    public virtual FinancialDate? MaturityDateNavigation { get; set; }
}
