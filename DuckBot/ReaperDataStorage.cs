using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;

namespace DuckBot
{
    internal static class ReaperDataStorage
    {
        private static Dictionary<ulong, long> NameScore = new Dictionary<ulong, long>();
		private static Dictionary<ulong, Tuple<long, long>> LastReapDict = new Dictionary<ulong, Tuple<long, long>>(); //First item is last harvest time, second item is previous harvest amount.
		private static Dictionary<ulong, long> ReaperBalance = new Dictionary<ulong, long>();

        public static void ClearLastReap()
		{
			LastReapDict.Clear();
		}
        public static bool HasHarvested(ulong id)
		{
			return LastReapDict.ContainsKey(id);
		}
        public static bool HasCorn(ulong id)
		{
			return NameScore.ContainsKey(id);
		}
        public static long GetCorn(ulong id)
		{
			return NameScore[id];
		}
        public static long GetHarvestTime(ulong id)
		{
			return LastReapDict[id].Item1;
		}
        public static long GetPrevHarvest(ulong id)
		{
			return LastReapDict[id].Item2;
		}
        public static List<long> GetScoreList()
		{
			return NameScore.Values.ToList();
		}
        public static ulong ScoreValueToKey(long val)
		{
			return NameScore.FirstOrDefault(x => x.Value == val).Key;
		}
        public static int GetHarvestRanking(ulong id)
		{
			if (!NameScore.ContainsKey(id)) return -1;
			long score = NameScore[id];
			List<long> scorelist = GetScoreList();
			scorelist.Sort();
			int ranking = scorelist.Count() - scorelist.IndexOf(score);
			return ranking;
		}
        public static void AddBalance(ulong id, long amount)
		{
			if (!ReaperBalance.ContainsKey(id))
				ReaperBalance.Add(id, 0);
            ReaperBalance[id] += amount;

			SaveReaperBalance();
		}
        public static void RemoveBalance(ulong id, long amount)
		{
			if (!ReaperBalance.ContainsKey(id))
                ReaperBalance.Add(id, 0);
            ReaperBalance[id] -= amount;

			SaveReaperBalance();
		}
        public static long GetBalance(ulong id)
		{
			if (!ReaperBalance.ContainsKey(id))
                ReaperBalance.Add(id, 0);

			SaveReaperData();

			return ReaperBalance[id];
		}
        public static Tuple<long, long, bool, bool, int> AddNameScore(ulong ReaperID, long addedscore, long recentreap)
        {
            // item1 returns addscore as long, item2 returns finalscore as long, item3 returns newleader as bool, item4 returns finalizegame as bool

            if (!NameScore.ContainsKey(ReaperID))
                NameScore.Add(ReaperID, 0);
            if (!LastReapDict.ContainsKey(ReaperID))
			{
				long defaulttime = 0;
				long defaultprev = -1;
				LastReapDict.Add(ReaperID, Tuple.Create(defaulttime, defaultprev));
			}
            long prevscore = new long();
            long finalscore = new long();
            bool newleader = false;
            bool finalizegame =  false;
            int multiplier = 0;
            prevscore = NameScore[ReaperID];
            finalscore = prevscore + addedscore;
            NameScore[ReaperID] = finalscore;

            SaveReaperData();

			LastReapDict[ReaperID] = Tuple.Create(recentreap, addedscore);

            if (Global.currentleader == 0)
                newleader = true;
            
            else if(finalscore > NameScore[(ulong)Global.currentleader])
                newleader = true;
            
            Global.currentcalculate++;
            if (Global.currentcalculate == Global.ticket)
            {
                Global.currentcalculate = 0;
                Global.ticket = 0;
            }

            if (finalscore > Global.winscore)
            {
                multiplier = FinalizeGame();
                finalizegame = true;
            }

            return Tuple.Create(addedscore, finalscore, newleader, finalizegame, multiplier);
        }

        static ReaperDataStorage()
        {
            //load data
            if (!ValidateExistence(@"/DuckBot/ReaperDataStorage.json")) return;
            string Reaperjson = File.ReadAllText(@"/DuckBot/ReaperDataStorage.json");
            NameScore = JsonConvert.DeserializeObject<Dictionary<ulong, long>>(Reaperjson);
            if (!ValidateExistence(@"/DuckBot/ReaperBalance.json")) return;
            string Balancejson = File.ReadAllText(@"/DuckBot/ReaperBalance.json");
            ReaperBalance = JsonConvert.DeserializeObject<Dictionary<ulong, long>>(Balancejson);
        }

		private static void SaveReaperData()
        {
            //save data
            string Reaperjson = JsonConvert.SerializeObject(NameScore, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/ReaperDataStorage.json", Reaperjson);
        }

		private static void SaveReaperBalance()
        {
            string Balancejson = JsonConvert.SerializeObject(ReaperBalance, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/ReaperBalance.json", Balancejson);
        }
        private static bool ValidateExistence(string file)
        {
            if(!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveReaperData();
                return false;
            }
            return true;
        }
        private static int FinalizeGame()
        {
            List<ulong> scorekeys = ReaperDataStorage.NameScore.Keys.ToList();
            int entries = scorekeys.Count();
            List<long> scorevalues = ReaperDataStorage.NameScore.Values.ToList();
            int overquarter = scorevalues.Count();
            scorevalues.Sort();
            int counter = 0;
            for (int i = 0; i < entries; i++)
            {
                long myValue = scorevalues[entries - i - 1];
                if (myValue > Global.winscore / 4)
                {
                    counter++;
                }
                else
                    break;
            }
            for (int i = 0; i < entries; i++)
            {
				AddBalance(scorekeys[i], NameScore[scorekeys[i]] * counter / 90000);
            }

            return counter;
        }
        internal static void CompleteFinalization()
        {
            Global.ChangeUtil("gamestart", "false");
            Global.ChangeUtil("currentleader", "0");
            Global.ChangeUtil("leaderboardmessageid", "0");
            NameScore.Clear();
			LastReapDict.Clear();
            SaveReaperData();
            SaveReaperBalance();
            Global.ChangeUtil("gamenum", (Global.gamenum+1).ToString());
        }
    }
}
