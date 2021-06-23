using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
        public class Mycorn : ModuleBase<SocketCommandContext>
        {
            EmbedBuilder buildermycorn = new EmbedBuilder();
            EmbedBuilder buildernogame = new EmbedBuilder();
            EmbedBuilder builderwrongchannel = new EmbedBuilder();
            ulong MycornUserId = new ulong();
            [Command("mycorn", RunMode = RunMode.Async)]
            public async Task MycornAsync()
            {
                string name = Context.Guild.GetUser(Context.User.Id).Nickname;
                if (name == null)
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
			MycornUserId = Context.User.Id;

			if (!ReaperDataStorage.HasCorn(MycornUserId))
                    return;
                
			long mycorn = ReaperDataStorage.GetCorn(MycornUserId);

                buildermycorn.WithTitle($"Corn inventory of {name}")
                            .WithThumbnailUrl("https://cdn.discordapp.com/attachments/427572233100328962/432674934074900480/corn-stalk-clipart-transparent-background-1.jpg.png")
                             .WithDescription($"You have **{mycorn / 3600000}** sheaves of corn, **{(mycorn % 3600000) / 60000}** ears of corn, and **{(mycorn % 60000) / 1000}** corn kernels.")
                            .WithColor(Color.Orange);
                
                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, buildermycorn.Build());

            return;
            }
        }
}
