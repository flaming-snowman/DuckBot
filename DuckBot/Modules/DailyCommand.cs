using System;
using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace DuckBot.Modules
{
    public class DailyCommand : InteractiveBase
    {
        private static Dictionary<ulong, Tuple<int, string>> DailyStreak = new Dictionary<ulong, Tuple<int, string>>();
        private static string currentdate;
        Random random = new Random();
        EmbedBuilder dailybuilder = new EmbedBuilder();
        [Command("daily", RunMode = RunMode.Async)]
        public async Task DailyAsync()
        {
            if (Context.Channel.Name != "duck-commands")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"duck-commands\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "duck-commands").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            ulong userid = Context.User.Id;
            bool cont = true;
            if (!DailyStreak.ContainsKey(userid))
            {
                DailyStreak.Add(userid, Tuple.Create(1, currentdate));
                cont = false;
            }
            if (cont)
            {
                if (DailyStreak[userid].Item2 == currentdate)
                {
                    await ReplyAsync($"{Context.User.Mention}, You have already run the daily command today, please try again tomorrow.");
                    return;
                }
            }

			int streak = DailyStreak[userid].Item1;
            Tuple<int, long> moneyearned = AddMoney(streak, userid, currentdate);

			string name = Context.Guild.GetUser(userid).Nickname;
            if (name == null)
            {
                name = Context.User.Username;
            }

			if(DateTime.Today.Month == 12 && DateTime.Today.Day == 25)
			{
				ReaperDataStorage.AddBalance(userid, 2500);

				dailybuilder.WithTitle($"Merry Christmas {name}!")
                        .AddField("Streak", $"You now have a streak of {streak}. Type `d.help daily` for more info")
                        .AddField("Merry Christmas!", "You have earned a bonus $2500 as a Christmas gift. Tell your friends in case they forget. Have a wonderful Christmas!")
                        .AddField($"Your standard streak earned ${moneyearned.Item1}", $"You now have ${moneyearned.Item2} total (including the Christmas gift).")
                        .WithColor(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
                await ReplyAsync($"{Context.User.Mention}, Merry Christmas!", false, dailybuilder.Build());

				return;
			}
         
            dailybuilder.WithTitle($"{name} has collected their daily reward!")
                        .AddField("Streak", $"You now have a streak of {streak}. Type `d.help daily` for more info")
                        .AddField("Reminders:", "Remember to submit stories for a chance to win roles and money. The garden is a great way to earn money.")
                        .AddField($"You earned ${moneyearned.Item1}", $"You now have ${moneyearned.Item2}")
                        .WithColor(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
            await ReplyAsync($"{Context.User.Mention}, you have collected your daily reward.", false, dailybuilder.Build());
        }
        internal static void ChangeStreaks(string prevdate, string date)
        {
            if (!ValidateExistence(@"/DuckBot/DailyStreak.json")) return;
            string Streakjson = File.ReadAllText(@"/DuckBot/DailyStreak.json");
            DailyStreak = JsonConvert.DeserializeObject<Dictionary<ulong, Tuple<int, string>>>(Streakjson);
            currentdate = date;
            if (prevdate == date)
                return;
            List<ulong> keys = new List<ulong>(DailyStreak.Keys);
            foreach (ulong key in keys)
            {
                Tuple<int, string> value = DailyStreak[key];
                if (value.Item2 == date)
                    continue;
                if (value.Item2 != prevdate)
                {
                    DailyStreak[key] = Tuple.Create(1, prevdate);
                    continue;
                }
                DailyStreak[key] = Tuple.Create(value.Item1 + 1, prevdate);
            }
            SaveStreak();
        }
        private static void SaveStreak()
        {
            string Streakjson = JsonConvert.SerializeObject(DailyStreak, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/DailyStreak.json", Streakjson);
        }
        private static bool ValidateExistence(string file)
        {
            if (!File.Exists(file))
            {
                if (file == @"/DuckBot/DailyStreak.json")
                {
                    File.WriteAllText(file, "");
                    SaveStreak();
                    return false;
                }
            }
            return true;
        }
        private static Tuple<int, long> AddMoney(int Streak, ulong userid, string date)
        {
            int money = new int();
            if (Streak == 1)
                money = 5;
            else if (Streak == 2)
                money = 10;
            else if (Streak == 3)
                money = 25;
            else if (Streak == 4)
                money = 50;
            else if (Streak == 5)
                money = 75;
            else if (Streak == 6)
                money = 100;
            else if (Streak > 6)
                money = 250;
			ReaperDataStorage.AddBalance(userid, money);
            DailyStreak[userid] = Tuple.Create(DailyStreak[userid].Item1, date);
            SaveStreak();
			return Tuple.Create(money, ReaperDataStorage.GetBalance(userid));
        }
    }
}
