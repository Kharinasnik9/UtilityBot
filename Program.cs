using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        const string telegramBotToken = "******";

        services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(telegramBotToken));
        services.AddHostedService<Bot>();
    })
    .Build();

await host.RunAsync();