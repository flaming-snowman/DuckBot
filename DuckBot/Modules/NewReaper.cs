using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
    public class NewReaper : ModuleBase<SocketCommandContext>
    {
        EmbedBuilder builderunauthorized = new EmbedBuilder();
        EmbedBuilder buildernewgame = new EmbedBuilder();
        EmbedBuilder buildernewleaderboard = new EmbedBuilder();
        [Command("startharvest", RunMode = RunMode.Async), RequireOwner]
        public async Task StartReaperAsync(string timebetween, string goaltime)
        {
            if (Global.gamestart)
            {
                builderunauthorized.WithTitle("Unauthorized")
                                   .WithDescription("Ongoing Harvest Simulator game, command can only be used to start new games when there is no ongoing game.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderunauthorized.Build());

                return;
            }
            if (!Global.gamestart)
            {
                await Global.ChangeUtil("reapwaittime", timebetween);
                await Global.ChangeUtil("winscore", goaltime);
                await Global.ChangeUtil("gamestart", "true");
                await Clock.RestartReaperTimer();

                buildernewgame.WithTitle("New Game of Harvest Simulator has Begun!")
                              .AddField("Time bewtween harvests:", $"**{Convert.ToInt64(timebetween) / 3600000}** hours and **{(Convert.ToInt64(timebetween) % 3600000) / 60000}** minutes")
                              .AddField("Goal:", $"Game will end when someone reaches **{Convert.ToInt64(goaltime) / 3600000}** sheaves of corn")
                              .WithColor(Color.Blue);
                buildernewleaderboard.WithDescription("Harvest Leaderboard")
                                     .WithDescription("No scores yet.")
                                     .WithColor(Color.Magenta);
                
                await ReplyAsync("@everyone, a new game of Harvest Simulator has begun!", false, buildernewgame.Build());
                var channelleaderboardpost = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "harvest-leaderboard");
                var sendmessage = await channelleaderboardpost.SendMessageAsync($"Play the game in {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "harvest-simulator").Mention}", false, buildernewleaderboard.Build());
                string messageid = Convert.ToString((sendmessage.Id));
                await Global.ChangeUtil("leaderboardmessageid", messageid);
                return;
            }
        }       
    }
}
