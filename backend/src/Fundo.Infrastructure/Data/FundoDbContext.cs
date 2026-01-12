using System;
using Microsoft.EntityFrameworkCore;
using Fundo.Domain.Entities;

namespace Fundo.Infrastructure.Data
{
    public class FundoDbContext : DbContext
    {
        public FundoDbContext(DbContextOptions<FundoDbContext> options) : base(options)
        {
        }

        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for currency
            modelBuilder.Entity<Loan>()
                .Property(l => l.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Loan>()
                .Property(l => l.CurrentBalance)
                .HasPrecision(18, 2);

            // Seed initial data
            modelBuilder.Entity<Loan>().HasData(
                new Loan { Id = 1, Amount = 1500m, CurrentBalance = 500m, ApplicantName = "Maria Silva", Status = LoanStatus.Active, CreatedAt = new DateTime(2026, 01, 12, 0, 0, 0, DateTimeKind.Utc) },
                new Loan { Id = 2, Amount = 3000m, CurrentBalance = 0m, ApplicantName = "João Santos", Status = LoanStatus.Paid, CreatedAt = new DateTime(2025, 12, 13, 0, 0, 0, DateTimeKind.Utc) },
                new Loan { Id = 3, Amount = 2000m, CurrentBalance = 1200m, ApplicantName = "Ana Costa", Status = LoanStatus.Active, CreatedAt = new DateTime(2026, 01, 07, 0, 0, 0, DateTimeKind.Utc) },
                new Loan { Id = 4, Amount = 5000m, CurrentBalance = 4500m, ApplicantName = "John Doe", Status = LoanStatus.Active, CreatedAt = new DateTime(2026, 01, 10, 0, 0, 0, DateTimeKind.Utc) },
                new Loan { Id = 5, Amount = 7500m, CurrentBalance = 2500m, ApplicantName = "Jane Smith", Status = LoanStatus.Active, CreatedAt = new DateTime(2026, 01, 05, 0, 0, 0, DateTimeKind.Utc) },
                new Loan { Id = 6, Amount = 10000m, CurrentBalance = 10000m, ApplicantName = "Robert Johnson", Status = LoanStatus.Active, CreatedAt = new DateTime(2026, 01, 11, 0, 0, 0, DateTimeKind.Utc) },
                new Loan { Id = 7, Amount = 8500m, CurrentBalance = 0m, ApplicantName = "Emily Williams", Status = LoanStatus.Paid, CreatedAt = new DateTime(2025, 12, 20, 0, 0, 0, DateTimeKind.Utc) },
                new Loan { Id = 8, Amount = 12000m, CurrentBalance = 7200m, ApplicantName = "Michael Brown", Status = LoanStatus.Active, CreatedAt = new DateTime(2026, 01, 09, 0, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
}