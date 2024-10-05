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

    public void Add(string key, string value)
    {
        if (inner.ContainsKey(key))
            throw new Exception($"Cache already have same key: {key}");

        if (!inner.TryAdd(key, value))
            throw new Exception($"Can not set value to cache. key {key}, value {value}");
    }

    public void Delete(string key)
    {
        inner.Remove(key, out var value);
    }
}