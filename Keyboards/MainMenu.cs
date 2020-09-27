using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
namespace Keyboards
{
    class MainMenu
    {
        public static InlineKeyboardMarkup Menu = new InlineKeyboardMarkup(new[]
            {
                new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Оценить кальян", "RateHookah&List")
                    },
                new[]
                    {
                        InlineKeyboardButton.WithCallbackData("О Нас", "AboutUs&Main")
                    }
            }
        );

        public static InlineKeyboardButton Back = InlineKeyboardButton.WithCallbackData("Вернуться в главное меню", "MainMenu");
        public static InlineKeyboardMarkup BackMU = InlineKeyboardButton.WithCallbackData("Вернуться в главное меню", "MainMenu");
    }
}