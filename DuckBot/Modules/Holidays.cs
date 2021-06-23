using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
namespace DuckBot.Modules
{
	public class Holidays : ModuleBase<SocketCommandContext>
	{
		[Command("addholiday", RunMode = RunMode.Async), RequireOwner]
        public async Task AddHoliday(string date1 = "", string date2 = "")
		{						
			DateTime first;
			DateTime second;
			if(!DateTime.TryParse(date1, out first))
			{
				await ReplyAsync("Invalid date format, failed to compute.");
				return;
			}
			if (date2 == "")
            {
                if (!Clock.Holidays.Contains(first.ToString("d")))
                {
                    Clock.Holidays.Add(first.ToString("d"));
                    Clock.SaveHolidays();
                    await ReplyAsync($"1 Date was added: {first.ToString("d")}");

					return;
                }
                await ReplyAsync("The date was already stored.");

                return;
            }
			if(!DateTime.TryParse(date2, out second) && date2 != "")
			{
				await ReplyAsync("Invalid date format, failed to compute.");
				return;
			}            
			else
			{
				if(first>second)
				{
					await ReplyAsync("First date is after second date. Cannot compute.");
					return;
				}
				int counter = 0;
				DateTime date = first;
				while(date <= second)
				{
					if (!Clock.Holidays.Contains(date.ToString("d")))
                    {
                        Clock.Holidays.Add(date.ToString("d"));
                        counter++;
                    }

					date = date.AddDays(1);
				}
				Clock.SaveHolidays();

				await ReplyAsync($"{counter} Dates were added in the range: {first.ToString("d")} to {second.ToString("d")}");
			}
			return;
		}
		[Command("holidays", RunMode = RunMode.Async)]
        public async Task HolidaysAsync()
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
			List<DateTime> dates = Clock.Holidays.Select(date => DateTime.Parse(date)).ToList();
			dates.Sort();
			string start;
			string end;
			string result = "The following dates are holidays: ";
			int i = 0;
			bool next = true;
			bool first = true;
			if(dates.Count == 0)
			{
				next = false;
				result = "No holidays stored.";
			}
			while (next)
			{
				int counter = 0;
				bool cont = true;
				start = dates[i].ToString("d");
				while(cont)
				{
					if(i+counter< dates.Count)
					{
						if (dates[i + counter] == dates[i].AddDays(counter))
                        {
                            counter++;
                        }
						else
						{
							cont = false;
						}
					}
					else
						cont = false;
				}
				end = dates[i + counter - 1].ToString("d");
				if(!first)
				{
					result = result + ", ";
				}
				if(start == end)
				{
					result = result + start;
				}
                if(start != end)
				{
					result = result + start + " through " + end;
				}
				if(i+counter < dates.Count)
				{
					i = i + counter;
				}
				else
				{
					next = false;
				}
				first = false;
			}
			result = result + ".";
			EmbedBuilder msgbuilder = new EmbedBuilder();
			msgbuilder.WithTitle("Holidays")
					  .WithDescription(result)
					  .WithColor(Color.Green);

			await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, msgbuilder.Build());
		}
	}
}
