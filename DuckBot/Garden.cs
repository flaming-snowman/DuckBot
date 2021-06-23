using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DuckBot.Modules;
using Newtonsoft.Json;

namespace DuckBot
{
    public static class Garden
    {
        private static Dictionary<UInt64, SeedProfile> GardenAcnts = new Dictionary<ulong, SeedProfile>();
        private static readonly SeedInfo c1info = new SeedInfo(5, 0, 20);
        private static readonly SeedInfo c2info = new SeedInfo(60, 0, 100);
        private static readonly SeedInfo c3info = new SeedInfo(480, 0, 500);
        private static readonly SeedInfo c4info = new SeedInfo(2880, 0, 2500);
        private static readonly SeedInfo p1info = new SeedInfo(2, 4, 50);
        private static readonly SeedInfo p2info = new SeedInfo(120, 1, 500);
        private static readonly SeedInfo p3info = new SeedInfo(60, 3, 600);
        private static readonly SeedInfo p4info = new SeedInfo(180, 7, 5000);
        static Garden()
        {
            if (!ValidateExistence(@"/DuckBot/GardenData.json")) return;
            string Gardenjson = File.ReadAllText(@"/DuckBot/GardenData.json");
            GardenAcnts = JsonConvert.DeserializeObject<Dictionary<UInt64, SeedProfile>>(Gardenjson);            
        }
        public static string HarvestSeeds(UInt64 id)
        {
            ValidateAcnt(id);
            Random rnd = new Random();
            double hours = Global.reapwaittime / 3600000;
            if(hours > 2)
			{
				hours = Math.Sqrt(2 * hours);
			}
			int c1 = rnd.Next(0, (int)Math.Ceiling(hours));
            int p1 = rnd.Next(0, (int)Math.Floor(hours/2));
            int c2 = 0;
            int c3 = 0;
            int c4 = 0;
            int p2 = 0;
            int p3 = 0;
            int p4 = 0;
            if (RunPercent() <= 15 * hours) c2 = 1;
            if (RunPercent() <= 5 * hours) c3 = 1;
            if (RunPercent() <= 1 * hours) c4 = 1;
            if (RunPercent() <= 4 * hours) p2 = 1;
            if (RunPercent() <= 4 * hours) p3 = 1;
            if (RunPercent() <= 1 * hours) p4 = 1;
            UpdateAcnt(id, c1, c2, c3, c4, p1, p2, p3, p4);
            return GenerateSeedMsg(c1, c2, c3, c4, p1, p2, p3, p4, "Seeds");
        }
        public static string GrowSeeds(string seed, UInt64 user)
        {
			ValidateAcnt(user);
            if (GardenAcnts[user].CurrentlyGrowing == "")
            {
                if (!DeductPlantedSeed(seed, user))
                    return "f";            
                GardenAcnts[user].CurrentlyGrowing = seed;                
                GardenAcnts[user].LastWatered = DateTime.Now.ToString();
                GardenAcnts[user].WaterNum = 0;
                SaveGardenData();
                return "s";
            }
            else
                return ShortToLongSeedName(GardenAcnts[user].CurrentlyGrowing);
        }
        private static bool DeductPlantedSeed(string seed, UInt64 id)
        {
            if(seed == "c1")
            {
                if (GardenAcnts[id].HedgehogCactusSeeds <= 0)
                    return false;
                GardenAcnts[id].HedgehogCactusSeeds = GardenAcnts[id].HedgehogCactusSeeds - 1;
                return true;
            }
            if (seed == "c2")
            {
                if (GardenAcnts[id].PricklyPearCactusSeeds <= 0)
                    return false;
                GardenAcnts[id].PricklyPearCactusSeeds = GardenAcnts[id].PricklyPearCactusSeeds - 1;
                return true;
            }            
            if (seed == "c3")
            {
                if (GardenAcnts[id].GoldenBarrelCactusSeeds<= 0)
                    return false;
                GardenAcnts[id].GoldenBarrelCactusSeeds = GardenAcnts[id].GoldenBarrelCactusSeeds - 1;
                return true;
            }          
            if (seed == "c4")
            {
                if (GardenAcnts[id].SaguaroCactusSeeds<= 0)
                    return false;
                GardenAcnts[id].SaguaroCactusSeeds = GardenAcnts[id].SaguaroCactusSeeds - 1;
                return true;
            }            
            if (seed == "p1")
            {
                if (GardenAcnts[id].GreenOnionSeeds<= 0)
                    return false;
                GardenAcnts[id].GreenOnionSeeds = GardenAcnts[id].GreenOnionSeeds - 1;
                return true;
            }           
            if (seed == "p2")
            {
                if (GardenAcnts[id].BellPepperSeeds<= 0)
                    return false;
                GardenAcnts[id].BellPepperSeeds = GardenAcnts[id].BellPepperSeeds - 1;
                return true;
            }           
            if (seed == "p3")
            {
                if (GardenAcnts[id].TomatoSeeds <= 0)
                    return false;
                GardenAcnts[id].TomatoSeeds = GardenAcnts[id].TomatoSeeds - 1;
                return true;
            }            
            if (seed == "p4")
            {
                if (GardenAcnts[id].RiceSeeds <= 0)
                    return false;
                GardenAcnts[id].RiceSeeds = GardenAcnts[id].RiceSeeds - 1;
                return true;
            }
            return false;
        }
        private static void UpdateAcnt(UInt64 id, int c1, int c2, int c3, int c4, int p1, int p2, int p3, int p4)
        {
            GardenAcnts[id].HedgehogCactusSeeds = GardenAcnts[id].HedgehogCactusSeeds + c1;
            GardenAcnts[id].PricklyPearCactusSeeds = GardenAcnts[id].PricklyPearCactusSeeds + c2;
            GardenAcnts[id].GoldenBarrelCactusSeeds = GardenAcnts[id].GoldenBarrelCactusSeeds + c3;
            GardenAcnts[id].SaguaroCactusSeeds = GardenAcnts[id].SaguaroCactusSeeds + c4;
            GardenAcnts[id].GreenOnionSeeds = GardenAcnts[id].GreenOnionSeeds + p1;
            GardenAcnts[id].BellPepperSeeds = GardenAcnts[id].BellPepperSeeds + p2;
            GardenAcnts[id].TomatoSeeds = GardenAcnts[id].TomatoSeeds + p3;
            GardenAcnts[id].RiceSeeds = GardenAcnts[id].RiceSeeds + p4;
            SaveGardenData();
        }
        public static string ShortToLongSeedName(string seed)
        {
            if (seed == "c1")
                return "HedgeHog Cactus";
            if (seed == "c2")
                return "Prickly Pear Cactus";
            if (seed == "c3")
                return "Golden Barrel Cactus";
            if (seed == "c4")
                return "Saguaro Cactus";
            if (seed == "p1")
                return "Green Onion Plant";
            if (seed == "p2")
                return "Bell Pepper Plant";
            if (seed == "p3")
                return "Tomato Plant";
            if (seed == "p4")
                return "Rice Plant";
            else
                return "Invalid";
        }
        public static Tuple<string, int, int, int> WaterPlants(UInt64 id) //Plant name, Water num, Water Req,Execution Result
        {
			ValidateAcnt(id);
            if(GardenAcnts[id].CurrentlyGrowing == "")
            {
                return Tuple.Create("", 0, 0, 1);
            }
            int TimeBetween = GetTimeBetweenWater(GardenAcnts[id].CurrentlyGrowing);
            int WaterReq = GetWaterNeeded(GardenAcnts[id].CurrentlyGrowing);
            if(GardenAcnts[id].WaterNum >= WaterReq)
            {
                return Tuple.Create(ShortToLongSeedName(GardenAcnts[id].CurrentlyGrowing), 0, 0, 2);
            }
            DateTime WaterTime = new DateTime();
            if (DateTime.TryParse(GardenAcnts[id].LastWatered, out WaterTime))
            {
                if (DateTime.Now - WaterTime > TimeSpan.FromMinutes(TimeBetween))
                {
                    GardenAcnts[id].LastWatered = DateTime.Now.ToString();
                    GardenAcnts[id].WaterNum = GardenAcnts[id].WaterNum + 1;
                    SaveGardenData();
                    return Tuple.Create(ShortToLongSeedName(GardenAcnts[id].CurrentlyGrowing), GardenAcnts[id].WaterNum, WaterReq, 0);
                }
                return Tuple.Create(ShortToLongSeedName(GardenAcnts[id].CurrentlyGrowing), (int)Math.Ceiling((WaterTime.AddMinutes(TimeBetween) - DateTime.Now).TotalSeconds), 0, 3);
            }
            else return Tuple.Create("", 0, 0, -1);
        }
        public static Tuple<string, int, int> CollectPlant(UInt64 id) //Plant name, Payout, Exe result
        {
			ValidateAcnt(id);
            if (GardenAcnts[id].CurrentlyGrowing == "")
            {
                return Tuple.Create("", 0, 1);
            }
            int TimeBetween = GetTimeBetweenWater(GardenAcnts[id].CurrentlyGrowing);
            int WaterReq = GetWaterNeeded(GardenAcnts[id].CurrentlyGrowing);
            if (GardenAcnts[id].WaterNum < WaterReq)
            {
                return Tuple.Create(ShortToLongSeedName(GardenAcnts[id].CurrentlyGrowing), 0, 2);
            }
            DateTime WaterTime = new DateTime();
            if (DateTime.TryParse(GardenAcnts[id].LastWatered, out WaterTime))
            {
                if (DateTime.Now - WaterTime > TimeSpan.FromMinutes(TimeBetween))
                {
                    UpdateGrown(id, GardenAcnts[id].CurrentlyGrowing);
                    int AddMoney = GetPayout(GardenAcnts[id].CurrentlyGrowing);
                    string prevplant = ShortToLongSeedName(GardenAcnts[id].CurrentlyGrowing);
                    GardenAcnts[id].CurrentlyGrowing = "";
                    SaveGardenData();
					ReaperDataStorage.AddBalance(id, AddMoney);
                    return Tuple.Create(prevplant, AddMoney, 0);
                }
                return Tuple.Create(ShortToLongSeedName(GardenAcnts[id].CurrentlyGrowing), (int)Math.Ceiling((WaterTime.AddMinutes(TimeBetween) - DateTime.Now).TotalSeconds), 3);
            }
            else return Tuple.Create("", 0, -1);
        }
        public static string GetCurGrowing(UInt64 id)
        {
			ValidateAcnt(id);
            return GardenAcnts[id].CurrentlyGrowing;
        }
        public static int GetWaterNum(UInt64 id)
        {
			ValidateAcnt(id);
            return GardenAcnts[id].WaterNum;
        }
        public static int GetNextWaterTime(UInt64 id)
        {
			ValidateAcnt(id);
            if (GardenAcnts[id].CurrentlyGrowing == "")
                return 0;
            if((DateTime.Parse(GardenAcnts[id].LastWatered).AddMinutes(GetTimeBetweenWater(GardenAcnts[id].CurrentlyGrowing)) - DateTime.Now).TotalSeconds < 0)
            {
                return 0;
            }
            return (int)(DateTime.Parse(GardenAcnts[id].LastWatered).AddMinutes(GetTimeBetweenWater(GardenAcnts[id].CurrentlyGrowing)) - DateTime.Now).TotalSeconds;
        }
        public static int GetPayout(string plant)
        {
            if (plant == "c1")
                return c1info.Payout;
            if (plant == "c2")
                return c2info.Payout;
            if (plant == "c3")
                return c3info.Payout;
            if (plant == "c4")
                return c4info.Payout;
            if (plant == "p1")
                return p1info.Payout;
            if (plant == "p2")
                return p2info.Payout;
            if (plant == "p3")
                return p3info.Payout;
            if (plant == "p4")
                return p4info.Payout;
            else return 0;
        }
        public static int GetWaterNeeded(string plant)
        {
            if (plant == "c1")
                return c1info.NumOfWaters;
            if (plant == "c2")
                return c2info.NumOfWaters;
            if (plant == "c3")
                return c3info.NumOfWaters;
            if (plant == "c4")
                return c4info.NumOfWaters;
            if (plant == "p1")
                return p1info.NumOfWaters;
            if (plant == "p2")
                return p2info.NumOfWaters;
            if (plant == "p3")
                return p3info.NumOfWaters;
            if (plant == "p4")
                return p4info.NumOfWaters;
            else return 0;
        }
        public static int GetTimeBetweenWater(string plant)
        {
            if (plant == "c1")
                return c1info.TimeBetweenWaters;
            if (plant == "c2")
                return c2info.TimeBetweenWaters;
            if (plant == "c3")
                return c3info.TimeBetweenWaters;
            if (plant == "c4")
                return c4info.TimeBetweenWaters;
            if (plant == "p1")
                return p1info.TimeBetweenWaters;
            if (plant == "p2")
                return p2info.TimeBetweenWaters;
            if (plant == "p3")
                return p3info.TimeBetweenWaters;
            if (plant == "p4")
                return p4info.TimeBetweenWaters;
            else return 0;
        }
        public static string ReturnSeeds(UInt64 id)
		{
			ValidateAcnt(id);
            return GenerateSeedMsg(GardenAcnts[id].HedgehogCactusSeeds, GardenAcnts[id].PricklyPearCactusSeeds, GardenAcnts[id].GoldenBarrelCactusSeeds, GardenAcnts[id].SaguaroCactusSeeds, GardenAcnts[id].GreenOnionSeeds, GardenAcnts[id].BellPepperSeeds, GardenAcnts[id].TomatoSeeds, GardenAcnts[id].RiceSeeds, "Seeds");
        }
        public static string ReturnPlantsGrown(UInt64 id)
        {
			ValidateAcnt(id);
            return GenerateSeedMsg(GardenAcnts[id].GrownPlants[0], GardenAcnts[id].GrownPlants[1], GardenAcnts[id].GrownPlants[2], GardenAcnts[id].GrownPlants[3], GardenAcnts[id].GrownPlants[4], GardenAcnts[id].GrownPlants[5], GardenAcnts[id].GrownPlants[6], GardenAcnts[id].GrownPlants[7], "Plants");
        }
        private static void UpdateGrown(UInt64 id, string plant)
        {
            if (plant == "c1")
                GardenAcnts[id].GrownPlants[0] = GardenAcnts[id].GrownPlants[0] + 1;
            if (plant == "c2")
                GardenAcnts[id].GrownPlants[1] = GardenAcnts[id].GrownPlants[1] + 1;
            if (plant == "c3")
                GardenAcnts[id].GrownPlants[2] = GardenAcnts[id].GrownPlants[2] + 1;
            if (plant == "c4")
                GardenAcnts[id].GrownPlants[3] = GardenAcnts[id].GrownPlants[3] + 1;
            if (plant == "p1")
                GardenAcnts[id].GrownPlants[4] = GardenAcnts[id].GrownPlants[4] + 1;
            if (plant == "p2")
                GardenAcnts[id].GrownPlants[5] = GardenAcnts[id].GrownPlants[5] + 1;
            if (plant == "p3")
                GardenAcnts[id].GrownPlants[6] = GardenAcnts[id].GrownPlants[6] + 1;
            if (plant == "p4")
                GardenAcnts[id].GrownPlants[7] = GardenAcnts[id].GrownPlants[7] + 1;           
        }
        private static string GenerateSeedMsg(int c1, int c2, int c3, int c4, int p1, int p2, int p3, int p4, string type)
        {
            string msg = "";
			int value = 0;
            if(c1 != 0)
            {
                msg = msg + $" {c1} Hedgehog Cactus {type},";
				value += c1 * GetPayout("c1");
            }
            if (c2 != 0)
            {
                msg = msg + $" {c2} Prickly Pear {type},";
				value += c2 * GetPayout("c2");
            }
            if (c3 != 0)
            {
                msg = msg + $" {c3} Golden Barrel Cactus {type},";
				value += c3 * GetPayout("c3");
            }
            if (c4 != 0)
            {
                msg = msg + $" {c4} Saguaro {type},";
				value += c4 * GetPayout("c4");
            }
            if (p1 != 0)
            {
                msg = msg + $" {p1} Green Onion {type},";
				value += p1 * GetPayout("p1");
            }
            if (p2 != 0)
            {
                msg = msg + $" {p2} Bell Pepper {type},";
				value += p2 * GetPayout("p2");
            }
            if (p3 != 0)
            {
                msg = msg + $" {p3} Tomato {type},";
				value += p3 * GetPayout("p3");
            }
            if (p4 != 0)
            {
                msg = msg + $" {p4} Rice {type},";
				value += p4 * GetPayout("p4");
            }
            msg = msg.TrimEnd(',');
            if(msg == "")
			{
				msg = "None";
			}
			msg = msg + $"\nValue: ${value}";

            return msg;
        }
        private static int RunPercent()
        {
            Random rnd = new Random();
            return rnd.Next(1, 100);
        }
        private static void ValidateAcnt(UInt64 id)
        {
            if(!GardenAcnts.ContainsKey(id))
            {
                GardenAcnts.Add(id, new SeedProfile());
                SaveGardenData();
            }
        }
        private static void SaveGardenData()
        {
            string Gardenjson = JsonConvert.SerializeObject(GardenAcnts, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/GardenData.json", Gardenjson);
        }
        private static bool ValidateExistence(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveGardenData();
                return false;
            }
            return true;
        }
    }
    public class SeedProfile
    {
        public int GreenOnionSeeds { get; set; } = 0;
        public int BellPepperSeeds { get; set; } = 0;
        public int TomatoSeeds { get; set; } = 0;
        public int RiceSeeds { get; set; } = 0;
        public int HedgehogCactusSeeds { get; set; } = 0;
        public int PricklyPearCactusSeeds { get; set; } = 0;
        public int GoldenBarrelCactusSeeds { get; set; } = 0;
        public int SaguaroCactusSeeds { get; set; } = 0;
        public string CurrentlyGrowing { get; set; } = "";
        public string LastWatered { get; set; } = "";
        public int WaterNum { get; set; } = 0;        
        public int[] GrownPlants { get; set; } = { 0, 0, 0, 0, 0, 0, 0, 0 };
    }
    public class SeedInfo
    {
        public readonly int TimeBetweenWaters;
        public readonly int NumOfWaters;
        public readonly int Payout;
        public SeedInfo (int time, int num, int pay)
        {
            TimeBetweenWaters = time;
            NumOfWaters = num;
            Payout = pay;
        }
    }
}
