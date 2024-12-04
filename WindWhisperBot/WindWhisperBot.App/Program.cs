// -
using System;

using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using Telegram.Bot;

using WindWhisperBot.Controllers;
using WindWhisperBot.Services;
using WindWhisperBot.Configuration;
using WindWhisperBot.Extensions;


namespace WindWhisperBot.App;

internal class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                // Задаем конфигурацию
                .ConfigureServices((hostContext, services) => ConfigureServices(services))
                // Позволяет поддерживать приложение активным в консоли
                .UseConsoleLifetime()
                // Собираем
                .Build();

            Console.WriteLine("Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");
        }
        catch (Exception ex)
        {
            Console.WriteLine("error: " + ex.Message);
            Console.ReadKey();
        }
    }


    internal static void ConfigureServices(IServiceCollection services)
    {
        AppSettings appSettings = BuildAppSettings();
        services.AddSingleton(appSettings);

        // хранилище сессий
        // Хранилище пользовательских данных в памяти
        services.AddSingleton<IStorage, MemoryStorage>();

        // Подключаем контроллеры сообщений и кнопок
        services.AddTransient<DefaultMessageController>();
        services.AddTransient<VoiceMessageController>();
        services.AddTransient<TextMessageController>();
        services.AddTransient<InlineKeyboardController>();

        // Регистрируем объект TelegramBotClient c токеном подключения
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
        // Регистрируем постоянно активный сервис бота
        services.AddHostedService<Bot>();
    }


    internal static AppSettings BuildAppSettings()
    {
        // получаем значение токена из файла
        string botToken = string.Empty;
        string privateDataFolder = DirectoryExtension.GetRootForLibrary("private-data");
        if (!string.IsNullOrWhiteSpace(privateDataFolder))
        {
            string botTokenFilePath = Path.Combine(privateDataFolder, "private-data", "BOT_TOKEN");
            if (File.Exists(botTokenFilePath))
            {
                botToken = File.ReadLines(botTokenFilePath).First();
            }
        }

        if (string.IsNullOrWhiteSpace(botToken))
            throw new Exception("token is not found");

        return new AppSettings()
        {
            BotToken = botToken,
        };
    }
}
