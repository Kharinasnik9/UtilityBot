using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using UtilityBot.Models;
using UtilityBot.Views;

namespace UtilityBot.Controllers
{
    public class NumberSumController
    {
        private readonly ITelegramBotClient _botClient;

        public NumberSumController(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task SumNumbersAsync(Message message, UserState userState)
        {
            if (message.Type != MessageType.Text)
            {
                return;
            }

            long sum = 0;
            foreach (var number in message.Text.Split(' '))
            {
                if (long.TryParse(number, out var parsedNumber))
                {
                    sum += parsedNumber;
                }
            }

            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: MenuMessages.GetNumberSumMessage(sum));
        }
    }
}
