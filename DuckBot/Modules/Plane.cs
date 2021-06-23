using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
    public class Plane : ModuleBase<SocketCommandContext>
    {
        EmbedBuilder builderplane = new EmbedBuilder();

        [Command("plane", RunMode = RunMode.Async)]
        public async Task PingAsync()
        {
            if (Context.Channel.Name != "botspam")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"botspam\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "botspam").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            builderplane.WithTitle("Here's the plane!")
                        .WithDescription("Isn't it beautiful?")
                        .WithImageUrl("https://cdn.discordapp.com/attachments/427543238724026379/427977038406615041/00f321e97de485b38fcc1d33a85edfce.jpg")
                        .WithColor(Color.LightGrey);
            
            await ReplyAsync("", false, builderplane.Build());

            return;
        }
    }
}