using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
namespace DuckBot.Modules
{
	public class InviteCommands : ModuleBase<SocketCommandContext>
	{
		[Command("invites", RunMode = RunMode.Async)]
		public async Task InvitesAsync()
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
			var data = Invite.GetInvites(Context.User.Id);
			EmbedBuilder inviteembed = new EmbedBuilder();
			string message = $"{Context.User.Mention}'s invites: ";
			foreach (ulong id in data)
			{
				message += $"\n <@{id}>";
			}
			if (data.Count == 0)
			{
				message += "none";
			}
			inviteembed.WithTitle($"{Context.User.Username}#{Context.User.Discriminator}'s invites")
					   .WithDescription(message)
					   .AddField("Count: ", data.Count)
					   .WithColor(Color.Green);

			await ReplyAsync($"{Context.User.Mention}, Duck Bot has responded to your command!", false, inviteembed.Build());
		}
		[Command("getinvites", RunMode = RunMode.Async)]
		public async Task GetInvitesAsync(IUser user)
		{
			var peking = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Peking Duck");
			if (!(Context.User as SocketGuildUser).Roles.Contains(peking)) return;
			if (Context.Channel.Name != "duck-commands")
			{
				EmbedBuilder builderwrongchannel = new EmbedBuilder();
				builderwrongchannel.WithTitle("This command may only be used in channel \"duck-commands\".")
								   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "duck-commands").Mention} to run this command.")
								   .WithColor(Color.Red);
				await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

				return;
			}

			var data = Invite.GetInvites(user.Id);
			EmbedBuilder inviteembed = new EmbedBuilder();
			string message = $"{user.Mention}'s invites: ";
			foreach (ulong id in data)
			{
				message += $"\n <@{id}>";
			}
			if (data.Count == 0)
			{
				message += "none";
			}
			inviteembed.WithTitle($"{user.Username}#{user.Discriminator}'s invites")
					   .WithDescription(message)
					   .AddField("Count: ", data.Count)
					   .WithColor(Color.Green);

			await ReplyAsync($"{Context.User.Mention}, Duck Bot has responded to your command!", false, inviteembed.Build());
		}
		[Command("myinfo", RunMode = RunMode.Async)]
        public async Task MyInfo()
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

			var data = Invite.GetUserInfo(Context.User.Id);
			EmbedBuilder infoembed = new EmbedBuilder();
			string invited = $"<@{data.Item1}>";            
			if(data.Item1 == 0)
			{
				invited = "Nobody knows";
			}
            
			infoembed.WithTitle($"{Context.User.Username}#{Context.User.Discriminator}'s Info")                       
			         .AddField("Invited by: ", invited)
			         .AddField("First Joined: ", data.Item2)
                       .WithColor(Color.Green);

			await ReplyAsync($"{Context.User.Mention}, Duck Bot has responded to your command!", false, infoembed.Build());
        }
	}
}
