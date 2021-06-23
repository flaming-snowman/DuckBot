using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        EmbedBuilder builderhelp = new EmbedBuilder();
        private Dictionary<string, string> PlantAliases = new Dictionary<string, string>
        {
            {"c1",  "hedgehog cactus, hedge, hedgehog, cactus1, c1"},
            {"c2",  "prickly pear cactus, pear, prickly pear, cactus2, c2"},
            {"c3",  "golden barrel cactus, barrel, golden barrel, cactus3, c3" },
            {"c4",  "saguaro cactus, saguaro, cactus4, c4"},
            {"p1", "green onion plant, onion, green onion, plant1, p1" },
            {"p2", "bell pepper plant, pepper, bell pepper, plant2, p2" },
            {"p3", "tomato plant, tomato, plant3, p3" },
            {"p4", "rice plant, rice, plant4, p4" }
        };
        [Command("help", RunMode = RunMode.Async)]
        public async Task HelpAsync(string help = "")
        {
            if (Context.Channel.Name != "botspam")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("Help commands may only be used in channel \"botspam\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "botspam").Mention} to run help commands.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            string name = Context.Guild.GetUser(Context.User.Id).Nickname;
            if(name==null)
            {
                name = Context.User.Username;
            }

            if (help == "ping")
            {
                builderhelp.WithTitle($"Info on ping command requested by {name}")
                           .WithDescription("Command will respond with pong and will tell you if the bot is online. No reply means offline or slow.")
                           .AddField("Usage:", "d.ping")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "duck")
            {
                builderhelp.WithTitle($"Info on duck command requested by {name}")
                           .WithDescription("Command will respond with a random duck picture, 5 second cooldown among all users")
                           .AddField("Usage:", "d.duck")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "plane")
            {
                builderhelp.WithTitle($"Info on plane command requested by {name}")
                           .WithDescription("Command will respond with everyone's favorite plane picture")
                           .AddField("Usage:", "d.plane")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if(help == "story")
            {
                builderhelp.WithTitle($"Info on story commands requested by {name}")
                           .AddField("submitstory.", "Type `d.help submitstory` for more info on how to submit a story.")
				           .AddField("preliminary voting", $"A weekly competition resetting every `Friday at 5 P.M. PST` will be held for all submitted stories in {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "storyboard").Mention}, where all stories at or above {Global.votes} upvotes will advance to the next round in {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "story-contest").Mention} and the submitters of those stories get $1000.")
				           .AddField("final voting", $"On `Thursday at 5 P.M. PST`, the votes for the recipes that advanced to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "story-contest").Mention} will be counted up. The winner will get $3000 and be awarded the Master Duck role. In the even of a tie, the tied people split the $3000 and the Master Duck role is randomly assigned to one of them.")
                           .WithColor(Color.Green);
            }
            if (help == "submitstory")
            {
                builderhelp.WithTitle($"Info on submitstory command requested by {name}")
                           .WithDescription($"Command can only be used in {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "submit-stories-here").Mention} and will submit a recipe into {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "storyboard").Mention}.")
                           .AddField("Usage:", "`d.submitstory Your story after it.` Markdown works as well. Use shift+enter to make new lines, don't spam space. If you can't for some reason, type \\n because the bot will render it as a newline. You may only submit a story **Once** per day to prevent spam.")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if(help == "trivia")
            {
                builderhelp.WithTitle($"Info on trivia command requested by {name}")
                           .WithDescription($"Command can only be used in {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "trivia").Mention} and will start a new trivia game with the specified number of questions. Only one game can be started at any time. Default number of questions is 5. Min is 1, max is 10, anything trying to break the system will default to 5.")
                           .AddField("Usage:", "`d.trivia <question amount>`")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "voicechat")
            {
                builderhelp.WithTitle($"Info on voicechat commands requested by {name}")
                           .AddField("How to claim:", "Use `d.claimvc` You will be claim the first avialable voice chat. Warning, if you disconnect from your voice chat, your voice chat can be claimed by another user.")
                           .AddField("How to invite people to your private voice chat?", "Use `d.invitevc` followed by mentioning them. For example, `d.invitevc @Duck Bot`. Note only the channel claimer may invite users.")
                           .AddField("I want to remove someone from my voice channel", "Use `d.removevc` followed by mentioning the user. Can only be used by channel claimer")
                           .AddField("Notes:", "The channel claimer can also mute and deafen other users that have been invited.")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "garden")
            {
                builderhelp.WithTitle($"Info on gardening requested by {name}")
                           .AddField("d.grow AND d.plant", "For more info, type: `d.help grow` or `d.help plant`")
                           .AddField("d.water", "For more info, type: `d.help water`")
                           .AddField("d.mygarden", "For more info, type: `d.help mygarden`")
                           .AddField("d.seeds", "For more info, type `d.help seeds`")
                           .AddField("d.grown", "For more info, type `d.help grown`")
                           .AddField("To see info on all seeds:", "Type `d.help allseeds`")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "simulator")
            {
                builderhelp.WithTitle($"Info on the harvest simulator game requested by {name}")
                           .AddField("d.harvest", "For more info, type: `d.help harvest`")
                           .AddField("d.crops", "For more info, type: `d.help crops`")
                           .AddField("d.mycorn", "For more info, type: `d.help mycorn`")
                           .AddField("d.leaderboard", "For more info, type `d.help leaderboard`")
				           .AddField("d.mystatus", "For more info, type `d.help mystatus`")
                           .AddField("Rules:", $"Try to harvest more corn than others. Type `d.help currentgame` for more info on the current game")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "money")
            {
                builderhelp.WithTitle($"Info on money commands requested by {name}")
                           .AddField("d.bank", "Type `d.bank` to access your bank balance.")
                           .AddField("d.shop", "Type `d.shop` to see the available items for purchase. You can buy colors and roles.")
				           .AddField("Roles", "Buy roles and manage your roles. `d.help roles` for more info.")
                           .AddField("How to earn money?", "Play harvest simulator, `d.help simulator` for more info. Corn to money conversion can be found by typing `d.help current game`.\nSubmit stories. Type `d.help story` for more info\nRun the daily command. `d.help daily` for more info.\nGrow crops in your garden. `d.help garden` for more info.")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
			if (help == "roles")
            {
                builderhelp.WithTitle($"Info on role commands requested by {name}")
                           .AddField("d.roleinv", "Use to check how many roles you own.")
                           .AddField("d.change", "You can change your role. `d.help change` for more info")
                           .AddField("To buy", "Looking to buy some roles? `d.help money` for more info. Type `d.shop` for a list of available items.")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
			if (help == "change")
            {
                builderhelp.WithTitle($"Info on changing roles requested by {name}")
				           .WithDescription("Use to change your role. Limited to twice per day.")
				           .AddField("Usage:", "`d.change color <insert color to change to>`\nFor example, use `d.change color grey` to change your color to grey if you own grey. No color can be accessed by `d.change color default`.")
                           .WithColor(Color.Green);
                
                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "grow" || help == "plant")
            {
                builderhelp.WithTitle($"Info on grow command requested by {name}")
                           .WithDescription("Use to grow crops in your garden. Limited to one crop at a time. Requires seeds to grow. `d.help seeds` for more info.")
                           .AddField("Usage:", "`d.grow <insert crop name/alias>`\nFor example, use `d.grow hedge` to grow a hedgehog cactus.")
				           .AddField("Note:", "d.plant serves the same function as d.grow")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "water")
            {
                builderhelp.WithTitle($"Info on water command requested by {name}")
                           .WithDescription("Use to water your plants in your garden. Different plants require different amounts of water and different time intervals. `d.plant` to see the status of your current plant or `d.help allseeds` for info on all seeds.")
                           .AddField("Usage:", "`d.water`")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "collect")
            {
                builderhelp.WithTitle($"Info on collect command requested by {name}")
                           .WithDescription("Use to collect your plants in your garden when they are ready. Different plants require different amounts of time and aounts of water. `d.plant` to see the status of your current plant or `d.help allseeds` for info on all seeds.")
                           .AddField("Usage:", "`d.collect`")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "mygarden")
            {
                builderhelp.WithTitle($"Info on mygarden command requested by {name}")
                           .WithDescription("Use to get the status of your plants.")
                           .AddField("Usage:", "`d.mygarden`")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "seeds")
            {
                builderhelp.WithTitle($"Info on seeds command requested by {name}")
                           .WithDescription("Use to see your number of seeds. `d.help allseeds` for info on all seeds.")
                           .AddField("To get seeds:", "Harvest in Harvest Simulator. More ways may be added in the future.")
                           .AddField("Usage:", "`d.seeds`")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "grown")
            {
                builderhelp.WithTitle($"Info on grown command requested by {name}")
                           .WithDescription("Use to see the plants you've grown over your lifetime.")
                           .AddField("Usage:", "`d.grown`")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "allseeds")
            {
                builderhelp.WithTitle($"List of all seeds requested by {name}")
                           .AddField("Hedgehog Cactus", "`d.help c1` for more info")
                           .AddField("Prickly Pear", "`d.help c2` for more info")
                           .AddField("Golden Barrel Cactus", "`d.help c3` for more info")
                           .AddField("Saguaro Cactus", "`d.help c4` for more info")
                           .AddField("Green Onion Plant", "`d.help p1` for more info")
                           .AddField("Bell Pepper Plant", "`d.help p2` for more info")
                           .AddField("Tomato Plant", "`d.help p3` for more info")
                           .AddField("Rice Plant", "`d.help p4` for more info")
                           .AddField("Misc", "All cacti require no water but generally award less money. All plants require some amount of water and generally are worth more money.")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "c1" || help == "c2" || help == "c3" || help == "c4" || help == "p1" || help == "p2" || help == "p3" || help == "p4")
            {
                int waterreq = Garden.GetWaterNeeded(help);
                string wateramnt = waterreq.ToString();
                int payout = Garden.GetPayout(help);
                if (waterreq == 0)
                {
                    wateramnt = "None";
                }
                int timebet = Garden.GetTimeBetweenWater(help);
                builderhelp.WithTitle($"Info on {Garden.ShortToLongSeedName(help)} requested by {name}")
                    .AddField("Required amount of water:", wateramnt)
                    .AddField("Time between actions (watering and collecting):", $"{timebet / 60} hours and {timebet%60} minutes")
                    .AddField("Payout upon collecting", payout)
                    .AddField("Names and Aliases (can be used alternatively for growing)", PlantAliases[help])
                    .WithColor(Color.Green);
                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
                if (help == "daily")
            {
                builderhelp.WithTitle($"Info on daily command requested by {name}")
                           .WithDescription("Run the command each day to increase your streak, you get money depending on your streak. A streak  of 1 gives $5 but a streak of 7 or more will award $250. If you forget to run the command on a certain day, you lose your streak.")
                           .AddField("Usage:", "`d.daily` As the command suggests, this can only be used once a day.")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "currentgame")
            {
                if (Global.gamestart)
                {
                    builderhelp.WithTitle($"Info on the harvest simulator game requested by {name}")
                               .AddField("Time between harvesting", $"{Global.reapwaittime / 3600000} hours and {(Global.reapwaittime % 3600000)/60000} minutes")
                               .AddField("Goal", $"Game will end when somone reaches {Global.winscore / 3600000} sheaves of corn.")
                               .AddField("How fast does corn grow?", "A sheaf takes 1 hour to grow, an ear in 1 minute, and a kernel in 1 second. Anyone can harvest and then the corn has to regrow.")
                               .AddField("Conversions:", "A sheaf equals 60 ears, an ear equals 60 kernels, kind of like hours:minutes:seconds. Corn automaticaly bundles up")
                               .AddField("What happens when the game ends?", "For every person that has more than a quarter of the goal sheaves, each sheaf is worth $40 more. For instance, 10 people have a quarter of the goal sheaves. So, each sheaf will be worth 40*10 = $400. Ears and kernels will be converted as fractional sheaves. This money will be added to your balance.")
                               .AddField("What's money for?", "Buy colors and roles. Type `d.help money` for more info")
                               .WithColor(Color.Green);

                    await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                    return;
                }
                if(!Global.gamestart)
                {
                    builderhelp.WithTitle($"Info on the harvest simulator game requested by {name}")
                           .AddField("No current game", $"Ask the owner to start a new one")
                           .AddField("What happens when games end?", "Each sheave you harvested will be sold for $360, each ear for $6, and every 10 kernels for $1. This money will be added to your balance. Your balance can be viewed at `d.bank`")
                           .AddField("What's money for?", "Buy colors and roles. `d.shop` for a list of available items.")
                           .WithColor(Color.Green);

                    await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                    return;
                }
            }

            if (help == "harvest")
            {
                builderhelp.WithTitle($"Info on harvest command requested by {name}")
                           .WithDescription($"Can be used every {Global.reapwaittime/3600000} hours to harvest the total corn available for harvest")
                           .AddField("Usage:", "d.harvest")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "crops")
            {
                builderhelp.WithTitle($"Info on crops command requested by {name}")
                           .WithDescription("Command will tell you the available corn for harvest")
                           .AddField("Usage:", "d.crops")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "mycorn")
            {
                builderhelp.WithTitle($"Info on mycrops command requested by {name}")
                           .WithDescription("Command will display how much corn you have harvested")
                           .AddField("Usage:", "d.mycorn")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            if (help == "leaderboard")
            {
                builderhelp.WithTitle($"Info on leaderboard command requested by {name}")
                           .WithDescription("Command will display the leaderboard for the current game of Harvest Simulator")
                           .AddField("Sheaves to win:", $"The current game is set to end when somebody harvests more than {Global.winscore/3600000} sheaves of corn")
                           .AddField("Usage:", "d.leaderboard")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
			if (help == "mystatus")
            {
                builderhelp.WithTitle($"Info on the mystatus command requested by {name}")
                           .WithDescription("Command will display the availibility of your harvest and your last harvest amount. Useful for when you don't know if you can harvest or if the bot didn't respond and you want to check if it recorded it.")
                           .AddField("Usage:", "d.mystatus")
                           .WithColor(Color.Green);

                await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

                return;
            }
            builderhelp.WithTitle("All commands (minus a few secret ones) for Duck Bot")
                       .AddField("Bot status", "For more info, type: `d.help ping`", true)
                       .AddField("Duck Pictures", "For more info, type: `d.help duck`")
                       .AddField("Stories", "For more info, type: `d.help story`")
                       .AddField("Trivia", "For more info, type: `d.help trivia`")
                       .AddField("Money", "There is a currency in this server. `d.help money` for more info")
			           .AddField("Roles", "Looking for colors? `d.help roles`")
                       .AddField("Voice chats", "You can claim voice chats in this server. `d.help voicechat` for more info")
                       .AddField("Garden", "Plant seeds and grow crops. Type `d.help garden` for more info")
                       .AddField("Harvest Simulator game", "For more info on how to play, type: `d.help simulator`")
                       .WithColor(Color.Green);

            await ReplyAsync($"{Context.User.Mention}, duck bot has responded to your command!", false, builderhelp.Build());

            return;
        }
    }
}
