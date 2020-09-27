using Keyboards;
using System;
using System.IO;
using Types;
using Telegram.Bot.Args;
using Telegram.Bot;

namespace Controller
{
    class Message
    {
        static TelegramBotClient client = ReviewsTGBot.Program.client;
        public static async void MessageController(object Sender, MessageEventArgs e)
        {
            try
            {
                var Msg = e.Message;
                var MsgFrom = e.Message.From;
                var UserIndex = User.FindUser(MsgFrom.Id);
                if (UserIndex == -1)
                {
                    if (Msg.From.Id == BotConfig.Config.GetConfig().SuperAdminId)
                        User.AddUser(MsgFrom.Id, MsgFrom.FirstName, MsgFrom.LastName, MsgFrom.Username, 2, out UserIndex);
                    else
                        User.AddUser(MsgFrom.Id, MsgFrom.FirstName, MsgFrom.LastName, MsgFrom.Username, 0, out UserIndex);
                    Types.User.Save();
                }

                else
                    User.UserBase[UserIndex].UpdateUser(MsgFrom.FirstName, MsgFrom.LastName, MsgFrom.Username);
                var CurrentUser = User.UserBase[UserIndex];
                System.Console.WriteLine($"{MsgFrom.FirstName} {MsgFrom.LastName} {MsgFrom.Username} Send Message: [{Msg.Text}] with perm lvl [{CurrentUser.LevelPermission}]");

                if (CurrentUser.LevelPermission >= 0)
                {



                    if (string.IsNullOrEmpty(CurrentUser.Status))
                    {
                        switch (Msg.Text)
                        {
                            case "/start":
                                using (FileStream stream = new FileStream(@"Images/MainMenu.png", FileMode.Open, FileAccess.Read))
                                {
                                    await client.SendPhotoAsync(MsgFrom.Id, stream, replyMarkup: MainMenu.Menu);
                                }
                                break;
                            case "/admin":
                                if (CurrentUser.LevelPermission > 0)
                                {
                                    await client.SendTextMessageAsync(Msg.From.Id, "Добро пожаловать в админ меню!", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(MsgFrom.Id, "Недостаточно прав.");
                                }
                                break;
                            case "/set":
                                BotConfig.Config.GetConfig().ReviewsGroupId = Msg.Chat.Id;
                                BotConfig.Config.Save();
                                await client.SendTextMessageAsync(Msg.Chat.Id, "Группа для получения комментариев успешно установлена");
                                break;
                        }
                    }
                    else
                    {
                        var Args = CurrentUser.Status.Split('&');
                        switch (Args[0])
                        {
                            case "RateHookah":
                                switch (Args[1])
                                {
                                    case "Send":
                                        var CurrentHookahMan = HookahMan.HookahWorkersBase[HookahMan.FindHookahMan(Args[2])];
                                        string Text4Send = "*Новый комментарий*\n\n";
                                        Text4Send += "<Кальянщик>\n";
                                        Text4Send += $"Имя: {CurrentHookahMan.Name}\n";
                                        Text4Send += $"Номер: {CurrentHookahMan.Id}\n";
                                        Text4Send += $"Оценка: {Args[3]}\n";
                                        if (!string.IsNullOrWhiteSpace(Msg.Text) && Msg.Text != "/skip")
                                            Text4Send += $"Комментарий: {Msg.Text}\n";
                                        Text4Send += "\n\n";
                                        Text4Send += $"<Информация о пользователе>\n";
                                        Text4Send += $"Имя: {CurrentUser.FName}\n";
                                        if (!string.IsNullOrEmpty(CurrentUser.LName))
                                            Text4Send += $"Фамилия: {CurrentUser.LName}\n";
                                        if (!string.IsNullOrEmpty(CurrentUser.UName))
                                            Text4Send += $"Никнэйм: @{CurrentUser.UName}\n";
                                        Text4Send += $"ID: {CurrentUser.Id}\n";
                                        await client.SendTextMessageAsync(BotConfig.Config.GetConfig().ReviewsGroupId, Text4Send);
                                        Types.User.UserBase[UserIndex].Status = null;
                                        try { await client.DeleteMessageAsync(int.Parse(Args[4]), int.Parse(Args[5])); } catch { }
                                        await client.SendTextMessageAsync(Msg.From.Id, "Спасибо за ваш отзыв! Ждем вас снова", replyMarkup: Keyboards.MainMenu.BackMU);
                                        break;
                                }
                                break;
                            case "Admin":
                                if (Msg.Text == "/Cancel")
                                {
                                    Types.User.UserBase[UserIndex].Status = null;
                                    await client.SendTextMessageAsync(Msg.From.Id, "Отменено");
                                    return;
                                }
                                switch (Args[1])
                                {
                                    case "HookahMan":
                                        switch (Args[2])
                                        {
                                            case "Add":
                                                {
                                                    switch (Args[3])
                                                    {
                                                        case "1":
                                                            Types.User.UserBase[UserIndex].Status = $"Admin&HookahMan&Add&2&{Msg.Text}";
                                                            await client.SendTextMessageAsync(Msg.From.Id, "Теперь введите имя кальянщика (Для отмены введите (/Cancel))");
                                                            Types.User.Save();
                                                            break;
                                                        case "2":
                                                            Types.User.UserBase[UserIndex].Status = $"Admin&HookahMan&Add&3&{Args[4]}&{Msg.Text}";
                                                            await client.SendTextMessageAsync(Msg.From.Id, "Теперь отправьте фото кальянщика (Для отмены введите (/Cancel))");
                                                            Types.User.Save();
                                                            break;
                                                        case "3":
                                                            if (Msg.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                                                            {
                                                                Types.HookahMan.AddHookahMan(Args[4], Args[5]);
                                                                using (FileStream stream = new FileStream($"Images/HookahWorkers/{Args[4]}.png", FileMode.Create))
                                                                {
                                                                    await client.GetInfoAndDownloadFileAsync(Msg.Photo[Msg.Photo.Length - 1].FileId, stream);
                                                                }
                                                                Types.User.UserBase[UserIndex].Status = null;
                                                                Types.User.Save();
                                                                Types.HookahMan.Save();
                                                                await client.SendTextMessageAsync(Msg.From.Id, "Кальянщик успешно добавлен", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                            }
                                                            break;
                                                    }

                                                }
                                                break;
                                            case "Edit":
                                                switch (Args[3])
                                                {
                                                    case "Id":
                                                        File.Move($"Images/HookahWorkers/{Types.HookahMan.HookahWorkersBase[Types.HookahMan.FindHookahMan(Args[4])].Id}.png", $"Images/HookahWorkers/{Msg.Text}.png");
                                                        Types.HookahMan.HookahWorkersBase[Types.HookahMan.FindHookahMan(Args[4])].Id = Msg.Text;
                                                        Types.User.UserBase[UserIndex].Status = null;
                                                        Types.HookahMan.Save();
                                                        await client.SendTextMessageAsync(Msg.From.Id, "Уникальный номер кальянщика обновлен", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                        break;
                                                    case "Name":
                                                        Types.HookahMan.HookahWorkersBase[Types.HookahMan.FindHookahMan(Args[4])].Name = Msg.Text;
                                                        Types.User.UserBase[UserIndex].Status = null;
                                                        Types.HookahMan.Save();
                                                        await client.SendTextMessageAsync(Msg.From.Id, "Имя кальянщика обновлено", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                        break;
                                                    case "Photo":
                                                        if (Msg.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                                                        {
                                                            using (FileStream stream = new FileStream($"Images/HookahWorkers/{Args[4]}.png", FileMode.OpenOrCreate))
                                                            {
                                                                await client.GetInfoAndDownloadFileAsync(Msg.Photo[Msg.Photo.Length - 1].FileId, stream);
                                                            }
                                                            Types.User.UserBase[UserIndex].Status = null;
                                                            Types.HookahMan.Save();
                                                            await client.SendTextMessageAsync(Msg.From.Id, "Фото кальянщика обновлено", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Shop":
                                        switch (Args[2])
                                        {
                                            case "Add":
                                                switch (Args[3])
                                                {
                                                    case "1":
                                                        Types.User.UserBase[UserIndex].Status = $"Admin&Shop&Add&2&{Msg.Text}";
                                                        await client.SendTextMessageAsync(Msg.From.Id, "Теперь введите описание заведения (Для отмены введите (/Cancel))");
                                                        Types.User.Save();
                                                        break;
                                                    case "2":
                                                        Types.User.UserBase[UserIndex].Status = $"Admin&Shop&Add&3&{Args[4]}&{Msg.Text}";
                                                        await client.SendTextMessageAsync(Msg.From.Id, "Теперь введите контакты заведения (Для отмены введите (/Cancel))");
                                                        Types.User.Save();
                                                        break;
                                                    case "3":
                                                        Types.User.UserBase[UserIndex].Status = $"Admin&Shop&Add&4&{Args[4]}&{Args[5]}&{Msg.Text}";
                                                        await client.SendTextMessageAsync(Msg.From.Id, "Теперь отправьте фото заведения (Для отмены введите (/Cancel))");
                                                        Types.User.Save();
                                                        break;
                                                    case "4":
                                                        if (Msg.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                                                        {
                                                            Types.Shop.AddShop(Args[4], Args[5], Args[6]);
                                                            using (FileStream stream = new FileStream($"Images/Shops/{Args[4]}.png", FileMode.Create))
                                                            {
                                                                await client.GetInfoAndDownloadFileAsync(Msg.Photo[Msg.Photo.Length - 1].FileId, stream);
                                                            }
                                                            Types.User.UserBase[UserIndex].Status = null;
                                                            Types.User.Save();
                                                            Types.Shop.Save();
                                                            await client.SendTextMessageAsync(Msg.From.Id, "Заведение успешно добавлено", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                        }
                                                        break;
                                                }
                                                break;
                                            case "Edit":
                                                switch (Args[3])
                                                {
                                                    case "Name":
                                                        File.Move($"Images/Shops/{Types.Shop.ShopsBase[Types.Shop.FindShop(Args[4])].Name}.png", $"Images/Shops/{Msg.Text}.png");
                                                        Types.Shop.ShopsBase[Types.Shop.FindShop(Args[4])].Name = Msg.Text;
                                                        Types.User.UserBase[UserIndex].Status = null;
                                                        Types.Shop.Save();
                                                        await client.SendTextMessageAsync(Msg.From.Id, "Название завадения обновлено", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                        break;
                                                    case "About":
                                                        Types.Shop.ShopsBase[Types.Shop.FindShop(Args[4])].About = Msg.Text;
                                                        Types.User.UserBase[UserIndex].Status = null;
                                                        Types.Shop.Save();
                                                        await client.SendTextMessageAsync(Msg.From.Id, "Описание заведения обновлено", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                        break;
                                                    case "Contacts":
                                                        Types.Shop.ShopsBase[Types.Shop.FindShop(Args[4])].Phone = Msg.Text;
                                                        Types.User.UserBase[UserIndex].Status = null;
                                                        Types.Shop.Save();
                                                        await client.SendTextMessageAsync(Msg.From.Id, "Контакты заведения обновлены", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                        break;
                                                    case "Photo":
                                                        if (Msg.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                                                        {
                                                            using (FileStream stream = new FileStream($"Images/Shops/{Args[4]}.png", FileMode.OpenOrCreate))
                                                            {
                                                                await client.GetInfoAndDownloadFileAsync(Msg.Photo[Msg.Photo.Length - 1].FileId, stream);
                                                            }
                                                            Types.User.UserBase[UserIndex].Status = null;
                                                            Types.Shop.Save();
                                                            await client.SendTextMessageAsync(Msg.From.Id, "Фото заведения обновлено", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                        }
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "Newsletter":
                                        {
                                            int Total = 0;
                                            int Success = 0;
                                            int Fail = 0;

                                            var list = Types.User.UserBase;
                                            await client.SendTextMessageAsync(Msg.From.Id, "Рассылка запущена. По окончанию рассылки вы получите статистику");
                                            foreach (var Item in list)
                                            {
                                                try
                                                {
                                                    await client.SendTextMessageAsync(Item.Id, Msg.Text);
                                                    Success++;
                                                }
                                                catch
                                                {
                                                    Fail++;
                                                }
                                                Total++;
                                            }
                                            Types.User.UserBase[UserIndex].Status = null;
                                            await client.SendTextMessageAsync(Msg.From.Id, $"Рассылка завершена!\n\nСтатистика\n\nВсего: {Total}\nУспешно отправлено: {Success}\nНеудачно: {Fail}");
                                        }
                                        break;
                                    case "SendPMessage":
                                        try
                                        {
                                            await client.SendTextMessageAsync(int.Parse(Args[2]), "<<Сообщение от Escobar Hookah>>\n\n" + Msg.Text);
                                            await client.SendTextMessageAsync(MsgFrom.Id, "Успешно отправлено");
                                        }
                                        catch
                                        {
                                            await client.SendTextMessageAsync(MsgFrom.Id, "Невозможно отправить сообщение данному пользователю");
                                        }
                                        Types.User.UserBase[UserIndex].Status = null;
                                        break;
                                    case "EditTexts":
                                        switch (Args[2])
                                        {
                                            case "AboutUs":
                                                Types.Texts.GetTexts().AboutUs = Msg.Text;
                                                Types.User.UserBase[UserIndex].Status = null;
                                                await client.SendTextMessageAsync(Msg.From.Id, "Раздел обновлен");
                                                break;
                                            case "BanMessage":
                                                Types.Texts.GetTexts().BanMessage = Msg.Text;
                                                Types.User.UserBase[UserIndex].Status = null;
                                                await client.SendTextMessageAsync(Msg.From.Id, "Раздел обновлен");
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    await client.SendTextMessageAsync(Msg.From.Id, Texts.GetTexts().BanMessage);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
    class Callback
    {
        static TelegramBotClient client = ReviewsTGBot.Program.client;
        public static async void CallbackController(object Sender, CallbackQueryEventArgs e)
        {
            try
            {


                var Cb = e.CallbackQuery;
                var CbFrom = e.CallbackQuery.From;
                var Msg = e.CallbackQuery.Message;




                var UserIndex = User.FindUser(CbFrom.Id);
                if (UserIndex == -1)
                {
                    User.AddUser(CbFrom.Id, CbFrom.FirstName, CbFrom.LastName, CbFrom.Username, 0, out UserIndex);
                    Types.User.Save();
                }
                else
                    User.UserBase[UserIndex].UpdateUser(CbFrom.FirstName, CbFrom.LastName, CbFrom.Username);
                var CurrentUser = User.UserBase[UserIndex];
                System.Console.WriteLine($"{CbFrom.FirstName} {CbFrom.LastName} {CbFrom.Username} callback: [{Cb.Data.Replace('&', ' ')}] with perm lvl [{CurrentUser.LevelPermission}]");



                if (CurrentUser.LevelPermission >= 0)
                {
                    var Args = Cb.Data.Split('&');

                    switch (Args[0])
                    {
                        case "RateHookah":
                            switch (Args[1])
                            {
                                case "List":
                                    try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                    await client.SendTextMessageAsync(CbFrom.Id, "Выберите кальянщика по его номеру", replyMarkup: HookahWorkersList.List4User);
                                    break;
                                case "Select":
                                    {
                                        var CurrentHookahMan = HookahMan.HookahWorkersBase[HookahMan.FindHookahMan(Args[2])];
                                        try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                        if (File.Exists($@"Images/HookahWorkers/{Args[2]}.png"))
                                            using (FileStream stream = new FileStream($@"Images/HookahWorkers/{Args[2]}.png", FileMode.Open, FileAccess.Read))
                                            {
                                                await client.SendPhotoAsync(CbFrom.Id, stream, caption: CurrentHookahMan.Name, replyMarkup: Keyboards.RateHookahMan.Rate(Args[2]));
                                            }
                                        else
                                            await client.SendTextMessageAsync(Cb.From.Id, CurrentHookahMan.Name, replyMarkup: Keyboards.RateHookahMan.Rate(Args[2]));
                                    }
                                    break;
                                case "Rate":
                                    try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                    await client.SendTextMessageAsync(Cb.From.Id, "Ваша оценка утчена, напишите дополнительно коментарий", replyMarkup: Keyboards.HookahComment.Comment(Args[2], Args[3]));
                                    break;
                                case "Comment":
                                    switch (Args[2])
                                    {
                                        case "Skip":
                                            var CurrentHookahMan = HookahMan.HookahWorkersBase[HookahMan.FindHookahMan(Args[3])];
                                            string Text4Send = "*Новый комментарий*\n\n";
                                            Text4Send += "<Кальянщик>\n";
                                            Text4Send += $"Имя: {CurrentHookahMan.Name}\n";
                                            Text4Send += $"Номер: {CurrentHookahMan.Id}\n";
                                            Text4Send += $"Оценка: {Args[4]}\n\n\n";
                                            Text4Send += $"<Информация о пользователе>\n";
                                            Text4Send += $"Имя: {CurrentUser.FName}\n";
                                            if (!string.IsNullOrEmpty(CurrentUser.LName))
                                                Text4Send += $"Фамилия: {CurrentUser.LName}\n";
                                            if (!string.IsNullOrEmpty(CurrentUser.UName))
                                                Text4Send += $"Никнэйм: @{CurrentUser.UName}\n";
                                            Text4Send += $"ID: {CurrentUser.Id}\n";
                                            await client.SendTextMessageAsync(BotConfig.Config.GetConfig().ReviewsGroupId, Text4Send);

                                            try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                            await client.SendTextMessageAsync(Cb.From.Id, "Спасибо за ваш отзыв! Ждем вас снова", replyMarkup: Keyboards.MainMenu.BackMU);
                                            break;
                                        case "Send":
                                            Types.User.UserBase[UserIndex].Status = $"RateHookah&Send&{Args[3]}&{Args[4]}&{Msg.Chat.Id}&{Msg.MessageId}";
                                            try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                            await client.SendTextMessageAsync(Cb.From.Id, "Напишите ваш отзыв. (Если передумали, напишите /skip)");
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case "MainMenu":
                            try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                            using (FileStream stream = new FileStream(@"Images/MainMenu.png", FileMode.Open, FileAccess.Read))
                            {
                                await client.SendPhotoAsync(CbFrom.Id, stream, replyMarkup: MainMenu.Menu);
                            }
                            break;
                        case "AboutUs":
                            switch (Args[1])
                            {
                                case "Main":
                                    try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                    if (File.Exists(@"Images/TeamPhoto.png"))
                                        using (FileStream stream = new FileStream("Images/TeamPhoto.png", FileMode.Open, FileAccess.Read))
                                        {
                                            await client.SendPhotoAsync(Cb.From.Id, stream, caption: Texts.GetTexts().AboutUs, replyMarkup: Keyboards.AboutUs.Main);
                                        }
                                    else
                                        await client.SendTextMessageAsync(Cb.From.Id, Texts.GetTexts().AboutUs, replyMarkup: Keyboards.AboutUs.Main);
                                    break;
                                case "Select":
                                    var CurrentShop = Types.Shop.ShopsBase[Types.Shop.FindShop(Args[2])];
                                    try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                    if (File.Exists($@"Images/Shops/{CurrentShop.Name}.png"))
                                        using (FileStream stream = new FileStream($@"Images/Shops/{CurrentShop.Name}.png", FileMode.Open, FileAccess.Read))
                                        {
                                            await client.SendPhotoAsync(Cb.From.Id, stream, caption: CurrentShop.About, replyMarkup: Keyboards.AboutUs.ReserveTable(Args[2]));
                                        }
                                    else
                                        await client.SendTextMessageAsync(Cb.From.Id, CurrentShop.Name + "\n\n" + CurrentShop.About, replyMarkup: Keyboards.AboutUs.ReserveTable(Args[2]));
                                    break;
                                case "Reserve":
                                    await client.SendTextMessageAsync(Cb.From.Id, Types.Shop.ShopsBase[Types.Shop.FindShop(Args[2])].Phone);
                                    break;
                            }
                            break;
                        case "Admin":
                            if (CurrentUser.LevelPermission > 0)
                            {
                                switch (Args[1])
                                {
                                    case "HookahMan":
                                        switch (Args[2])
                                        {
                                            case "Add":
                                                {
                                                    Types.User.UserBase[UserIndex].Status = "Admin&HookahMan&Add&1";
                                                    await client.SendTextMessageAsync(Cb.From.Id, "Введите уникальный номер кальянщика (Для отмены введите (/Cancel))");
                                                }
                                                break;
                                            case "Edit":
                                                {
                                                    switch (Args[3])
                                                    {
                                                        case "List":
                                                            try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Список кальянщиков", replyMarkup: Keyboards.HookahWorkersList.List4Admin("Edit"));
                                                            break;
                                                        case "Select":
                                                            try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Что нужно отредактировать?", replyMarkup: Keyboards.Admin.HookahManEdit(Args[4]));
                                                            break;
                                                        case "Id":
                                                            Types.User.UserBase[UserIndex].Status = $"Admin&HookahMan&Edit&Id&{Args[4]}";
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Введите новый уникальный номер кальянщика (Для отмены введите (/Cancel))");
                                                            break;
                                                        case "Name":
                                                            Types.User.UserBase[UserIndex].Status = $"Admin&HookahMan&Edit&Name&{Args[4]}";
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Введите новое имя кальянщика (Для отмены введите (/Cancel))");
                                                            break;
                                                        case "Photo":
                                                            Types.User.UserBase[UserIndex].Status = $"Admin&HookahMan&Edit&Photo&{Args[4]}";
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Отправьте новое фото кальянщика (Для отмены введите (/Cancel))");
                                                            break;
                                                    }
                                                }
                                                break;
                                            case "Remove":
                                                switch (Args[3])
                                                {
                                                    case "List":
                                                        try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                        await client.SendTextMessageAsync(Cb.From.Id, "Список кальянщиков", replyMarkup: Keyboards.HookahWorkersList.List4Admin("Remove"));
                                                        break;
                                                    case "Select":
                                                        Types.HookahMan.DeleteHookahMan(Args[4]);
                                                        if (File.Exists($"Images/HookahWorkers/{Args[4]}.png"))
                                                            File.Delete($"Images/HookahWorkers/{Args[4]}.png");
                                                        Types.HookahMan.Save();
                                                        try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                        await client.SendTextMessageAsync(Cb.From.Id, "Кальянщик удален", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                        break;
                                                }

                                                break;
                                        }
                                        break;
                                    case "Shop":
                                        switch (Args[2])
                                        {
                                            case "Add":
                                                {
                                                    Types.User.UserBase[UserIndex].Status = "Admin&Shop&Add&1";
                                                    await client.SendTextMessageAsync(Cb.From.Id, "Введите название заведения (Для отмены введите (/Cancel))");
                                                }
                                                break;
                                            case "Edit":
                                                {
                                                    switch (Args[3])
                                                    {
                                                        case "List":
                                                            try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Список заведений", replyMarkup: Keyboards.AboutUs.List("Edit"));
                                                            break;
                                                        case "Select":
                                                            try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Что нужно отредактировать?", replyMarkup: Keyboards.Admin.ShopEditList(Args[4]));
                                                            break;
                                                        case "Name":
                                                            Types.User.UserBase[UserIndex].Status = $"Admin&Shop&Edit&Name&{Args[4]}";
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Введите новое название заведения (Для отмены введите (/Cancel))");
                                                            break;
                                                        case "About":
                                                            Types.User.UserBase[UserIndex].Status = $"Admin&Shop&Edit&About&{Args[4]}";
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Введите новое описание заведения (Для отмены введите (/Cancel))");
                                                            break;
                                                        case "Contacts":
                                                            Types.User.UserBase[UserIndex].Status = $"Admin&Shop&Edit&Contacts&{Args[4]}";
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Введите новые контакты заведения (Для отмены введите (/Cancel))");
                                                            break;
                                                        case "Photo":
                                                            Types.User.UserBase[UserIndex].Status = $"Admin&Shop&Edit&Photo&{Args[4]}";
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Отправьте новое фото заведения (Для отмены введите (/Cancel))");
                                                            break;
                                                    }
                                                }
                                                break;
                                            case "Remove":
                                                {
                                                    switch (Args[3])
                                                    {
                                                        case "List":
                                                            try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Список заведений", replyMarkup: Keyboards.AboutUs.List("Remove"));
                                                            break;
                                                        case "Select":
                                                            Types.Shop.RemoveShop(Args[4]);
                                                            if (File.Exists($"Images/Shops/{Args[4]}.png"))
                                                                File.Delete($"Images/Shops/{Args[4]}.png");
                                                            Types.Shop.Save();
                                                            try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                            await client.SendTextMessageAsync(Cb.From.Id, "Заведение удалено", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                            break;
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                    case "MainMenu":
                                        try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                        await client.SendTextMessageAsync(Cb.From.Id, "Админ меню", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                        break;
                                    case "Newsletter":
                                        Types.User.UserBase[UserIndex].Status = "Admin&Newsletter";
                                        await client.SendTextMessageAsync(Cb.From.Id, "Введите ваше сообщение для рассылки (Для отмены введите /Cancel)");
                                        break;
                                    case "SendPMessage":
                                        switch (Args[2])
                                        {
                                            case "List":
                                                try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                await client.SendTextMessageAsync(Cb.From.Id, "Выберите пользователя для отправки приватного сообщения", replyMarkup: Keyboards.Admin.UserList("SendPMessage"));
                                                break;
                                            case "Select":
                                                Types.User.UserBase[UserIndex].Status = $"Admin&SendPMessage&{Args[3]}";
                                                await client.SendTextMessageAsync(Cb.From.Id, "Теперь отправьте сообщение");
                                                break;
                                        }

                                        break;
                                    case "Ban":
                                        switch (Args[2])
                                        {
                                            case "List":
                                                try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                await client.SendTextMessageAsync(Cb.From.Id, "Список не заблокированных пользователей", replyMarkup: Keyboards.Admin.ToBanList());
                                                break;
                                            case "Select":
                                                Types.User.UserBase[Types.User.FindUser(int.Parse(Args[3]))].Ban();
                                                Types.User.Save();
                                                try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                await client.SendTextMessageAsync(Cb.From.Id, "Пользователь заблокирован", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                break;
                                        }
                                        break;
                                    case "Unban":
                                        switch (Args[2])
                                        {
                                            case "List":
                                                try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                await client.SendTextMessageAsync(Cb.From.Id, "Список заблокированных пользователей", replyMarkup: Keyboards.Admin.ToUnbanList());
                                                break;
                                            case "Select":
                                                Types.User.UserBase[Types.User.FindUser(int.Parse(Args[3]))].LevelPermission = 0;
                                                Types.User.Save();
                                                try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                await client.SendTextMessageAsync(Cb.From.Id, "Пользователь разблокирован", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                break;
                                        }
                                        break;
                                    case "EditTexts":
                                        switch (Args[2])
                                        {
                                            case "List":
                                                try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                await client.SendTextMessageAsync(Cb.From.Id, "Какой текст нужно отредактировать", replyMarkup: Keyboards.Admin.TextsList());
                                                break;
                                            case "Select":
                                                switch (Args[3])
                                                {
                                                    case "AboutUs":
                                                        Types.User.UserBase[UserIndex].Status = "Admin&EditTexts&AboutUs";
                                                        await client.SendTextMessageAsync(Cb.From.Id, "Отправьте новый текст для раздела \"О Нас\"");
                                                        break;
                                                    case "BanMessage":
                                                        Types.User.UserBase[UserIndex].Status = "Admin&EditTexts&BanMessage";
                                                        await client.SendTextMessageAsync(Cb.From.Id, "Отправьте новый текст сообщения о блокировке");
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case "AddAdmin":
                                        switch (Args[2])
                                        {
                                            case "List":
                                                try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                await client.SendTextMessageAsync(Cb.From.Id, "Выберите пользователя", replyMarkup: Keyboards.Admin.ToAdminList());
                                                break;
                                            case "Select":
                                                Types.User.UserBase[Types.User.FindUser(int.Parse(Args[3]))].LevelPermission = 1;
                                                await client.SendTextMessageAsync(Cb.From.Id, "Пользователь успешно добавлен в администраторы", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                break;
                                        }
                                        break;
                                    case "RemoveAdmin":
                                        switch (Args[2])
                                        {
                                            case "List":
                                                try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                                                await client.SendTextMessageAsync(Cb.From.Id, "Выберите пользователя", replyMarkup: Keyboards.Admin.FromAdminList());
                                                break;
                                            case "Select":
                                                Types.User.UserBase[Types.User.FindUser(int.Parse(Args[3]))].LevelPermission = 0;
                                                await client.SendTextMessageAsync(Cb.From.Id, "Пользователь успешно удален из администраторов", replyMarkup: Keyboards.Admin.Main(CurrentUser.LevelPermission));
                                                break;
                                        }
                                        break;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    try { await client.DeleteMessageAsync(Msg.Chat.Id, Msg.MessageId); } catch { }
                    await client.SendTextMessageAsync(Cb.From.Id, Texts.GetTexts().BanMessage);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}