//-
using System;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using WindWhisperBot.Services;
using WindWhisperBot.Utilities;


namespace WindWhisperBot.Controllers;

public class TextMessageController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;

    public TextMessageController(
        ITelegramBotClient telegramBotClient,
        IStorage memoryStorage)
    {
        _telegramClient = telegramBotClient;
        _memoryStorage = memoryStorage;
    }


    public async Task Handle(Message message, CancellationToken ct)
    {
        Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");

        // Telegram.Bot: migration to 22.x
        // SendTextMessageAsync -> SendMessage
        // await _telegramClient.SendMessage(message.Chat.Id,
        //     $"Получено текстовое сообщение", cancellationToken: ct);

        switch (message.Text)
        {
            case "/start":

                // Объект, представляющий кнопки
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add([
                    InlineKeyboardButton.WithCallbackData($" Числа" , $"num"),
                    InlineKeyboardButton.WithCallbackData($" Текст" , $"txt"),
                ]);

                // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                await _telegramClient.SendMessage(message.Chat.Id,
                    $"<b>Наш бот считает числа или текст.</b> {Environment.NewLine}"
                    + $"{Environment.NewLine}Отправьте сообщение"
                    + $" со списком чисел или текстом{Environment.NewLine}",
                    cancellationToken: ct, parseMode: ParseMode.Html,
                    replyMarkup: new InlineKeyboardMarkup(buttons));

                break;
            default:
                // await _telegramClient.SendMessage(message.Chat.Id,
                //     $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct);

                string userChoice = _memoryStorage.GetSession(message.Chat.Id).FlowOption switch
                {
                    "num" => " Числа",
                    "txt" => " Текст",
                    _ => string.Empty
                };
                string calcResult = _memoryStorage.GetSession(message.Chat.Id).FlowOption switch
                {
                    "num" => NumCalc.Calculate(message.Text),
                    "txt" => TxtCalc.Calculate(message.Text),
                    _ => string.Empty
                };
                await _telegramClient.SendMessage(message.Chat.Id,
                    $"<b>Текущий режим - {userChoice}.</b>{Environment.NewLine}"
                    + $"Ваш результат: {calcResult}",
                    cancellationToken: ct, parseMode: ParseMode.Html);

                break;
        }
    }
}
