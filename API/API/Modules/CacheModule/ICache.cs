namespace API.Modules.CacheModule;

public interface ICache
{
    public string? Get(string key);
    public void Add(string key, string value);
    public void Delete(string key);
}