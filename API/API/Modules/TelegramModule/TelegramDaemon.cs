using API.Infrastructure;
using API.Infrastructure.Config;
using API.Modules.CacheModule;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace API.Modules.TelegramModule;

public interface ITelegramDaemon
{
}

public class TelegramDaemon : ITelegramDaemon
{
    private readonly ITelegramBotClient botClient;
    private readonly ILog log;
    private readonly ICache cache;

    public TelegramDaemon(ILog log, ICache cache)
    {
        if (string.IsNullOrEmpty(Config.TelegramApiKey))
        {
            botClient = null;
            log.Info("Telegram api key is not initialised. Bot is inactive");
            return;
        }

        this.log = log;
        this.cache = cache;
        
        botClient = new TelegramBotClient(Config.TelegramApiKey);
        var recieverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message,
            },
            // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
            // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
            ThrowPendingUpdates = true, 
        };
        
        using var cts = new CancellationTokenSource();
        
        botClient.StartReceiving(UpdateHandler, ErrorHandler, recieverOptions, cts.Token);
    }
    
    private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    if (update.Message == null)
                    {
                        log.Info("Message is null. No response");
                        return;
                    }

                    var message = update.Message;
                    await HandleMessage(message.Text, message.From, message.Chat, message.Contact?.PhoneNumber);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.ToString());
        }
    }

    private async Task HandleMessage(string? text, User? user, Chat chat, string? phoneNumber)
    {
        if (text == "/start")
        {
            await botClient.SendTextMessageAsync(
                chat.Id,
                @"Это бот команды Крымчане, по аутентификации в сервисе Биллинга",
                replyMarkup: defaultKeyboard);
            return;
        }

        if (phoneNumber != null)
        {
            var truncatedPhone = PhoneConverter.ToPhoneWithoutRegMask(phoneNumber);
            if (truncatedPhone == null)
                throw new Exception("Incorrect phone");
            var verificationCode = cache.Get(truncatedPhone);
            if (verificationCode == null)
            {
                await botClient.SendTextMessageAsync(
                    chat.Id,
                    @$"Не нашёл для вас код верификации =(",
                    replyMarkup: defaultKeyboard);
            }
            else
            {
                await botClient.SendTextMessageAsync(
                    chat.Id,
                    @$"Ваш код верификации: {verificationCode}",
                    replyMarkup: defaultKeyboard);
            }

            return;
        }

        await botClient.SendTextMessageAsync(chat.Id, @"Ничего не понял.", replyMarkup: defaultKeyboard);
    }

    private static ReplyKeyboardMarkup defaultKeyboard = new(new List<KeyboardButton[]>()
    {
        new []
        {
            new KeyboardButton("Get verification code") {RequestContact = true}
        }
    })
    {
        ResizeKeyboard = true,
    };

    private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        log.Error(ErrorMessage);
        return Task.CompletedTask;
    }
}
