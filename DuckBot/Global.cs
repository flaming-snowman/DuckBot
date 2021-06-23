using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DuckBot
{
    internal static class Global
    {
        internal static int ticket = 0;
        internal static int currentcalculate = 0;

        internal static long reapwaittime = new long();
        internal static long winscore = new long();
        internal static long currentleader = new long();
        internal static bool gamestart = new bool();
        internal static long leaderboardmessageid = new long();
        internal static long lastrecipeid = new long();
        internal static string weektime = null;
        internal static int gamenum = new int();
		internal static bool norecipes = new bool();
		internal static long masterduck = new long();
		internal static bool harvesthours = new bool();
        internal static int votes = new int();
        private static Dictionary<string, string> utilvals= new Dictionary<string, string>();

        static Global()
        {
            ValidateExistence(@"/DuckBot/Util.json");
            string utiljson = File.ReadAllText(@"/DuckBot/Util.json");
            utilvals = JsonConvert.DeserializeObject<Dictionary<string, string>>(utiljson);
            if (!utilvals.ContainsKey("reapwaittime"))
                utilvals.Add("reapwaittime", "0");
            if (!utilvals.ContainsKey("winscore"))
                utilvals.Add("winscore", "0");
            if (!utilvals.ContainsKey("gamestart"))
                utilvals.Add("gamestart", "false");
            if (!utilvals.ContainsKey("currentleader"))
                utilvals.Add("currentleader", "0");
            if (!utilvals.ContainsKey("leaderboardmessageid"))
                utilvals.Add("leaderboardmessageid", "0");
            if (!utilvals.ContainsKey("lastrecipeid"))
                utilvals.Add("lastrecipeid", "0");
            if (!utilvals.ContainsKey("weektime"))
                utilvals.Add("weektime", "");
            if (!utilvals.ContainsKey("gamenum"))
                utilvals.Add("gamenum", "1");
			if (!utilvals.ContainsKey("norecipes"))
                utilvals.Add("norecipes", "true");
			if (!utilvals.ContainsKey("masterduck"))
                utilvals.Add("masterduck", "0");
			if (!utilvals.ContainsKey("harvesthours"))
                utilvals.Add("harvesthours", "false");
            if (!utilvals.ContainsKey("votes"))
                utilvals.Add("votes", "7");
            SaveUtilData();
            reapwaittime = Convert.ToInt64(utilvals["reapwaittime"]);
            winscore = Convert.ToInt64(utilvals["winscore"]);
            currentleader = Convert.ToInt64(utilvals["currentleader"]);
            gamestart = Convert.ToBoolean(utilvals["gamestart"]);
            leaderboardmessageid = Convert.ToInt64(utilvals["leaderboardmessageid"]);
            lastrecipeid = Convert.ToInt64(utilvals["lastrecipeid"]);
            weektime = utilvals["weektime"].ToString();
            gamenum = Convert.ToInt32(utilvals["gamenum"]);
			norecipes = Convert.ToBoolean(utilvals["norecipes"]);
			masterduck = Convert.ToInt64(utilvals["masterduck"]);
			harvesthours = Convert.ToBoolean(utilvals["harvesthours"]);
            votes = Convert.ToInt32(utilvals["votes"]);
        }
        private static void ChangeVars()
        {
            reapwaittime = Convert.ToInt64(utilvals["reapwaittime"]);
            winscore = Convert.ToInt64(utilvals["winscore"]);
            gamestart = Convert.ToBoolean(utilvals["gamestart"]);
            currentleader = Convert.ToInt64(utilvals["currentleader"]);
            leaderboardmessageid = Convert.ToInt64(utilvals["leaderboardmessageid"]);
            lastrecipeid = Convert.ToInt64(utilvals["lastrecipeid"]);
            weektime = utilvals["weektime"].ToString();
            gamenum = Convert.ToInt32(utilvals["gamenum"]);
			norecipes = Convert.ToBoolean(utilvals["norecipes"]);
			masterduck = Convert.ToInt64(utilvals["masterduck"]);
			harvesthours = Convert.ToBoolean(utilvals["harvesthours"]);
            votes = Convert.ToInt32(utilvals["votes"]);
        }
        private static void SaveUtilData()
        {
            //save data
            string utiljson = JsonConvert.SerializeObject(utilvals, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/Util.json", utiljson);
        }
        internal static Task ChangeUtil(string key, string value)
        {
            //add or change data
            if (!utilvals.ContainsKey(key))
                utilvals.Add(key, "");
            
            utilvals[key] = value;
            ChangeVars();
            SaveUtilData();

            return Task.CompletedTask;
        }
        private static void ValidateExistence(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
            }
        }
    }

}
