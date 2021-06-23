using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DuckBot.Modules
{
    public class Reap : ModuleBase<SocketCommandContext>
    {
        EmbedBuilder buildernogame = new EmbedBuilder();
        EmbedBuilder builderwrongchannel = new EmbedBuilder();
        [Command("harvest", RunMode = RunMode.Async)]
        public async Task ReapAsync()
        {
            string username = Context.User.Username;
            string name = Context.Guild.GetUser(Context.User.Id).Nickname;
            var channelrun = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "harvest-simulator");
            if (name == null)
            {
                name = username;
            }
            if (Context.Channel.Name != "harvest-simulator")
            {
                builderwrongchannel.WithTitle("Harvest Simulator commands may only be used in the designated channel")
                                   .WithDescription($"{name}, please go to {channelrun.Mention} to run Harvest Simulator commands.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            if(!Global.gamestart)
            {
                buildernogame.WithTitle("No ongoing Harvest Simulator")
                             .WithDescription("Please alert the owner to start a new game")
                             .WithColor(Color.Red);
                
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, buildernogame.Build());

                return;
            }
			if (!Global.harvesthours)
            {
                buildernogame.WithTitle("Closed")
				             .WithDescription("Hours of operation are from 3 P.M. to 10:30 P.M. PST on weekdays and 8:30 A.M. to 10:30 P.M. PST on weekends and holidays.")
                             .WithColor(Color.Red);

                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, buildernogame.Build());

                return;
            }
            int ticketnum = Global.ticket;
            Global.ticket++;
            while (Global.currentcalculate != ticketnum)
            { }

            long timebetweenreaps = new long();
            long timeuntilnewreap = new long();
			ulong userid = Context.User.Id;

			if (ReaperDataStorage.HasHarvested(userid))
            {
				timebetweenreaps = Clock.GetTime() - ReaperDataStorage.GetHarvestTime(userid);
                if (timebetweenreaps < Global.reapwaittime)
                {
                    Global.currentcalculate++;
                    if (Global.currentcalculate == Global.ticket)
                    {
                        Global.currentcalculate = 0;
                        Global.ticket = 0;
                    }

                    timeuntilnewreap = Global.reapwaittime - timebetweenreaps;
                    EmbedBuilder builderreapfail = new EmbedBuilder();
                    builderreapfail.WithTitle($"{name}, You may not harvest yet")
                           .WithThumbnailUrl(Context.User.GetAvatarUrl())
                           .WithDescription($"Please wait **{Global.reapwaittime / 3600000}** hours and **{(Global.reapwaittime % 3600000) / 60000}** minutes between harvesting. \n Wait **{timeuntilnewreap / 3600000}** hours, **{timeuntilnewreap % 3600000/ 60000}** minutes and **{(timeuntilnewreap % 60000) / 1000}** seconds to harvest again")
                           .WithColor(Color.Red);
                    
                    await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderreapfail.Build());

                    return;
                }
            }

            var reaperitems =  Clock.AddReaperScore(userid);
            string seeditems = Garden.HarvestSeeds(Context.User.Id);           
            // item1 returns addscore as long, item2 returns finalscore as long, item3 returns newleader as bool, item4 returns finalizegame as bool, item5 as multiplier for money           
            if (reaperitems.Item3)
            {
                EmbedBuilder builderleader = new EmbedBuilder();

                builderleader.WithTitle($"{name} has taken the lead in Harvest Simulator")
                             .WithDescription("They have been awarded the Harvest Leader role")
                             .WithColor(Color.Blue);
                if (Global.currentleader != 0)
                    await ReplyAsync($"<@{Global.currentleader}>, you are no longer the Harvest Leader", false, builderleader.Build());
                if (Global.currentleader == 0)
                    await ReplyAsync("", false, builderleader.Build());
                var user = Context.User;
                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Harvest Leader");
                if (Global.currentleader != 0)
                {
                    var oldleader = Context.Guild.GetUser((ulong)Global.currentleader);
                    if (oldleader != null)
                    {
                        await (oldleader as IGuildUser).RemoveRoleAsync(role);
                    }
                }
                await (user as IGuildUser).AddRoleAsync(role);
                await Global.ChangeUtil("currentleader", Context.User.Id.ToString());
            }

            EmbedBuilder builderreap = new EmbedBuilder();
            builderreap.WithTitle($"{name} has succesfully harvested!")
                       .WithThumbnailUrl(Context.User.GetAvatarUrl())
                       .AddField("Harvest Amount", $"You have harvested **{reaperitems.Item1 / 3600000}** sheaves of corn, **{(reaperitems.Item1 % 3600000) / 60000}** ears of corn, and **{(reaperitems.Item1 % 60000) / 1000}** corn kernels.")
                       .AddField("Total Crops", $"**{name}** now has a total of **{reaperitems.Item2 / 3600000}** sheaves of corn, **{(reaperitems.Item2 % 3600000) / 60000}** ears of corn, and **{(reaperitems.Item2 % 60000) / 1000}** corn kernels.")
                       .AddField("Seeds Collected", $"You collected the following seeds: {seeditems}.")
                       .WithColor(Color.Gold);

            await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderreap.Build());

            if (reaperitems.Item4)
            {
                EmbedBuilder builderwin = new EmbedBuilder();

                builderwin.WithTitle($"{name} has won this round of Harvest Simulator!")
                          .WithThumbnailUrl(Context.User.GetAvatarUrl())
                          .WithDescription($"All the corn each person has gathered will be added to their balance at a conversion of ${reaperitems.Item5 * 40} per sheave, ears and kernels will be converted as fractional sheaves. Type `d.help money` to see what you can do with money.")
                          .WithColor(Color.Blue);
                await ReplyAsync($"{Context.User.Mention} has won!", false, builderwin.Build());

                var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Harvest Leader");
                if (Global.currentleader != 0)
                {
                    var oldleader = Context.Guild.GetUser((ulong)Global.currentleader);
                    if (oldleader != null)
                    {
                        await (oldleader as IGuildUser).RemoveRoleAsync(role);
                    }
                }
                if (Global.leaderboardmessageid != 0)
                    await SendLeaderboard(true, name, username);

                ReaperDataStorage.CompleteFinalization();

                return;
            }
           
            if (Global.leaderboardmessageid != 0)
                await SendLeaderboard(false, name, username);

            return;
        }
        private async Task SendLeaderboard(bool endgame, string name, string username)
        {
            var channelleaderboardpost = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "harvest-leaderboard");

            var message = await channelleaderboardpost.GetMessageAsync(Convert.ToUInt64(Global.leaderboardmessageid)) as IUserMessage;

			List<long> scorevalues = ReaperDataStorage.GetScoreList();
            int entries = scorevalues.Count();
            if (entries > 30)
			{
				entries = 30;
			}
            scorevalues.Sort();
            string sendmessage = "";
            for (int i = 0; i < entries; i++)
            {
                long myValue = scorevalues[entries - i - 1];
				ulong myKey = ReaperDataStorage.ScoreValueToKey(myValue);
                sendmessage = sendmessage + $"**{i + 1}**: <@{myKey}> — **{myValue / 3600000}** sheaves, **{(myValue % 3600000) / 60000}** ears, and **{(myValue % 60000) / 1000}** kernels of corn.\n";
            }

            EmbedBuilder builderleaderboard = new EmbedBuilder();
            if(endgame)
            {
                builderleaderboard.WithTitle($"Harvest Leaderboard-Game {Global.gamenum} has ended")
                              .WithThumbnailUrl("https://cdn.discordapp.com/attachments/427572233100328962/433398892491702293/medals-1622902_960_720.png")
                              .WithDescription(sendmessage)
                              .AddField("Winner Nickname:", name, true)
                              .AddField("Username:", username, true)
                              .WithTimestamp(DateTime.UtcNow)
                              .WithFooter("Game ended:", "https://cdn.discordapp.com/attachments/427572233100328962/435310228263796737/images.png")
                                  .WithColor(Color.Gold);
            }
            if(!endgame)
            {
                builderleaderboard.WithTitle($"Harvest Leaderboard-Game {Global.gamenum} in progress")
                                  .WithThumbnailUrl("https://cdn.discordapp.com/attachments/427572233100328962/433398892491702293/medals-1622902_960_720.png")
                                  .WithDescription(sendmessage)
                                  .AddField("Latest Harvester Nickname:", name, true)
                                  .AddField("Username:", username, true)
                                  .WithTimestamp(DateTime.UtcNow)
                                  .WithFooter("Last updated most recent harvest:", "https://cdn.discordapp.com/attachments/427572233100328962/435310228263796737/images.png")
                                  .WithColor(Color.Magenta);
            }

            await message.ModifyAsync(msg => msg.Embed = builderleaderboard.Build());
            return;
        }
		[Command("mystatus", RunMode = RunMode.Async)]
        public async Task StatusAsync()
        {
			string username = Context.User.Username;
            string name = Context.Guild.GetUser(Context.User.Id).Nickname;
            var channelrun = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "harvest-simulator");
            if (name == null)
            {
                name = username;
            }
            if (Context.Channel.Name != "harvest-simulator")
            {
                builderwrongchannel.WithTitle("Harvest Simulator commands may only be used in the designated channel")
                                   .WithDescription($"{name}, please go to {channelrun.Mention} to run Harvest Simulator commands.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            if (!Global.gamestart)
            {
                buildernogame.WithTitle("No ongoing Harvest Simulator")
                             .WithDescription("Please alert the owner to start a new game")
                             .WithColor(Color.Red);

                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, buildernogame.Build());

                return;
            }
			if (!Global.harvesthours)
            {
                buildernogame.WithTitle("Closed")
                             .WithDescription("Hours of operation are from 3 P.M. to 10:30 P.M. PST on weekdays and 8:30 A.M. to 10:30 P.M. PST on weekends and holidays.")
                             .WithColor(Color.Red);

                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, buildernogame.Build());

                return;
            }
            long timebetweenreaps = new long();
            long timeuntilnewreap = new long();
			ulong userid = Context.User.Id;
			long lastharvestamount = -1;
			if (ReaperDataStorage.HasHarvested(userid))
            {
				lastharvestamount = ReaperDataStorage.GetPrevHarvest(userid);
				timebetweenreaps = Clock.GetTime() - ReaperDataStorage.GetHarvestTime(userid);
                if (timebetweenreaps < Global.reapwaittime)
                {
                    timeuntilnewreap = Global.reapwaittime - timebetweenreaps;

                    EmbedBuilder builderreapfail = new EmbedBuilder();
					if(lastharvestamount == -1)
					{
						builderreapfail.WithTitle($"{name}, You do not have an available harvest")
                                       .WithThumbnailUrl(Context.User.GetAvatarUrl())
                                       .WithDescription($"Wait **{timeuntilnewreap / 3600000}** hours, **{timeuntilnewreap % 3600000 / 60000}** minutes and **{(timeuntilnewreap % 60000) / 1000}** seconds to harvest again.")
						               .AddField("Remember", $"You have to wait **{Global.reapwaittime / 3600000}** hours, **{Global.reapwaittime % 3600000/ 60000}** minutes and **{(Global.reapwaittime % 60000) / 1000}** seconds between harvests.")
						               .AddField("Last harvest amount:", "No last harvest today to report.")
                                       .WithColor(Color.Red);
					}
					else
					{
						builderreapfail.WithTitle($"{name}, You do not have an available harvest")
                                   .WithThumbnailUrl(Context.User.GetAvatarUrl())
                                   .WithDescription($"Wait **{timeuntilnewreap / 3600000}** hours, **{timeuntilnewreap % 3600000 / 60000}** minutes and **{(timeuntilnewreap % 60000) / 1000}** seconds to harvest again.")
						               .AddField("Remember", $"You have to wait **{Global.reapwaittime / 3600000}** hours, **{Global.reapwaittime % 3600000/ 60000}** minutes and **{(Global.reapwaittime % 60000) / 1000}** seconds between harvests.")
                                   .AddField("Last harvest amount:", $"You last harvested for **{lastharvestamount / 3600000}** sheaves, **{lastharvestamount % 3600000 / 60000}** ears, and **{(lastharvestamount % 60000) / 1000}** kernels of corn. \n\nNote that the leaderboard may be outdated. `d.mycorn` for an accurate amount.")
                                   .WithColor(Color.Red);

					}
                    await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderreapfail.Build());

                    return;
                }

            }
			EmbedBuilder builderavail = new EmbedBuilder();
			if(lastharvestamount == -1)
			{
				builderavail.WithTitle($"{name}, You have an available harvest")
							.AddField("Last harvest amount:", "No last harvest today to report.")
							.WithColor(Color.Green);
			}
			else
			{
				builderavail.WithTitle($"{name}, You have an available harvest")
				            .AddField("Last harvest amount:", $"You last harvested for **{lastharvestamount / 3600000}** sheaves, **{lastharvestamount % 3600000 / 60000}** ears, and **{(lastharvestamount % 60000) / 1000}** kernels of corn. \n\nNote that the leaderboard may be outdated. `d.mycorn` for an accurate amount.")
                            .WithColor(Color.Green);
			}
			await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderavail.Build());

			return;
        }
    }
}