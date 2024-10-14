namespace API.Modules.CacheModule;

public interface ICache
{
    public string? Get(string key);
    public void AddOrUpdate(string key, string value);
    public void Delete(string key);
}