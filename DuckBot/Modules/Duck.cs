using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DuckBot.Modules
{
    public class Duck : ModuleBase<SocketCommandContext>
    {
        int n = 84;
        string[] urls = new string[] { "https://cdn.discordapp.com/attachments/427543238724026379/427546244517199882/image.jpg", "https://cdn.discordapp.com/attachments/427543238724026379/427862905762086922/animal-bird-duck-poultry-64719.jpeg", "https://cdn.discordapp.com/attachments/427543238724026379/427862935604559873/duckling-birds-yellow-fluffy-162140.jpeg", "https://cdn.discordapp.com/attachments/427543238724026379/427862998758195200/pexels-photo-209035.jpeg", "https://cdn.discordapp.com/attachments/427543238724026379/427863027300171787/pexels-photo-226597.jpeg", "https://cdn.discordapp.com/attachments/427964680170897408/427964957251076108/41sfz8dKX1L._SL500_AC_SS350_.jpg", "https://cdn.discordapp.com/attachments/427964680170897408/427971218755551274/36723371-duck.jpg", "https://cdn.discordapp.com/attachments/427543238724026379/427977038406615041/00f321e97de485b38fcc1d33a85edfce.jpg", "https://cdn.discordapp.com/attachments/427543238724026379/427977229557825539/1_copy_2.jpg", "https://cdn.discordapp.com/attachments/427543238724026379/427977326022754325/1_copy.jpg", "https://cdn.discordapp.com/attachments/427543238724026379/427977610262347786/1-1_copy_2.jpg", "https://cdn.discordapp.com/attachments/427543238724026379/427977777782849538/1-1_copy.jpg", "https://cdn.discordapp.com/attachments/427543238724026379/427977915011956737/1-1.jpg", "https://cdn.discordapp.com/attachments/427543238724026379/427978271205097484/1-2.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/427978476289523734/1-3.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/427979432871985164/1-5.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/427979516049096704/1-6.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/427979765677424652/1-7.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/427979855297118216/1-8.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/427980042065412096/1-9.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428034354409897986/1-10.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428034508974194692/1-11.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428034587260616705/1-12.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428034654323474452/1-13.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428034750700191745/1-14.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428034811857338379/1-15.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428034893298008065/1-16.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428034971228307460/1-17.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035027767656460/1-18.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035111175454722/1-19.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035225717702656/1-20.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035293560700928/1-21.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035354445217794/1-22.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035411336495104/1-23.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035463358709760/1-24.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035647144460291/1-25.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035705411862528/1-26.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035761191911434/1-27.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035813067063326/1-28.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035869782573057/1-29.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428035932730556419/1-30.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428036816004710405/1.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428036889925124097/d.10.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428036971697274880/d.11.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428037027590701056/d.12.JPG", "https://cdn.discordapp.com/attachments/427572233100328962/428041779753910321/d.13.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428041874436128768/d.14.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428041943403069440/d.15.jpeg", "https://cdn.discordapp.com/attachments/427572233100328962/428042012613279765/d1.JPG", "https://cdn.discordapp.com/attachments/427572233100328962/428042084457644054/d2.jpeg", "https://cdn.discordapp.com/attachments/427572233100328962/428042139042316289/d3.jpeg", "https://cdn.discordapp.com/attachments/427572233100328962/428042212308287498/d4.jpeg", "https://cdn.discordapp.com/attachments/427572233100328962/428042289252925470/d5.jpeg", "https://cdn.discordapp.com/attachments/427572233100328962/428042384077488128/d6.jpeg", "https://cdn.discordapp.com/attachments/427572233100328962/428042437399937034/d7.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/428042496015204374/d8.jpeg", "https://cdn.discordapp.com/attachments/427572233100328962/428042553674301440/d9.jpeg", "https://cdn.discordapp.com/attachments/427572233100328962/428042612016939008/d16.JPG", "https://cdn.discordapp.com/attachments/427572233100328962/431238411534467113/duck59.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238474000105474/duck60.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238525795434499/duck61.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238630762217474/duck62.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238774253551616/duck63.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238775314841600/duck64.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238804410597388/duck65.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238825445163019/duck66.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238843128086529/duck67.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238892864274444/duck68.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238898228658177/duck69.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431238917136842772/duck70.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239597750747146/duck71.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239620361977856/duck72.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239640708546561/duck73.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239657976496139/duck74.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239683125673994/duck75.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239707385659423/duck76.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239725509246977/duck77.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239746564653077/duck78.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239767762403338/duck79.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239785881796608/duck80.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239801384206346/duck81.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239834976256000/duck82.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239865146015744/duck83.jpg", "https://cdn.discordapp.com/attachments/427572233100328962/431239878764658698/duck84.png"};

        int num = new int();
        EmbedBuilder builderduck = new EmbedBuilder();
        EmbedBuilder builderspam = new EmbedBuilder();

        Random rand = new Random();
        [Command("duck", RunMode = RunMode.Async)]
        public async Task PingAsync()
        {
            if (Context.Channel.Name != "botspam")
            {
                EmbedBuilder builderwrongchannel = new EmbedBuilder();
                builderwrongchannel.WithTitle("This command may only be used in channel \"botspam\".")
                                   .WithDescription($"Please go to {Context.Guild.TextChannels.FirstOrDefault(x => x.Name == "botspam").Mention} to run this command.")
                                   .WithColor(Color.Red);
                await ReplyAsync($"{ Context.User.Mention}, duck bot has responded to your command!", false, builderwrongchannel.Build());

                return;
            }
            string name = Context.Guild.GetUser(Context.User.Id).Nickname;
            if(name == null)
            {
                name = Context.User.Username;
            }
                
            num = rand.Next(0, n - 1);

            builderduck.WithTitle("Here's a duck picture!")
                        .WithImageUrl(urls[num])
                        .WithColor(Color.DarkGreen);
            
            var duckseconds =  Clock.CheckDuckTime();

            if (duckseconds.Equals(0))
            {
                await ReplyAsync("", false, builderduck.Build());
                await Clock.BeginDuckTimer();

                return;
            }
            else if (!duckseconds.Equals(0))
            {
                await ReplyAsync($":no_entry_sign: **{name}**, please wait **5** seconds between requesting duck pictures. Wait **{duckseconds}** more seconds *Note that all users share this timer*");

                return;
            }
        }
    }
}
