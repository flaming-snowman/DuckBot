using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DuckBot.Modules
{
    public class Verify : ModuleBase<SocketCommandContext>
    {
		[Command("verify", RunMode = RunMode.Async)]
        public async Task VerifyAsync([Remainder] string text)
        {
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Unverified");
            if (!(Context.User as SocketGuildUser).Roles.Contains(role) || Context.Channel.Name != "verify") return;
            var sendtochannel = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "verify-users");
            var usechnl = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "admin-cmd");
            var msg = await sendtochannel.SendMessageAsync($"New user: {Context.User.Mention}");
            ulong ID = msg.Id;
            EmbedBuilder verifymsg = new EmbedBuilder();
            verifymsg
                .WithTitle($"{Context.User.Username} wants access to this server")
                .WithDescription("Submitted message: " + text)
                .AddField("User", $"{Context.User.Mention}", true)
                .AddField("User ID:", Context.User.Id + "", true)
                .AddField("Instructions:", $"Accept this user by using `d.accept {ID}` or deny with `d.deny {ID}` in {usechnl.Mention}.")
                .WithColor(Color.Orange);

            await msg.ModifyAsync(x => { x.Embed = verifymsg.Build(); });
            await ReplyAsync($"{Context.User.Mention} Your submission has been logged");
            await (Context.User as IGuildUser).RemoveRoleAsync(role);
        }
        [Command("accept", RunMode = RunMode.Async)]
        public async Task AcceptAsync(ulong msgid)
        {
            if (Context.Channel.Name != "admin-cmd") return;
            var chnl = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "verify-users");
            var msg = chnl.GetMessageAsync(msgid).Result;
            if (msg == null)
            {
                await ReplyAsync("Invalid ID");

                return;
            }
            if (msg.Embeds.FirstOrDefault().Color.ToString() != Color.Orange.ToString())
            {
                await ReplyAsync("Application has already been processed. Try another one.");

                return;
            }
			ulong userid = msg.MentionedUserIds.FirstOrDefault();
            var user = Context.Guild.GetUser(userid);
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Duckling");
            if (user != null)
            {
                await user.AddRoleAsync(role);
                await ReplyAsync($"{user.Mention} has been accepted.");
				Invite.UserInviteGood(userid);
                string text = msg.Embeds.First().Description;
                EmbedBuilder acceptmsg = new EmbedBuilder();
                acceptmsg.WithTitle($"{user.Username}#{user.Discriminator} has been accepted to this server.")
                         .WithDescription(text)
                         .AddField("User:", user.Mention, true)
                         .AddField("User ID:", user.Id + "", true)
                         .WithColor(Color.Green);
                await (msg as IUserMessage).ModifyAsync(x => x.Embed = acceptmsg.Build());

                return;
            }
            await ReplyAsync("Error");
        }
        [Command("deny", RunMode = RunMode.Async)]
        public async Task DenyAsync(ulong msgid)
        {
            if (Context.Channel.Name != "admin-cmd") return;
            var chnl = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "verify-users");
            var msg = chnl.GetMessageAsync(msgid).Result;
            if (msg == null)
            {
                await ReplyAsync("Invalid ID");

                return;
            }
            if (msg.Embeds.FirstOrDefault().Color.ToString() != Color.Orange.ToString())
            {
                await ReplyAsync("Application has already been processed. Try another one.");

                return;
            }
            var user = Context.Guild.GetUser(msg.MentionedUserIds.FirstOrDefault());
            if (user != null)
            {
				await user.SendMessageAsync($"You were denied access to {Context.Guild.Name}. If you feel this was done incorrectly, please join back and try again. Your verification message may not have been detailed or improperly filled.");
                await user.KickAsync("Denied access");
                await ReplyAsync($"{user.Mention} has been denied.");
                string text = msg.Embeds.First().Description;
                EmbedBuilder acceptmsg = new EmbedBuilder();
                acceptmsg.WithTitle($"{user.Username}#{user.Discriminator} has been denied access to this server.")
                         .WithDescription(text)
                         .AddField("User:", user.Mention, true)
                         .AddField("User ID:", user.Id + "", true)
                         .WithColor(Color.Red);
                await (msg as IUserMessage).ModifyAsync(x => x.Embed = acceptmsg.Build());

                return;
            }
            await ReplyAsync("Error");
        }
    }
}
