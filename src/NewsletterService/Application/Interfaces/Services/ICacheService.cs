using MediatR;

namespace Application.Interfaces.Services;

public interface ICacheService
{
    Task<bool> DoesKeyExist(string key);
    Task AddCache<T>(string key, T value, TimeSpan expiry);
    Task<T?> GetOrAddCache<T>(string key, RequestHandlerDelegate<T> handler, TimeSpan expiry);
    Task RemoveCache(string[] keys);
}
