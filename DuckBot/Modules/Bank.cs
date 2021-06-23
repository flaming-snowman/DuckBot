using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
    public class Bank : ModuleBase<SocketCommandContext>
    {
        EmbedBuilder builderbank = new EmbedBuilder();

        [Command("bank", RunMode = RunMode.Async)]
        public async Task BankAsync()
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
			ulong id = Context.User.Id;
            string name = Context.Guild.GetUser(Context.User.Id).Nickname;
            if(name==null)
            {
                name = Context.User.Username;
            }
            builderbank.WithTitle($"{name}'s bank account")
			           .WithDescription($"{name} has a balance of ${ReaperDataStorage.GetBalance(id)}.")
                       .WithThumbnailUrl(Context.User.GetAvatarUrl())
                       .WithColor(0x541c1c);
            await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderbank.Build());

            return;
        }
    }
}
