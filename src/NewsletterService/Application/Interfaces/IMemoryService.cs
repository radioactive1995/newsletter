namespace Application.Interfaces;

public interface IMemoryService
{
    Task<bool> DoesKeyExist(string key);
    Task StoreKeyValuePair<T>(string key, T value, TimeSpan expiry);
}
