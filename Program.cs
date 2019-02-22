using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Net.Cache;
using System.Net.NetworkInformation;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using DiscordBot.Classes;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using DiscordBot.Commands;
using Color = System.Drawing.Color;
using Console = Colorful.Console;

namespace DiscordBot
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _service;
        private static string _token;
        private bool HasStarted = false;
        private Stopwatch DisconnectTimer;

        public static void Main(string[] args)
        {
            //GetStartData();
            Task.Run(ItemDrops);
            Task.Run(SqlBackup);
            try
            {
                GlobalVars.Ic =
                    new IgnoreChannels(@"");//For ignoring channels, give file location here
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
            new Program().MainAsync().GetAwaiter().GetResult();
        }


        public async Task MainAsync()
        {
            DiscordSocketConfig config = new DiscordSocketConfig
            {
                MessageCacheSize = 400,
                AlwaysDownloadUsers = true
            };
            
            _client = new DiscordSocketClient(config);
            _commands = new CommandService();
           // _client.Log += Log;

            _client.MessageReceived += MessageReceived;

            _service = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            await InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, "");
            await _client.StartAsync();
            await _client.SetGameAsync("Finding objects");
            Console.Title = "Chicken Discord Bot";
            Console.WriteLine("Loaded, waiting for commands");

            

            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            _client.Ready += IsReady;
            _client.MessageReceived += HandleCommandAsync;
            //_client.JoinedGuild += UserJoinedLogs;
            _client.UserJoined += UserJoinedLogs;
            _client.UserLeft += UserLeftLogs;
            _client.MessageDeleted += MessageDeletedLogs;
            _client.UserBanned += UserBannedLogs;
            _client.UserUnbanned += UserUnbannedLogs;
            _client.MessageUpdated += MessageUpdatedLogs;
            _client.ReactionAdded += ReactionUpdated;
            _client.ReactionRemoved += ReactionRemoved;
            _client.Disconnected += Disconnected;
            

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task Disconnected(Exception arg)
        {
            await Task.Delay(1);
            DisconnectTimer.Start();
        }

        private async Task IsReady()
        {
            var guilds = _client.Guilds.ToList();
            
                
            
            
            
            
            foreach (SocketGuild guild in guilds)
            {


                Console.WriteLineFormatted("Guild found : {0}", Color.LightGoldenrodYellow, Color.White,
                    guild.Name);
                GlobalVars.DiscordGroupNames.Add(guild.Name);
                if (guild.Id == 361255623456849923)
                {
                    
                    var channels = guild.Channels;
                    foreach (SocketGuildChannel channel in channels)//does not work
                    {
                        if (channel.Id == 386495658418503680)
                        {
                            ISocketMessageChannel messageChannel = (ISocketMessageChannel) channel;
                            if (HasStarted == false)
                            {
                                //await messageChannel.SendMessageAsync("Bot has fully loaded");
                                //await GlobalVars.AutoTC.MainProgram();
                                Console.WriteWithGradient("Bot has fully loaded \n", Color.Yellow, Color.Fuchsia, 14);
                                HasStarted = true;
                            }
                            else
                            {
                                DisconnectTimer.Stop();
                                
                                await messageChannel.SendMessageAsync("Bot has reconnected to discord, time i have been unable to reconnect :  " + DisconnectTimer.Elapsed);
                                DisconnectTimer.Reset();
                                Console.WriteLineWithGradient("Bot has reconnected \n", Color.Red, Color.Green, 14);
                                
                            }
                                


                        }
                    }
                }
            }
        }

        private async Task ReactionRemoved(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel messageChannel, SocketReaction reaction)
        {
            await Task.Delay(1);
            if (messageChannel.Id == 478856296083488769)
            {
                SqlLeaderboard sql = new SqlLeaderboard();
                
                Emote one = Emote.Parse("<:1_:479318122805854209>");
                Emote two = Emote.Parse("<:2_:479318122457989120>");
                Emote three = Emote.Parse("<:3_:479316551397408770>");
                Emote four = Emote.Parse("<:4_:479316552068628500>");
                Emote five = Emote.Parse("<:5_:479316551745404958>");
                if (reaction.Emote.Name == one.Name)
                    sql.SqlReactionAdded(reaction.MessageId,11);
                else if (reaction.Emote.Name == two.Name)
                    sql.SqlReactionAdded(reaction.MessageId,12);
                else if (reaction.Emote.Name == three.Name)
                    sql.SqlReactionAdded(reaction.MessageId,13);
                else if(reaction.Emote.Name == four.Name)
                    sql.SqlReactionAdded(reaction.MessageId,14);
                else if(reaction.Emote.Name == five.Name)
                    sql.SqlReactionAdded(reaction.MessageId,15);
                sql.SqlClose();
            }
        }

        private async Task ReactionUpdated(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel messageChannel, SocketReaction reaction)
        {
            
            

            //lewd id = 458292308325040128
            if (messageChannel.Id == 478856296083488769 || messageChannel.Id == 458292308325040128) // TODO CHANGE ID BACK TO OTHER
            {

                //📝
                try
                {
                    SqlLeaderboard sql = new SqlLeaderboard();
                    Emoji emote = new Emoji("\U0001F4DD");
                    
                    
                    
                   /* Emoji one = new Emoji("\x0031\xFE0F\x20E3");
                    Emoji two = new Emoji("\x0032\xFE0F\x20E3");
                    Emoji three = new Emoji("\x0033\xFE0F\x20E3");
                    Emoji four = new Emoji("\x0034\xFE0F\x20E3");
                    Emoji five = new Emoji("\x0035\xFE0F\x20E3");*/
                    //Console.WriteLine("started");

                    Emote one = Emote.Parse("<:1_:479318122805854209>");
                    Emote two = Emote.Parse("<:2_:479318122457989120>");
                    Emote three = Emote.Parse("<:3_:479316551397408770>");
                    Emote four = Emote.Parse("<:4_:479316552068628500>");
                    Emote five = Emote.Parse("<:5_:479316551745404958>");
                    //Emote addedEmote = (Emote) reaction.Emote;
                    
    
                    IUserMessage message = await cache.GetOrDownloadAsync();


                    if (reaction.Emote.Name == emote.Name)
                    {
                        try
                        {

                            IReadOnlyCollection<IUser> reactionsAdded = await message.GetReactionUsersAsync(emote);
                            foreach (IUser user in reactionsAdded)
                            {
                                await message.RemoveReactionAsync(emote, user);
                            }
                            
                            if (sql.SqlSearchPostId(message.Id) == false)
                            {


                                //await message.RemoveReactionAsync(emote, message.Author );


                                //to do fix (does not delete if i added it to a diffrent users message)

                                if (message.Attachments.Count == 0)
                                {
                                    sql.SqlAddNewPost(message.Id, message.Author.Username, message.Content);
                                }
                                else
                                {
                                    sql.SqlAddNewPost(message.Id, message.Author.Username,
                                        message.Attachments.FirstOrDefault().Url);
                                }

                                await message.AddReactionAsync(one);
                                await message.AddReactionAsync(two);
                                await message.AddReactionAsync(three);
                                await message.AddReactionAsync(four);
                                await message.AddReactionAsync(five);
                              
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            sql.SqlClose();
                            throw;
                        }
                    }
                    else if (reaction.Emote.Name == one.Name)
                        sql.SqlReactionAdded(reaction.MessageId,1);   
                    else if (reaction.Emote.Name == two.Name)
                        sql.SqlReactionAdded(reaction.MessageId,2);
                    else if (reaction.Emote.Name == three.Name)
                        sql.SqlReactionAdded(reaction.MessageId,3);
                    else if(reaction.Emote.Name == four.Name)
                        sql.SqlReactionAdded(reaction.MessageId,4);
                    else if(reaction.Emote.Name == five.Name)
                        sql.SqlReactionAdded(reaction.MessageId,5);
                    sql.SqlClose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
                
                      
               
            }
            Emoji heart = new Emoji("\U0001F49A");
            if (reaction.Emote.Name == heart.Name)
            {
                
                IUserMessage message = await cache.GetOrDownloadAsync();
                
                IReadOnlyCollection<IUser> reactionsAdded = await message.GetReactionUsersAsync(heart);
                
                foreach (IUser user in reactionsAdded)
                {
                    string[] list = new string[]
                    {
                        user.Username,
                        message.Id.ToString(),
                        message.Author.Username
                    };
                    Console.WriteLineFormatted("{0} Added the lewd emote to message {1} | Message owner is {2}", Color.DarkRed,Color.White,list);
                  
                    await message.RemoveReactionAsync(heart, user);
                }

                
               
                await message.AddReactionAsync((Emote.Parse("<:eaturgreens:483262810730659841>")));
            }
            
        }

        private async Task MessageUpdatedLogs(Cacheable<IMessage,ulong> cache, SocketMessage message, ISocketMessageChannel messageChannel)
        {
            if (message.Author.IsBot) return;
            if (GlobalVars.Ic.IdFoundInList(message.Channel.Id)) return;
            if (message.Content.Contains("http://")) return;
            if (message.Content.Contains("https://")) return;
            //Console.WriteLine(message.Channel.Id + " channel id");
            //Console.WriteLine(PrivateDiscord(message.Channel.Id)); //test
            
            IMessage editedMessage = await cache.GetOrDownloadAsync();
            SocketGuild guild = _client.GetGuild(361255623456849923);
            EmbedBuilder eb = new EmbedBuilder();
            IUserMessage mes = (IUserMessage) message;
            IGuildChannel chan = (IGuildChannel) messageChannel;
            
            eb.WithAuthor(message.Author.Username + message.Author.Discriminator, message.Author.GetAvatarUrl());
            eb.WithColor(50, 50, 200);
            /*var newString = message.Content.ToString();
            newString = newString.Remove('@');
            newString = newString.Replace("```", "\"");
            newString = newString.Replace("``", "\"");
            Console.WriteLine("content 2 started");
            var newEditString = editedMessage.Content.ToString();
            newString = newString.Remove('@');
            newString = newString.Replace("```", "\"");
            newString = newString.Replace("``", "\"");
            Console.WriteLine("adding field started");*/
            eb.AddField("Edited Message Now Contains:","```"+message.Content+"```");
            eb.AddField("Message Did Contain", "```" + editedMessage.Content + "```");
            eb.AddField("Information:",
                "```Message sent time : " + message.CreatedAt + " GMT \nMessage edit time : " + message.EditedTimestamp + " GMT \nIn channel : " + message.Channel +
                "\nGuild : "+chan.Guild.Name+"```");
            await guild.GetTextChannel(444912231176601600).SendMessageAsync("",false,eb.Build());
        }
        
        private async Task UserUnbannedLogs(SocketUser user, SocketGuild guild)
        {
            await guild.GetTextChannel(444912231176601600).SendMessageAsync("User "+user.Mention +" Has been been removed from the ban list");
        }
        

        
        private async Task UserBannedLogs(SocketUser user, SocketGuild guild)
        {
            await guild.GetTextChannel(444912231176601600).SendMessageAsync("User "+user.Mention +" Has been banned. RIP");
        }

        private async Task MessageDeletedLogs(Cacheable<IMessage, ulong> cacheable,
            ISocketMessageChannel messageChannel)
        {
            //await messageChannel.GetMessageAsync(cacheable.Id);
            
            IMessage message = await cacheable.GetOrDownloadAsync();

            //   if (message.Channel.Id == 458292308325040128 || message.Channel.Id == 381190850157215755 || message.Channel.Id == 385864005480349706)return;
            
            if(message.Author.IsBot) return;
            if (GlobalVars.Ic.IdFoundInList(message.Channel.Id)) return;
            SocketGuild guild = _client.GetGuild(361255623456849923);
            EmbedBuilder eb = new EmbedBuilder();
            IGuildChannel chan = (IGuildChannel) messageChannel;
            eb.WithAuthor(message.Author.Username + message.Author.Discriminator, message.Author.GetAvatarUrl());
            eb.WithColor(240, 50, 50);
            /*string newString = message.Content.ToString();
            newString = newString.Remove('@');
            newString = newString.Replace("```", "\"");
            newString = newString.Replace("``", "\"");*/
            if (message.Content == "" || message.Content is null)
            {
                eb.AddField("Deleted message",message.Attachments.FirstOrDefault().Url);
            }
            else
            {
                eb.AddField("Deleted Message Contained:","```"+message.Content+"```");
            }
            eb.AddField("Information:",
                "```Message sent time : " + message.CreatedAt + " GMT \nIn channel : " + message.Channel +
                "\nHow many Attachments : " + message.Attachments.Count + "\nGuild : "+chan.Guild.Name+"```");
            
            await guild.GetTextChannel(444912231176601600).SendMessageAsync("",false,eb.Build());   
        }
        
        private async Task UserLeftLogs(SocketGuildUser user)
        {
            await user.Guild.GetTextChannel(444912231176601600).SendMessageAsync("User "+user.Mention +" Has left.");
        }
        
        private async Task UserJoinedLogs(SocketGuildUser user)
        {
            
            if (user.Id == 321084764092366868)
            {
                var guilds = _client.Guilds.ToList();






                foreach (SocketGuild guild in guilds)
                {
                
                    
                    var channels = guild.Channels;
                    foreach (SocketGuildChannel channel in channels) //does not work
                    {
                        if (channel.Id == 361255623456849925)
                        {
                            ISocketMessageChannel messageChannel = (ISocketMessageChannel) channel;
                            await messageChannel.SendMessageAsync("Welcome back dragonfriend (again)");
                        }
                    }
                }
            }



            await user.Guild.GetTextChannel(444912231176601600).SendMessageAsync("User "+user.Mention +" Has joined.");
        }
        
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {

            if (!(messageParam is SocketUserMessage message)) return;
            if (message.Author.IsBot)
                return;
            int argPos = 0;

           
            await Task.Factory.StartNew(() => ResponseToMessage(message.Content, message, message.Channel));
            await ChannelsStuff(message,message.Channel);
           
            //if user says !, or says @bot username then do command. Improve later by chaning isbot check
            if (!(message.HasCharPrefix('!', ref argPos) && message.Author.IsBot == false || message.HasMentionPrefix(_client.CurrentUser, ref argPos) && message.Author.IsBot == false)) return;

            SocketCommandContext context = new SocketCommandContext(_client, message);

            bool truAdmin = false;
                
            SocketGuildUser userA = (SocketGuildUser) message.Author;
            foreach (SocketRole role in userA.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    truAdmin = true;
                    break;
                }
            }
            
            await DoChatCommand(context,argPos,truAdmin);
            
            

            
            
            Console.WriteLine(string.Concat(context.Message.Timestamp.UtcDateTime, " ", context.Message.Author.Username," at ",context.Guild, " in ",context.Channel," said ",context.Message));
            await LogOutputs(context.Message.ToString(), message.Author.Id);
           
        }


        private async Task ResponseToMessage(string content, SocketUserMessage message, ISocketMessageChannel channel)
        {
            bool pm = false;
            if (message.Author.IsBot)
                return;
            
            if (!(channel.Id == 386495658418503680 && channel.Id == 458292308325040128))
                pm = true;
            content = content.ToLower();
            string output = "";
            string[] eachWord = content.Split(' ');
            
            Random random = new Random();
            
            List<string> responseToList = new List<string>(
                new string[]
                {
                    "boop","beep","boep","bap","baop"
                });
            
            List<string> outputReponse = new List<string>(
                new string[]
                {
                    "boop","beep","beep boop", "bap", "baaaaap", "beop", "bop"
                });
            int rnum = 0;
           
            
            foreach (string word in eachWord)
            {
                foreach (string response in responseToList)
                {
                    if (word == response)
                    {
                        rnum = random.Next(0, outputReponse.Count);
                        output += outputReponse[rnum] + ' ';
                    }
                }
            }

            /*if (output == "")
            {
               
                responseToList = new List<string>(
                    new string[]
                    {
                        "love you","<3 you", "<3"
                    });
                
                outputReponse = new List<string>(
                    new string[]
                    {
                        "Robots do not feel love","aww thanks","love you too, dont tell vamist","<3"
                    });
                
                
                foreach (string response in responseToList)
                {
                    if (content == response)
                    {
                        rnum = random.Next(0, outputReponse.Count);
                        output += outputReponse[rnum] + ' ';
                    }
                }
            }*/

            if (output == "")
            {
                responseToList = new List<string>(
                    new string[]
                    {
                        "fuck you","go away", "idiot bot","bad bot","broken bot","bot broke"
                    });
                
                outputReponse = new List<string>(
                    new string[]
                    {
                        "no u","bad human","good bot"
                    });
                
                foreach (string response in responseToList)
                {
                    if (content == response)
                    {
                        rnum = random.Next(0, outputReponse.Count);
                        output += outputReponse[rnum] + ' ';
                    }
                }
            } 
              
           

            if (output != "")
            {
                Console.WriteLine("User : " + message.Author.Username + " did " + message.Content);
                if (message.Author.Id == 301370472594014218 || message.Author.Id == 142300409002852352)
                {
                    await message.Channel.SendMessageAsync(output);
                }
                else
                {
                    if (pm)
                    {
                        //await message.Author.SendMessageAsync(output);
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync(output);
                    }
                }

                
            }
        }

        private async Task ChannelsStuff(SocketUserMessage message, ISocketMessageChannel channel)
        {
            
            if (message.Channel.Id == 474218307780608020 || message.Channel.Id == 488422435935485973)
            {
                DiscordUserHandler user = new DiscordUserHandler((SocketGuildUser)message.Author);
                if (!user.Moderator)
                {
                    await message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                    await message.AddReactionAsync(Emote.Parse("<:kag_disagree:399994688633045004>"));   
                }
            }
        }

        private async Task DoChatCommand(SocketCommandContext context, int argPos, bool isUserTrustedAdmin = false)
        {
            bool doCommand = false;
            if(context.Channel.Name == "bot-area" || context.Channel.Name == "bot-testing" || context.Channel.Id == 483258510021361664)//could get id of said group instead of name
            {
                doCommand = true;
            }
            else if (context.Message.Content.Contains("!mc") && context.Channel.Id == 474628525454786562)
            {
                doCommand = true;
            }
            else if (context.Message.Content.Contains("!burn")  || context.Message.Content.Contains("!clean") || context.Message.Content.Contains("!lenny") || context.Message.Content.Contains("!mystery"))
            {
                doCommand = true;
            }
            else if (context.Message.Channel.Id == 458292308325040128 && context.Message.Content.Contains("!lewd"))
            {
                doCommand = true;
            }


            if (doCommand || isUserTrustedAdmin)
            {
                IResult result = await _commands.ExecuteAsync(context, argPos, _service);
                if (!result.IsSuccess)
                {
                    await LogOutputs(result.ErrorReason.ToString(), context.Message.Author.Id);
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }

       /* private Task Log(LogMessage msg)//logs, not sure what it does yet, something into the console, for now i'l make my own
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }*/

        public string GetChannelTopic(ulong id)//gets channel topic. NOTE FOR FUTURE, USE IT
        {
            if (!(_client.GetChannel(id) is SocketTextChannel channel)) return "";
            return channel.Topic;
        }
        //
        //
        //
        public async Task LogOutputs(string text, ulong id)
        {
            await Task.Delay(1);
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.WriteLine(string.Concat(DateTime.Now," || ",id," || ", text));
            }
        }

        
        bool PrivateDiscord(ulong id)
        {
            for(byte a = 0; a <= (GlobalVars.PrivateDiscordChannel.Length - 1); a++)
            {
                if (id == GlobalVars.PrivateDiscordChannel[a])
                {
                    return true;
                }
            }
            return false;
        }
        //could improve and or change later. more compressed or maybe change how often it is saved
        static async Task SqlBackup()
        {
            while (true)
            {
                await Task.Delay(1000 * 60 * 60 * 24);
                try
                {
                    Sql sql = new Sql();
                    sql.SqlBackUp();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Backup error: " + e);
                }
            }
        }

        public static async Task SqlTask(ulong userid, string username)
        {
            await Task.Delay(1);
            Sql sql = new Sql();
            sql.SqlExpriance(userid,username,1);
        }

        static async Task ItemDrops()//used to make the mini item drop game
        {
            //loop
            GlobalVars.Items = new JsonItems(@"");//File location goes here
            Random rnum = new Random();
           
            while (true)
            {
                int wait = rnum.Next(1, 21); //20
                await Task.Delay(1000 * 60 * 9 + wait); // 9
                int num = rnum.Next(1, 1000);
                if (num > 0 && num < 250)
                {
                    //bombs
                    int num1 = rnum.Next(2, 7);
                   // GlobalVars.Bombs = GlobalVars.Bombs + num1;
                    GlobalVars.Items.Bombs += num1;
                }
                else if (num > 249 && num < 500)
                {
                    //firearrows
                    int num1 = rnum.Next(2, 5);
                    //GlobalVars.Firearrows = GlobalVars.Firearrows + num1;
                    GlobalVars.Items.FArrows += num1;
                }
                else if (num > 499 && num < 750)
                {
                    //saws
                    //GlobalVars.Saws = GlobalVars.Saws + 1;
                    GlobalVars.Items.Saws += 1;
                }
                else if (num > 749 && num < 1001)
                {
                    //gold coins
                    int num1 = rnum.Next(1, 60);
                   // GlobalVars.Goldcoins = GlobalVars.Goldcoins + num1;
                    GlobalVars.Items.Coins += num1;
                }
                else
                {
                    //just incase.
                    GlobalVars.Bombs += 1;
                    GlobalVars.Items.Bombs += 1;
                }

                if (rnum.Next(1, 1000) > 499)
                    GlobalVars.Caltrops += 1;
                GlobalVars.Spawntrue = true;
                GlobalVars.Items.SaveAll();
            //DiscordSocketClient _client = new DiscordSocketClient();
            //await _client.SetGameAsync("Has found "+GlobalVars.bombs + GlobalVars.firearrows + GlobalVars.goldcoins + GlobalVars.saws + " items!");
            }
            
        }

        private static void GetStartData()
        {
            string fileLocation = @"./settings.da";
            if (File.Exists(fileLocation))
            {
                string token = "";
                Console.WriteLine("done");
                string[] lines = File.ReadAllLines(fileLocation);
                Console.WriteLine("Beep");
                foreach (string text in lines)
                {
                    Console.WriteLine("hi");
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (text[i] == '=')
                        {
                            Console.WriteLine("tre");
                            for (int a = 0; a < text.Length; i++)
                            {
                                if(text[a] == ' ' || text[a] == '=')
                                    continue;
                                token += text[a];
                            }
                        }
                    }
                }
                Console.WriteLine(token);
                _token = token;
            }
            else
            {
                 Console.WriteLine("does not exist");   
                File.Create(fileLocation);
                string text = "";
                File.WriteAllText(fileLocation,text);
                _token = "";
                Console.WriteLine(_token);
            }    
        }
        //
        //
        //
        //
        //Realtime response
        //
        //
        //
        //

        private async Task MessageReceived(SocketMessage message)//never make this higher then 0 or 1.
        {
            await Task.Delay(1);
        }
        
        bool AllAdminCheck(ulong id)
        {
            for(byte a = 0; a <= (GlobalVars.AllAdminRanks.Length - 1); a++)
            {
                if (id == GlobalVars.AllAdminRanks[a])
                {
                    return true;
                }
            }
            return false;
        }
        
        bool TrustedAdminCheck(ulong id)
        {
            if (id == GlobalVars.TrustedAdminId)
                return true;
            if (id == GlobalVars.OwnerId)
                return true;

            return false;
        }

    }
}
    