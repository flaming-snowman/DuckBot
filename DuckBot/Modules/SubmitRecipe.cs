using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.Addons.Interactive;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace DuckBot.Modules
{
    public class SubmitRecipe : InteractiveBase
    {
        EmbedBuilder builderrecipe = new EmbedBuilder();
        EmbedBuilder buildersuccess = new EmbedBuilder();
        EmbedBuilder builderfail = new EmbedBuilder();
        EmbedBuilder buildernull = new EmbedBuilder();

        [Command("submitstory", RunMode = RunMode.Async)]
        public async Task SubmitrecipeAsync([Remainder] string text = "")
        {
            //Context.User;
            //Context.Client;
            //Context.Guild;
            //Context.Message;
            //Context.Channel.

            long submituser = Convert.ToInt64(Context.User.Id);
            string name = Context.Guild.GetUser(Context.User.Id).Nickname;
            if(name==null)
            {
                name = Context.User.Username;
            }
            var sendtochannel = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "storyboard");
            var sendfromchannel = Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "submit-stories-here");

            builderrecipe.WithTitle("__**Story**__")
                         .AddField("Submitted by Discord User: ", $"{Context.User.Mention}", true)
                         .AddField("Nickname:", $"{name}", true)
                         .WithDescription(text)
                         .WithColor(Color.Teal);


            buildersuccess.WithTitle("Success!")
                          .WithDescription($"Your story has been submitted to {sendtochannel.Mention}.")
                  .WithColor(Color.LightGrey);


            builderfail.WithTitle("Error!")
                       .WithDescription($"Wrong channel. The d.submitstory command can only be used in {sendfromchannel.Mention}.")
                       .WithColor(Color.Red);
            

            buildernull.WithTitle("Error!")
                       .WithDescription("No message. The d.submitstory command can only be used with a provided story following it.")
                       .WithColor(Color.Red);
            
            if (Context.Channel.Name == "submit-stories-here" && text != "")
            {
                if (Storerecipetime.VerifySubmit(submituser))
                {
                    await sendfromchannel.SendMessageAsync($"{Context.User.Mention}, Type `confirm` to submit and `cancel` to cancel and make adjustments. Your recipe is attached the way it will be submitted. Command will cancel in 30 seconds if no reply.", false, builderrecipe.Build());
                    var response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(30));
                    if(response == null)
                    {
                        await ReplyAsync("You timed out, please try again");

                        return;
                    }
                    int counter = 0;
                    while(response.Content.ToLower() != "confirm" && response.Content.ToLower() != "cancel")
                    {
                        counter++;
                        if(counter > 4)
                        {
                            await ReplyAsync("You suck, you couldn't even type `confirm` or `cancel`.");

                            return;
                        }
                        await ReplyAsync("Invalid message. Type `confirm` or `cancel`. 10 seconds to respond.");
                        response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(10));

                        if(response == null)
                        {
                            await ReplyAsync("You timed out, please try again.");

                            return;
                        }
                    }
                    if(response.Content.ToLower() == "confirm")
                    {
                        await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, buildersuccess.Build());
                        RestUserMessage msg = await sendtochannel.SendMessageAsync(Context.User.Mention, false, builderrecipe.Build());
                        Storerecipetime.MessageSend(submituser);
                        var up = new Emoji("⬆");
                        await msg.AddReactionAsync(up);
                        var down = new Emoji("⬇");
                        await msg.AddReactionAsync(down);

                        return;
                    }
                    if(response.Content.ToLower() == "cancel")
                    {
                        await ReplyAsync("You cancelled your submission. Your story was not submitted. You may try again and resubmit.");

                        return;
                    }
                }
                await ReplyAsync($":no_entry_sign: {Context.User.Mention}, you have already submitted a story today, please try again tomorrow.");
            }
            else if (Context.Channel.Name != "submit-stories-here")
            {
                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderfail.Build());

                return;
            }
            else if (text == "")
            {
                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, buildernull.Build());

                return;
            }
        }
        [Command("changevotes"), RequireOwner]
        public async Task VoteChangeAsync(int votes)
        {
            await Global.ChangeUtil("votes", votes.ToString());

            await ReplyAsync($"The required votes to join the story contest has been changed to {votes}.");
        }
    }
    internal static class Storerecipetime
    {
        private static List<long> Recipesubmitted = new List<long>();
        static Storerecipetime()
        {
            //load data
            if (ValidateExistence(@"/DuckBot/Recipesubmitted.json"))
            {
                string submittedjson = File.ReadAllText(@"/DuckBot/Recipesubmitted.json");
                Recipesubmitted = JsonConvert.DeserializeObject<List<long>>(submittedjson);
                SaveSubmittedData();
            }
        }
        internal static bool VerifySubmit(long id)
        {
            if (Recipesubmitted.Contains(id))
                return false;

            return true;
        }
        internal static void MessageSend(long id)
        {
            Recipesubmitted.Add(id);
            SaveSubmittedData();
        }
        private static void SaveSubmittedData()
        {
            //save data
            string data = JsonConvert.SerializeObject(Recipesubmitted, Formatting.Indented);
            File.WriteAllText(@"/DuckBot/Recipesubmitted.json", data);
        }
        internal static void ClearRecipeSubmitted()
        {
            Recipesubmitted.Clear();
            SaveSubmittedData();
        }
        private static bool ValidateExistence(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
                SaveSubmittedData();
                return false;
            }
            return true;
        }
    }
    public static class VoteRecipe
    {
        internal static async Task CheckTime()
        {
            //Time when method needs to be called
            string WeekTime = Global.weektime;

            var dateNow = DateTime.Now;

            DateTime date = DateTime.ParseExact(WeekTime, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
            
            TimeSpan ts;
			if (date > dateNow)
				ts = date - dateNow;
			else
			{
				date = date.AddDays(7);
				await Global.ChangeUtil("weektime", date.ToString("yyyy-MM-dd HH:mm:ss"));
				ts = date - dateNow;
			}
            //waits certan time and run the code
            Clock.StartFridayTimer(ts, date);
			date = date.AddDays(-1);
			ts = date - dateNow;
			if(ts.TotalMilliseconds>0)
			{
				Clock.StartThursdayTimer(ts);
			}
        }
        internal static async Task MoveRecipes(SocketGuild guild)
        {
			int requiredvotes = Global.votes;
            Dictionary<ulong, Tuple<ulong, int>> messageids = new Dictionary<ulong, Tuple<ulong, int>>(); //Key: AuthorID, Value: MessageID, Votes
            ulong id = (ulong)Global.lastrecipeid;
            var sendtochannel = guild.TextChannels.FirstOrDefault(x => x.Name == "storyboard");
            var finalchannel = guild.TextChannels.FirstOrDefault(x => x.Name == "story-contest");
            var up = new Emoji("⬆");
            var down = new Emoji("⬇");
            IEnumerable<IMessage> messagesold = await sendtochannel.GetMessagesAsync(id, Direction.After).FlattenAsync();
            var msgs = await finalchannel.GetMessagesAsync(100).FlattenAsync();
            await finalchannel.DeleteMessagesAsync(msgs);
			if (!messagesold.Any())
			{
				await finalchannel.SendMessageAsync("No stories were even submitted this week.");
				var sendmessage2 = await sendtochannel.SendMessageAsync("No stories were submitted this week.");
				await Global.ChangeUtil("lastrecipeid", sendmessage2.Id.ToString());
				await Global.ChangeUtil("norecipes", "true");
				return;
			}
            for (int i = 0; i < messagesold.Count(); i++)
            {
                IMessage imessage = messagesold.ElementAt(i);
                IUserMessage message = imessage as IUserMessage;
                ulong authorid = message.MentionedUserIds.FirstOrDefault();
                int count = message.Reactions[up].ReactionCount;                
                if (count >= requiredvotes)
                {
                    if (messageids.ContainsKey(authorid))
                    {
                        if (count >= messageids[authorid].Item2)
                        {
                            messageids[authorid] = Tuple.Create(message.Id, count);
                        }
                        continue;
                    }
                    messageids.Add(authorid, Tuple.Create(message.Id, count));
                }
            }
            
			if(messageids.Count == 0)
			{
				var sendmessage1 = await sendtochannel.SendMessageAsync($"No stories reached the required vote threshold of {Global.votes} this week.");
				await finalchannel.SendMessageAsync("No recipes reached the required vote threshold this week.");
				await Global.ChangeUtil("lastrecipeid", sendmessage1.Id.ToString());
				await Global.ChangeUtil("norecipes", "true");
				return;
			}
            for (int i = 0; i < messageids.Count; i++)
            {
                var pair = messageids.ElementAt(i);
                var message = sendtochannel.GetMessageAsync(pair.Value.Item1).Result.Embeds;
				var sent = await finalchannel.SendMessageAsync($"This story originally voted at {pair.Value.Item2}! Submitted by: {guild.GetUser(pair.Key).Mention}", false, (Discord.Embed)message.First());
                await sent.AddReactionAsync(up);
                await sent.AddReactionAsync(down);
                
				ReaperDataStorage.AddBalance(pair.Key, 1000);
            }

            var sendmessage = await sendtochannel.SendMessageAsync($"All stories at or above {requiredvotes} for the past week have been moved to {finalchannel.Mention} to determine the Master Duck. Each submitter has earned $1000. Type `d.help story` for more info.");
            await Global.ChangeUtil("lastrecipeid", sendmessage.Id.ToString());
			await Global.ChangeUtil("norecipes", "false");
        }
		internal static async Task GetWinner(SocketGuild guild)
		{
			var finalchannel = guild.TextChannels.FirstOrDefault(x => x.Name == "story-contest");
			var Winner = new Dictionary<ulong, ulong>(); //Winners in a dictionary of authorid, messageid.
			if (Global.norecipes)
			{
				await finalchannel.SendMessageAsync("No stories so no winners. Nobody gets Master Duck.");
				var role1 = guild.Roles.FirstOrDefault(x => x.Name == "Master Duck");
				var oldmaster1 = guild.GetUser((ulong)Global.masterduck);
                if (oldmaster1 != null)
                {
                    await oldmaster1.RemoveRoleAsync(role1);
                }
				await Global.ChangeUtil("masterduck", "0");
				return;
			}
			if(!Global.norecipes)
			{
				var msgs = await finalchannel.GetMessagesAsync(100).FlattenAsync();
				int max = 0;
				for (int i = 0; i < msgs.Count(); i++)
                {
					IMessage imessage = msgs.ElementAt(i);
                    IUserMessage message = imessage as IUserMessage;
                    ulong authorid = message.MentionedUserIds.FirstOrDefault();
					var up = new Emoji("⬆");
                    int count = message.Reactions[up].ReactionCount;
                    Console.WriteLine(count);
                    if(count == max)
					{
						Winner.Add(authorid, message.Id);
						continue;
					}
					if (count > max)
                    {
						Winner.Clear();
						Winner.Add(authorid, message.Id);
						max = count;
                    }
                }
				var role = guild.Roles.FirstOrDefault(x => x.Name == "Master Duck");
				if(Winner.Count == 1)
				{
					await finalchannel.SendMessageAsync($"Congratulations {guild.GetUser(Winner.First().Key).Mention} for winning the Master Duck role for having the best story of the week with {max} votes. They have been awarded the prize money of $3000.");
					ReaperDataStorage.AddBalance(Winner.First().Key, 3000);
					var newmaster = guild.GetUser(Winner.First().Key);
                    if(newmaster != null)
                    {
                        await newmaster.AddRoleAsync(role);
                    }

					var oldmaster = guild.GetUser((ulong)Global.masterduck);
					if(oldmaster != null)
					{
						await oldmaster.RemoveRoleAsync(role);
					}
					await Global.ChangeUtil("masterduck", Winner.FirstOrDefault().Key.ToString());
					return;
				}
				else if(Winner.Count > 1)
				{
					int money = 3000 / Winner.Count;
					string winners = "";
					for (int i = 0; i < Winner.Count; i++)
					{
						winners = winners + $" <@{Winner.ElementAt(i).Key}> ";
						ReaperDataStorage.AddBalance(Winner.ElementAt(i).Key, money);
					}
					Random rnd = new Random();
					int winner = (rnd.Next(0, Winner.Count) % Winner.Count);
					ulong winnerid = Winner.ElementAt(winner).Key;
					await finalchannel.SendMessageAsync($"Congratulations to {winners} for having the best, highest voted stories of the week with a tied score of {max} each. They have split the prize money of $3000 and each recieved ${money}. The Master Duck role was randomly assigned to <@{winnerid}>.");
					var newmaster = guild.GetUser(winnerid);
                    if(newmaster != null)
					{
						await newmaster.AddRoleAsync(role);
					}               
                    var oldmaster = guild.GetUser((ulong)Global.masterduck);
                    if (oldmaster != null)
                    {
                        await oldmaster.RemoveRoleAsync(role);
                    }
					await Global.ChangeUtil("masterduck", winnerid.ToString());
                    return;
				}
			}
		}
    }
}