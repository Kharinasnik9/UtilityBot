using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using UtilityBot.Views;
using UtilityBot.Models;
using Telegram.Bot.Types;


namespace UtilityBot.Controllers
{
    public class SymbolCounterController
    {
        private readonly ITelegramBotClient _botClient;

        public SymbolCounterController(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task CountSymbolsAsync(Message message, UserState userState)
        {
            if (message.Type != MessageType.Text)
            {
                return;
            }

            int symbolCount = message.Text.Length;
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: MenuMessages.GetSymbolCountMessage(symbolCount));
        }
    }
}
