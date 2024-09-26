namespace API.Infrastructure;

public static class Config
{
    public static void Init()
    {
        
    }
    
    public static bool IsDebug { get; set; }

    public static class Database
    {
        public static string ConnectionString => IsDebug
            ? throw new NotImplementedException() // TODO: разобраться бы с конфигом
            : Environment.GetEnvironmentVariable("DATABASE_CONNECTION")!;
    }
}