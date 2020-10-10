using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace rassilkabot
{
    class Program
    {
        public static TelegramBotClient bot = new TelegramBotClient("1328219094:AAFjFsz80--0N_hT1K1-FRXVpkQvyQdGAWA");
        public static List<UserInfo> users = new List<UserInfo>();
        public static List<UserInfo> admins = new List<UserInfo>();
        static void Main(string[] args)
        {
            admins.Add(new UserInfo { id = 728384906 });
            bot.StartReceiving();
            bot.OnMessage += (sender, e) =>
            {
                checking(e.Message);
            };
            while (true)
            {
                string temp = Console.ReadLine();
                if(temp == "users")
                {
                    for(int i = 0; i <= users.Count - 1; i++) {
                        Console.WriteLine($"{users[i].id}");
                    }
                }
            }
        }
        static async void checking(Message message)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"[{DateTime.Now}] ");
                Console.ResetColor();
                Console.Write($"{message.From.FirstName} {message.From.Username} {message.Chat.Title}: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(message.Text);
                Console.ResetColor();
                if (message.Text == "/start")
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "Здравствуйте, это бот оповещатель. Чтобы подписаться на рассылку оповещений, напишите <b>/subscribe</b>, а чтобы отписаться, напишите <b>/unsubscribe</b>", parseMode: ParseMode.Html);
                }
                if (message.Text == "/subscribe")
                {
                    users.Add(new UserInfo() { id = message.From.Id });
                    await bot.SendTextMessageAsync(message.Chat.Id, "Вы подписались на рассылку оповещений.");
                }
                if (message.Text == "/unsubscribe")
                {
                    users.Remove(users.Find(m => m.id == message.From.Id));
                    await bot.SendTextMessageAsync(message.Chat.Id, "Вы отписались от рассылку оповещений.");
                }
                if (message.Text.StartsWith("/send") && admins.Exists(m=>m.id == message.From.Id))
                {
                    string text = string.Join(" ",message.Text.Split(" ").Skip(1));
                    for(int i = 0; i<= users.Count - 1; i++)
                    {
                        await bot.SendTextMessageAsync(users[i].id, text);
                    }
                    await bot.SendTextMessageAsync(message.Chat.Id, $"Оповещение было отправлено: {users.Count} пользователям");
                }
                if(message.Text.StartsWith("/add") && admins.Exists(m => m.id == message.From.Id)){
                    admins.Add(new UserInfo { id = long.Parse(message.Text.Split(" ")[1]) });
                }
            }
            catch (Exception) { }
        }
    }
    class UserInfo
    {
        public long id;
    }
}
