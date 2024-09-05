using Application.Interfaces.Repositories;
using Domain.Users;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(
    IDbContextFactory<NewsletterContext> factory) : IUserRepository
{
    public async Task<User?> GetUserByEmails(string[] emails)
    {
        await using NewsletterContext db = await factory.CreateDbContextAsync();
        
        return (await db.Users.ToListAsync())
            .Find(u => u.Emails.Exists(uEmail => emails.Contains(uEmail)));

        //await using NewsletterContext db = await factory.CreateDbContextAsync();
        //
        //// Construct individual LIKE conditions for each email
        //var emailConditions = emails.Select(email => $"Emails LIKE '%{email}%'");
        //
        //// Join the conditions with "OR"
        //var whereClause = string.Join(" OR ", emailConditions);
        //
        //// Construct the full SQL query
        ////var sql = $"SELECT * FROM Users WHERE {whereClause}";
        //
        //return await db.Users
        //    .FromSql($"SELECT * FROM Users WHERE {whereClause}")
        //    .FirstOrDefaultAsync();
    }

    public async Task AddUser(User User)
    {
        await using NewsletterContext db = await factory.CreateDbContextAsync();
        await db.Users.AddAsync(User);
        await db.SaveChangesAsync();
    }

    public async Task UpdateUser(User User)
    {
        await using NewsletterContext db = await factory.CreateDbContextAsync();
        db.Users.Update(User);
        await db.SaveChangesAsync();
    }

    public async Task<User?> GetUserByOid(string oid)
    {
        await using NewsletterContext db = await factory.CreateDbContextAsync();
        return await db.Users.FirstOrDefaultAsync(e => e.LocalId == oid || e.MicrosoftId == oid || e.GoogleId == oid);
    }
}
