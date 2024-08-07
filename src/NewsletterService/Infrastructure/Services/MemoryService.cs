using Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.Services;

public class MemoryService(
    IDistributedCache distributedCache) : IMemoryService
{
    public async Task<bool> DoesKeyExist(string key)
    {
        var result = await distributedCache.GetStringAsync(key);

        return !string.IsNullOrWhiteSpace(result);
    }

    public async Task StoreKeyValuePair<T>(string key, T value, TimeSpan expiry)
    {
        string? stringValue = null;
        if (typeof(T) == typeof(string))
        {
            stringValue = value as string;
            await distributedCache.SetStringAsync(key, stringValue ?? string.Empty, options: new()
            {
                SlidingExpiration = expiry,
                AbsoluteExpiration = DateTime.Now.Add(expiry)
            });

            return;
        }

        stringValue = JsonSerializer.Serialize(value);
        await distributedCache.SetStringAsync(key, stringValue, options: new() 
        {
            SlidingExpiration = expiry,
            AbsoluteExpiration = DateTime.Now.Add(expiry)
        });
    }
}
