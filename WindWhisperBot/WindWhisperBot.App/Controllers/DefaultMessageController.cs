//-
using System;

using System.Threading;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types;


namespace WindWhisperBot.Controllers;

public class DefaultMessageController
{
    private readonly ITelegramBotClient _telegramClient;

    public DefaultMessageController(
        ITelegramBotClient telegramBotClient)
    {
        _telegramClient = telegramBotClient;
    }


    public async Task Handle(Message message, CancellationToken ct)
    {
        Console.WriteLine($"Контроллер {GetType().Name} получил сообщение");

        // Telegram.Bot: migration to 22.x
        // SendTextMessageAsync -> SendMessage
        await _telegramClient.SendMessage(message.Chat.Id,
            $"Получено сообщение не поддерживаемого формата", cancellationToken: ct);
    }
}
