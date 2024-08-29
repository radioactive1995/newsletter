using Domain.Articles;
using Domain.Common;
using Domain.Subscribers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Infrastructure.Persistance;

public class NewsletterContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Subscriber> Subscribers { get; set; }

    public NewsletterContext(DbContextOptions<NewsletterContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable(nameof(Articles));

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.MarkdownContent).IsRequired();

            entity.HasData(new Article()
            {
                Id = 1,
                Title = "Dapr Introduction and Service-to-Service Invocation Part I",
                MarkdownContent = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Persistance", "Data", "article1.txt")),
                Author = "Sultan Dzjumajev",
                CreatedDate = new DateTime(year: 2024, month: 8, day: 10, hour: default, minute: default, second: default, kind: DateTimeKind.Utc),
                EditedDate = new DateTime(year: 2024, month: 8, day: 10, hour: default, minute: default, second: default, kind: DateTimeKind.Utc),
            });
        });


        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.ToTable(nameof(Subscribers));

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Email).IsRequired();
        });

        foreach (var entitetType in modelBuilder.Model.GetEntityTypes())
        {
            if (entitetType.ClrType.IsSubclassOf(typeof(Entity<>)))
            {
                var entitetBygger = modelBuilder.Entity(entitetType.ClrType);

                entitetBygger.Property(nameof(Entity<int>.CreatedDate)).IsRequired();
                entitetBygger.Property(nameof(Entity<int>.EditedDate)).IsRequired();
                entitetBygger.Property(nameof(Entity<int>.Version)).IsRowVersion();
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var timestamp = DateTime.Now;
        foreach (var entry in ChangeTracker.Entries<AuditEntity>()
            .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified))
        {
            entry.Entity.EditedDate = timestamp;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = timestamp;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
