using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
namespace Keyboards
{
    class HookahWorkersList
    {
        public static InlineKeyboardMarkup List4User
        {
            get
            {
                var list = Types.HookahMan.HookahWorkersBase;
                var Keyboard = new List<List<InlineKeyboardButton>>();

                for (int i = 0; i < list.Count;)
                {
                    var Row = new List<InlineKeyboardButton>();
                    for (int j = 0; j < 3 && i < list.Count; j++, i++)
                    {
                        Row.Add(InlineKeyboardButton.WithCallbackData(list[i].Id, $"RateHookah&Select&{list[i].Id}"));
                    }
                    Keyboard.Add(Row);
                }
                Keyboard.Add(new List<InlineKeyboardButton>() {Keyboards.MainMenu.Back});
                return Keyboard.ToArray();
            }
            private set{}
        }

        public static InlineKeyboardMarkup List4Admin(string Act)
        {

                var list = Types.HookahMan.HookahWorkersBase;
                var Keyboard = new List<List<InlineKeyboardButton>>();

                for (int i = 0; i < list.Count;)
                {
                    var Row = new List<InlineKeyboardButton>();
                    for (int j = 0; j < 3 && i < list.Count; j++, i++)
                    {
                        Row.Add(InlineKeyboardButton.WithCallbackData(list[i].Id, $"Admin&HookahMan&{Act}&Select&{list[i].Id}"));
                    }
                    Keyboard.Add(Row);
                }
                Keyboard.Add(new List<InlineKeyboardButton>() {InlineKeyboardButton.WithCallbackData("Назад", "Admin&MainMenu")});
                return Keyboard.ToArray();
        }
    }
}