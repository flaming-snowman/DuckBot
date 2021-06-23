using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.WebSocket;
using Discord.Rest;
using DuckBot.Modules;
using Newtonsoft.Json;

namespace DuckBot
{
    internal static class Clock
    {
        private static Stopwatch duckstopwatch;
        private static Stopwatch ReaperTimer;
        private static Stopwatch VCstopwatch;
        private static Timer PeriodicMessage;
        private static double ducktime = new double();
        private static double duckseconds = new double();
        private static double vctime = new double();
        private static double vcseconds = new double();
        private static long lastreap = 0;
        private static long time = new long();
        private static string prevdate;
        private static string date;
        private static DateTime dateTime;
        private static SocketGuild guild;
        private static SocketTextChannel logchnl;
        private static RestUserMessage deletemsg;
		internal static List<string> Holidays = new List<string>();

        internal static async Task IntializeTimers(SocketGuild socketGuild)
        {
            guild = socketGuild;
            duckstopwatch = new Stopwatch();
            ReaperTimer = new Stopwatch();
            VCstopwatch = new Stopwatch();
            duckstopwatch.Start();
            ReaperTimer.Start();
            VCstopwatch.Start();
            logchnl = guild.TextChannels.FirstOrDefault(x => x.Name == "staff-logs");
            var harvestchnl = guild.TextChannels.FirstOrDefault(x => x.Name == "harvest-simulator");
            if (Global.harvesthours && Global.gamestart)
            {
                EmbedBuilder restartmsg = new EmbedBuilder();
                restartmsg.WithTitle("A swarm of locusts devoured all the corn. Farmers are outraged and are willing to harvest immediately.")
                    .WithDescription("Crops will have to regrow. Everyone now has an available harvest.")
                    .WithThumbnailUrl("https://cdn.discordapp.com/attachments/427572233100328962/432674934074900480/corn-stalk-clipart-transparent-background-1.jpg.png")
                    .WithTimestamp(DateTime.UtcNow)
                    .WithFooter("Event occurred: ")
                    .WithColor(Color.DarkGreen);

                await harvestchnl.SendMessageAsync("Locust event.", false, restartmsg.Build());
            }
            await logchnl.SendMessageAsync("Bot restarted");
            lastreap = 0;
			ReaperDataStorage.ClearLastReap();
            Console.WriteLine("Timers Intialized");            
        }
		internal static async Task StartMsgTimer(SocketGuild socketGuild)
        {
			guild = socketGuild;
            logchnl = guild.TextChannels.FirstOrDefault(x => x.Name == "staff-logs");
            var harvestchnl = guild.TextChannels.FirstOrDefault(x => x.Name == "harvest-simulator");           
            deletemsg = await logchnl.SendMessageAsync("Beep Boop");
            PeriodicMessage = new Timer(TimeSpan.FromMinutes(2).TotalMilliseconds);
            PeriodicMessage.Elapsed += KeepAwake;
            PeriodicMessage.AutoReset = true;
            PeriodicMessage.Start();
        }

        private static async void KeepAwake(object sender, ElapsedEventArgs e)
        {
            await deletemsg.DeleteAsync();
            deletemsg = await logchnl.SendMessageAsync("Beep Boop");
        }

