using Domain.Articles;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistance;

public class NewsletterContext : DbContext
{
    public DbSet<Article> Articles { get; set; }

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
                Title = "Why coding makes you smarter",
                MarkdownContent = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Persistance", "Data", "article1.txt")),
                Author = "Sultan Dzjumajev"
            });

            entity.HasData(new Article()
            {
                Id = 2,
                Title = "Dapr Introduction and Sevice-to-sevice invocation Part I",
                MarkdownContent = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Persistance", "Data", "article2.txt")),
                Author = "Sultan Dzjumajev"
            });
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
