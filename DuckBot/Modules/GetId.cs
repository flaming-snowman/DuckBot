using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
    public class GetId : ModuleBase<SocketCommandContext>
    {
        [Command("getid", RunMode = RunMode.Async), RequireOwner]
        public async Task PingAsync()
        {

            //Context.User;
            //Context.Client;
            //Context.Guild;
            //Context.Message;
            //Context.Channel.
            await ReplyAsync($"{Context.Client.CurrentUser.Mention} || {Context.User.Mention} sent {Context.Message.Content} in {Context.Channel.Id} in {Context.Guild.Name}!");
        }
    }
}
