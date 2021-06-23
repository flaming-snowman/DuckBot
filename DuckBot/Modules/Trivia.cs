using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Newtonsoft.Json;

namespace DuckBot.Modules
{
    public class Trivia : InteractiveBase
    {
        private RootObject questionsRootObject;

        [Command("trivia", RunMode = RunMode.Async)]
        public async Task TriviaAsync([Remainder] int amount = 5)
        {
            if (Context.Channel.Name != "trivia")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in the designated channel")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "trivia").Mention} to play trivia.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            if(CurrentTrivia.Current())
            {
                await ReplyAsync($"{Context.User.Mention}, there is an ongoing trivia game, wait until it finishes to start a new one. You may spectate the ongoing one. Note that only the person who created that game can answer the questions. You may comment but your messages will not affect anything.");

                return;
            }
            CurrentTrivia.Changecurrent(true);
            if (amount < 1 || amount > 10)
                amount = 5;
            
            var json = new WebClient().DownloadString($"https://opentdb.com/api.php?amount={amount}&type=multiple&encode=base64");

            questionsRootObject = JsonConvert.DeserializeObject<RootObject>(json);
            int correct = 0;
            await ReplyAsync($"{Context.User.Mention}, a new trivia game with {amount} questions has been started for you!");

            for (int i = 0; i < amount; i++)
            {
                StaticVariables.Question = Base64Decoder(questionsRootObject.results[i].question); //save question as static string
                StaticVariables.Answers = new List<string>();
                for (int j = 0; j < 3; j++)
                {
                    StaticVariables.Answers.Add(Base64Decoder(questionsRootObject.results[i].incorrect_answers[j]));
                }
                StaticVariables.Answers.Add(Base64Decoder(questionsRootObject.results[i].correct_answer));
                StaticVariables.CorrectAnswer = Base64Decoder(questionsRootObject.results[i].correct_answer);
                StaticVariables.Category = Base64Decoder(questionsRootObject.results[i].category);
                StaticVariables.Difficulty = Base64Decoder(questionsRootObject.results[i].difficulty);
                int timeallowed = 30;
                string answer = "";
                if (StaticVariables.Difficulty == "hard")
                {
                    timeallowed = 60;
                }
                else if (StaticVariables.Difficulty == "medium")
                {
                    timeallowed = 45;
                }
                else if (StaticVariables.Difficulty == "easy")
                {
                    timeallowed = 30;
                }
                StaticVariables.Answers.Shuffle();
                for (int k = 0; k < 4; k++)
                {
                    if (StaticVariables.Answers[k] == StaticVariables.CorrectAnswer)
                    {
                        if (k == 0)
                            answer = "a";
                        if (k == 1)
                            answer = "b";
                        if (k == 2)
                            answer = "c";
                        if (k == 3)
                            answer = "d";
                    }
                }
                EmbedBuilder DisplayBuilder = new EmbedBuilder();
                DisplayBuilder.WithTitle($"Trivia Question {i + 1} of {amount}")
                              .WithDescription($"**Question**: __{StaticVariables.Question}__\n\n**Category: {StaticVariables.Category}**    **Difficulty**: {StaticVariables.Difficulty.ToUpper()}. Time alloted: **{timeallowed} seconds**." )
                              .AddField("Choice A: ", StaticVariables.Answers[0], true)
                              .AddField("Choice B: ", StaticVariables.Answers[1], true)
                              .AddField("Choice C: ", StaticVariables.Answers[2], true)
                              .AddField("Choice D: ", StaticVariables.Answers[3], true)
                              .AddField("Oops, I didn't mean to start a new game.", "Type `cancel` to immdiately cancel the command.")
                              .WithThumbnailUrl("https://www.google.com/url?sa=i&rct=j&q=&esrc=s&source=images&cd=&cad=rja&uact=8&ved=2ahUKEwj34Omrj83aAhVMzFQKHU-7B0MQjRx6BAgAEAU&url=http%3A%2F%2Fwww.1380kcim.com%2Fnews%2F2016%2Fshare-your-knowledge-at-smch-trivia-night%2F&psig=AOvVaw332rLQ_PlPp7DCdy0GgcCm&ust=1524460267958721")
                              .WithColor(Color.Teal);
                await ReplyAsync("", false, DisplayBuilder.Build());
                var response = await NextMessageAsync(true, true, TimeSpan.FromSeconds(timeallowed));
                if (response == null)
                {
                    await ReplyAsync("You timed out. Next question.");
                }
                else if (response.Content.ToLower() == "cancel")
                {
                    await ReplyAsync("Game canceled");
                    break;
                }
                else if (response.Content.ToLower() != "a" && response.Content.ToLower() != "b" && response.Content.ToLower() != "c" && response.Content.ToLower() != "d")
                {
                    await ReplyAsync("Invalid response. Next question.");
                }
                else if (response.Content.ToLower() == answer)
                {
                    correct++;
                    await ReplyAsync($"Your answer was correct! You have correctly answered {correct} of the {i + 1} questions given so far.");
                }
                else if (response.Content.ToLower() != answer)
                {
                    await ReplyAsync($"Your answer was incorrect. The correct answer was {answer.ToUpper()}. You have correctly answered {correct} of the {i+1} questions given so far.");
                }
            }
            await ReplyAsync($"{Context.User.Mention}, you scored {correct} out of {amount} on this trivia quiz.");
            CurrentTrivia.Changecurrent(false);

            return;
        }
        private string Base64Decoder (string encodedtext)
        {
            byte[] data = Convert.FromBase64String(encodedtext);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
    }
    public class Result
    {
        public string category { get; set; }
        public string type { get; set; }
        public string difficulty { get; set; }
        public string question { get; set; }
        public string correct_answer { get; set; }
        public List<string> incorrect_answers { get; set; }
    }
    public class RootObject
    {
        public int response_code { get; set; }
        public List<Result> results { get; set; }
    }
    public static class StaticVariables
    {
        public static string Question { get; set; }
        public static List<string> Answers { get; set; }
        public static string CorrectAnswer { get; set; }
        public static string Category { get; set; }
        public static string Difficulty { get; set; }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    internal static class CurrentTrivia
    {
        private static bool currenttrivia = false;
        internal static bool Current()
        {
            if (currenttrivia)
                return true;
            else return false;
        }
        internal static void Changecurrent(bool status)
        {
            currenttrivia = status;
        }
    }
}
