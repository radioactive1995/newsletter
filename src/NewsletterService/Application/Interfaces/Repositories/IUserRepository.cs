using Domain.Users;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByEmails(string[] emails);
    Task UpdateUser(User User);
    Task AddUser(User User);
    Task<User?> GetUserByOid(string oid);
}
