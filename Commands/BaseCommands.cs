using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using DiscordBot.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace DiscordBot.Commands
{
    public class BaseCommands : ModuleBase<SocketCommandContext>
    {
        [NormalCommands("!say","Copys what you post, example:\n !say Cheese")]
        [Command("say")]
        public async Task SayAsync([Remainder]string echo)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(66, 244, 155);
            eb.WithAuthor(Context.Message.Author.Username, Context.Message.Author.GetAvatarUrl());
            eb.WithDescription(echo);
            await ReplyAsync("", false, eb.Build());
        }
        
        
        
        [NormalCommands("!help", "View all the commands")]
        [Command("help")]
        public async Task HelpMe()
        {
            string format = "";
            MethodInfo[] methordsInfo = typeof(BaseCommands).GetMethods();
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(12, 245, 249);
            eb.WithTitle("Help");
            for (int a = 0; a < methordsInfo.Length; a++)
            {
                NormalCommands data = methordsInfo[a].GetCustomAttribute<NormalCommands>(false);
                
                if (data != null)
                {
                    format += data.Name + ", ";
                    //eb.AddField(data.Name, data.Desc);
                }
            }

            eb.AddField("Use ``CHelp commandName`` to get more info on an existing command", format);
            await ReplyAsync("", false, eb.Build());
        }
        
        
        
        
        [Command("chelp")]
        public async Task CommandHelp(string command)
        {
            string temp = command;
            bool found = false;
            MethodInfo[] methordsInfo = typeof(BaseCommands).GetMethods();
            if (command.Contains('!'))
            {
                temp = "";
                foreach (char letter in command)
                {
                    if (letter != '!')
                    {
                        temp += letter;
                    }
                            
                }
            }
            
                        
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(12, 245, 249);
            //eb.WithTitle(command + " extra help");
            for (int a = 0; a < methordsInfo.Length; a++)
            {
                NormalCommands data = methordsInfo[a].GetCustomAttribute<NormalCommands>(false);
                
                if (data != null)
                {
                    string name = "";
                    foreach (char letter in data.Name)
                    {
                        if (letter == ' ')
                            break;

                        if (letter != '!')
                        {
                            name += letter;
                        }
                            
                    }
                    if (name.ToLower() == temp.ToLower())
                    {
                        eb.AddField(temp + " with more info", data.Desc);
                        found = true;
                    }
                    //eb.AddField(data.Name, data.Desc);
                }
            }

            if (!found)
                eb.AddField("No command found", "Please check the data entered");

            
            
            await ReplyAsync("", false, eb.Build());

          
        }

        [NormalCommands("!changelog", "View the latest changes to the bot")]
        [Command("changelog")]
        [Alias("cl")]
        public async Task ChangeLog()
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(12, 245, 249);
            eb.WithTitle("Latest change log (v 1.5556)");
            eb.AddField("!info", "View information about the bot");
            eb.AddField("Chat reponse", "bot will now pm you if you do boop or what not in the wrong channel + \n" +
                                        "(I'm working on this for all commands)");
            eb.AddField("!startTc", "Admins can now start TC with a new command");
            eb.AddField("!mc", "This command has been removed (sicne minecraft is over)");
            await ReplyAsync("", false, eb.Build());
        }
        
        
        
        [Command("soon")]
        public async Task FutureCommands()
        {
        
            var eb = new EmbedBuilder();
            eb.WithColor(176, 232, 229);
            eb.WithTitle("Commands coming in the future!");
            eb.WithDescription("Holding off base update, may never come!");
            eb.AddField("Send ideas", "Always happy to add more!");

            await ReplyAsync("", false, eb.Build());
        }
        
        
        [Command("price")]
        public async Task Price()
        {
            List<string> strings = new List<string>
            {
                "https://i.imgur.com/ylVrFTv.png",
                "https://i.imgur.com/HFgD8ng.png",
                "https://i.imgur.com/6OK61fZ.png",
                "https://i.imgur.com/Yjdl3Gt.png",
                "https://i.imgur.com/ts8XX48.png"
            };
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(234, 234, 9);
            eb.WithTitle("Current price that objects sell/Buy for!");
            eb.WithDescription("Prices will change randomly in a future update!!");
            eb.AddField("Bombs", "10 gold coins each. Will cost 20 gold to buy", true);
            eb.AddField("Fire arrows", "15 gold coins each. Will cost 30 gold to buy", true);
            eb.AddField("Saws", "50 gold coins each. Will cost 100 gold to buy", true);
            eb.WithThumbnailUrl(strings[GlobalVars.Rnum.Next(strings.Count)]);

            await ReplyAsync("", false, eb.Build());
        }

        
            
        //
        //
        //
        //
        //buy stuff
        //
        //
        //
        //

        
        [Command("buylist")]
        public async Task Buying()
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(234, 234, 9);
            eb.WithTitle("Items for airdrops");
            eb.AddField("Pig", "200 coins", true);
            eb.AddField("Assult rifle", "400 coins", true);
            eb.AddField("Blaster", "1000 coins", true);
            eb.AddField("Jug Hammer", "10000 coins", true);
            eb.AddField("Hover Bike", "10000 coins", true);
            eb.AddField("Slave Kit", "500 coins", true);
            await ReplyAsync("",false,eb.Build());

        }
        
        [NormalCommands("!take","Steal an item off the floor, example:\n!take bombs")]
        [Command("Take")]
        public async Task Pickup()
        {
            await ReplyAsync("Make sure you input some text");
        }

        
       /* [Command("Takeall")]
        public async Task PickupAll()
        {
            if (GlobalVars.Caltrops > 0)
            {
                
                Sql sql = new Sql();
                var items = sql.SqlReader(Context.Message.Author.Id);
                //int divTotal = (items.goldCoins % GlobalVars.Caltrops);
                int temp = items.goldCoins;
                if (items.bombs != 0)
                    temp += (items.bombs * 10);
                if (items.fire_arrows != 0)
                    temp += (items.fire_arrows * 15);
                if (items.saws != 0)
                    temp += (items.saws * 50);
                int divTotal = (temp / 100) * ((GlobalVars.Caltrops + 1) / 2 ); 
                sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,0,0,0,-divTotal);
                await ReplyAsync("There are " + GlobalVars.Caltrops + " on the floor, you lost " + divTotal + " coins");
                GlobalVars.Caltrops = 0;
            }
            else
            {
                Sql sql = new Sql();
                int temp = GlobalVars.Items.Coins;
                GlobalVars.Items.Coins = 0;
                temp += (GlobalVars.Items.FArrows * 15);
                GlobalVars.Items.FArrows = 0;
                temp += (GlobalVars.Items.Bombs * 10);
                GlobalVars.Items.Bombs= 0;
                temp += (GlobalVars.Items.Saws * 50);
                GlobalVars.Items.Saws = 0;

                if (temp == 0)
                    await ReplyAsync("Nothing to take");
                else
                {
                    sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,0,0,0,temp);
                    await ReplyAsync("You have stolen " + temp + " coins");   
                }
            }
        }*/

        //pick up
        [Command("take")]
        [Alias("steal")]
        public async Task Pickup([Remainder]string text)
        {
            Sql sql = new Sql();
            switch(text.ToLower())
            {
                case "bombs":
                case "bomb":
                    if(GlobalVars.Items.Bombs > 0)
                    {
                        try
                        {
                            int quicktake = GlobalVars.Items.Bombs;
                            GlobalVars.Items.Bombs = 0;
                            GlobalVars.LastBombUser = Context.Message.Author.Username;
                            sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,quicktake);
                            await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                            GlobalVars.Items.SaveAll();
                        }
                        catch
                        {
                            await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                            throw;
                        }

                    }
                    else
                    {
                        await Context.Message.AddReactionAsync(Emote.Parse("<:kag_disagree:399994688633045004>"));
                        await ReplyAsync("No bombs here, sorry. Last user to take the bombs was "+ ((string.IsNullOrEmpty(GlobalVars.LastBombUser)) ?  "Nobody": GlobalVars.LastBombUser));//TODO clean
                    }

                    break;
                
                case "fire_arrow":
                case "fire":
                case "firearrows":
                case "firearrow":
                case "fire arrows":
                case "fire arrow":
                case "fire_arrows":
                    if (GlobalVars.Items.FArrows > 0)
                    {
                        try
                        {
                            int quicktake = GlobalVars.Items.FArrows;
                            GlobalVars.Items.FArrows = 0;
                            sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,0,quicktake);
                            GlobalVars.LastFireArrowUser = Context.Message.Author.Username;
                            await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                            GlobalVars.Items.SaveAll();
                        }
                        catch
                        {
                            await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                            throw;
                        }
                    }
                    else
                    {
                        await Context.Message.AddReactionAsync(Emote.Parse("<:kag_disagree:399994688633045004>"));
                        await ReplyAsync("No fire arrows here, sorry. Last user to take Fire Arrow was "+ (
                                             (string.IsNullOrEmpty(GlobalVars.LastFireArrowUser)) ?  "Nobody": GlobalVars.LastFireArrowUser));
                    }

                    break;

                case "saws":
                case "saw":
                    if (GlobalVars.Items.Saws > 0)
                    {
                        try
                        {
                            int quicktake = GlobalVars.Items.Saws;
                            GlobalVars.Items.Saws = 0;
                            sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,0,0,quicktake);
                            GlobalVars.LastSawUser = Context.Message.Author.Username;
                            await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                            GlobalVars.Items.SaveAll();
                        }
                        catch
                        {
                            await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                            throw;
                        }
                    }
                    else
                    {
                        await Context.Message.AddReactionAsync(Emote.Parse("<:kag_disagree:399994688633045004>"));
                        await ReplyAsync("No saw here, sorry! Last user to take the saws was "+ (
                                             (string.IsNullOrEmpty(GlobalVars.LastSawUser)) ?  "Nobody": GlobalVars.LastSawUser));
                    }

                    break;

                case "goldcoins":
                case "goldcoin":
                case "gold coins":
                case "gold coin":
                case "coin":
                case "coins":
                case "gold_coins":
                case "gold":
                    if (GlobalVars.Items.Coins > 0)
                    {
                        try
                        {
                            int quicktake = GlobalVars.Items.Coins;
                            GlobalVars.Items.Coins = 0;
                            sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,0,0,0,quicktake);
                            GlobalVars.LastGoldUser = Context.Message.Author.Username;
                            await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                            GlobalVars.Items.SaveAll();
                        }
                        catch
                        {
                            await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                            throw;
                        }
                    }
                    else
                    {
                        await Context.Message.AddReactionAsync(Emote.Parse("<:kag_disagree:399994688633045004>"));
                        await ReplyAsync("No coins here, sorry! Last user to take the gold coins was " + (
                                             (string.IsNullOrEmpty(GlobalVars.LastGoldUser)) ?  "Nobody": GlobalVars.LastGoldUser));
                    }

                    break;

                default:
                    await Context.Message.AddReactionAsync(Emote.Parse("<:kag_disagree:399994688633045004>"));
                    await ReplyAsync("Make sure you typed the correct object!");
                    break;
            }
            //await ReplyAsync("Fix is coming soon, days attempting to fix it : 2");
        }

        [NormalCommands("!invo","View what items you have collected")]
        [Command("inventory")]
        [Alias("invo","inven","invontory")]
        public async Task Inv(IUser user = null)
        {
            
            try
            {
                Sql sql = new Sql();
                var eb = new EmbedBuilder();
                double value = 0;
                if (user == null)
                {
                    var items = sql.SqlReader(Context.Message.Author.Id);
                    eb.WithTitle(Context.Message.Author.Username + " Inventory!");
                    eb.AddField("Bombs", items.bombs, true);
                    eb.AddField("Fire arrows", items.fire_arrows, true);
                    eb.AddField("Saws", items.saws, true);
                    eb.AddField("Gold coins", items.goldCoins, true);
                    if (items.bombs != 0)
                        value += items.bombs * 10;
                    if (items.fire_arrows != 0)
                        value += items.fire_arrows * 15;
                    if (items.saws != 0)
                        value += items.saws * 50;
                    value += items.goldCoins;
                }
                else
                {
                    var items = sql.SqlReader(user.Id);
                    eb.WithTitle(user.Username + " Inventory!");
                    eb.AddField("Bombs", items.bombs, true);
                    eb.AddField("Fire arrows", items.fire_arrows, true);
                    eb.AddField("Saws", items.saws, true);
                    eb.AddField("Gold coins", items.goldCoins, true);
                    if (items.bombs != 0)
                        value += items.bombs * 10;
                    if (items.fire_arrows != 0)
                        value += items.fire_arrows * 15;
                    if (items.saws != 0)
                        value += items.saws * 50;
                    value += items.goldCoins;
                }
                
                eb.WithColor(206, 24, 136);

                eb.WithThumbnailUrl("https://i.imgur.com/7IJVRoh.png");
                

                eb.WithFooter("Total invo value : " + value);
                await ReplyAsync("", false, eb.Build());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("ERROR : dead");
                Console.ReadLine();
                throw;
            }
            
            //await ReplyAsync("Fix is coming soon, days attempting to fix it : 2");
        }
        
        [NormalCommands("!mystery","Its a mystery")]
        [Command("mystery")]
        public async Task WasteCommand2()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=YQpLNCRIxWA "+Emote.Parse("<:mystery:382795988303216650>"));
        }
        
        [NormalCommands("!sell","Sell items you have collected for gold, example:\n!sell 40 bombs")]
        [Command("sell")]
        public async Task Sell(string text)//text does not need to be used, just incase the user types !sell bombs it will have a reponse
        {
            await ReplyAsync("Make sure you place a number before the text.");
        }

        [Command("sell")]
        public async Task Sell(int amount)//same as above
        {
            await ReplyAsync("Make sure you do !sell number object");
        }

        [Command("sell")]
        public async Task Sell(string text, int amount)
        {
            await Sell(amount, text);
        }

        [Command("sell")]
        public async Task Sell(int amount, string text)
        {
            Sql sql = new Sql();
            if (amount <= 0)
                await ReplyAsync($"Nice try {Context.Message.Author.Mention}");
            int sellprice;
            var items = sql.SqlReader(Context.Message.Author.Id);
            switch (text.ToLower())
            {
                case "bombs":
                case "bomb":
                    if (amount <= items.bombs)
                    {
                        sellprice = amount * 10;
                        sql.SqlExpriance(Context.Message.Author.Id, Context.Message.Author.Username, 0, 0, 0, -amount,
                            0, 0, sellprice);
                        await ReplyAsync("Sold " + amount + " bombs for " + sellprice);
                    }
                    else
                    {
                        await ReplyAsync("No can do");
                    }
                break;

                case "saws":
                case "saw":
                    if (amount <= items.saws)
                    {
                        sellprice = amount * 50;
                        sql.SqlExpriance(Context.Message.Author.Id, Context.Message.Author.Username, 0, 0, 0, 0, 0,
                            -amount, sellprice);
                        await ReplyAsync("Sold " + amount + " saws for " + sellprice);
                    }
                    else
                    {
                        await ReplyAsync("No can do");
                    }
                break;

                case "fire":
                case "firearrows":
                case "fire_arrows":
                case "fire_arrow":
                    if (amount <= items.fire_arrows)
                    {
                        sellprice = amount * 15;
                        sql.SqlExpriance(Context.Message.Author.Id, Context.Message.Author.Username, 0, 0, 0, 0,
                            -amount, 0, sellprice);
                        await ReplyAsync("Sold " + amount + " fire arrows for " + sellprice);
                    }
                    else
                    {
                        await ReplyAsync("No can do");
                    }
                break;

                default:
                    if (text == "gold" || text == "gold_coins" || text == "coins" || text == "gold_coins")
                    {
                        await ReplyAsync("Can't sell gold coins.");
                    }
                break;
            }
        }

        /*[Command("myrank")]
        public async Task MyRank()
        {
            JsonSerializer serializer = new JsonSerializer();
            Int32 Messages;
            //Int32 Level;
            //Int32 Exp;
            using (StreamReader sr = new StreamReader(GlobalVars.JsonCurrentPath + @"\Exp\" + Context.Message.Author.Id.ToString() + ".json"))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                dynamic results = serializer.Deserialize(reader);
                Messages = results.MessageSentTotal;
                //Exp = results.Xp;
                //Level = results.Level;
            }
            await ReplyAsync("Messages sent : " + Messages);
        }*/
        /*[NormalCommands("!rank","Data collected about you")]
        [Command("myrank")]
        [Alias("rank")]
        public async Task SqlTesting()
        {
            Sql sql = new Sql();
            var exp = sql.SqlReader(Context.Message.Author.Id);
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(66, 182, 244);
            eb.AddField("Messages sent", exp.message,true);
            eb.AddField("Experience", exp.exp,true);
            eb.AddField("Level", exp.level,true);
            eb.AddField("WIP", "EXP + Level will be worked on soon",true);
            await ReplyAsync("",false,eb.Build());
        }*/
       
        
        
        [NormalCommands("!bet", "Bet your gold coins! Example:\n!bet 400")]
        [Command("bet")]
        public async Task BetTask(int amount)
        {
            //todo fix crappy code
            Sql sql = new Sql();
            var items = sql.SqlReader(Context.Message.Author.Id);
            if (amount > items.goldCoins)
            {
                await ReplyAsync("Can't bet more then what you own!");
                throw new ArgumentException("Not enough gold");//amazing code 10/10 would do again
            }

            if (amount < 1)
            {
                await ReplyAsync("Can't bet nothing");
                throw new ArgumentException("To low of a number");
            }

            int rNum = GlobalVars.Rnum.Next(0, 101);
            Console.WriteLine(Context.Message.Author + " Has got num " + rNum);
            if (rNum < 68)
            {
                if(amount == 1)
                    await ReplyAsync(Context.User.Mention+" lost " + amount + " Coin... You should bet a bit more next time");
                else
                    await ReplyAsync(Context.User.Mention+" lost " + amount + " Coins... Better luck next time!");
                sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,0,0,0,-amount);
                sql.SqlExpriance(386306265388679168,"Foghorn",0,0,0,0,0,0,+amount);
            }
            else
            {
                int wonAmount = int.Parse(Math.Round(amount * (1.3 + ((float)(rNum - 9) / 100)) , 0, MidpointRounding.AwayFromZero).ToString());
                await ReplyAsync("You won 🎉! "+Context.User.Mention+" have gained " + wonAmount + " coins!");
                sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,0,0,0,wonAmount - amount);
            }
        }


        [Command("gay")]
        [Alias("howgay")]
        public async Task HowGay([Remainder] string text = null)
        {
            int gayAmount = GlobalVars.Rnum.Next(0, 101);
            EmbedBuilder eb = new EmbedBuilder();
            if (text is null || text == " " || text == "")
                eb.AddField("How gay is " + Context.Message.Author.Username, gayAmount + "% 🏳️️‍🌈"); 
            else
                eb.AddField("How gay is " + text, gayAmount + "% 🏳️️‍🌈");
            await ReplyAsync("", false, eb.Build());
        }

        [Command("gay")]
        [Alias("howgay")]
        public async Task HowGay2(IUser user)
        {
            await HowGay(user.Username);
        }
        
        [Command("trap")]
        [Alias("howtrap")]
        public async Task Howtrap([Remainder] string text = null)
        {
            int amount = GlobalVars.Rnum.Next(0, 101);
            EmbedBuilder eb = new EmbedBuilder();
            if (text is null || text == " " || text == "")
                eb.AddField("How much of a trap is " + Context.Message.Author.Username, amount + "%"); 
            else
                eb.AddField("How much of a trap is " + text, amount + "%");
            await ReplyAsync("", false, eb.Build());
        }

        [Command("trap")]
        [Alias("howtrap")]
        public async Task HowTrap2(IUser user)
        {
            Howtrap(user.Username.ToString());
        }


        [Command("loli")]
        [Alias("howloli")]
        public async Task HowLoli([Remainder] string text = null)
        {
            int amount = GlobalVars.Rnum.Next(0, 101);
            EmbedBuilder eb = new EmbedBuilder();
            if (text is null || text == " " || text == "")
                eb.AddField("How much of a loli is " + Context.Message.Author.Username, amount + "%"); 
            else if(text.ToLower() == "rajang" || text.ToLower() == "nice racist british loli gal")
                eb.AddField("How much of a loli is " + text, "100%");
            else
                eb.AddField("How much of a loli is " + text, amount + "%");
            await ReplyAsync("", false, eb.Build());
        }

        [Command("loli")]
        [Alias("howloli")]
        public async Task HowLoli2(IUser user)
        {
            await HowLoli(user.Username);
        }
        
        [Command("furry")]
        [Alias("howfurry")]
        public async Task HowFurry([Remainder] string text = null)
        {
            int amount = GlobalVars.Rnum.Next(0, 101);
            EmbedBuilder eb = new EmbedBuilder();
            if (text is null || text == " " || text == "")
                eb.AddField("How much of a furry is " + Context.Message.Author.Username, amount+ "% 🐱" ); 
            else
                eb.AddField("How much of a furry is " + text, amount + "% 🐱");
            await ReplyAsync("", false, eb.Build());
        }
        
        [Command("furry")]
        [Alias("howfurry")]
        public async Task HowFurry2(IUser user)
        {
            await HowFurry(user.Username);
        }
        
        
        

        [NormalCommands("!top","View the richest people")]
        [Command("top")]
        public async Task Top10(int yes = 10)
        {
            Sql sql = new Sql();
            var items = sql.SqlRankTuple(yes);
            try
            {
                var eb = new EmbedBuilder();
                Console.WriteLine("Length : " + items.Length);
                Console.WriteLine("" + items);
                eb.AddField("Top " + yes + " Richest people", items);
                eb.WithColor(255, 255, 0);
                await ReplyAsync("", false, eb.Build());
            }
            catch
            {
                await ReplyAsync(
                    "Vamist is fixing this command, fix will be out tomorrow (currently anything higher then 26 will make the message size bigger then what discord allows)");
            }
        }
        
         [NormalCommands("!d6 number","Roll a 6 sided dice.")]
        [Command("d6")]
        public async Task D6(int number = 1)
        {
            if (number > 20) return;
            for(byte a = 0; a < number; a++)
            {
                byte num = byte.Parse(GlobalVars.Rnum.Next(1, 7).ToString());
                switch (num)
                {
                    case 1:
                        await ReplyAsync("You rolled a 1 ⚀");
                        break;

                    case 2:
                        await ReplyAsync("You rolled a 2 ⚁");
                        break;

                    case 3:
                        await ReplyAsync("You rolled a 3 ⚂");
                        break;

                    case 4:
                        await ReplyAsync("You rolled a 4 ⚃");
                        break;

                    case 5:
                        await ReplyAsync("You rolled a 5 ⚄");
                        break;

                    case 6:
                        await ReplyAsync("You rolled a 6 ⚅");
                        break;
                    
                }
            }
        }

        [NormalCommands("!d2","Roll a 2 sided dice, example:\n!d2 6")]
        [Command("d2")]
        public async Task D2(int number = 1)
        {
            if (number > 20) return;
            for(byte a = 0; a < number; a++)
            {
                byte num = byte.Parse(GlobalVars.Rnum.Next(1, 3).ToString());
                switch (num)
                {
                    case 1:
                        await ReplyAsync($"You rolled a {num} ⚀");
                        break;

                    case 2:
                        await ReplyAsync($"You rolled a {num} ⚁");
                        break;
                }
            }
        }

        [NormalCommands("!d1","Roll a 1 sided dice, example\n!d1 2")]
        [Command("d1")]
        public async Task D1(int number = 1)
        {
            if (number > 20) return;
            for(byte a = 0; a < number; a++)
            {
                await ReplyAsync($"You rolled a 1 ⚀");
            }
        }
        
        [NormalCommands("!roll side number", "make your own dice to roll")]
        [Command("roll")]
        public async Task roll(int side, int number = 1)
        {
            if (number > 5) return;
            for(byte a = 0; a < number; a++)
            {
                int num = GlobalVars.Rnum.Next(1, side);
                await ReplyAsync($"You rolled a {num}");
            }
        }
        
        

        [NormalCommands("!sysInfo","View the system resources")]
        [Command("sysInfo")]//need to clean up and or improve
        public async Task InfoStart()
        {
            PerformanceCounter ramcounter = new PerformanceCounter("Memory", "Committed Bytes");
            PerformanceCounter cpuTotal = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter systemtime = new PerformanceCounter("System", "System Up Time");
            PerformanceCounter networkReceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", "Microsoft Hyper-V Network Adapter");//Microsoft Hyper-V Network Adapter
            PerformanceCounter networkSent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", "Microsoft Hyper-V Network Adapter"); //Realtek RTL8192CU Wireless LAN 802.11n USB 2.0 Network Adapter
            dynamic PacketsReceived = networkReceived.NextValue();
            dynamic PacketsSent = networkSent.NextValue();
            dynamic cputotal = cpuTotal.NextValue();
            dynamic ramuseage = ramcounter.NextValue();
            dynamic systemUpTime = systemtime.NextValue();
            await Task.Delay(1000);
            PacketsReceived = networkReceived.NextValue();
            cputotal = cpuTotal.NextValue();
            PacketsSent = networkSent.NextValue();
            ramuseage = ramcounter.NextValue();
            systemUpTime = systemtime.NextValue();
            byte colour = byte.Parse(Math.Round(cputotal, 0, MidpointRounding.ToEven).ToString());//why did i do this
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(100 + colour, 255 - colour, 66);
            eb.WithTitle("Server current system stats");
            eb.AddField("Hardware", "stats in Mega Bytes & %", true);
            eb.AddField("CPU Usage", Math.Round(cputotal, 2, MidpointRounding.ToEven) + " %", true);
            eb.AddField("Memory used", Math.Round(ramuseage / 1048576, 2, MidpointRounding.ToEven) + " MB", true);
            eb.AddField("Network", "stats in Mega bits", true);
            eb.AddField("Data sent", Math.Round(PacketsSent / 125000, 2, MidpointRounding.ToEven) + " Mbps", true);
            eb.AddField("Data received", Math.Round(PacketsReceived / 125000, 2, MidpointRounding.ToEven) + " Mbps", true);
            eb.WithFooter("MB = Mega Bytes || Mbps = Mega bits per second || System up time : " + TimeSpan.FromSeconds(Math.Round(systemUpTime,0,MidpointRounding.ToEven)));
            await ReplyAsync("", false, eb.Build());
        }

        [NormalCommands("!give","Give a user some of your own gold, example:\n!give Vamist 400")]
        [Command("give")]
        [Alias("donate")]
        public async Task Donate(int amount, IUser user)
        {
            Sql sql = new Sql();
            var items = sql.SqlReader(Context.Message.Author.Id);
            if (amount > items.goldCoins)
            {
                await ReplyAsync("Can't give more then what you have");
                throw new ArgumentException("Not enough gold");
            }
            if (amount < 1)
            {
                await ReplyAsync("Can't give nothing nothing");
                throw new ArgumentException("Can't give nothing or less");
            }
            sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,0,0,0,-amount);
            sql.SqlExpriance(user.Id,user.Username,0,0,0,0,0,0,amount);
            await ReplyAsync("Done");
        }

        [Command("give")]
        [Alias("donate")]
        public async Task Donate(IUser user, int amount)
        {
            await Donate(amount, user);
        }
        
        [NormalCommands("!aaa","Turn something into a bunch of aaaaaaa for no reason, example:\n!aaaaaa do you have the stupid??")]
        [Command("aaaa")]
        [Alias("aa","aaa")]
        public async Task DoYouWannaAaaaForMe([Remainder]string textToAaaaa)
        {
            string outPut = "";
            foreach (char letter in textToAaaaa)
            {
                if (letter == '?' || letter == '!' || letter == ' ' )
                    outPut += letter;
                else
                {
                    if (GlobalVars.Rnum.Next(0, 10) > 4)
                        outPut += 'a';
                    else
                        outPut += 'A';
                }
            }
            await ReplyAsync(outPut);
        }

        [Command("8ball")]
        public async Task AskMeAnotherQuestion([Remainder] string textToAaaaa)
        {
            int response = GlobalVars.Rnum.Next(0, 9);
            switch (response)
            {
                case 0:
                    await ReplyAsync("Yes");
                    break;
                
                case 1:
                    await ReplyAsync("No");
                    break;
                
                case 2:
                    await ReplyAsync("Maybe");
                    break;
                
                case 3:
                    await ReplyAsync("Ask again later");
                    break;
                
                case 4:
                    await ReplyAsync(@"Yesn't");
                    break;
                
                case 5:
                    await ReplyAsync("I've been bribed to say yes");
                    break;
                
                case 6:
                    await ReplyAsync("Most likely");
                    break;
                
                case 7:
                    await ReplyAsync("Don't count on it");
                    break;
                
                case 8:
                    await ReplyAsync("Nope");
                    break;
            }
        }
        




        [NormalCommands("!floor","View items on the floor to take!")]
        [Command("items")]
        [Alias("floor", "item")]
        public async Task ItemFloor()
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(66, 244, 176);
            eb.WithTitle("Items on the floor right now");
            eb.WithFooter("Type !take itemName to take the object");
            if (GlobalVars.Items.Bombs > 0)
                eb.AddField("Bombs", GlobalVars.Items.Bombs, true);
            if (GlobalVars.Items.FArrows > 0)
                eb.AddField("Fire Arrows", GlobalVars.Items.FArrows, true);
            if (GlobalVars.Items.Saws > 0)
                eb.AddField("Saws", GlobalVars.Items.Saws, true);
            if (GlobalVars.Items.Coins > 0)
                eb.AddField("Gold Coins", GlobalVars.Items.Coins, true);
            //if (GlobalVars.Caltrops > 0)
              //  eb.AddField("Caltrops found!", GlobalVars.Caltrops + " have been cleaned up", true);
                
            if (GlobalVars.Items.Coins == 0 && GlobalVars.Items.Saws == 0 && GlobalVars.Items.FArrows == 0 && GlobalVars.Items.Bombs == 0)
            {
                GlobalVars.Caltrops = 0;
                await ReplyAsync("No items currently on the ground, check back later.");
                return;
            }
            await ReplyAsync("", false, eb.Build());
            //await ReplyAsync("Fix is coming soon, days attempting to fix it : 2");
        }
        
        [NormalCommands("!tc","View the status of TC")]
        [Command("tc")]
        public async Task TcStatus()
        {
            await Task.Run(() => StatusOfServers("109.228.14.252", 50309));
        }
        
        
        [Command("rm")]
        [Alias("Robs_Madness", "RobsMad")]
        public async Task RobsMadnessStatus()
        {
            await Task.Run(() => StatusOfServers("109.228.14.252", 50301));
        }

        [Command("GF")]
        [Alias("Gold", "GoldFloat")]
        public async Task GoldFloatsStatus()
        {
            await Task.Run(() => StatusOfServers("109.228.14.252", 50302));
        }
        
        
        
        [Command("ww2")]
        [Alias("ww11", "wwii")]
        public async Task WoldWarStatus()
        {
            await Task.Run(() => StatusOfServers("109.228.14.252", 50303));
        }
        
        [NormalCommands("!ros","View the status of RoS")]
        [Command("ros")]
        [Alias("ros2", "ro2")]
        public async Task ROS()
        {
            await Task.Run(() => StatusOfServers("109.228.14.252", 50314));
        }

        //[NormalCommands("!mc", "View the status of minecraft")]
        [Command("mc")]
        [Alias("minecraft")]
        public async Task MC()
        {
            //https://api.mcsrvstat.us/1/109.228.14.252
            try
            {
                string date = DateTime.Now.Day.ToString();
                string time = DateTime.Now.TimeOfDay.ToString();
                var eb = new EmbedBuilder();
                IUserMessage waiting = await ReplyAsync("Waiting for api.mcsrvstat response...");
                await Task.Run(() => MainCommands.McServerStat("https://api.mcsrvstat.us/1/109.228.14.252"));
                waiting.DeleteAsync();
                eb.WithTitle("Minecraft server");
                if (ServerStatus.McPlayerCount > 0)
                    eb.WithColor(Color.Green);
                else
                    eb.WithColor(Color.Red);
                eb.AddField("Server IP:Port", ServerStatus.Ip + ":"+ServerStatus.Port,true);
                eb.AddField("Player count", ServerStatus.McPlayerCount, true);
                eb.AddField("Players in game", ServerStatus.McPlayersPlaying ?? "No current players are playing");
                await ReplyAsync("", false, eb.Build());
            }
            catch (WebException e)
            {
                await ReplyAsync("An webException has occoured, please re-do the command");
                //await MC();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                await ReplyAsync("Something went wrong, please contact your local vamist and report this problem");
            }
            
        }

        [Command("vb")]
        [Alias("volleyball", "ball")]
        public async Task VolleyBall()
        {
            await Task.Run(() => StatusOfServers("109.228.14.252", 50307));
        }


        [NormalCommands("!APIStatus","Ping api.kag2d.com")]
        [Command("APIStatus")]
        public async Task StatusOfAPI()
        {
            bool reponse = websiteReponseCode("api.kag2d.com");
            if (reponse)
                await ReplyAsync("API has given a reponse!");
            else
                await ReplyAsync("API is not responding!");
        }

        
        [Command("serverList")]
        public async Task aLotOfServers()
        {
            TimeSpan time = DateTime.Today.TimeOfDay;
           
            string url =  DateTime.Today.Date.ToString(@"yyyy-MM-dd");
            HttpWebRequest getRequest = (HttpWebRequest) WebRequest.Create(
                "https://api.kag2d.com/v1/game/thd/kag/servers?filters=[{“field”: “currentPlayers”, “op”: “ge”, “value”: 5},{“field”: “lastUpdate”, “op”: “ge”, “value”:\"" +
                url + " " + DateTime.UtcNow.Hour.ToString("HH") + ":00:00"+"\"}]");
            getRequest.Method = "GET";
            HttpWebResponse getreponse = (HttpWebResponse)getRequest.GetResponse();
            Stream newStream = getreponse.GetResponseStream();
            using (var handler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            })
            using (StreamReader sr = new StreamReader(newStream))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;
                var reponse = sr.ReadToEnd();
                Console.WriteLine(reponse);
                JObject a = JsonConvert.DeserializeObject<JObject>(reponse);

                int length = a["serverList"].Count();
                Console.WriteLine(length);
                
                var eb = new EmbedBuilder();
                eb.WithTitle("Server list");

               // eb.AddField(ServerList.);
                
              
              //  await ReplyAsync("", false, eb.Build());   
            }

            newStream.Close();
            getreponse.Close();

        }
        
        

        [NormalCommands("!info","View data about the bot")]
        [Command("info")]
        public async Task AboutBot()
        {
            EmbedBuilder eb = new EmbedBuilder();
            string channelsName = "";
            foreach (string channel in GlobalVars.DiscordGroupNames)
            {
                channelsName += channel + '\n';
                
            }
            eb.Title = "About this bot";
            eb.WithColor(244, 72, 66);
            eb.AddField("Language", "C#");
            eb.AddField("API used", "Discord Dot Net");
            eb.AddField("Maker", "Vamist (or Vam_Jam)");
            eb.AddField("Discords i'm apart of", channelsName);
            eb.AddField("How to view commands", "Type !help or !adminhelp");
            await ReplyAsync("", false, eb.Build());

        }

        [NormalCommands("!status","View the status of any kag server, example: !status 109.228.14.252 50309")]
        [Command("status")]//nobody uses this, they only use !tc
        public async Task StatusOfServers(string ip = null, int port = 0)
        {
            try
            {
                string date = DateTime.Now.Day.ToString();
                string time = DateTime.Now.TimeOfDay.ToString();
                var eb = new EmbedBuilder();
                bool reponse = websiteReponseCode("api.kag2d.com");
                if (reponse)
                {
                    await Task.Run(() =>
                        MainCommands.ServerStat("https://api.kag2d.com/v1/game/thd/kag/server/" + ip + "/" + port +
                                                "/status"));
                    eb.WithTitle(ServerStatus.Name);
                    eb.AddField("Players", ServerStatus.CurrentPlayers, true);
                    
                    if (ServerStatus.Day == false)
                    {
                        eb.AddField("Day or Night?", "Day :sunny:", true);
                        eb.WithColor(79, 182, 242);
                    }
                    else
                    {
                        eb.AddField("Day or Night?", "Night :full_moon:", true);
                        eb.WithColor(3, 83, 130);
                    }
                    eb.AddField("Server IP:Port", ServerStatus.Ip + ":"+ServerStatus.Port,true);

                    eb.AddField("Players in game", ServerStatus.Playerlist ?? "No current players");
                    eb.AddField("Minimap", "Click the image to view it better");
                    eb.WithImageUrl("https://api.kag2d.com/v1/game/thd/kag/server/" + ip + "/" + port + "/minimap?" +
                                    date + time);
                    await ReplyAsync("", false, eb.Build());
                }
                else
                {
                    await ReplyAsync("KAG API is currently down, please try later.");
                }
            }
            catch (WebException e)
            {
                await ReplyAsync("An web exception has occoured, please re-do the command");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                await ReplyAsync("Something is wrong, ping vamist to check what is wrong");
            }
        }

        [Command("Testy")]
        [RequireOwner]
        public async Task JsonMctest()
        {
            await ReplyAsync("Channel ignore list is being made");
            IgnoreChannels Ic = new IgnoreChannels(@"");//file location

            await ReplyAsync("done");
            
            
        }
        
        [Command("rosUpdate")]
        [NormalCommands("!tcUpdate","Join the RoS update role (You will get notified when an update occours)")]
        [Alias("rosUp", "RoSU", "ros2up","ros2u","ros2update")]
        public async Task AddingRoSUpdateRole()
        {
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            SocketRole role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "RoS Updates");
            SocketRole userRole = user.Roles.FirstOrDefault(x => x.Name == "RoS Updates");
            if (user.IsBot)
            {
                return;
            }
            
            if (userRole == role)
            {
                await user.RemoveRoleAsync(role);

                await ReplyAsync("Removed " + user.Mention + " from RoS Updates list!");
                //await Task.Delay(5000);
                //await message.DeleteAsync();
                //await Context.Message.DeleteAsync();
            }
            else
            {
                await user.AddRoleAsync(role);

                await ReplyAsync("Added " + user.Mention + " to RoS Updates list!");
                // await Task.Delay(5000);
                // await message.DeleteAsync();
                // await Context.Message.DeleteAsync();
            }
        }
        
        [NormalCommands("!tcUpdate","Join the TC update role (You will get notified when an update occours)")]
        [Command("tcUpdate")]
        [Alias("tcupdates", "tcU")]
        public async Task AddingTCuRole()
        {
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            SocketRole role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "TC Updates");
            SocketRole userRole = user.Roles.FirstOrDefault(x => x.Name == "TC Updates");
            if (user.IsBot)
            {
                return;
            }
            
            if (userRole == role)
            {
                await user.RemoveRoleAsync(role);

                await ReplyAsync("Removed " + user.Mention + " from TC Updates list!");
                //await Task.Delay(5000);
                //await message.DeleteAsync();
                //await Context.Message.DeleteAsync();
            }
            else
            {
                await user.AddRoleAsync(role);

                await ReplyAsync("Added " + user.Mention + " to TC Updates list!");
                // await Task.Delay(5000);
                // await message.DeleteAsync();
                // await Context.Message.DeleteAsync();
            }
        }
        
        [NormalCommands("!CustomRole","Create your own custom role, must have the colorful role to do so!\nExample: !CustomRole 255 100 250 A good role name")]
        [Command("CustomRole")]
        public async Task CustomRole(byte red, byte green, byte blue, [Remainder] string name)
        {
            
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            SocketRole SoloRole = null;
            int rolePos = 0;
            foreach (SocketRole role in user.Roles)
            {
                if (role.Position > rolePos)
                    rolePos = role.Position;
                if (role.Id == 474225965254311938)
                    allowed = true;    

                if (role.Members.Count() == 1)
                    SoloRole = role;
            }


            if (allowed)
            {
                Color tempColor = new Color(red,green,blue);
                if (SoloRole == null)
                {             
                    
                    var a = await Context.Guild.CreateRoleAsync(name, null, tempColor);
                    await a.ModifyAsync(role =>
                    {
                        role.Position = rolePos + 1;

                    });
                    
                    await user.AddRoleAsync(a);
                    await ReplyAsync("Done");
                }
                else
                {
                    try
                    {
                        await SoloRole.ModifyAsync(role =>
                        {
                            role.Color = tempColor;
                            role.Name = name;
                            role.Position = rolePos + 1;

                        });
                        await ReplyAsync("Done");
                    }
                    catch (Exception e)
                    {
                        await ReplyAsync("Should be done, but i'm missing permission for something.");
                    }


                    

                }


            }
            else
            {
                await ReplyAsync("You do not have the colorful role");
            }
            
                

        }


       
        
        public bool websiteReponseCode(string apiLink)
        {

            Ping ping = new Ping();
            PingReply response = ping.Send(apiLink);

            if (response.Status != IPStatus.Success)
                return false;

            return true;
        }
    }
}