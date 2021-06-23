using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord;
using Discord.WebSocket;
using System.Linq;
using DuckBot.Modules;
namespace DuckBot
{
    public static class Invite
    {
        private static Dictionary<ulong, List<ulong>> InviterData;
        private static Dictionary<ulong, Tuple<ulong, string>> UserInfo;
        private static IReadOnlyCollection<IInviteMetadata> oldinvites;
        private static IReadOnlyCollection<IInviteMetadata> newinvites;
        static Invite()
        {
            if (!ValidateExistence(@"/DuckBot/InviterData.json")) return;
            string inviterjson = File.ReadAllText(@"/DuckBot/InviterData.json");
            InviterData = JsonConvert.DeserializeObject<Dictionary<ulong, List<ulong>>>(inviterjson);

            if (!ValidateExistence(@"/DuckBot/UserData.json")) return;
            string userjson = File.ReadAllText(@"/DuckBot/UserData.json");
            UserInfo = JsonConvert.DeserializeObject<Dictionary<ulong, Tuple<ulong, string>>>(userjson);
        }
        public static void LoadInvites(SocketGuild guild)
        {
            oldinvites = guild.GetInvitesAsync().Result;
        }
		public static List<ulong> GetInvites(ulong id)
		{
			if (!InviterData.ContainsKey(id)) return new List<ulong>();
			return InviterData[id];
		}
        public static Tuple<ulong, string> GetUserInfo(ulong id)
		{			
			if(!UserInfo.ContainsKey(id))
			{
				ulong num = 0;
				return Tuple.Create(num, "Nobody knows");
			}
			return UserInfo[id];
		}
        internal static Tuple<string, ulong> InviteUsed(SocketGuild guild, SocketGuildUser user)
        {
            newinvites = guild.GetInvitesAsync().Result;
            var old = ToDict(oldinvites);
            var newd = ToDict(newinvites);
			oldinvites = newinvites;
            foreach (KeyValuePair<string, Tuple<int, ulong>> pair in newd)
            {
                if (!old.ContainsKey(pair.Key) && pair.Value.Item1 > 0)
                {
                    Updatedata(pair.Value.Item2, user.Id, DateTime.Now.ToString());
                    return Tuple.Create(pair.Key, pair.Value.Item2);
                }
                if (old[pair.Key].Item1 < pair.Value.Item1)
                {
                    Updatedata(pair.Value.Item2, user.Id, DateTime.Now.ToString());
                    return Tuple.Create(pair.Key, pair.Value.Item2);
                }
            }
            return Tuple.Create("Expired Invite", new ulong());
        }
        private static void Updatedata(ulong userid, ulong invited, string date)
        {
            if (!UserInfo.ContainsKey(invited))
            {
                UserInfo.Add(invited, Tuple.Create(userid, date));
            }
        }
        internal static void UserInviteGood(ulong invited)
		{
			if (!UserInfo.ContainsKey(invited)) return;
			ulong userid = UserInfo[invited].Item1;
			if (!InviterData.ContainsKey(userid))
            {
                InviterData.Add(userid, new List<ulong>());
            }
			if(!InviterData[userid].Contains(invited))
			{
				InviterData[userid].Add(invited);
                SaveData();
                ReaperDataStorage.AddBalance(userid, 1000);
			}
		}
        private static Dictionary<string, Tuple<int, ulong>> ToDict(IReadOnlyCollection<IInviteMetadata> invites)
        {
            Dictionary<string, Tuple<int, ulong>> returnval = new Dictionary<string, Tuple<int, ulong>>();
            foreach (IInviteMetadata inv in invites)
            {
                returnval.Add(inv.Code, Tuple.Create((int)inv.Uses, inv.Inviter.Id));
            }
            return returnval;
        }
        private static void SaveData()
        {
            string inviterjson = JsonConvert.SerializeObject(InviterData, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/InviterData.json", inviterjson);

            string userjson = JsonConvert.SerializeObject(UserInfo, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/UserData.json", userjson);
        }
        private static bool ValidateExistence(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveData();
                return false;
            }
            return true;
        }
    }
}
