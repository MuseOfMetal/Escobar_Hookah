using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using BotConfig;
namespace ReviewsTGBot
{
    class Program
    {
        public static TelegramBotClient client;
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Loading texts");
                Types.Texts.Load();
                Print("[ OK ] The texts loaded successful", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                Print("An error occured while loading users DB: " + e.Message, ConsoleColor.Red);
                Console.ReadLine();
                return;
            }
            try
            {
                Console.WriteLine("Loading users DB");
                Types.User.Load();
                Print("[ OK ] Users DB loaded successful", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                Print("An error occured while loading users DB: " + e.Message, ConsoleColor.Red);
                Console.ReadLine();
                return;
            }
            try
            {
                Console.WriteLine("Loading shops DB");
                Types.Shop.Load();
                Print("[ OK ] Shops DB loaded successful", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                Print("An error occured while loading shops DB: " + e.Message, ConsoleColor.Red);
                Console.ReadLine();
                return;
            }
            try
            {
                Console.WriteLine("Loading hookah workers DB");
                Types.HookahMan.Load();
                Print("[ OK ] Hookah Workers DB loaded successful", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                Print("An error occured while loading hookah workers DB: " + e.Message, ConsoleColor.Red);
                Console.ReadLine();
                return;
            }
            try
            {
                Console.WriteLine("Loading config");
                Config.Load();
                Print("[ OK ] Config loaded successful", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                Print("An error occured while loading the configuration: " + e.Message, ConsoleColor.Red);
                Console.ReadLine();
                return;
            }
            try
            {
                Console.WriteLine("Configuring the bot");
                var cfg = Config.GetConfig();
                client = new TelegramBotClient(cfg.BotToken);
                var botme = client.GetMeAsync().GetAwaiter().GetResult();
                client.OnMessage += Controller.Message.MessageController;
                client.OnCallbackQuery += Controller.Callback.CallbackController;
                Print("[ OK ] " + botme.Username + " configured successful", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                Print("An error occured while setting up the bot: " + e.Message, ConsoleColor.Red);
                Console.ReadLine();
                return;
            }
            Print("[ OK ] Bot started", ConsoleColor.Green);
            client.StartReceiving();

            while (true)
            {
                System.Threading.Thread.Sleep(100000000);
            }
        }

        static void Print(string Text, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(Text);
            Console.ResetColor();
        }
    }
}
