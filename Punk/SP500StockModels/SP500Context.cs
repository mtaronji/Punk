using System;
using Microsoft.EntityFrameworkCore;

using Punk.SP500StockModels;

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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
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

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
