using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
    public class Ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping", RunMode = RunMode.Async)]
        public async Task PingAsync()
        {
            EmbedBuilder builderping = new EmbedBuilder();

            builderping.WithTitle("Pong!")
                       .WithDescription("Bot is online!")
                   .WithColor(Color.Blue);

            await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderping.Build());

            return;
        }
    }
}
