using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;
using System.Threading.Tasks;
using System.Linq;

namespace DuckBot.Modules
{
	[Group("grow"), Alias("plant")]
    public class Plant : InteractiveBase
    {        
        [Command("hedgehog cactus", RunMode = RunMode.Async)]
        [Alias("hedge", "hedgehog", "cactus1", "c1")]
        public async Task PlantHedgeCact()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            await ReplyAsync(":cactus: Do you wish to plant a hedgehog cactus? Reply <y/n>");
            var response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(30));
            if(response == null)
            {
                await ReplyAsync("Timed Out");
                return;
            }
            if(response.Content.ToLower() == "y")
            {
                string result = Garden.GrowSeeds("c1", Context.User.Id);
                if (result == "s")
                {
                    int time = Garden.GetTimeBetweenWater("c1");                    
                    await ReplyAsync($":cactus: You planted a hedgehog cactus. Come back in {time} minutes to collect it.");
                    return;
                }
                if(result == "f")
                {
                    await ReplyAsync(":x: You do not have any hedgehog cactus seeds to plant");
                    return;
                }
                else
                    await ReplyAsync($"You are already growing a {result}. Finish growing it and collect it to plant a new one.");
            }
            else
            {
                await ReplyAsync("Succesfully cancelled.");
            }
        }
        [Command("prickly pear cactus", RunMode = RunMode.Async)]
        [Alias("pear", "prickly pear", "cactus2", "c2")]
        public async Task PlantPrickPearAsync()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            await ReplyAsync(":cactus: Do you wish to plant a prickly pear cactus? Reply <y/n>");
            var response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(30));
            if (response == null)
            {
                await ReplyAsync("Timed Out");

                return;
            }
            if (response.Content.ToLower() == "y")
            {
                string result = Garden.GrowSeeds("c2", Context.User.Id);
                if (result == "s")
                {
                    int time = Garden.GetTimeBetweenWater("c2");
                    await ReplyAsync($":cactus: You planted a prickly pear cactus. Come back in {time / 60} hour to collect it");

                    return;
                }
                if (result == "f")
                {
                    await ReplyAsync(":x: You do not have any prickly pear cactus seeds to plant");

                    return;
                }
                else
                    await ReplyAsync($"You are already growing a {result}. Finish growing it and collect it to plant a new one.");
            }
            else
            {
                await ReplyAsync("Succesfully cancelled.");
            }
        }
        [Command("golden barrel cactus", RunMode = RunMode.Async)]
        [Alias("barrel", "golden barrel", "cactus3", "c3")]
        public async Task PlantGoldBarAsync()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            await ReplyAsync(":cactus: Do you wish to plant a golden barrel cactus? Reply <y/n>");
            var response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(30));
            if (response == null)
            {
                await ReplyAsync("Timed Out");

                return;
            }
            if (response.Content.ToLower() == "y")
            {
                string result = Garden.GrowSeeds("c3", Context.User.Id);
                if (result == "s")
                {
                    int time = Garden.GetTimeBetweenWater("c3");
                    await ReplyAsync($":cactus: You planted a golden barrel cactus. Come back in {time / 60} hours to collect it");

                    return;
                }
                if (result == "f")
                {
                    await ReplyAsync(":x: You do not have any golden barrel cactus seeds to plant");

                    return;
                }
                else
                    await ReplyAsync($"You are already growing a {result}. Finish growing it and collect it to plant a new one.");
            }
            else
            {
                await ReplyAsync("Succesfully cancelled.");
            }
        }
        [Command("saguaro cactus", RunMode = RunMode.Async)]
        [Alias("saguaro", "cactus4", "c4")]
        public async Task PlantSaguaroAsync()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            await ReplyAsync(":cactus: Do you wish to plant a saguaro cactus? Reply <y/n>");
            var response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(30));
            if (response == null)
            {
                await ReplyAsync("Timed Out");

                return;
            }
            if (response.Content.ToLower() == "y")
            {
                string result = Garden.GrowSeeds("c4", Context.User.Id);
                if (result == "s")
                {
                    int time = Garden.GetTimeBetweenWater("c4");
                    await ReplyAsync($":cactus: You planted a saguaro cactus. Come back in {time / 60} hours to collect it");

                    return;
                }
                if (result == "f")
                {
                    await ReplyAsync(":x: You do not have any saguaro cactus seeds to plant");

                    return;
                }
                else
                    await ReplyAsync($"You are already growing a {result}. Finish growing it and collect it to plant a new one.");
            }
            else
            {
                await ReplyAsync("Succesfully cancelled.");
            }
        }
        [Command("green onion plant", RunMode = RunMode.Async)]
        [Alias("onion", "green onion", "plant1","p1")]
        public async Task PlantGrOnionAsync()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            await ReplyAsync(":seedling: Do you wish to grow a green onion plant? Reply <y/n>");
            var response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(30));
            if (response == null)
            {
                await ReplyAsync("Timed Out");

                return;
            }
            if (response.Content.ToLower() == "y")
            {
                string result = Garden.GrowSeeds("p1", Context.User.Id);
                if (result == "s")
                {
                    int time = Garden.GetTimeBetweenWater("p1");
                    await ReplyAsync($":seedling: You planted a green onion plant. Come back in {time} minutes to water it");

                    return;
                }
                if (result == "f")
                {
                    await ReplyAsync(":x: You do not have any green onion seeds to plant");

                    return;
                }
                else
                    await ReplyAsync($"You are already growing a {result}. Finish growing it and collect it to plant a new one.");
            }
            else
            {
                await ReplyAsync("Succesfully cancelled.");
            }
        }
        [Command("bell pepper plant", RunMode = RunMode.Async)]
        [Alias("pepper", "bell pepper", "plant2", "p2")]
        public async Task PlantBellPepAsync()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            await ReplyAsync(":seedling: Do you wish to grow a bell pepper plant? Reply <y/n>");
            var response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(30));
            if (response == null)
            {
                await ReplyAsync("Timed Out");

                return;
            }
            if (response.Content.ToLower() == "y")
            {
                string result = Garden.GrowSeeds("p2", Context.User.Id);
                if (result == "s")
                {
                    int time = Garden.GetTimeBetweenWater("p2");
                    await ReplyAsync($":seedling: You planted a bell pepper plant. Come back in {time / 60} hours to water it");

                    return;
                }
                if (result == "f")
                {
                    await ReplyAsync(":x: You do not have any bell pepper seeds to plant");

                    return;
                }
                else
                    await ReplyAsync($"You are already growing a {result}. Finish growing it and collect it to plant a new one.");
            }
            else
            {
                await ReplyAsync("Succesfully cancelled.");
            }
        }
        [Command("tomato plant", RunMode = RunMode.Async)]
        [Alias("tomato", "plant3", "p3")]
        public async Task PlantTomatoAsync()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            await ReplyAsync(":tomato: Do you wish to grow a tomato plant? Reply <y/n>");
            var response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(30));
            if (response == null)
            {
                await ReplyAsync("Timed Out");

                return;
            }
            if (response.Content.ToLower() == "y")
            {
                string result = Garden.GrowSeeds("p3", Context.User.Id);
                if (result == "s")
                {
                    int time = Garden.GetTimeBetweenWater("p3");
                    await ReplyAsync($":tomato: You planted a tomato plant. Come back in {time / 60} hours to water it");

                    return;
                }
                if (result == "f")
                {
                    await ReplyAsync(":x: You do not have any tomato seeds to plant");

                    return;
                }
                else
                    await ReplyAsync($"You are already growing a {result}. Finish growing it and collect it to plant a new one.");
            }
            else
            {
                await ReplyAsync("Succesfully cancelled.");
            }
        }
        [Command("rice plant", RunMode = RunMode.Async)]
        [Alias("rice", "plant4", "p4")]
        public async Task PlantRiceAsync()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            await ReplyAsync(":ear_of_rice: Do you wish to grow a rice plant? Reply <y/n>");
            var response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(30));
            if (response == null)
            {
                await ReplyAsync("Timed Out");

                return;
            }
            if (response.Content.ToLower() == "y")
            {
                string result = Garden.GrowSeeds("p4", Context.User.Id);
                if (result == "s")
                {
                    int time = Garden.GetTimeBetweenWater("p4");
                    await ReplyAsync($":ear_of_rice: You planted a rice plant. Come back in {time / 60} hours to water it");

                    return;
                }
                if (result == "f")
                {
                    await ReplyAsync(":x: You do not have any rice seeds to plant");

                    return;
                }
                else
                    await ReplyAsync($"You are already growing a {result}. Finish growing it and collect it to plant a new one.");
            }
            else
            {
                await ReplyAsync("Succesfully cancelled.");
            }
        }
    }
    public class PlantCommands : ModuleBase<SocketCommandContext>
    {
        [Command("mygarden", RunMode = RunMode.Async)]
        public async Task MyGardenAsync()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            string cname = Garden.GetCurGrowing(Context.User.Id);
            if(cname == "")
            {
                await ReplyAsync("You are not currently growing anything.");
                return;
            }
            string plant = Garden.ShortToLongSeedName(Garden.GetCurGrowing(Context.User.Id));
            int waternum = Garden.GetWaterNum(Context.User.Id);
            int timeuntilnextwater = Garden.GetNextWaterTime(Context.User.Id);
            int waterreq = Garden.GetWaterNeeded(cname);
            bool collect = false;
            bool act = false;
            if(waternum == waterreq)
            {
                collect = true;
            }
            if(timeuntilnextwater == 0)
            {
                act = true;
            }
            if(collect)
            {
                if (act)
                {
                    await ReplyAsync($"Your {plant} is ready to collect!");
                    return;
                }
                await ReplyAsync($"Your {plant} requires no more water but is not ready. Wait {timeuntilnextwater / 3600} hours, {(timeuntilnextwater % 3600) / 60} minutes, and {timeuntilnextwater % 60} seconds.");
                return;
            }
            if (act)
            {
                await ReplyAsync($"You may water your plant now. Your {plant} has been watered {waternum} of {waterreq} times already.");
                return;
            }
            await ReplyAsync($"You may water your plant in {timeuntilnextwater / 3600} hours, {(timeuntilnextwater % 3600) / 60} minutes, and {timeuntilnextwater % 60} seconds. Your {plant} has been watered {waternum} of {waterreq} times already.");
        }
        [Command("water", RunMode = RunMode.Async)]
        public async Task WaterPlant()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            var result = Garden.WaterPlants(Context.User.Id);
            if (result.Item4 == 1)
            {
                await ReplyAsync(":droplet: You are not currently growing a plant. You watered the air");
                return;
            }
            if (result.Item4 == 2)
            {
                await ReplyAsync($":droplet: Your {result.Item1} requires no more water. Wait to collect it");
                return;
            }
            if (result.Item4 == 3)
            {
                await ReplyAsync($":droplet: You may not water your {result.Item1} until {result.Item2 / 3600} hours, {(result.Item2 % 3600) / 60} minutes, and {result.Item2 % 60} seconds later. You wouldn't want to kill it with too much water, would you?");
                return;
            }
            if (result.Item4 == 0)
            {
                await ReplyAsync($":droplet: You succesfully watered your {result.Item1}. You have watered it {result.Item2} of {result.Item3} times.");
                return;
            }
            else
                await ReplyAsync("Code broke :(");
        }
        [Command("collect", RunMode = RunMode.Async)]
        public async Task CollectPlant()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            var result = Garden.CollectPlant(Context.User.Id);
            if(result.Item3 == 1)
            {
                await ReplyAsync("You're not growing a plant. You collected some dirt for $0");

                return;
            }
            if(result.Item3 == 2)
            {               
                await ReplyAsync($":droplet: Your {result.Item1} requires more water before it can be collected");

                return;
            }            
            if(result.Item3 == 3)
            {
                await ReplyAsync($"Your {result.Item1} requires no more water but still needs time to finish growing. Collect it {result.Item2 / 3600} hours, {(result.Item2 % 3600) / 60} minutes, and {result.Item2 % 60} seconds later");

                return;
            }
            if (result.Item3 == 0)
            {
				await ReplyAsync($":moneybag: You collected your {result.Item1} for ${result.Item2}. You now have ${ReaperDataStorage.GetBalance(Context.User.Id)}");

                return;
            }
            else
                await ReplyAsync("Code broke :(");
        }
        [Command("seeds", RunMode = RunMode.Async)]
        public async Task SeedsAsync()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            await ReplyAsync($"You have the following seeds: {Garden.ReturnSeeds(Context.User.Id)}");
        }
        [Command("grown")]
        public async Task GrownAsync()
        {
            if (Context.Channel.Name != "garden")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"garden\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "garden").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            await ReplyAsync($"You've grown the following: {Garden.ReturnPlantsGrown(Context.User.Id)}");
        }
    }
}
