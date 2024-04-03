
using System;
using System.Collections.Generic;


namespace Punk.SP500StockModels
{
    public partial class Price
    {
        public string Ticker { get; set; } = null!;

        public DateOnly Date { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public double AdjClose { get; set; }

        public double Low { get; set; }

        public double High { get; set; }

        public int Volume { get; set; }

        public virtual FinancialDate DateNavigation { get; set; } = null!;

        public virtual Stock TickerNavigation { get; set; } = null!;
    }
}

