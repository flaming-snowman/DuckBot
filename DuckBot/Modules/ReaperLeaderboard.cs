using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
    public class ReaperLeaderboard : ModuleBase<SocketCommandContext>
    {
        EmbedBuilder buildernogame = new EmbedBuilder();
        EmbedBuilder builderwrongchannel = new EmbedBuilder();
        [Command("leaderboard", RunMode = RunMode.Async)]
        public async Task LeaderboardAsync()
        {
            string name = Context.Guild.GetUser(Context.User.Id).Nickname;
            if(name==null)
            {
                name = Context.User.Username;
            }

            if (Context.Channel.Name != "harvest-simulator")
            {
                builderwrongchannel.WithTitle("Harvest Simulator commands may only be used in the designated channel")
                                   .WithDescription($"{name}, please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "harvest-simulator").Mention} to run Harvest Simulator commands.")
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
			List<long> scorevalues = ReaperDataStorage.GetScoreList();
            int entries = scorevalues.Count();
            int count = entries;
            if (entries > 5)
                count = 5;
            scorevalues.Sort();
            string message = "";
            for (int i = 0; i < count; i++)
            {
                long myValue = scorevalues[entries - i - 1];
				ulong myKey = ReaperDataStorage.ScoreValueToKey(myValue);
                message = message + $"**{i+1}**: <@{myKey}> — **{myValue / 3600000}** sheaves, **{(myValue % 3600000) / 60000}** ears, and **{(myValue % 60000) / 1000}** kernels of corn.\n";
            }
			int rank = ReaperDataStorage.GetHarvestRanking(Context.User.Id);
			string rankmsg = rank.ToString();
            if(rank == -1)
			{
				rankmsg = "no rank";
			}
				
            EmbedBuilder builderleaderboard = new EmbedBuilder();
            builderleaderboard.WithTitle("Harvest Leaderboard for the Top 5")
                              .WithThumbnailUrl("https://cdn.discordapp.com/attachments/427572233100328962/433398892491702293/medals-1622902_960_720.png")
                              .WithDescription(message)
			                  .AddField("Your ranking:", rankmsg)
                              .AddField("For the full leaderboard, visit", $"{Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "harvest-leaderboard").Mention}")
                              .AddField("Requested by:", name)
                              .WithColor(Color.Magenta);
            
            await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderleaderboard.Build());
        }
    }
}
