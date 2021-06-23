using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using DuckBot.Modules;
using System.Linq;

namespace DuckBot
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
		private bool run1 = true;

        public async Task RunBotAsync()
        {
			Console.WriteLine("Starting up Duck Bot v1.56");
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<InteractiveService>()
                .AddSingleton<ReliabilityService>()
                .BuildServiceProvider();
            
			string botToken = "REDACTED";
            //event subscriptions
            _client.Log += Log;
            _client.UserJoined += AnnounceUserJoined;
            _client.UserLeft += AnnounceUserLeft;
            _client.ReactionAdded += ReactionFound;
            _client.Ready += OnLogin;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, botToken);

            await _client.StartAsync();

            await _client.SetGameAsync("d.help||By Flaming Snowman");            

            Clock.DailyEventsStart();

			await Task.Delay(-1);
        }

        private async Task OnLogin()
        {
            var guild = _client.GetGuild(427315861045641216);
			Invite.LoadInvites(guild);
			//await VoteRecipe.CheckTime(); //recipes broken
            if(run1)
			{
				await Clock.IntializeTimers(guild);
                for (int i = 0; i < 4; i++)
                {
                    await VoiceChat.ClearUsers(i, guild);
                }
				run1 = false;
			}
            else
			{
				SocketTextChannel logchnl = guild.TextChannels.FirstOrDefault(x => x.Name == "staff-logs");
				await logchnl.SendMessageAsync("Bot soft reset");
			}
			await Clock.StartMsgTimer(guild);
        }
        
        private async Task AnnounceUserJoined(SocketGuildUser user)
        {
			if (user.IsBot) return;
			var data = Invite.InviteUsed(user.Guild, user);
            EmbedBuilder builderannounce = new EmbedBuilder();

            builderannounce.WithTitle($"Welcome our newest arrival, {user.Username}!")
                           .AddField("Date joined:", DateTime.Now)
			               .AddField("Invite code:", data.Item1)
			               .AddField("Invited by: ", $"<@{data.Item2}>")
                   .WithColor(Color.Teal);
            
            var guild = user.Guild;
            var channelannounce = guild.TextChannels.FirstOrDefault(x => x.Name == "arrivals-and-departures");
            await channelannounce.SendMessageAsync("", false, builderannounce.Build());
        }
        
        private async Task AnnounceUserLeft(SocketGuildUser user)
        {
            EmbedBuilder builderannounce = new EmbedBuilder();

            builderannounce.WithTitle($"{user.Username} has departed.")
                           .WithDescription($"{user.Mention} is no longer a part of {user.Guild.Name}.")
                   .WithColor(Color.DarkRed);

            var guild = user.Guild;
            var channelannounce = guild.TextChannels.FirstOrDefault(x => x.Name == "arrivals-and-departures");
            await channelannounce.SendMessageAsync("", false, builderannounce.Build());
        }

        private async Task ReactionFound(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel chnl, SocketReaction reaction)
        {			
            if(chnl.Name == "welcome-and-rules")
            {		
				var guild = (chnl as SocketTextChannel).Guild;
                var user = guild.GetUser(reaction.UserId);
                var message = msg.GetOrDownloadAsync().Result;
                var role = guild.Roles.FirstOrDefault(x => x.Name == "Unverified");
                var duckrole = guild.Roles.FirstOrDefault(x => x.Name == "Duckling");                
				if (message.Id == 516425922983755790 && reaction.Emote.Name == "✅" && !user.Roles.Contains(duckrole)) { await user.AddRoleAsync(duckrole); }
                Invite.UserInviteGood(user.Id); //remove if verification readded
                await message.RemoveReactionAsync(reaction.Emote, user);
            }			 
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null || message.Author.IsBot || message.Author.IsWebhook) return;

            int argPos = 0;

            if (message.HasStringPrefix("d.", ref argPos) || message.HasStringPrefix("D.", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
