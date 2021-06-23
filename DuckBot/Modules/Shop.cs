using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;

namespace DuckBot.Modules
{
    public class Shop : InteractiveBase
    {
        EmbedBuilder buildershop = new EmbedBuilder();
        EmbedBuilder builderbuy = new EmbedBuilder();
        EmbedBuilder builderpreview = new EmbedBuilder();
        static string[] AvailableRoles = new string[] { "Grey", "Ebony", "Forest Green", "Brown", "Maroon", "Light Yellow", "Light Purple", "Magenta", "Dark Blue", "Duck Yellow", "Green", "Red Orange", "Orange", "Blue", "Red", "Pink", "Sky Blue", "Light Green", "Raspberry", "Aquamarine", "Cyan", "Neon Green", "Neon Pink" };
        int numRoles = AvailableRoles.Length;
        private static Dictionary<ulong, List<string>> Roles = new Dictionary<ulong, List<string>>();
		static Shop()
        {
            if (!ValidateExistence(@"/DuckBot/BoughtItems.json")) return;
            string Rolesjson = File.ReadAllText(@"/DuckBot/BoughtItems.json");
			Roles = JsonConvert.DeserializeObject<Dictionary<ulong, List<string>>>(Rolesjson);
			//Console.WriteLine("test");
        }
        [Command("shop", RunMode = RunMode.Async)]
        public async Task OpenShopAsync()
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
			ulong id = Context.User.Id;
            buildershop.WithTitle($"Duck Shop")
                       .AddField("Colors–Tier 1: $5000", "Grey, Ebony")
                       .AddField("Colors–Tier 2: $7,500", "Forest Green, Brown, Maroon")
                       .AddField("Colors–Tier 3: $12,500",  "Light Yellow, Light Purple, Magenta, Dark Blue")
                       .AddField("Colors–Tier 4: $20,000", "Duck Yellow, Green, Red Orange, Orange, Blue, Red")
                       .AddField("Colors–Tier 5: $32,500", "Pink, Sky Blue, Light Green, Raspberry, Aquamarine")
                       .AddField("Colors–Tier 6: $50,000", "Cyan, Neon Green, Neon Pink")
                       .AddField("To buy a color:", "Type `d.buy color <insert color>`\nFor example, `d.buy color neon green` to buy the color Neon Green")
                       .WithThumbnailUrl("https://cdn.discordapp.com/attachments/427572233100328962/434046315639472128/shop-front-clipart-2.jpg")
                       .WithColor(3447003);
            await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, buildershop.Build());

