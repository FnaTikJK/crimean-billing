using System.Collections.Concurrent;

namespace API.Modules.CacheModule;

public class CacheInMemory : ICache
{
    private readonly ConcurrentDictionary<string, string> inner;

    public CacheInMemory()
    {
        inner = new ConcurrentDictionary<string, string>();
    }


    public string? Get(string key)
    {
        if (inner.TryGetValue(key, out var value))
            return value;
        return null;
    }

    public void AddOrUpdate(string key, string value)
    {
        inner.AddOrUpdate(key, value, (a, b) => value);
    }

    public void Delete(string key)
    {
        inner.Remove(key, out var value);
    }
}