        internal static Task BeginDuckTimer()
        {
            duckstopwatch.Start();

            return Task.CompletedTask;
        }
        internal static Task RestartReaperTimer()
        {
            ReaperTimer.Restart();
            lastreap = 0;
            return Task.CompletedTask;
        }
        internal static long GetTime()
        {
            return ReaperTimer.ElapsedMilliseconds;
        }
        internal static double CheckDuckTime()
        {
            ducktime = duckstopwatch.ElapsedMilliseconds;
            duckseconds = 5 - ducktime / 1000;
            if (ducktime >= 5000)
            {
                duckstopwatch.Reset();
                return 0;
            }
            else
                return duckseconds;
        }
        internal static double CheckVCTime()
        {
            vctime = VCstopwatch.ElapsedMilliseconds;
            vcseconds = 5 - vctime / 1000;
            if (vctime >= 5000)
            {
                VCstopwatch.Restart();
                return 0;
            }
            else
				return vcseconds;
        }
        internal static long GetCrops()
        {
            return ReaperTimer.ElapsedMilliseconds - lastreap;
        }
        internal static Tuple<long, long, bool, bool, int> AddReaperScore(ulong ReaperPlayerID)
        {
            time = ReaperTimer.ElapsedMilliseconds;
            var returnvar = ReaperDataStorage.AddNameScore(ReaperPlayerID, time-lastreap, time);
            lastreap = time;
            return returnvar;
        }
        internal static void StartFridayTimer(TimeSpan ts, DateTime IndateTime)
        {
            Timer FridayTimer = new Timer(ts.TotalMilliseconds);
            FridayTimer.AutoReset = false;
            FridayTimer.Start();
			Console.WriteLine($"Timer started for {(DateTime.Now + ts).DayOfWeek}, {DateTime.Now + ts}");
            dateTime = IndateTime;            
            FridayTimer.Elapsed += FridayTimer_Elapsed;
        }
		internal static void StartThursdayTimer(TimeSpan ts)
        {
            Timer ThursdayTimer = new Timer(ts.TotalMilliseconds);
			ThursdayTimer.AutoReset = false;
			ThursdayTimer.Start();
			Console.WriteLine($"Timer started for {(DateTime.Now + ts).DayOfWeek}, {DateTime.Now + ts}");                       
			ThursdayTimer.Elapsed += ThursdayTimer_Elapsed;
        }
        private static async void FridayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
			dateTime = dateTime.AddDays(7);
            await Global.ChangeUtil("weektime", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
			await VoteRecipe.CheckTime();
            await VoteRecipe.MoveRecipes(guild);
        }
		private static async void ThursdayTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			await VoteRecipe.GetWinner(guild);
		}
        internal static void DailyEventsStart()
        {
            if (ValidateExistence(@"/DuckBot/Currentday.json"))
            {
                string datejson = File.ReadAllText(@"/DuckBot/Currentday.json");               
                prevdate = JsonConvert.DeserializeObject<string>(datejson);
                Console.WriteLine($"Date found: {prevdate}");
            }
            date = DateTime.Today.ToString("d");
            if (prevdate != date)
            {
                Storerecipetime.ClearRecipeSubmitted();
				StoreChangeRoles.ClearRoleChanged();
                SaveCurrentDay();
                Console.WriteLine($"Date changed: {date}");
            }
			if(ValidateExistence(@"/DuckBot/Holidays.json"))
			{
				string holidayjson = File.ReadAllText(@"/DuckBot/Holidays.json");
				Holidays = JsonConvert.DeserializeObject<List<string>>(holidayjson);
				SaveHolidays();
			}
			DailyCommand.ChangeStreaks(prevdate, date);
            prevdate = date;
			bool pastharvestopen = false;
			bool beforeharvestclose = false;
			var datenow = DateTime.Now;
			DateTime tom = new DateTime(datenow.AddDays(1).Year, datenow.AddDays(1).Month, datenow.AddDays(1).Day, 2, 0, 0);
			StartDailyTimer(tom - datenow);
			DateTime harvestclose = new DateTime(datenow.Year, datenow.Month, datenow.Day, 22, 30, 0);
			if(datenow.DayOfWeek == DayOfWeek.Saturday || datenow.DayOfWeek == DayOfWeek.Sunday || Holidays.Contains(date))
			{
				DateTime harveststart = new DateTime(datenow.Year, datenow.Month, datenow.Day, 8, 30, 0);

				if (harveststart>datenow)
				{
					StartHarvestOpen(harveststart-datenow);
				}
				else
				{					
					pastharvestopen = true;
				}                
			}
			else
			{
				DateTime harveststart = new DateTime(datenow.Year, datenow.Month, datenow.Day, 15, 0, 0);

                if (harveststart>datenow)
                {
                    StartHarvestOpen(harveststart-datenow);
                }
                else
                {                    
                    pastharvestopen = true;
                }        
			}
			if(harvestclose>datenow)
			{
				StartHarvestClose(harvestclose - datenow);
				beforeharvestclose = true;
			}
			if (beforeharvestclose && pastharvestopen)
			{
				Global.ChangeUtil("harvesthours", "true");
			}
        }
		private static void StartDailyTimer(TimeSpan ts)
        {
            Timer DailyTimer = new Timer(ts.TotalMilliseconds);
			DailyTimer.AutoReset = false;
			DailyTimer.Start();
			DailyTimer.Elapsed += DailyTimer_Elapsed;
        }
		static void DailyTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			DailyEventsStart();
		}
		private static void StartHarvestOpen(TimeSpan ts)
        {
            Timer HarvestOpen = new Timer(ts.TotalMilliseconds);
			HarvestOpen.AutoReset = false;
			HarvestOpen.Start();
			HarvestOpen.Elapsed += HarvestOpen_Elapsed;
        }
		static void HarvestOpen_Elapsed(object sender, ElapsedEventArgs e)
		{
			Global.ChangeUtil("harvesthours", "true");
			RestartReaperTimer();
			ReaperDataStorage.ClearLastReap();
		}        
		private static void StartHarvestClose(TimeSpan ts)
        {
            Timer HarvestClose = new Timer(ts.TotalMilliseconds);
			HarvestClose.AutoReset = false;
			HarvestClose.Start();
			HarvestClose.Elapsed += HarvestClose_Elapsed;
        }
		static void HarvestClose_Elapsed(object sender, ElapsedEventArgs e)
		{
			Global.ChangeUtil("harvesthours", "false");
		}        
        private static bool ValidateExistence(string file)
        {
            if (!File.Exists(file))
            {
                if (file == @"/DuckBot/Currentday.json")
                {
                    File.WriteAllText(file, JsonConvert.SerializeObject(DateTime.Today.ToString("d")));
                    prevdate = DateTime.Today.ToString("d");
                    return false;
                }
				File.WriteAllText(file, "");
				return false;
            }
            return true;
        }
        private static void SaveCurrentDay()
        {
            string data = JsonConvert.SerializeObject(date, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/Currentday.json", data);
        }
		internal static void SaveHolidays()
		{            
            List<string> Remove = new List<string>();
			for (int i = 0; i < Holidays.Count; i++)
			{
				if(DateTime.Parse(Holidays[i]) < DateTime.Today)
				{
                    Remove.Add(Holidays[i]);
				}
			}
            for (int j = 0; j < Remove.Count; j++)
            {
                Holidays.Remove(Remove[j]);
            }
			string holidaysdata = JsonConvert.SerializeObject(Holidays, Formatting.Indented);
			File.WriteAllText(@"/DuckBot/Holidays.json", holidaysdata);
		}
    }
}
