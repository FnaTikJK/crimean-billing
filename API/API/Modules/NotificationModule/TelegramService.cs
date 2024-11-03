using API.Infrastructure;
using API.Infrastructure.Config;
using Telegram.Bot;

namespace API.Modules.TelegramModule;

public interface ITelegramService
{
    Task<Result<bool>> SendMessage(long chatId, string text);
}

public class TelegramService : ITelegramService
{
    private readonly ITelegramBotClient botClient;
    private readonly ILog log;

    public TelegramService(ILog log)
    {
        if (string.IsNullOrEmpty(Config.TelegramApiKey))
        {
            botClient = null;
            log.Info("Telegram api key is not initialised. Bot is inactive");
            return;
        }
        botClient = new TelegramBotClient(Config.TelegramApiKey);
    }
    
    public async Task<Result<bool>> SendMessage(long chatId, string text)
    {
        await botClient.SendTextMessageAsync(chatId, text);
        return Result.Ok(true);
    }
}