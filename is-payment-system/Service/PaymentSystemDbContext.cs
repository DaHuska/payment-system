using System;
using System.Configuration;
using is_payment_system.Model;
using Microsoft.EntityFrameworkCore;

namespace is_payment_system.Service;

public class PaymentSystemDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<LogEntry> Logs => Set<LogEntry>();
    public DbSet<Merchant> Merchants => Set<Merchant>();

    public PaymentSystemDbContext()
    {
    }

    public PaymentSystemDbContext(DbContextOptions<PaymentSystemDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        var cs = ConfigurationManager.ConnectionStrings["PaymentSystemDB"]?.ConnectionString
                 ?? throw new InvalidOperationException("Missing connection string: PaymentSystemDB (App.config)");

        cs = Environment.ExpandEnvironmentVariables(cs);

        optionsBuilder.UseMySql(cs, ServerVersion.AutoDetect(cs));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            entity.Property(u => u.FirstName).HasColumnName("FirstName").IsRequired();
            entity.Property(u => u.LastName).HasColumnName("LastName").IsRequired();
            entity.Property(u => u.Email).HasColumnName("Email").IsRequired();
            entity.Property(u => u.Password).HasColumnName("Password").IsRequired();
            entity.Property(u => u.Role).HasColumnName("Role").HasConversion<int>();
            entity.Property(u => u.DateCreated).HasColumnName("DateCreated");
            entity.Property(u => u.IsActive).HasColumnName("IsActive");

            entity.Ignore(u => u.Cards);
            entity.Ignore(u => u.Transactions);
            entity.Ignore(u => u.Merchants);
        });

        modelBuilder.Entity<Card>(entity =>
        {
            entity.ToTable("Cards");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            entity.Property(c => c.CardNumber).HasColumnName("CardNumber").IsRequired();
            entity.Property(c => c.Balance).HasColumnName("Balance").IsRequired();
            entity.Property(c => c.CVV).HasColumnName("CVV").IsRequired();
            entity.Property(c => c.Iban).HasColumnName("Iban").IsRequired();
            entity.Property(c => c.CreatedDate).HasColumnName("CreatedDate");
            entity.Property(c => c.ExpirationDate).HasColumnName("ExpirationDate");
            
            entity.Property<int>("UserId").HasColumnName("UserId");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transactions");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            entity.Property(t => t.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)");
            entity.Property(t => t.Timestamp).HasColumnName("Timestamp");
            entity.Property(t => t.Status).HasColumnName("Status").HasConversion<int>();
            entity.Property(t => t.Sender).HasColumnName("SenderId");
            entity.Property(t => t.Recipient).HasColumnName("MerchantId");
        });

        modelBuilder.Entity<Merchant>(entity =>
        {
            entity.ToTable("Merchants");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            entity.Property(m => m.BusinessName).HasColumnName("BusinessName").IsRequired();
            entity.Property(m => m.Email).HasColumnName("Email").IsRequired();
            entity.Property(m => m.Phone).HasColumnName("Phone").IsRequired();
            entity.Property(m => m.Balance).HasColumnName("Balance").HasColumnType("decimal(18,2)");
            entity.Property(m => m.Status).HasColumnName("Status").HasConversion<int>();
            entity.Property(m => m.CreatedAt).HasColumnName("CreatedAt");
        });

        modelBuilder.Entity<LogEntry>(entity =>
        {
            entity.ToTable("Logs");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            entity.Property(l => l.EventId).HasColumnName("EventId").IsRequired();
            entity.Property(l => l.Message).HasColumnName("Message").IsRequired();
            entity.Property(l => l.Timestamp).HasColumnName("Timestamp");
            entity.Property(l => l.LoggerType).HasColumnName("LoggerType").IsRequired();
        });
    }
}
