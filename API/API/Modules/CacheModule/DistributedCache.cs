﻿namespace API.Modules.CacheModule;

public class DistributedCache : ICache
{
    public string? Get(string key)
    {
        throw new NotImplementedException();
    }

    public void AddOrUpdate(string key, string value)
    {
        throw new NotImplementedException();
    }

    public void Delete(string key)
    {
        throw new NotImplementedException();
    }
}