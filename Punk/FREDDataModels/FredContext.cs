using System.Globalization;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Punk.Exceptions;

namespace Punk.FREDDataModels;

public partial class FredContext : DbContext
{
    public FredContext()
    {
    }

    public FredContext(DbContextOptions<FredContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FinancialDate> FinancialDates { get; set; }

    public virtual DbSet<Observation> Observations { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("DataSource = DataContexts/FRED.db;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialDate>(entity =>
        {
            entity.HasKey(e => e.Date);

            entity.ToTable("FinancialDate");

            entity.Property(e => e.Date).HasColumnType("Date");
        });

        modelBuilder.Entity<Observation>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => new { e.SeriesId, e.Date }, "IX_Observations_SeriesID_Date").IsUnique();

            entity.Property(e => e.SeriesId).HasColumnName("SeriesID");

            entity.HasOne(d => d.Series).WithMany().HasForeignKey(d => d.SeriesId);
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.Property(e => e.SeriesId).HasColumnName("SeriesID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public async Task<IEnumerable<object>> GetObservations(string seriesid, string? start = null, string? end = null)
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

        var observations = await Observations.Where(x => x.SeriesId == seriesid && x.Date <= enddate && x.Date >= startdate).Select(x => (object)x).ToListAsync();
        return observations;
    }

    public async Task<IEnumerable<object>> Query(Expression<Func<Observation, bool>> query)
    {
        var observations = await this.Observations.Where(query).Select(x => (object)x).ToListAsync();
        return observations;
    }
    public async Task<IEnumerable<object>> Join(string seriesid1, string seriesid2, string? start = null, string? end = null)
    {
        List<object> observations = new List<object>();

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
        var matchesid1 = this.Observations.Where(x => x.SeriesId == seriesid1 && x.Date <= enddate && x.Date > startdate);
        var matchesid2 = this.Observations.Where(x => x.SeriesId == seriesid2 && x.Date <= enddate && x.Date > startdate);

        observations = await matchesid1.Join(matchesid2, arg1 => arg1.Date, arg2 => arg2.Date, (obs1, obs2) => new { Observation1 = obs1, Observation2 = obs2 })
            .Select(x =>
            new
            {
                xdate = x.Observation1.Date,
                xobs = x.Observation1.ObservedValue,
                ydate = x.Observation2.Date,
                yobs = x.Observation2.ObservedValue
            }
 
        ).ToListAsync<object>();
        return observations;
    }

    public async Task<IEnumerable<Observation>> SMA(uint duration, string seriesid, string? start = null, string? end = null)
    {
        Queue<Observation> q = new Queue<Observation>();
        List<Observation> smas = new List<Observation>();
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

        var prices = await this.Observations.Where(x => x.SeriesId == seriesid && x.Date >= startdate && x.Date <= enddate).OrderBy(x => x.Date).Select(x => x).ToListAsync();

        if (prices != null)
        {
            foreach (var pr in prices)
            {
                q.Enqueue(pr);
                if (q.Count == duration)
                {
                    var avgobservedvalue = q.Average(x => x.ObservedValue);
                    var date = pr.Date;
                    smas.Add(new Observation { Date = date, ObservedValue = avgobservedvalue });
                    q.Dequeue();
                }
            }
        }
        else
        {
            throw new PunkQueryException($"Could not find price Data for ticker {seriesid}");
        }
        return smas;

    }

    public async Task<IEnumerable<Observation>> EMA(uint duration, string seriesid, string? start = null, string? end = null)
    {
        var k = 2.0 / (duration + 1.0);
        Queue<Observation> q = new Queue<Observation>();
        List<Observation> emas = new List<Observation>();
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

        var prices = await this.Observations.Where(x => x.SeriesId == seriesid && x.Date >= startdate && x.Date <= enddate).OrderBy(x => x.Date).Select(x => x).ToListAsync();
        if (prices != null)
        {
            double ema_observedvalue = 0.0;

            foreach (var pr in prices)
            {
                q.Enqueue(pr);
                if (q.Count == duration)
                {
                    ema_observedvalue = q.Average(x => x.ObservedValue);
                    var date = pr.Date;
                    emas.Add(new Observation { Date = date, ObservedValue = ema_observedvalue });
                }
                else if (q.Count > duration)
                {
                    ema_observedvalue = pr.ObservedValue * k + ema_observedvalue * (1 - k);
                    var date = pr.Date;
                    emas.Add(new Observation { Date = date, ObservedValue = ema_observedvalue});
                }
                else
                {
                    //beginning
                }
            }
        }
        else
        {
            throw new PunkQueryException($"Could not find price Data for ticker {seriesid}");
        }
        return emas;

    }

    //lag pulls data from the past into a future date
    public async Task<IEnumerable<object>> Lead(string seriesid, int lookforward, string? start = null, string? end = null)
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
        var obs = await this.Observations.Where(x => x.SeriesId == seriesid && x.Date <= enddate && x.Date >= startdate).OrderBy(x => x.Date).OrderBy(x => x.Date).Select(x => x).ToListAsync();

        int maxIndex = obs.Count - 1;
        for (int i = 0; i < obs.Count; i++)
        {
            if (i + lookforward < maxIndex)
            {
                data.Add(
                    new
                    {
                        leaddate = obs[i + lookforward].Date,
                        leadobservedvalue = obs[i + lookforward].ObservedValue,
                        date = obs[i].Date,
                        observedvalue = obs[i].ObservedValue,
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

    public async Task<IEnumerable<object>> Lag(string seriesid, int lookback, string? start = null, string? end = null)
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
        var obs = await this.Observations.Where(x => x.SeriesId == seriesid && x.Date <= enddate && x.Date >= startdate).OrderBy(x => x.Date).OrderBy(x => x.Date).Select(x => x).ToListAsync();

        int maxIndex = obs.Count - 1;
        for (int i = 0; i < obs.Count; i++)
        {
            if (i + lookback < maxIndex)
            {
                data.Add(
                    new
                    {
                        date = obs[i + lookback].Date,
                        observedvalue = obs[i + lookback].ObservedValue,
                        lagdate = obs[i].Date,
                        lagobservedvalue = obs[i].ObservedValue,
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
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
