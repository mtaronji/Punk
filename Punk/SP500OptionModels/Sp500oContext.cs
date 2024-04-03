using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
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

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


//Scaffold code 
//Scaffold-DbContext "DataSource=DataContexts/SP500O.db;" Microsoft.EntityFrameworkCore.Sqlite -OutputDir SP500OptionModels