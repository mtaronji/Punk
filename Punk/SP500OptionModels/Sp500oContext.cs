using Microsoft.EntityFrameworkCore;
using Punk.Exceptions;
using System.Globalization;

namespace Punk.SP500OptionModels;

public partial class Sp500oContext : DbContext
{
    public Sp500oContext()
    {
    }

    public Sp500oContext(DbContextOptions<Sp500oContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FinancialDate> FinancialDates { get; set; }

    public virtual DbSet<OptionCode> OptionCodes { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<SequenceComplete> SequenceCompletes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("DataSource=DataContexts/SP500O.db;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialDate>(entity =>
        {
            entity.HasKey(e => e.Date);

            entity.ToTable("FinancialDate");
        });

        modelBuilder.Entity<OptionCode>(entity =>
        {
            entity.HasKey(e => e.Code);
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Price");

            entity.HasIndex(e => new { e.Duration, e.Code }, "IX_Price_Duration_Code").IsUnique();

            entity.Property(e => e.Vwap).HasColumnName("VWAP");

            entity.HasOne(d => d.CodeNavigation).WithMany()
                .HasForeignKey(d => d.Code)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MaturityDateNavigation).WithMany().HasForeignKey(d => d.MaturityDate);
        });

        modelBuilder.Entity<SequenceComplete>(entity =>
        {
            entity.HasKey(e => e.Code);

            entity.ToTable("SequenceComplete");

            entity.Property(e => e.Code).HasColumnType("nvarchar(25)");

            entity.HasOne(d => d.CodeNavigation).WithOne(p => p.SequenceComplete).HasForeignKey<SequenceComplete>(d => d.Code);
        });

        OnModelCreatingPartial(modelBuilder);
    }
    public async Task<IEnumerable<Price>> GetPrices(string code)
    {
        List<Price> smas = new List<Price>();
        
        var prices = await this.Prices.Where(x => x.Code == code).Select(x => x).ToListAsync();
        return prices;
    }

    public async Task<IEnumerable<Price>> SMA(uint duration, string code)
    {
        Queue<Price> q = new Queue<Price>();
        List<Price> smas = new List<Price>();
        
        var prices = await this.Prices.Where(x => x.Code == code).Select(x => x).ToListAsync();

        if (prices != null)
        {
            foreach (var pr in prices)
            {
                q.Enqueue(pr);
                if (q.Count == duration)
                {
                    var avgclose = q.Average(x => x.Close);
                    var avgopen = q.Average(x => x.Open);
                    var avglow = q.Average(x => x.Low);
                    var avghigh = q.Average(x => x.High);
                    var dur = pr.Duration;
                    smas.Add(new Price { Duration = dur, Open = avgopen, Low = avglow, High = avghigh, Close = avgclose });
                    q.Dequeue();
                }
            }
        }
        else
        {
            throw new PunkQueryException($"Could not find price Data for ticker {code}");
        }
        if(smas.Count == 0) { throw new Exceptions.PunkQueryException("We don't have enough of that option data to make a moving average"); }
        return smas;

    }

    public async Task<IEnumerable<Price>> EMA(uint duration,string code)
    {
        var k = 2.0 / (duration + 1.0);
        Queue<Price> q = new Queue<Price>();
        List<Price> emas = new List<Price>();
        

        var prices = await this.Prices.Where(x => x.Code == code).Select(x => x).ToListAsync();
        if (prices != null)
        {
            double ema_open = 0.0, ema_low = 0.0, ema_high = 0.0;
            double? ema_close = 0.0, ema_adjclose = 0.0;
            foreach (var pr in prices)
            {
                if(pr == null) { throw new Exceptions.PunkQueryException("Unable to find data for option"); }
                q.Enqueue(pr);
                if (q.Count == duration)
                {
                    ema_adjclose = pr.Adjclose * k + ema_adjclose * (1 - k);
                    ema_close = pr.Close * k + ema_close * (1 - k);
                    ema_open = pr.Open * k + ema_open * (1 - k);
                    ema_low = pr.Low * k + ema_low * (1 - k);
                    ema_high = pr.High * k + ema_high * (1 - k);
                    var dur = pr.Duration;
                    emas.Add(new Price { Duration = dur, Open = ema_open, Low = ema_low, High = ema_high, Close = ema_close });
                    q.Dequeue();
                }
            }
        }
        else
        {
            throw new PunkQueryException($"Could not find price Data for ticker {code}");
        }
        if (emas.Count == 0) { throw new Exceptions.PunkQueryException("We don't have enough of that option data to make a moving average"); }
 
        return emas;

    }



    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


//Scaffold code 
//Scaffold-DbContext "DataSource=DataContexts/SP500O.db;" Microsoft.EntityFrameworkCore.Sqlite -OutputDir SP500OptionModels