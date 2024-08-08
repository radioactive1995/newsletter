using Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.Services;

public class CacheService(
    IDistributedCache cache) : ICacheService
{
    public async Task<bool> DoesKeyExist(string key)
    {
        var result = await cache.GetStringAsync(key);

        return !string.IsNullOrWhiteSpace(result);
    }

    public async Task AddCache<T>(string key, T value, TimeSpan expiry)
    {
        string? stringValue = null;
        if (typeof(T) == typeof(string))
        {
            stringValue = value as string;
            await cache.SetStringAsync(key, stringValue ?? string.Empty, options: new()
            {
                SlidingExpiration = expiry,
                AbsoluteExpiration = DateTime.Now.Add(expiry)
            });

            return;
        }

        stringValue = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, stringValue, options: new() 
        {
            SlidingExpiration = expiry,
            AbsoluteExpiration = DateTime.Now.Add(expiry)
        });
    }

    public async Task<T?> GetOrAddCache<T>(string key, RequestHandlerDelegate<T> handler, TimeSpan expiry)
    {
        var cachedValue = await cache.GetStringAsync(key);

        if (!string.IsNullOrWhiteSpace(cachedValue))
        {
            return JsonSerializer.Deserialize<T>(cachedValue);
        }

        var result = await handler();

        if (result == null) return default;

        await cache.SetStringAsync(key, JsonSerializer.Serialize(result), new DistributedCacheEntryOptions
        {
            SlidingExpiration = expiry,
            AbsoluteExpiration = DateTime.Today.AddMonths(1)
        });

        return result;
    }


    public async Task RemoveCache(string[] keys)
    {
        List<Task> arbeid = [];

        foreach (var key in keys)
        {
            arbeid.Add(cache.RemoveAsync(key));
        }

        await Task.WhenAll(arbeid);
    }
}
