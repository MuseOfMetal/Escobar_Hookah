using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
namespace Keyboards
{
    class AboutUs
    {
        public static InlineKeyboardMarkup Main
        {
            get
            {
                var list = Types.Shop.ShopsBase;
                var Keyboard = new List<List<InlineKeyboardButton>>();

                for (int i = 0; i < list.Count; i++)
                {
                    Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(list[i].Name, $"AboutUs&Select&{list[i].Name}") });
                }

                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Назад", "MainMenu") });

                return Keyboard.ToArray();
            }
            private set { }
        }

        public static InlineKeyboardMarkup List(string Act)
        {
            var list = Types.Shop.ShopsBase;
            var Keyboard = new List<List<InlineKeyboardButton>>();

            for (int i = 0; i < list.Count; i++)
            {
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData(list[i].Name, $"Admin&Shop&{Act}&Select&{list[i].Name}") });
            }

            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Назад", "Admin&MainMenu") });

            return Keyboard.ToArray();
        }
        public static InlineKeyboardMarkup ReserveTable(string Id)
        {
            return new InlineKeyboardMarkup
            (
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Забронировать столик", $"AboutUs&Reserve&{Id}")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад","AboutUs&Main")
                    }
                }
            );
        }
    }
}