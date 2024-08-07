using Application.Interfaces;
using Domain.Subscribers;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;
public class SubscriberRepository
    (IDbContextFactory<NewsletterContext> factory) : ISubscriberRepository
{
    public async Task<bool> DoesSubscriberExistWithEmail(string email)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Subscribers.AnyAsync(e => e.Email == email);
    }

    public async Task AddSubscriber(Subscriber subscriber)
    {
        await using var db = await factory.CreateDbContextAsync();
        await db.Subscribers.AddAsync(subscriber);
        await db.SaveChangesAsync();
    }
    
    public async Task<int> GetSubscribersCount()
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Subscribers.CountAsync();
    }
}
