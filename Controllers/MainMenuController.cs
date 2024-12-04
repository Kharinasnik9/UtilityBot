using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Views;
using Telegram.Bot;

public class MainMenuController
{
    private readonly ITelegramBotClient _botClient;

    public MainMenuController(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendMainMenuAsync(Message message)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Выберите действие:",
            replyMarkup: MenuMessages.GetMainMenu());
    }
}