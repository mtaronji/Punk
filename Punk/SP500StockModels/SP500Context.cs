using System.Globalization;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Punk.Exceptions;
using MathNet.Numerics.LinearAlgebra;


namespace Punk.SP500StockModels;
public partial class SP500Context : DbContext
{
    public SP500Context()
    {
    }

    public SP500Context(DbContextOptions<SP500Context> options)
        : base(options)
    {
    }

    public virtual DbSet<FinancialDate> FinancialDates { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source= DataContexts/SP500.db;");  //debugging connnection string

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialDate>(entity =>
        {
            entity.HasKey(e => e.Date);

            entity.ToTable("FinancialDate");
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Price");

            entity.HasIndex(e => e.Date, "IX_Price_Date");

            entity.HasIndex(e => new { e.Ticker, e.Date }, "IX_Price_Ticker_Date").IsUnique();

            entity.HasOne(d => d.DateNavigation).WithMany().HasForeignKey(d => d.Date);

            entity.HasOne(d => d.TickerNavigation).WithMany().HasForeignKey(d => d.Ticker);
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Ticker);

            entity.ToTable("Stock");
        });

        OnModelCreatingPartial(modelBuilder);
    }
    public async Task<Matrix<double>> GetPricesMatrix(string Ticker, string? start = null, string? end = null)
    {
        DateOnly startdate, enddate;
        if (start != null && end != null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        else if (start != null && end == null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.MaxValue;
        }
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }

        var pricesmatrix = await this.Prices.Where(x => x.Ticker == Ticker && x.Date <= enddate && x.Date >= startdate).Select(x => x).ToMatrixAsync();
        return pricesmatrix;
    }
    public async Task<IEnumerable<object>> GetPrices(string Ticker, string? start = null, string? end = null)
    {
        DateOnly startdate, enddate;
        if (start != null && end != null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        else if(start != null && end == null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.MaxValue;
        }
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }

        var prices = await this.Prices.Where(x => x.Ticker == Ticker && x.Date <= enddate && x.Date >= startdate).Select(x => (object)x).ToListAsync();
        return prices;
    }

    public async Task<IEnumerable<object>> Query(Expression<Func<Price,bool>> query)
    {
        var prices = await this.Prices.Where(query).Select(x => (object)x).ToListAsync();
        return prices;
    }
    public async Task<IEnumerable<object>> Join(string Ticker1, string Ticker2, string? start = null, string? end = null)
    {
   
        DateOnly startdate, enddate;
        if (start != null && end != null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        else if (start != null && end == null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.MaxValue;
        }
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }
        var matchesTicker1 = this.Prices.Where(x => x.Ticker == Ticker1 && x.Date <= enddate && x.Date > startdate);
        var matchesTicker2 = this.Prices.Where(x => x.Ticker == Ticker2 && x.Date <= enddate && x.Date > startdate);

        var prices = await matchesTicker1.Join(matchesTicker2, arg1 => arg1.Date, arg2 => arg2.Date, (Ticker1, Ticker2) => new { ticker1 = Ticker1, ticker2 = Ticker2 })
            .Select(x =>   
            new
            {
                xdate = x.ticker1.Date,
                xclose = x.ticker1.Close,
                xadjclose = x.ticker1.AdjClose,
                xopen = x.ticker1.Open,
                xhigh = x.ticker1.High,
                xlow = x.ticker1.Low,
                ydate = x.ticker2.Date,
                yclose = x.ticker2.Close,
                yadjclose = x.ticker2.AdjClose,
                yopen = x.ticker2.Open,
                yhigh = x.ticker2.High,
                ylow = x.ticker2.Low
            }
        ).ToListAsync<object>();
        return prices;
    }

    public async Task<IEnumerable<object>>  SMA(uint duration, string ticker, string? start = null, string? end = null)
    {
        Queue<Price> q = new Queue<Price>();
        List<Price> smas = new List<Price>();
        DateOnly startdate, enddate;
        if (start != null && end != null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        else if (start != null && end == null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.MaxValue;
        }
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }

        var prices = await this.Prices.Where(x => x.Ticker == ticker).OrderBy(x => x.Date).ToListAsync();    

        if(prices != null)
        {
            foreach(var pr in prices)
            {
                q.Enqueue(pr);
                if(q.Count == duration)
                {
                    var avgclose = q.Average(x => x.Close);
                    var avgadjclose = q.Average(x => x.AdjClose);
                    var avgopen = q.Average(x => x.Open);
                    var avglow = q.Average(x => x.Low);
                    var avghigh = q.Average(x => x.High);
                    var date = pr.Date;
                    smas.Add(new Price{Date = date, Open = avgopen, Low = avglow, High = avghigh, Close = avgclose, AdjClose = avgadjclose});
                    q.Dequeue();
                }            
            }
        }
        else
        {
            throw new PunkQueryException($"Could not find price Data for ticker {ticker}");
        }
        return smas.Where(x => x.Date >= startdate && x.Date <= enddate).ToList<object>();

    }

    public async Task<IEnumerable<object>> EMA(uint duration, string ticker, string? start = null, string? end = null)
    {
        var k = 2.0 / (duration + 1.0);
        Queue<Price> q = new Queue<Price>();
        List<Price> emas = new List<Price>();
        DateOnly startdate, enddate;
        if (start != null && end != null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        else if (start != null && end == null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.MaxValue;
        }
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }

        var prices = await this.Prices.Where(x => x.Ticker == ticker).OrderBy(x => x.Date).Select(x => x).ToListAsync();
        if (prices != null)
        {
            double ema_close = 0.0, ema_open = 0.0, ema_low = 0.0, ema_high = 0.0, ema_volume = 0.0, ema_adjclose = 0.0;

            foreach (var pr in prices)
            {
                q.Enqueue(pr);
                if (q.Count == duration)
                {
                    ema_close = q.Average(x => x.Close);
                    ema_open = q.Average(x => x.Open);
                    ema_low = q.Average(x => x.Open);
                    ema_high = q.Average(x => x.Open);
                    ema_volume = q.Average(x => x.Volume);
                    ema_adjclose = q.Average(x => x.AdjClose);
                    var date = pr.Date;
                    emas.Add(new Price { Date = date, Open = ema_open, Low = ema_low, High = ema_high, AdjClose = ema_adjclose, Close = ema_close, Volume = (int)Math.Round(ema_volume,0) });
                }
                else if(q.Count > duration)
                {
                    ema_close = pr.Close * k + ema_close * (1 - k);
                    ema_adjclose = pr.AdjClose * k + ema_adjclose * (1 - k);
                    ema_open = pr.Open * k + ema_open * (1 - k);
                    ema_low = pr.Low * k + ema_low * (1 - k);
                    ema_high = pr.High * k + ema_high * (1 - k);
                    var date = pr.Date;
                    ema_volume = pr.Volume * k + ema_volume * (1 - k);
                    emas.Add(new Price { Date = date, Open = ema_open, Low = ema_low, High = ema_high, AdjClose = ema_adjclose, Close = ema_close, Volume = (int)Math.Round(ema_volume, 0) });
                }
                else
                {
                    //beginning
                }
            }
        }
        else
        {
            throw new PunkQueryException($"Could not find price Data for ticker {ticker}");
        }
        return emas.Where(x => x.Date >= startdate && x.Date <= enddate).ToList<object>();

    }

    //lag pulls data from the past into a future date
    public async Task<IEnumerable<object>> Lead(string ticker, int lookforward, string? start = null, string? end = null)
    {
        List<object> data = new();
        DateOnly startdate, enddate;
        if (start != null && end != null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        else if (start != null && end == null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.MaxValue;
        }
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }
        var prices = await this.Prices.Where(x => x.Ticker == ticker && x.Date <= enddate && x.Date >= startdate).OrderBy(x => x.Date).OrderBy(x => x.Date).Select(x => x).ToListAsync();

        int maxIndex = prices.Count - 1;
        for(int i = 0; i < prices.Count; i++)
        {
            if(i + lookforward < maxIndex)
            {
                data.Add(
                    new
                    {
                        leaddate = prices[i+ lookforward].Date, 
                        leadclose = prices[i + lookforward].Close,
                        leadopen = prices[i + lookforward].Open,
                        leadlow = prices[i + lookforward].Low,
                        leadhigh = prices[i + lookforward].High,
                        leadvolume = prices[i + lookforward].Volume,
                        date = prices[i].Date,
                        close = prices[i].Close,
                        open = prices[i].Open,
                        low = prices[i ].Low,
                        high = prices[i].High,
                        volume = prices[i].Volume
                    }        
               );
            }
            else
            {
                break;
            }
        }
        return data;
    }

    public async Task<IEnumerable<object>> Lag(string ticker, int lookback, string? start = null, string? end = null)
    {
        List<object> data = new();
        DateOnly startdate, enddate;
        if (start != null && end != null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        else if (start != null && end == null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.MaxValue;
        }
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }
        var prices = await this.Prices.Where(x => x.Ticker == ticker && x.Date <= enddate && x.Date >= startdate).OrderBy(x => x.Date).OrderBy(x => x.Date).Select(x => x).ToListAsync();

        int maxIndex = prices.Count - 1;
        for (int i = 0; i < prices.Count; i++)
        {
            if (i + lookback < maxIndex)
            {
                data.Add(
                    new
                    {
                        date = prices[i + lookback].Date,
                        close = prices[i + lookback].Close,
                        open = prices[i + lookback].Open,
                        low = prices[i + lookback].Low,
                        high = prices[i + lookback].High,
                        volume = prices[i + lookback].Volume,
                        lagdate = prices[i].Date,
                        lagclose = prices[i].Close,
                        lagopen = prices[i].Open,
                        laglow = prices[i].Low,
                        laghigh = prices[i].High,
                        lagvolume = prices[i].Volume
                    }
               );
            }
            else
            {
                break;
            }
        }
        return data;
    }

    public async Task<IEnumerable<object>> DailyGains(string Ticker, string? start = null, string? end = null)
    {
        DateOnly startdate, enddate;
        if (start != null && end != null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        else if (start != null && end == null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.MaxValue;
        }
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }

        var prices = await this.Prices.Where(x => x.Ticker == Ticker && x.Date <= enddate && x.Date >= startdate).OrderBy(x => x.Date).Select(x => x).ToListAsync();
        List<object> Gains = new List<object>();
        for (int i = 1; i < prices.Count; i++ )
        {
            var gain = prices[i].AdjClose / prices[i - 1].AdjClose - 1;
            Gains.Add(new { Date = prices[i].Date, Gain = gain});
        }
        return Gains;
    }

    public async Task<IEnumerable<object>> SectorDailyGains()
    {
        List<string> Sectors = new List<string>()
        {
            "XLE","XLF","XLU","XLI","GDX", "XLK","XLV","XLY","XLP","XLB","XOP", "IYR",
            "XHB","ITB","VNQ","GDXJ","IYE","OIH","XME","XRT","SMH","IBB","KBE","KRE",
            "XTL"
        };

        List<object> SectorGains = new();
        foreach( var sector in Sectors )
        {
            var sectorgains = await this.Prices.Where(t => t.Ticker == sector).OrderByDescending(x => x.Date).Take(2).Select(x => x).ToListAsync();
            SectorGains.Add(new {SectorETF = sector, Date = sectorgains[0].Date, Gain = sectorgains[0].AdjClose / sectorgains[1].AdjClose - 1 });
        }

        return SectorGains;
     }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

public static partial class IQueryableExtensions
{
    public static async Task<Matrix<double>> ToMatrixAsync(
        this IQueryable<Price> source, 
        CancellationToken cancellationToken = default)
    {

        List<Vector<double>> rows = new();
        await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            Vector<double> rowvector = Vector<double>.Build.DenseOfArray(new double[] { element.Open, element.Low, element.High, element.AdjClose, element.Volume });
            rows.Add(rowvector);
        }
        return Matrix<double>.Build.DenseOfRowVectors(rows);

    }
}