            return;
        }

        [Command("buy", RunMode = RunMode.Async)]
        public async Task BuyRoleAsync(string typeinput = "", [Remainder]string iteminput = "")
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
            string type = typeinput.ToLower();
            string item = iteminput.ToLower();
            if (type == "color")
            {
                if (item == "grey")
                {
                    int price = 5000;
                    string name = "Grey";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "ebony")
                {
                    int price = 5000;
                    string name = "Ebony";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "forest green")
                {
                    int price = 7500;
                    string name = "Forest Green";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "brown")
                {
                    int price = 7500;
                    string name = "Brown";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "maroon")
                {
                    int price = 7500;
                    string name = "Maroon";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "light yellow")
                {
                    int price = 12500;
                    string name = "Light Yellow";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "light purple")
                {
                    int price = 12500;
                    string name = "Light Purple";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "magenta")
                {
                    int price = 12500;
                    string name = "Magenta";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "dark blue")
                {
                    int price = 12500;
                    string name = "Dark Blue";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "duck yellow")
                {
                    int price = 20000;
                    string name = "Duck Yellow";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "green")
                {
                    int price = 20000;
                    string name = "Green";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "red orange")
                {
                    int price = 20000;
                    string name = "Red Orange";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "orange")
                {
                    int price = 20000;
                    string name = "Orange";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "blue")
                {
                    int price = 20000;
                    string name = "Blue";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "red")
                {
                    int price = 20000;
                    string name = "Red";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "pink")
                {
                    int price = 32500;
                    string name = "Pink";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "sky blue")
                {
                    int price = 32500;
                    string name = "Sky Blue";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "light green")
                {
                    int price = 32500;
                    string name = "Light Green";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "raspberry")
                {
                    int price = 32500;
                    string name = "Raspberry";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "aquamarine")
                {
                    int price = 32500;
                    string name = "Aquamarine";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "cyan")
                {
                    int price = 50000;
                    string name = "Cyan";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "neon green")
                {
                    int price = 50000;
                    string name = "Neon Green";
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    await BuyItem(item, price, name);
                    return;
                }
                if (item == "neon pink")
                {
                    int price = 50000;
                    string name = "Neon Pink";
                    await SendPreview(item, price, name);
                    var msg = await NextMessageAsync(true, true, TimeSpan.FromMinutes(1));
                    if (msg.Content.ToLower() != "confirm")
                        return;
                    if (!EnoughMoney(price))
                    {
                        await SendFail(item, price);
                        return;
                    }
                    await BuyItem(item, price, name);
                    return;
                }
            }

            await ReplyAsync($"{Context.User.Mention}, please try again with a shop item");
            return;
                
        }
        private bool EnoughMoney(int price)
        {
			if (ReaperDataStorage.GetBalance(Context.User.Id) >= price)
            {
                return true;
            }
            return false;
        }
        private async Task SendFail(string item, int price)
        {
            builderbuy.WithTitle($"Not enough money to buy shop item: **{item}**")
			          .AddField("You have:", $"${ReaperDataStorage.GetBalance(Context.User.Id)}", true)
                      .AddField($"Shop item {item} costs:", $"${price}", true)
                      .WithColor(Color.Red);

            await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderbuy.Build());

            return;
        }
        private async Task SendPreview(string item, int price, string name)
        {
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == name);
            builderpreview.WithTitle($"Do you want to buy shop item **{item}**")
                          .AddField("Price", $"It will cost you ${price}.")
                          .AddField("Color:", "The color is shown on the bar on the left of the embed")
                          .AddField("To buy:", "Type `confirm` to confirm your purchase, you have 60 seconds, any other message will cancel it.")
                          .WithColor(role.Color);
            await ReplyAsync($"{Context.User.Mention}", false, builderpreview.Build());
        }
        private async Task BuyItem(string item, int price, string name)
        {
            var user = Context.User;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == name);
            var userid = Context.User.Id;
            VerifyRoleId(userid);
            if (Roles[userid].Contains(item))
            {
                await ReplyAsync($"{Context.User.Mention}, you already had the color {name} you color enthusiast. Type `d.roleinv` for all your roles you already have.");

                return;
            }
            
            for (int i = 0; i < numRoles; i++)
            {
                if ((user as IGuildUser).RoleIds.Contains(Context.Guild.Roles.FirstOrDefault(x => x.Name == AvailableRoles[i]).Id))
                {
                    await (user as IGuildUser).RemoveRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == AvailableRoles[i]));
                    await ReplyAsync($"{Context.User.Mention}, your previous color of {AvailableRoles[i]} was removed. You can change your color back with the `d.change` commmand. `d.help change` for more info.");
                }
            }

            await (user as IGuildUser).AddRoleAsync(role);
            Roles[userid].Add(item);
            SaveShopData();

			ReaperDataStorage.RemoveBalance(userid, price);
            builderbuy.WithTitle($"Successful purchase of shop item: **{item}**")
			          .AddField($"Shop item {item} costed you:", $"${price}")
			          .AddField("Your remaining balance:", $"${ReaperDataStorage.GetBalance(userid)}")
                      .WithColor(Color.DarkTeal);
            
            await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderbuy.Build());

            return;
        }
        private static void VerifyRoleId(ulong id)
        {
            if(!Roles.ContainsKey(id))
            {
                Roles.Add(id, new List<string>());
            }
        }
        [Command("roleinv", RunMode = RunMode.Async)]
        public async Task RoleInvAsync()
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
            string name = Context.Guild.GetUser(Context.User.Id).Nickname;
            string username = Context.User.Username;
            ulong userid = Context.User.Id;
            if (name == null)
            {
                name = username;
            }
            string roles = "You own no roles.";
            VerifyRoleId(userid);
            if(Roles[userid].Count != 0)
            {
                roles = "You have the following roles:";
                foreach (string role in Roles[userid])
					roles = roles + $" {role}; ";
            }
            EmbedBuilder Roleinv = new EmbedBuilder();

            Roleinv.WithTitle($"Role Inventory of {name}")
                   .WithDescription(roles)
                   .WithColor(Color.Green);
            await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, Roleinv.Build());
        }
        [Command("change", RunMode = RunMode.Async)]
        public async Task ChangeRoleAsync(string typeinput = "", [Remainder]string iteminput = "")
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
            if (typeinput == "color")
            {
				if(StoreChangeRoles.VerifySubmit((long)Context.User.Id))
				{
					await ReplyAsync("You have already changed your color 2 times today. Limited to twice per day.");

					return;
				}
				if(iteminput == "default")
				{
					int counter = 0;
					for (int i = 0; i < numRoles; i++)
                    {
                        if ((Context.User as IGuildUser).RoleIds.Contains(Context.Guild.Roles.FirstOrDefault(x => x.Name == AvailableRoles[i]).Id))
                        {
                            await (Context.User as IGuildUser).RemoveRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == AvailableRoles[i]));
                            await ReplyAsync($"Success! {Context.User.Mention}, your previous color of {AvailableRoles[i]} was removed. You can change your color back with the `d.change` commmand. `d.help change` for more info.");
							counter++;
                        }
                    }
                    if(counter == 0)
					{
						await ReplyAsync("You had the default color previously.");

						return;
					}
					await ReplyAsync($"You now have no color!");
					StoreChangeRoles.ChangeRoleSuccess((long)Context.User.Id);

					return;
				}
                if (Roles[Context.User.Id].Contains(iteminput))
                {
                    TextInfo textInfo = new CultureInfo("en-US",false).TextInfo;
                    string itemname = textInfo.ToTitleCase(iteminput);
                    var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == itemname);
					if((Context.User as IGuildUser).RoleIds.Contains(role.Id))
					{
						await ReplyAsync($"You already had the color {iteminput} equipped!");
						return;
					}
                    for (int i = 0; i < numRoles; i++)
                    {
                        if ((Context.User as IGuildUser).RoleIds.Contains(Context.Guild.Roles.FirstOrDefault(x => x.Name == AvailableRoles[i]).Id))
                        {
                            await (Context.User as IGuildUser).RemoveRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == AvailableRoles[i]));
                            await ReplyAsync($"{Context.User.Mention}, your previous color of {AvailableRoles[i]} was removed. You can change your color back with the `d.change` commmand. `d.help change` for more info.");
                        }
                    }
                    await (Context.User as IGuildUser).AddRoleAsync(role);
                    await ReplyAsync($"Success! You changed your color to {iteminput}");
					StoreChangeRoles.ChangeRoleSuccess((long)Context.User.Id);

					return;
                }
                else
                    await ReplyAsync("Either that color doesn't exist or you haven't bought it yet!");
            }
        }
        private static void SaveShopData()
        {
            //save data
            string Reaperjson = JsonConvert.SerializeObject(Roles, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/BoughtItems.json", Reaperjson);
        }
        private static bool ValidateExistence(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveShopData();
                return false;
            }
            return true;
        }
    }
	internal static class StoreChangeRoles
    {
		private static Dictionary<long, int> Rolechanged = new Dictionary<long, int>();
        static StoreChangeRoles()
        {
            //load data
            if (ValidateExistence(@"/DuckBot/Rolechanged.json"))
            {
                string submittedjson = File.ReadAllText(@"/DuckBot/Rolechanged.json");
				Rolechanged = JsonConvert.DeserializeObject<Dictionary<long, int>>(submittedjson);
                SaveRoleChangedData();
            }
        }
        internal static bool VerifySubmit(long id)
        {
			if (!Rolechanged.ContainsKey(id))
			{
				Rolechanged.Add(id, 0);
				return false;
			}
			if (Rolechanged[id]<2)
			{
				return false;
			}
            return true;
        }
        internal static void ChangeRoleSuccess(long id)
        {
			Rolechanged[id] = Rolechanged[id] + 1;
			SaveRoleChangedData();
        }
		private static void SaveRoleChangedData()
        {
            //save data
			string data = JsonConvert.SerializeObject(Rolechanged, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/Rolechanged.json", data);
        }
        internal static void ClearRoleChanged()
        {
			Rolechanged.Clear();
			SaveRoleChangedData();
        }
        private static bool ValidateExistence(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
				SaveRoleChangedData();
                return false;
            }
            return true;
        }
    }
}
