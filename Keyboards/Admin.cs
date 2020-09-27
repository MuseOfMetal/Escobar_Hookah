using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
namespace Keyboards
{
    class Admin
    {
        public static InlineKeyboardMarkup Main(int lvlperm)
        {
            var Keyboard = new List<List<InlineKeyboardButton>>();
            if (lvlperm == 1 || lvlperm == 2)
            {
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Добавить кальянщика", "Admin&HookahMan&Add") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Добавить заведение", "Admin&Shop&Add") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Редактировать данные о кальянщике", "Admin&HookahMan&Edit&List") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Редактировать данные о заведении", "Admin&Shop&Edit&List") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Удалить кальянщика", "Admin&HookahMan&Remove&List") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Удалить заведение", "Admin&Shop&Remove&List") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Сделать рассылку", "Admin&Newsletter") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Отправить сообщение пользователю", "Admin&SendPMessage&List") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Заблокировать пользователя", "Admin&Ban&List") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Разблокировать пользователя", "Admin&Unban&List") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Редактировать тексты", "Admin&EditTexts&List") });
            }
            if (lvlperm == 2)
            {
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Добавить администратора", "Admin&AddAdmin&List") });
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Удалить администратора", "Admin&RemoveAdmin&List") });
            }
            return Keyboard.ToArray();
        }

        public static InlineKeyboardMarkup HookahManEdit(string id)
        {
            var Keyboard = new List<List<InlineKeyboardButton>>();
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Уникальный номер", $"Admin&HookahMan&Edit&Id&{id}") });
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Имя", $"Admin&HookahMan&Edit&Name&{id}") });
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Фото", $"Admin&HookahMan&Edit&Photo&{id}") });
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Назад", "Admin&HookahMan&List") });
            return Keyboard.ToArray();

        }

        public static InlineKeyboardMarkup ShopEditList(string id)
        {
            var Keyboard = new List<List<InlineKeyboardButton>>();
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Название", $"Admin&Shop&Edit&Name&{id}") });
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Описание", $"Admin&Shop&Edit&About&{id}") });
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Контакты", $"Admin&Shop&Edit&Contacts&{id}") });
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Фото", $"Admin&Shop&Edit&Photo&{id}") });
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Назад", "Admin&Shop&List") });
            return Keyboard.ToArray();
        }

        public static InlineKeyboardMarkup UserList(string Act)
        {
            var Keyboard = new List<List<InlineKeyboardButton>>();
            var list = Types.User.UserBase;
            for (int i = 0; i < list.Count; i++)
            {
                Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData($"{list[i].Id}", $"Admin&{Act}&Select&{list[i].Id}") });
            }
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Назад", "Admin&MainMenu") });
            return Keyboard.ToArray();
        }
        public static InlineKeyboardMarkup ToBanList()
        {
            var Keyboard = new List<List<InlineKeyboardButton>>();
            var list = Types.User.UserBase;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].LevelPermission == 0)
                    Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData($"{list[i].Id}", $"Admin&Ban&Select&{list[i].Id}") });
            }
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Назад", "Admin&MainMenu") });
            return Keyboard.ToArray();
        }

        public static InlineKeyboardMarkup ToUnbanList()
        {
            var Keyboard = new List<List<InlineKeyboardButton>>();
            var list = Types.User.UserBase;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].LevelPermission == -1)
                    Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData($"{list[i].Id}", $"Admin&Unban&Select&{list[i].Id}") });
            }
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Назад", "Admin&MainMenu") });
            return Keyboard.ToArray();
        }

        public static InlineKeyboardMarkup TextsList()
        {
            var Keyboard = new List<List<InlineKeyboardButton>>();
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("О Нас", $"Admin&EditTexts&Select&AboutUs") });
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Сообщение о блокировке", $"Admin&EditTexts&Select&BanMessage") });
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Назад", "Admin&MainMenu") });
            return Keyboard.ToArray();
        }



        public static InlineKeyboardMarkup ToAdminList()
        {
            var Keyboard = new List<List<InlineKeyboardButton>>();
            var list = Types.User.UserBase;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].LevelPermission == 0)
                    Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData($"{list[i].Id}", $"Admin&AddAdmin&Select&{list[i].Id}") });
            }
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Назад", "Admin&MainMenu") });
            return Keyboard.ToArray();
        }


        public static InlineKeyboardMarkup FromAdminList()
        {
            var Keyboard = new List<List<InlineKeyboardButton>>();
            var list = Types.User.UserBase;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].LevelPermission == 1)
                    Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData($"{list[i].Id}", $"Admin&RemoveAdmin&Select&{list[i].Id}") });
            }
            Keyboard.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Назад", "Admin&MainMenu") });
            return Keyboard.ToArray();
        }
    }
}