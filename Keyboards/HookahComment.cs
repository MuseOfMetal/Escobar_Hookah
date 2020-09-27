using Telegram.Bot.Types.ReplyMarkups;
namespace Keyboards
{
    class HookahComment
    {
        public static InlineKeyboardMarkup Comment(string id, string rate)
        {
            return new InlineKeyboardMarkup
            (
                new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Пропустить", $"RateHookah&Comment&Skip&{id}&{rate}"),
                        InlineKeyboardButton.WithCallbackData("Отправить комментарий",$"RateHookah&Comment&Send&{id}&{rate}")
                    }
                }
            );
        }
    }
}