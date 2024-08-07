using Application.Interfaces;
using Infrastructure.Persistance;

namespace Infrastructure.Repositories;

public class UnitOfWork(
    NewsletterContext db) : IUnitOfWork
{
    public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
    { 
        return await db.SaveChangesAsync(cancellationToken); 
    }
}
