using Domain.Articles;
using Domain.Common;
using Domain.Subscribers;
using Microsoft.EntityFrameworkCore;
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
                CreatedDate = new DateTime(year: 2024, month: 8, day: 10),
                EditedDate = new DateTime(year: 2024, month: 8, day: 10),
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
}
