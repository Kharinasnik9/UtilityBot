using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;

namespace UtilityBot.Views
{
    public static class MenuMessages
    {
        // Главное меню
        public static InlineKeyboardMarkup GetMainMenu()
        {
            var keyboard = new List<InlineKeyboardButton[]>
        {
            new [] { InlineKeyboardButton.WithCallbackData("Подсчитать символы") },
            new [] { InlineKeyboardButton.WithCallbackData("Подсчитать числа") }
        };
            return new InlineKeyboardMarkup(keyboard);
        }

        // Сообщение о подсчете символов
        public static string GetSymbolCountMessage(int count)
        {
            return $"Количество символов: {count}";
        }

        // Сообщение о сумме чисел
        public static string GetNumberSumMessage(long sum)
        {
            return $"Сумма введенных чисел: {sum}";
        }
    }
}
