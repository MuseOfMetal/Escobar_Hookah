using Telegram.Bot.Types.ReplyMarkups;
namespace Keyboards
{
    class RateHookahMan
    {
        public static InlineKeyboardMarkup Rate(string id)
        {
            return new InlineKeyboardMarkup
            (
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("1 🤬", $"RateHookah&Rate&{id}&1"),
                        InlineKeyboardButton.WithCallbackData("2 🥴", $"RateHookah&Rate&{id}&2"),
                        InlineKeyboardButton.WithCallbackData("3 😕", $"RateHookah&Rate&{id}&3")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("4 🙂", $"RateHookah&Rate&{id}&4"),
                        InlineKeyboardButton.WithCallbackData("5 😘", $"RateHookah&Rate&{id}&5")
                    }
                    ,
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", $"RateHookah&List")
                    }
                }
            );
        }
    }
}