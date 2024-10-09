namespace API.Infrastructure.Config;

public static class ConfigReader
{
    public static void Init(bool isDebug)
    {
        Config.IsDebug = isDebug;
        Config.DatabaseConnectionString = isDebug
            ? GetStringIfExists(DatabaseConnectionStringKey)
            : Environment.GetEnvironmentVariable(DatabaseConnectionStringKey)!;
    }

    private static readonly string DatabaseConnectionStringKey = "DATABASE_CONNECTION_STRING";
    
    private static string PathToConfig => Environment.GetEnvironmentVariable("CONFIG_PATH") 
                                          ?? @"../Config";

    private static string GetStringIfExists(string varName) => Search(varName);
    private static int? GetIntIfExists(string varName) => int.TryParse(Search(varName), out var number) 
        ? number 
        : null; 

    private static string Search(string varName)
    {
        using var stream = File.Open(PathToConfig, FileMode.Open);
        var reader = new StreamReader(stream);
        while (true)
        {
            var parsed = reader.ReadLine()?.Split(" = ");
            if (parsed == null)
                return null;

            if (parsed[0] == varName)
                return parsed[1];
        }
    }
}