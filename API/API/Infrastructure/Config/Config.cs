namespace API.Infrastructure.Config;

public static class Config
{
    public static bool IsDebug { get; set; }

    public static string DatabaseConnectionString { get; set; }
    
    public static string MailBoxLogin { get; set; }
    public static string MailBoxPassword { get; set; }
    
    public static string TelegramApiKey { get; set; }
}