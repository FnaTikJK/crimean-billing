namespace API.Infrastructure.Config;

public static class ConfigReader
{
    private static bool IsDebug;
    
    public static void Init(bool isDebug)
    {
        IsDebug = isDebug;
        Config.IsDebug = isDebug;
        Config.DatabaseConnectionString = GetStringByEnv(DatabaseConnectionStringKey)!;
        Config.MailBoxLogin = GetStringByEnv(MailBoxLoginKey)!;
        Config.MailBoxPassword = GetStringByEnv(MailBoxPasswordKey)!;
        Config.TelegramApiKey = GetStringByEnv(TelegramApiKey)!;
    }

    private static readonly string DatabaseConnectionStringKey = "DATABASE_CONNECTION_STRING";

    private static readonly string MailBoxLoginKey = "MAIL_BOX_LOGIN";
    private static readonly string MailBoxPasswordKey = "MAIL_BOX_PASSWORD";

    private static readonly string TelegramApiKey = "TELEGRAM_API_KEY";

    private static string PathToConfig => Environment.GetEnvironmentVariable("CONFIG_PATH") 
                                          ?? @"../Config";

    private static string? GetStringByEnv(string varName) => IsDebug
        ? GetStringIfExists(varName)
        : Environment.GetEnvironmentVariable(varName)!;
    
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