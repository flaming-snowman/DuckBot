using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
        public class Crops : ModuleBase<SocketCommandContext>
        {
            EmbedBuilder buildercrops = new EmbedBuilder();
            EmbedBuilder buildernogame = new EmbedBuilder();
            EmbedBuilder builderwrongchannel = new EmbedBuilder();
            [Command("crops", RunMode = RunMode.Async)]
            public async Task ReapAsync()
            {
                string name = Context.Guild.GetUser(Context.User.Id).Nickname;
                if(name == null)
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
			if (!Global.harvesthours)
            {
                buildernogame.WithTitle("Closed")
                             .WithDescription("Hours of operation are from 3 P.M. to 10:30 P.M. PST on weekdays and 8:30 A.M. to 10:30 P.M. PST on weekends and holidays.")
                             .WithColor(Color.Red);

                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, buildernogame.Build());

                return;
            }
                long crops = Clock.GetCrops();
                buildercrops.WithTitle($"Crop status requested by {name}")
                        .WithThumbnailUrl("https://cdn.discordapp.com/attachments/427572233100328962/432674934074900480/corn-stalk-clipart-transparent-background-1.jpg.png")
                        .WithDescription($"There are currently **{crops / 3600000}** sheaves of corn, **{(crops % 3600000) / 60000}** ears of corn, and **{(crops % 60000) / 1000}** corn kernels available for harvest.")
                        .WithColor(Color.Orange);
                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, buildercrops.Build());
            }
        }
}
