//-
using System;

using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using WindWhisperBot.Services;


namespace WindWhisperBot.Controllers;

public class InlineKeyboardController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;

    public InlineKeyboardController(
        ITelegramBotClient telegramBotClient,
        IStorage memoryStorage)
    {
        _telegramClient = telegramBotClient;
        _memoryStorage = memoryStorage;
    }


    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        Console.WriteLine($"Контроллер {GetType().Name} обнаружил нажатие на кнопку");

        // Telegram.Bot: migration to 22.x
        // SendTextMessageAsync -> SendMessage
        // await _telegramClient.SendMessage(callbackQuery.From.Id,
        //     $"Обнаружено нажатие на кнопку {callbackQuery.Data}", cancellationToken: ct);

        if (callbackQuery?.Data == null)
            return;

        // Обновление пользовательской сессии новыми данными
        _memoryStorage.GetSession(callbackQuery.From.Id).FlowOption = callbackQuery.Data;

        // Генерим информационное сообщение
        string userChoice = callbackQuery.Data switch
        {
            "num" => " Числа",
            "txt" => " Текст",
            _ => string.Empty
        };

        // Отправляем в ответ уведомление о выборе
        await _telegramClient.SendMessage(callbackQuery.From.Id,
            $"<b>Текущий режим - {userChoice}.{Environment.NewLine}</b>"
            + $"{Environment.NewLine}Можно поменять в главном меню",
            cancellationToken: ct, parseMode: ParseMode.Html);
    }
}
