using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
    public class ManualOverride : ModuleBase<SocketCommandContext>
    {
        [Command("override", RunMode = RunMode.Async), RequireOwner]
        public async Task ManualOverrideAsync()
        {
            await ReplyAsync("Bot force restarted");

            Environment.Exit(1);
        }
        [Command("beep", RunMode = RunMode.Async)]
        public async Task ManualBeepBoop()
        {
            if (Context.Channel.Name != "admin-cmd") return;
			await Clock.StartMsgTimer(Context.Guild);
            await ReplyAsync("Beep Boop initiation complete");
            return;
        }
		[Command("fixstory", RunMode = RunMode.Async), RequireOwner]
        public async Task ManualRecipeContest()
		{
			await VoteRecipe.MoveRecipes(Context.Guild);
			return;
		}
		[Command("diagnostics", RunMode = RunMode.Async), RequireOwner]
        [Alias("report")]
        public async Task GetDiagnosticsAsync()
		{
			EmbedBuilder diagnosticBuilder = new EmbedBuilder();
			diagnosticBuilder.WithTitle("Program Info")
							 .AddField("Current Uptime", DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime())
							 .AddField("Memory Used", GC.GetTotalMemory(true))
			                 .WithColor(Color.Green);
			await ReplyAsync("", false, diagnosticBuilder.Build());
		}
    }
}
