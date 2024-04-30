using System;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Punk.Exceptions;
using Punk.SP500StockModels;

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
        => optionsBuilder.UseSqlite("Data Source= DataContexts/SP500.db;");

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

    public async Task<IEnumerable<Price>> GetPrices(string Ticker, string start = null, string? end = null)
    {
        List<Price> smas = new List<Price>();
        DateOnly startdate, enddate;
        if (start != null && end != null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }

        var prices = await this.Prices.Where(x => x.Ticker == Ticker && x.Date <= enddate && x.Date > startdate).Select(x => x).ToListAsync();
        return prices;
    }

    public async Task<IEnumerable<Price>> Query(Expression<Func<Price,bool>> query)
    {
        List<Price> prices = new List<Price>();
        prices = await this.Prices.Where(query).Select(x => x).ToListAsync();
        return prices;
    }
    public async Task<IEnumerable<object>> Join(string Ticker1, string Ticker2)
    {
        List<object> prices = new List<object>();
        var matchesTicker1 = this.Prices.Where(x => x.Ticker == Ticker1);
        var matchesTicker2 = this.Prices.Where(x => x.Ticker == Ticker2);

        prices = await matchesTicker1.Join(matchesTicker2, arg1 => arg1.Date, arg2 => arg2.Date, (Ticker1, Ticker2) => new { ticker1 = Ticker1, ticker2 = Ticker2 }

        ).Select(x =>   
            new
            {
                xclose = x.ticker1.Close,
                xopen = x.ticker1.Open,
                xhigh = x.ticker1.High,
                xlow = x.ticker1.Low,
                yclose = x.ticker2.Close,
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
        if(start != null && end != null)
        {
            startdate = DateOnly.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            enddate = DateOnly.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }
  
        var prices = await this.Prices.Where(x => x.Ticker == ticker && x.Date <= enddate).Select(x => x).ToListAsync();

        if(prices != null)
        {
            foreach(var pr in prices)
            {
                q.Enqueue(pr);
                if(q.Count == duration)
                {
                    var avgclose = q.Average(x => x.Close);
                    var avgopen = q.Average(x => x.Open);
                    var avglow = q.Average(x => x.Low);
                    var avghigh = q.Average(x => x.High);
                    var date = pr.Date;
                    smas.Add(new Price{Date = date, Open = avgopen, Low = avglow, High = avghigh, Close = avgclose });
                    q.Dequeue();
                }            
            }
        }
        else
        {
            throw new PunkQueryException($"Could not find price Data for ticker {ticker}");
        }
        return smas;

    }

    public async Task<IEnumerable<Price>> EMA(uint duration, string ticker, string? start = null, string? end = null)
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
        else
        {
            enddate = DateOnly.MaxValue;
            startdate = DateOnly.MinValue;
        }

        var prices = await this.Prices.Where(x => x.Ticker == ticker && x.Date <= enddate).Select(x => x).ToListAsync();
        if (prices != null)
        {
            double ema_close = 0.0, ema_open = 0.0, ema_low = 0.0, ema_high = 0.0;
            foreach (var pr in prices)
            {
                q.Enqueue(pr);
                if (q.Count == duration)
                {
                    ema_close = pr.Close * k + ema_close * (1 - k);
                    ema_open = pr.Close * k + ema_open * (1 - k);
                    ema_low = pr.Close * k + ema_low * (1 - k);
                    ema_high = pr.Close * k + ema_high * (1 - k);
                    var date = pr.Date;
                    emas.Add(new Price { Date = date, Open = ema_open, Low = ema_low, High = ema_high, Close = ema_close });
                    q.Dequeue();
                }
            }
        }
        else
        {
            throw new PunkQueryException($"Could not find price Data for ticker {ticker}");
        }
        return emas;

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
