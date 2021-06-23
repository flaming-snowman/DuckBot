using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DuckBot.Modules
{
    public class VoiceChat : ModuleBase<SocketCommandContext>
    {
        EmbedBuilder builderclaim = new EmbedBuilder();

        [Command("claimvc", RunMode = RunMode.Async)]
        public async Task ClaimVoiceChatAsync()
        {
            if (Context.Channel.Name != "lfg")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("Voice channel commands may only be used in channel \"lfg\" (looking for group).")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "lfg").Mention} to run voice channel commands.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            double time = Clock.CheckVCTime();
            if(!time.Equals(0))
            {
                await ReplyAsync($":no_entry_sign: {Context.User.Mention}, please wait **5** seconds between claiming voice channels. Wait **{time}** more seconds *Note that all users share this timer*");

                return;
            }
            if(ChannelClaimed.Claimed.ContainsValue(Tuple.Create(true, Context.User.Id)))
            {
                await ReplyAsync($"{Context.User.Mention}, You have already claimed a voice channel! Invite users by mentioning them after typing `d.invitevc`");
            }
            int channelnum = ChannelClaimed.GetAvail(Context.Guild, Context.User.Id);
            if(channelnum == -1)
            {
                await ReplyAsync($"{Context.User.Mention}, No available team voice channels. Wait until a channel is abandoned by their current leader to try again.");

                return;
            }
            await ClearUsers(channelnum, Context.Guild);
            var user = Context.User as SocketGuildUser;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == $"Team {channelnum+1} Leader");
            await user.AddRoleAsync(role);
            await ReplyAsync($"Congrats {Context.User.Mention}, you claimed the voice channel `team-{channelnum + 1}`. Mention a user after `d.invitevc` to invite them to your voice channel. Note that someone else can claim your channel if you disconnect.");
        }
        internal static async Task ClearUsers(int num, SocketGuild guild)
        {
            //remove leader
            var role = guild.Roles.FirstOrDefault(x => x.Name == $"Team {num+1} Leader");
            foreach (SocketGuildUser member in role.Members)
                await member.RemoveRoleAsync(role);
            //remove members
            role = guild.Roles.FirstOrDefault(x => x.Name == $"Team {num + 1}");
            foreach (SocketGuildUser member in role.Members)
                await member.RemoveRoleAsync(role);
        }
        [Command("invitevc", RunMode = RunMode.Async)]
        public async Task VoiceChatAsync([Remainder]string text = "")
        {
            if (Context.Channel.Name != "lfg")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("Voice channel commands may only be used in channel \"lfg\" (looking for group).")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "lfg").Mention} to run voice channel commands.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            if(!ChannelClaimed.Claimed.ContainsValue(Tuple.Create(true, Context.User.Id)))
            {
                await ReplyAsync("You have not claimed a voice channel. Type `d.claimvc` to claim one.");
                return;
            }
            Tuple<bool, ulong> myValue = Tuple.Create(true, Context.User.Id);
            int myKey = ChannelClaimed.Claimed.FirstOrDefault(x => x.Value == myValue).Key;

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == $"Team {myKey + 1} Leader");
            var memrole = Context.Guild.Roles.FirstOrDefault(x => x.Name == $"Team {myKey + 1}");
            foreach (SocketGuildUser member in Context.Message.MentionedUsers)
                await member.AddRoleAsync(memrole);
            await ReplyAsync($"{Context.User.Mention}, {Context.Message.MentionedUsers.Count} users were invited to your voice channel `team-{myKey + 1}`.");

            return;
        }
        [Command("removevc", RunMode = RunMode.Async)]
        public async Task RemoveUserVC([Remainder]string text = "")
        {
            if (Context.Channel.Name != "lfg")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("Voice channel commands may only be used in channel \"lfg\" (looking for group).")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "lfg").Mention} to run voice channel commands.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            if (!ChannelClaimed.Claimed.ContainsValue(Tuple.Create(true, Context.User.Id)))
            {
                await ReplyAsync("You have not claimed a voice channel. Type `d.claimvc` to claim one.");
                return;
            }
            Tuple<bool, ulong> myValue = Tuple.Create(true, Context.User.Id);
            int myKey = ChannelClaimed.Claimed.FirstOrDefault(x => x.Value == myValue).Key;

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == $"Team {myKey + 1} Leader");
            var memrole = Context.Guild.Roles.FirstOrDefault(x => x.Name == $"Team {myKey + 1}");
            var fromchannel = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Name == $"Team-{myKey+1}");
            var tochannel = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Name == "Quacks!");
            foreach (SocketGuildUser member in Context.Message.MentionedUsers)
            {
                await member.RemoveRoleAsync(memrole);
                if(fromchannel.Users.Contains(member))
                {
                    await member.ModifyAsync(x => x.Channel = tochannel);
                }
            }
            
            await ReplyAsync($"{Context.User.Mention}, {Context.Message.MentionedUsers.Count} users were removed from your voice channel `team-{myKey + 1}`.");

            return;
        }
    }
    internal static class ChannelClaimed
    {
        internal static Dictionary<int, Tuple<bool, ulong>> Claimed = new Dictionary<int, Tuple<bool, ulong>>(); //channel num, bool claimed, long id of claimer
        internal static int GetAvail(SocketGuild guild, ulong id)
        {
            for (int i = 0; i< 4; i++)
            {
                if(!NotAvailable(i))
                {
                    ClaimVC(i, true, id);
                    return i;
                }
                var vc = guild.VoiceChannels.FirstOrDefault(x => x.Name == $"team-{i+1}");
                if(User(i) == 0)
                {
                    ClaimVC(i, true, id);
                    return i;
                }
                if (!vc.Users.Contains(guild.GetUser(User(i))))
                {
                    ClaimVC(i, true, id);
                    return i;
                }
            }
            return -1;
        }
        private static bool NotAvailable(int num)
        {
            if (!Claimed.ContainsKey(num))
                Claimed.Add(num, Tuple.Create(false, Convert.ToUInt64(0)));

            return Claimed[num].Item1;
        }
        private static void ClaimVC(int num, bool status, ulong id)
        {
            if (!Claimed.ContainsKey(num))
                Claimed.Add(num, Tuple.Create(status, id));
            Claimed[num] = Tuple.Create(status, id);
        }
        private static UInt64 User(int num)
        {
            if (!Claimed.ContainsKey(num))
                return 0;
            return Claimed[num].Item2;
        }
    }
}
