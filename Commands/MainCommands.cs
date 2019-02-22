using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

/*
 * todo clean up code, place in seperate files, or break away parts, like what i did with status. super flexable.
 * todo fix SQL backup..
 * todo split off commands into normal, admin, testing and god commands
 */

namespace DiscordBot.Commands
{
    public class MainCommands : ModuleBase<SocketCommandContext>
    {
        
        

        [AdminCommands("!AddRole","Add a role to a user (Don't use for now)")]
        [Command("AddRole")]
        public async Task AddUserRole(SocketGuildUser user, [Remainder] string roleName)
        {
            SocketGuildUser author = (SocketGuildUser)Context.Message.Author;
            bool admin = false;
            foreach (SocketRole role in author.Roles)
            {
                if (role.Id == 364040665149079564 || role.Id == 361262968199184397 || role.Id == 361262801387520000)
                    admin = true;
            }

            if (admin)
            {
                SocketRole roleForUser;
                switch (roleName.ToLower())
                {
                    case "gulag":
                    case "jail":
                    case "cage":
                        roleForUser = Context.Guild.Roles.FirstOrDefault(x => x.Id == 377203918557675530);
                        break;
                    
                    default:
                        await ReplyAsync("Role name not found");
                        return;
                }


                if (user.Roles.Contains(roleForUser))
                {
                    await ReplyAsync("User is already in this role!");
                }
                else
                {
                    await user.AddRoleAsync(roleForUser);
                    await ReplyAsync("Done");   
                }
            }
            else
            {
                await ReplyAsync("You don't have the role to add/remove roles.");
            }
        }
        
        [AdminCommands("!RemoveRole","Remove a role to a user (Don't use for now)")]
        [Command("RemoveRole")]
        public async Task RemoveUserRole(SocketGuildUser user, [Remainder] string roleName)
        {
            SocketGuildUser author = (SocketGuildUser)Context.Message.Author;
            bool admin = false;
            foreach (SocketRole role in author.Roles)
            {
                if (role.Id == 364040665149079564 || role.Id == 361262968199184397 || role.Id == 361262801387520000)
                    admin = true;
            }

            if (admin)
            {
                SocketRole roleForUser;
                switch (roleName.ToLower())
                {
                    case "gulag":
                    case "jail":
                    case "cage":
                        roleForUser = Context.Guild.Roles.FirstOrDefault(x => x.Id == 377203918557675530);
                        break;
                    
                    default:
                        await ReplyAsync("Role name not found");
                        return;
                }

                if (user.Roles.Contains(roleForUser))
                {
                    await user.RemoveRoleAsync(roleForUser);
                    await ReplyAsync("Done");   
                }
                else
                {
                    await ReplyAsync("User is not in " + roleName);
                }
            }
            else
            {
                await ReplyAsync("You don't have the role to add/remove roles.");
            }
        }
        
        [AdminCommands("!AdminHelp","Displays the admin help menu")]
        [Command("AdminHelp")]
        public async Task AHelp()
        {      
            
            SocketGuildUser author = (SocketGuildUser)Context.Message.Author;
            bool admin = false;
            foreach (SocketRole role in author.Roles)
            {
                if (AllAdminCheck(role.Id))
                {
                    admin = true;
                    break;
                }
            }

            if (admin)
            {
                MethodInfo[] methordsInfo = typeof(MainCommands).GetMethods();
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithColor(12, 245, 249);
                eb.WithTitle("Admin Help");
                for (int a = 0; a < methordsInfo.Length; a++)
                {
                    AdminCommands data = methordsInfo[a].GetCustomAttribute<AdminCommands>(false);
                
                    if (data != null)
                    {
                        eb.AddField(data.Name + "      | Is Trusted admin only = "+data.SuperAdminOnly, data.Desc);
                    }
                }
                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync("Not cool enough");
            }


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

        bool AdminCheck(ulong id)
        {
            for(byte a = 0; a <= (GlobalVars.AdminRanks.Length - 1); a++)
            {
                if (id == GlobalVars.AdminRanks[a])
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

        bool OwnerOnlyCheck(ulong id)
        {
            if (id == GlobalVars.OwnerId)
                return true;
            return false;
        }
        
        [GodCommands("!OwnerHelp","Displays the Owner help menu")]
        [Command("OwnerHelp")]
        public async Task OHelp()
        {      
            
            SocketGuildUser author = (SocketGuildUser)Context.Message.Author;
            bool admin = false;
            foreach (SocketRole role in author.Roles)
            {
                if (OwnerOnlyCheck(role.Id))
                {
                    admin = true;
                    break;
                }
            }

            if (admin)
            {
                MethodInfo[] methordsInfo = typeof(MainCommands).GetMethods();
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithColor(12, 245, 249);
                eb.WithTitle("Owner Help");
                for (int a = 0; a < methordsInfo.Length; a++)
                {
                    GodCommands data = methordsInfo[a].GetCustomAttribute<GodCommands>(false);
                
                    if (data != null)
                    {
                        eb.AddField(data.Name, data.Desc);
                    }
                }
                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                await ReplyAsync("Not cool enouhg");
            }


        }

        [Command("lewd")]
        [Alias("lewdleaderboard", "leaderboard", "goodstuff", "lewderboard", "lewdboard")]
        [RequireNsfw]
        public async Task LewdLeaderboard()
        {
            if (Context.Channel.Id == 458292308325040128 || Context.Channel.Id == 478856296083488769)
            {
                SqlLeaderboard sql = new SqlLeaderboard();
                Top5Content t5 = new Top5Content();
                try
                {
                    t5 = sql.SqlTop5();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                //Top5Content t5 = sql.SqlTop5();
                Console.WriteLine("done");
                await ReplyAsync("**Most voted lewd content is** : \n" +
                                 t5.One.Content +" \n" +
                                 "**User who posted** : " + t5.One.Username + "\n"+
                                 "**Rating** : " + t5.One.AvarageRating +
                                 "\n");
                
                await ReplyAsync("|----------------------------------|\n" +
                                 "**Second most voted lewd content is** : \n" +
                                 t5.Two.Content +" \n" +
                                 "**User who posted** : " + t5.Two.Username + "\n"+
                                 "**Rating** : " + t5.Two.AvarageRating+
                                 "\n\n\n");
                
                await ReplyAsync("|----------------------------------|\n" +
                                 "**Third most voted lewd content is** : \n" +
                                 t5.Three.Content +" \n" +
                                 "**User who posted** : " + t5.Three.Username + "\n"+
                                 "**Rating** : " + t5.Three.AvarageRating+
                                 "\n\n\n");
                
                await ReplyAsync("|----------------------------------|\n" +
                                 "**Foruth most voted lewd content is** : \n" +
                                 t5.Four.Content +" \n" +
                                 "**User who posted** : " + t5.Four.Username + "\n"+
                                 "**Rating** : " + t5.Four.AvarageRating+
                                 "\n\n\n");
                
                await ReplyAsync("|----------------------------------|\n" +
                                 "**Fifth most voted lewd content is** : \n" +
                                 t5.Five.Content +" \n" +
                                 "**User who posted** : " + t5.Five.Username + "\n"+
                                 "**Rating** : " + t5.Five.AvarageRating+
                                 "\n\n\n");
                
                
                sql.SqlClose();
                
                
            }
            else
            {
                await ReplyAsync("Can not do this command here");
            }
               
        }
        
        
        
        [AdminCommands("!burn amount","Remove x amount of messages (5 per 1 sec is the API limit)")]
        [Command("burn")]
        [Alias("clean","clear")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task DeleteMessages(uint amount)
        {
            SocketTextChannel castedChannel = (SocketTextChannel) Context.Channel;

            var messages = await Context.Channel.GetMessagesAsync((int) amount + 1).FlattenAsync();

            await castedChannel.DeleteMessagesAsync(messages);
                
        }
        
        [AdminCommands("!burn user amount","Remove user x amount of messages (5 per 1 sec is the API limit)")]
        [Command("burn")]
        [Alias("clean","clear")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task DeleteUserMessages(SocketUser user, int amount = 50)
        {
            SocketTextChannel castedChannel = (SocketTextChannel) Context.Channel;
           

            var messages = await Context.Channel.GetMessagesAsync((int) amount + 1).FlattenAsync();

            foreach (IMessage userMessage in messages)
            {
                if (userMessage.Author.Id == user.Id)
                {
                    await userMessage.DeleteAsync();
                }
            }
            //await castedChannel.DeleteMessagesAsync(messages);
            
                
        }
        
        
        
        [Command("Data")]
        [RequireOwner]
        public async Task DataGetter(string select, string order)
        {
            Sql sql = new Sql();
            var items = sql.SqlDataReturn(select, order);
            var eb = new EmbedBuilder();
            eb.AddField(order ,items);
            eb.WithColor(150, 0, 230);
            await ReplyAsync("", false, eb.Build());
        }
        
        

        [GodCommands("!spawn object amount","Spawns in an object on the floor")]
        [Command("spawn")]
        public async Task Spawn(string text, int amount = 40)
        {
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }

            if (allowed)
            {
                switch (text)
                {
                    case "bombs":
                    case "bomb":
                        GlobalVars.Items.Bombs += amount;
                        await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                        break;

                    case "gold":
                    case "coins":
                    case "coin":
                        GlobalVars.Items.Coins += amount;
                        await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                        break;

                    case "saw":
                    case "saws":
                        GlobalVars.Items.Saws += amount;
                        await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                        break;

                    case "fire_arrows":
                    case "fire":
                    case "fire_arrow":
                    case "arrows":
                    case "arrow":
                        GlobalVars.Items.FArrows += amount;
                        await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                        break;

                    default:
                        await Context.Message.AddReactionAsync(Emote.Parse("<:kag_disagree:399994688633045004>"));
                        break;
                }
            }

        }

        [AdminCommands("!startTC", "Start up TC!", true)]
        [Command("starttc")]
        public async Task StartTc()
        {
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }

            if (allowed)
            {
                if (await GlobalVars.TC.StartServer())
                    await ReplyAsync("Done");
                else
                    await ReplyAsync("Error, check console");
            }
        }
        
        [AdminCommands("!startRos", "Start up Ros!", true)]
        [Command("startros")]
        public async Task Startros()
        {
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }

            if (allowed)
            {
                if (await GlobalVars.RoS.StartServer())
                    await ReplyAsync("Done");
                else
                    await ReplyAsync("Error, check console");
            }
        }
        
        
        [AdminCommands("!killww2","Close down ww2 so it can be restarted",true)]
        [Command("killww2")]
        // [RequireUserPermission(GuildPermission.Administrator)] using this while tflippy does not have admin role
        public async Task KillWW2()
        {
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            if (user.Id == 263634996122746891)
                allowed = true;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }

            if (allowed)
            {
                Process[] proc = Process.GetProcessesByName("ROS");
                try
                {
                    proc[0].Kill();
                    await ReplyAsync(
                        $"WW2 Closed successfully {Context.Message.Author.Mention}, it will restart shortly");
                }
                catch (Exception e)
                {
                    await ReplyAsync(
                        $"WW2 Can not be closed right now {Context.Message.Author.Mention}, {e.GetType().Name}");
                }
            }

        }

        [AdminCommands("!killTC","Close down TC so it can be restarted",true)]
        [Command("killtc")]
       // [RequireUserPermission(GuildPermission.Administrator)] using this while tflippy does not have admin role
        public async Task KillTc()
        {
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }

            if (allowed)
            {
                Process[] proc = Process.GetProcessesByName("TC");
                try
                {
                    proc[0].Kill();
                    await ReplyAsync($"TC Closed successfully {Context.Message.Author.Mention}, it will restart shortly");
                }
                catch(Exception e)
                {
                    await ReplyAsync($"TC Can not be closed right now {Context.Message.Author.Mention}, {e.GetType().Name}");
                }
            }
    
        }
        
        [AdminCommands("killRoS","Close down RoS so it can be restarted",true)]
        public async Task KillRos()
        {
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }

            if (allowed)
            {
                Process[] proc = Process.GetProcessesByName("RoS");
                try
                {
                    proc[0].Kill();
                    await ReplyAsync($"RoS Closed successfully {Context.Message.Author.Mention}, it will restart shortly");
                }
                catch(Exception e)
                {
                    await ReplyAsync($"RoS Can not be closed right now {Context.Message.Author.Mention}, {e.GetType().Name}");
                }
            }
    
        }

        [Command("IsTCAlive")]
        public async Task CheckTc()
        {
            string aaa = "API says : " + await GlobalVars.TC.CheckIfServerIsDead();
            Process[] processes = Process.GetProcessesByName("TC");
            if (processes.Length > 0)
                aaa += "\nIs TC.exe active : True";
            else
                aaa += "\nIs TC.exe active : False";

            aaa += "\nAPI last checked : " + (GlobalVars.TC.lastTimeChecked.TimeOfDay.Minutes - DateTime.Now.Minute) + " minutes ago";
            await ReplyAsync(aaa + "\nNext API check : In " + ((GlobalVars.TC.lastTimeChecked.TimeOfDay.Minutes + 2.5) - DateTime.Now.Minute) + " minutes");
        }
        
        [Command("IsRoSAlive")]
        public async Task CheckRoS()
        {
            string aaa = "API says : " + await GlobalVars.RoS.CheckIfServerIsDead();
            Process[] processes = Process.GetProcessesByName("RoS");
            if (processes.Length > 0)
                aaa += "\nIs RoS.exe active : True";
            else
                aaa += "\nIs RoS.exe active : False";

            aaa += "\nAPI last checked : " + (GlobalVars.RoS.lastTimeChecked.TimeOfDay.Minutes - DateTime.Now.Minute) + " minutes ago";
            await ReplyAsync(aaa + "\nNext API check : In " + ((GlobalVars.RoS.lastTimeChecked.TimeOfDay.Minutes + 2.5) - DateTime.Now.Minute) + " minutes");
        }

        [Command("killTerr")]
        public async Task KillT()
        {
            if (Context.Message.Author.Id == 273470797916405770 || Context.Message.Author.Id == 142300409002852352)
            {
                if (GlobalVars.TerrariaPid != 0)
                {
                    Process proc = Process.GetProcessById(GlobalVars.TerrariaPid);
                    proc.Kill();
                    GlobalVars.TerrariaPid = 0;
                }
                else
                {
                    try
                    {
                        Process[] proc = Process.GetProcessesByName("tModLoaderServer");
                        proc[0].Kill();
                        await ReplyAsync($"Done {Context.Message.Author.Mention}");
                    }
                    catch
                    {
                        await ReplyAsync($"Could not do (might already be closed) {Context.Message.Author.Mention}");
                    }
                }
            }
        }
        
        [Command("TCcoins")]
        [RequireOwner]
        private async Task TradingCoins(string userName, int amount)
        {
            //"/rcon CPlayer@ p = getPlayerByUsername('Vamist');p.server_setCoins(p.getCoins()+50);";
            IUserMessage response = await ReplyAsync("Please check that fughorn is connect to TC, otherwise your gold may be lost. " +
                                                     "\nAdd a tick emote if you are sure its connected"+
                                                     "\nNo refunds will be given for your mistakes. If it is ours, we will refund it. (Tell Vamist)");
            await response.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
            for (int a = 0; a < 100; a++)
            {
                var ticked = await response.GetReactionUsersAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                foreach (IUser user in ticked)
                {
                    if (user.Id == Context.User.Id)
                    {
                        await response.DeleteAsync();
                        a = 100;
                        break;
                    }   
                }    
                if (a == 99)
                {
                    await ReplyAsync("Airdrop request removed");
                    await response.DeleteAsync();
                    return;
                }
            }
            Sql sql = new Sql();
            var items = sql.SqlReader(Context.Message.Author.Id);
            if (amount > 30000)
            {
                await ReplyAsync("Make sure the amount you trade is less then 30,000");
                return;
            }
            if (items.goldCoins < amount)
            {
                await ReplyAsync("You don't have enough coins to transfer.");
                return;
            }
            await ServerStat("https://api.kag2d.com/v1/game/thd/kag/server/109.228.14.252/50309/status");
            if (ServerStatus.Playerlist.Contains(userName))
            {
                sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,0,0,0,-amount);
                await Context.Guild.GetTextChannel(399975541865840640).SendMessageAsync("!rcon /CPlayer@ p = getPlayerByUsername('"+ userName +"');p.server_setCoins(p.getCoins()+"+amount+");");
                await ReplyAsync("Done! "+amount+" of gold coins given to "+userName);   
            }
            else
            {
                await ReplyAsync("User not found. Check current users ingame using !tc. It is case sensative.");
            }
            
        }


        private async void Airdrops(string userName, [Remainder]string itemName)
        {
            await ReplyAsync(
                "Command removed until new list of items comes in, bugs have been fixed and other issues have been sorted");
            return;
            
            
             IUserMessage response = await ReplyAsync("Please check that fughorn is connect to TC, otherwise your gold may be lost. " +
                             "\nAdd a tick emote if you are sure its connected"+
                             "\nNo refunds will be given for your mistakes. If it is ours, we will refund it. (Tell Vamist)");
             await response.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
             for (int a = 0; a < 100; a++)
             {
                 var ticked = await response.GetReactionUsersAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
                 foreach (IUser user in ticked)
                 {
                     if (user.Id == Context.User.Id)
                     {
                         await response.DeleteAsync();
                         a = 100;
                         break;
                     }   
                 }    
                 if (a == 99)
                 {
                     await ReplyAsync("Airdrop request removed");
                     await response.DeleteAsync();
                     return;
                 }
             }
             //await Context.Message.AddReactionAsync(Emote.Parse("<:kag_agree:399994688578256906>"));
             Sql sql = new Sql();
             var items = sql.SqlReader(Context.Message.Author.Id);
             string itemRequest = "";
             
             switch (itemName.ToLower())
             {
                     case "pigger":
                     case "piggy":
                     case "pig":
                     case "piglet":
                         itemRequest = "piglet";
                         break;
                     
                     case "assult rifle":
                     case "rifle":
                     case "assultrifle":
                     case "assultrifl":
                         itemRequest = "assaultrifle";
                         break;
                     
                     case "jug":
                     case "jugger":
                     case "juggernaught":
                     case "hammer":
                     case "juggernaught hammer":
                     case "jugg hammer":
                     case "jug hammer":
                         itemRequest = "juggernauthammer";
                         break;
                     
                     case "blaster":
                     case "blast":
                     case "blazter":
                         itemRequest = "blaster";
                         break;
                     
                     case "slave kit":
                     case "slave":
                     case "slav kit":
                     case "kit":
                     case "slavkit":
                     case "slavekit":
                     case "shackle":
                     case "shackles":
                         itemRequest = "shackles";
                         break;
                     
                     case "bike":
                     case "hoverbike":
                     case "hover":
                     case "hover bike":
                         itemRequest = "hoverbike";
                         break;
                         
                     default:
                         itemRequest = "";
                         break;
             }
 
             if (itemRequest != "")
             {
                 switch(itemRequest)
                 {
                     case"piglet":
                         if (items.goldCoins < 200)
                         {
                             await ReplyAsync("Can't allow you to buy a air drop. Not enough coins for a "+ itemName);
                         }
                         else
                         {
                             await BuyingObject(200, userName, itemRequest);
                         }
                         break;
                     
                     case"assaultrifle":
                         if (items.goldCoins < 400)
                         {
                             await ReplyAsync("Can't allow you to buy a air drop. Not enough coins for a " + itemName);
                         }
                         else
                         {
                             await BuyingObject(400, userName, itemRequest);
                         }
                         break;
                     
                     case"juggernauthammer":
                         if (items.goldCoins < 10000)
                         {
                             await ReplyAsync("Can't allow you to buy a air drop. Not enough coins for a " + itemName);
                         }
                         else
                         {
                             await BuyingObject(10000, userName, itemRequest);
                         }
                         break;
                     
                     case"blaster":
                         if (items.goldCoins < 1000)
                         {
                             await ReplyAsync("Can't allow you to buy a air drop. Not enough coins for a " + itemName);
                         }
                         else
                         {
                             await BuyingObject(1000, userName, itemRequest);
                         }
                         break;
                     
                     case"shackles":
                         if (items.goldCoins < 500)
                         {
                             await ReplyAsync("Can't allow you to buy a air drop. Not enough coins for a " + itemName);
                         }
                         else
                         {
                             await BuyingObject(500, userName, itemRequest);
                         }
                         break;
                     
                     case"hoverbike":
                         if (items.goldCoins < 10000)
                         {
                             await ReplyAsync("Can't allow you to buy a air drop. Not enough coins for a " + itemName);
                         }
                         else
                         {
                             await BuyingObject(10000, userName, itemRequest);
                         }
                         break;
                          
                 }
             }
             else
             {
                 await ReplyAsync("Make sure you request the correct object. Type !buy to get the list " +
                                  "\nThat or make sure the order goes !buy username ObjectName");
             }
        }

        public async Task BuyingObject(int cost, string userName, string itemRequest)
        {
            Sql sql = new Sql();
            await ServerStat("https://api.kag2d.com/v1/game/thd/kag/server/109.228.14.252/50309/status");
            if (ServerStatus.Playerlist.Contains(userName))
            {
                sql.SqlExpriance(Context.Message.Author.Id,Context.Message.Author.Username,0,0,0,0,0,0,-400);
                await Context.Guild.GetTextChannel(399975541865840640).SendMessageAsync("!airdrop "+itemRequest+" "+userName);
                await ReplyAsync("Done, Air drop should be coming to " + userName);   
            }
            else
            {
                await ReplyAsync("User not found. Check current users ingame using !tc. It is case sensative.");
            }
        }
        
        [Command("Buy")]
        public async Task BuyingTest(string userName, [Remainder] string itemName)
        {
            await Task.Delay(1);
            ThreadStart task1 = () => Airdrops(userName, itemName);
            Thread t1 = new Thread(task1);
            t1.Start();
        }



        [Command("runTerr")]
        public async Task RunT()
        {
            if (Context.Message.Author.Id == 273470797916405770 || Context.Message.Author.Id == 142300409002852352)
            {
                try
                {
                    ProcessStartInfo terr = new ProcessStartInfo
                    {
                        FileName = @"",//file location
                        Arguments = "-config someserverconfig.txt",
                        UseShellExecute = true,
                        CreateNoWindow = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    };
                    Process x = Process.Start(terr);
                    GlobalVars.TerrariaPid = x.Id;
                    await ReplyAsync("Done " + Context.Message.Author.Mention);
                }
                catch
                {
                    await ReplyAsync("Could not do that, not sure why..");
                }
            }
        }
        
        
        

        [Command("test")]
        public async Task Test([Remainder]IUser user)
        {
            await ReplyAsync(user.Id.ToString());
        }

       /* [Command("syncInvo")]
        [RequireOwner]
        public async Task UpdateInvo()
        { 
            if(File.Exists(GlobalVars.JsonOldPath + Context.Message.Author.Id.ToString() + ".json"))
                await ReplyAsync(Json.JsonSync(Context.Message.Author.Id, Context.Message.Author.Username));
            else
                await ReplyAsync("Old invo file not found, if you have used this command before, the file has been moved to an backup location.");
        }*/

        [Command("backup")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task BackUpJsons()
        {
            try
            {
                Sql sql = new Sql();
                sql.SqlBackUp();
                await ReplyAsync("Done");
            }
            catch(Exception e)
            {
                await ReplyAsync("Failed. Check console for errors");
                Console.WriteLine(e);
            }
            
        }
        
        [GodCommands("!removePrivateChannel channelID","Remove the channel from the ignore channel list.")]
        [Command("removeprivatechannel")]
        [Alias("rpc")]
        public async Task removePriavteChannel(ulong id)
        {
            
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }

            if (allowed)
            {
                GlobalVars.Ic.RemoveChannel(id);
                GlobalVars.Ic.SaveIgnoreList();
                await ReplyAsync("Done");
            }
            else
            {
                await ReplyAsync("You can not do this");
            }
        }


        [Command("warcleanup")]
        public async Task CleanUpTheMess()
        {
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }

            if (allowed)
            {
                await ReplyAsync("Working on cleaning up, commands wont work until i'm done");
                var a = Context.Guild.Channels;
                Emote eaturgreens = Emote.Parse("<:eaturgreens:483262810730659841>");
                foreach (SocketGuildChannel channel in a)
                {
                    ISocketMessageChannel chan = (ISocketMessageChannel) channel;
                    var messages = await chan.GetMessagesAsync(100).FlattenAsync();
                    Console.WriteLine("done with " + Context.Channel);
                    foreach (IMessage message in messages)
                    {
                        IUserMessage mess = (IUserMessage) message;
                        IReadOnlyCollection<IUser> reactionsAdded = await mess.GetReactionUsersAsync(eaturgreens);
                        foreach (IUser userr in reactionsAdded)
                        {
                            await mess.RemoveReactionAsync(eaturgreens, userr);
                        }
                        
                    }
                }

                await ReplyAsync("done");
            }
        }


        [GodCommands("!addPrivateChannel channelID","Make a channel immune to bot logging")]
        [Command("addprivatechannel")]
        [Alias("apc")]
        public async Task addPriavteChannel(ulong id)
        {
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }

            if (allowed)
            {
                GlobalVars.Ic.AddChannel(id);
                GlobalVars.Ic.SaveIgnoreList();
                await ReplyAsync("Done");
            }
            else
            {
                await ReplyAsync("You can not do this");
            }
        }
        
        
        

       /* [Command("migrate")] //dont need for now
        [RequireOwner]
        public async Task MigrateJsonToSql()
        {
            try//remeber to change .remove length on if ur testing ok?
            {
                string path = GlobalVars.JsonCurrentPath;
                foreach (string file in Directory.GetFiles(path))
                {
                    string id = file.Remove(0,56);
                    id = id.Remove(18);
                    var items = Json.JsonReturnTuple(ulong.Parse(id));
                    Sql sql = new Sql();
                    sql.SqlExpriance(ulong.Parse(id),null,0,0,items.bombs,items.fire_arrows,items.saws,items.gold_coins);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await ReplyAsync("failed, check console");
                throw;
            }

            await ReplyAsync("done");
        }*/

        /*[Command("Blacklist")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task GiveBlacklist(string user = null)
        {
            sbyte number = 0;
            string output = "";
            string temp = "";
            bool startOutpt = false;
            List<string> userList = new List<string>();
            List<string> banList = new List<string>();
            List<string> ipBanList = new List<string>();
            List<string> banTime = new List<string>();
            List<string> banReason = new List<string>();
            string[] lines = File.ReadAllLines(@"D:\TempKAG\King Arthur's Gold - TC\Security\blacklist.cfg");
            foreach (string line in lines)
            {
                if (line.Contains("blacklist ="))
                {
                    startOutpt = true;
                }
                if (startOutpt == true)
                {
                    foreach (char currentChar in line)
                    {
                        if (currentChar == ';')
                        {
                            if (number == 0)
                            {
                                userList.Add(output);
                                temp = output;
                                output = "";
                            }
                            if(number == 1)
                            {
                                ipBanList.Add(output);
                                temp += output;
                                output = "";
                            }
                            if(number == 2)
                            {
                                banTime.Add(output);
                                temp += output;
                                output = "";
                            }
                            if (number == 3)
                            {
                                banReason.Add(output);
                                temp += output;
                                banList.Add(temp);
                                output = "";
                                temp = "";
                                number = -1;
                            }
                            number++;
                        }
                        else if (currentChar == '	')
                        {
                            //ignore
                        }
                        else
                        {
                            output += currentChar;
                        }
                        if (output == "blacklist = ")
                        {
                            output = "";//reset
                        }
                    }
                }
            }

            if (user == null)
            {
                string combindedString = string.Join( "\n", userList.ToArray() );
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithColor(12, 245, 249);
                eb.WithTitle("Current user blacklist");
                eb.WithFooter("Type !blacklist username for more detail about a user!");
                eb.AddField("User names currently banned", combindedString);
                await ReplyAsync("", false, eb.Build());
            }
            else if(userList.ToString().Contains(user))
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithColor(12, 245, 249);
                eb.WithTitle("Current user blacklist");
                eb.WithFooter("to do");
                //eb.AddField("User names currently banned", userList.ToString());
                await ReplyAsync("", false, eb.Build());
            }
                            

        }*/
        
        
        [AdminCommands("!logs Date1 Date2","Sends the logs between date 1 and date 2, date 2 can be null(so any logs created in Date1 will be sent)", true)]
        [Command("logs")]
        [Alias("log")]
        public async Task GettingLogs(string date1, string date2 = null)
        {
            
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }
            
            
            if (allowed)
            {
                DateTime dateConvert1 = Convert.ToDateTime(date1);
                if (string.IsNullOrEmpty(date2))
                {
                    date2 = DateTime.Now.Date.ToString();
                }
                DateTime dateConvert2 = Convert.ToDateTime(date2);
                string path = @"D:\TempKAG\King Arthur's Gold - TC\Logs\";
                string path2 = @"";//file location
                string path3 = @"";
                foreach (string file in Directory.GetFiles(path))
                {
                    

                    DateTime modification = File.GetLastWriteTime(file);
                    if (modification.Date >= dateConvert1 && modification.Date <= dateConvert2)
                    {
                        try
                        {
                            File.Copy(file, path2 + Path.GetFileName(file), true);
                        }
                        catch (IOException e)
                        {
                            await ReplyAsync("A file can not be added to the incoming file, its currently in use");
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e);
                            
                        }
                    }
                }
                ZipFile.CreateFromDirectory(path2, path3);

                await Context.Channel.SendFileAsync(path3, "Here you go!");
                foreach (string file in Directory.GetFiles(path2))
                {
                    File.Delete(file);
                }
                File.Delete(path3);
            }
            else
            {
                await ReplyAsync("You're not allowed to do that, sorry.");
            }
            
        }

        /*[Command("lewdemote")]
        public async Task AddLewdEmote(ulong id)
        {
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }


            if (allowed)
            {
                //SocketTextChannel castedChannel = (SocketTextChannel) Context.Channel;

                try
                {
                    SocketUserMessage message = (SocketUserMessage) Context.Channel.GetCachedMessage(id);

                    await message.AddReactionAsync((Emote.Parse("<:eaturgreens:483262810730659841>")));
                    await Context.Message.DeleteAsync();

                }
                catch (Exception e)
                {
                    //SocketUserMessage message = (SocketUserMessage) Context.Channel.GetCachedMessage(id);
                    Cacheable<IUserMessage, ulong> cache = new Cacheable<IUserMessage, ulong>();
                    SocketUserMessage message = (SocketUserMessage) cache.GetOrDownloadAsync();
                    await message.AddReactionAsync((Emote.Parse("<:eaturgreens:483262810730659841>")));
                    await Context.Message.DeleteAsync();
                }
            }
        }
*/
        
        [AdminCommands("!logs howFarBack(number)","Sends the logs from todays date to the number to specified (e.g !logs 2 will be today to 2 days ago)", true)]
        [Command("logs")]
        [Alias("log")]
        public async Task GettingLogsDate(int days = 2)
        {
            
            bool allowed = false;
            SocketGuildUser user = (SocketGuildUser) Context.Message.Author;
            foreach (SocketRole role in user.Roles)
            {
                if (TrustedAdminCheck(role.Id))
                {
                    allowed = true;
                    break;
                }
            }
            
            
            if (allowed)
            {
                DateTime dateConvert1 = DateTime.Now.Date.AddDays(-(days));
                Console.WriteLine(dateConvert1);
                string date2 = DateTime.Now.Date.ToString();
                DateTime dateConvert2 = Convert.ToDateTime(date2);
                string path = @"D:\TempKAG\King Arthur's Gold - TC\Logs\";
                string path2 = @"";
                string path3 = @"";
                foreach (string file in Directory.GetFiles(path))
                {
                    DateTime modification = File.GetLastWriteTime(file);
                    if (modification.Date >= dateConvert1 && modification.Date <= dateConvert2)
                    {
                        try
                        {
                            File.Copy(file, path2 + Path.GetFileName(file));
                            
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e);
                            await ReplyAsync("A file can not be added to the incoming file, its currently in use");
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e);
                            
                        }
                    }
                }
                ZipFile.CreateFromDirectory(path2, path3);

                try
                {
                    await Context.Channel.SendFileAsync(path3, "Here you go!");
                }
                catch (Exception e)
                {
                    await ReplyAsync("Discord says the file is too big to send.");
                    
                }
                
                foreach (string file in Directory.GetFiles(path2))
                {
                    
                    File.Delete(file);
                }
                File.Delete(path3);
            }
            else
            {
                await ReplyAsync("You're not allowed to do that, sorry.");
            }
            
        }


        //
        //
        //
        //
        //Reading and making and saving files
        //
        //
        //
        //

        //save json

        //save array list here
        //[0] - bombs
        //[1] - fire_arrows
        //[2] - saws
        //[3] - gold_coins

        //saves and makes if not found
        /*
        static public void Saveing(string username, string num, string obj, int arraynum, bool replace)
        {


            string path = @".\data\" + username + num + ".txt";
            start:
            if (File.Exists(path))
            {
                if (replace == false)
                {
                    string[] temp = File.ReadAllLines(path);
                    int temp2 = int.Parse(temp[arraynum]);
                    int temp3 = int.Parse(obj);
                    int temp4 = temp2 + temp3;
                    temp[arraynum] = temp4.ToString();
                    File.WriteAllLines(path, temp);
                }
                else if (replace == true)
                {
                    string[] temp = File.ReadAllLines(path);
                    temp[arraynum] = obj;
                    File.WriteAllLines(path, temp);
                }
            }
            else
            {
                string[] temp = { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                File.WriteAllLines(path, temp);
                goto start;
            }

        }*/

        //Contact KAG API
        //TODO fix characters having an effect on the outputted code. Remove any discord markdown on usernames



        public async Task getAlotOfServers()
        {
            //https://api.kag2d.com/v1/game/thd/kag/servers?filters=[{%E2%80%9Cfield%E2%80%9D:%20%E2%80%9CcurrentPlayers%E2%80%9D,%20%E2%80%9Cop%E2%80%9D:%20%E2%80%9Cge%E2%80%9D,%20%E2%80%9Cvalue%E2%80%9D:%201},{%E2%80%9Cfield%E2%80%9D:%20%E2%80%9ClastUpdate%E2%80%9D,%20%E2%80%9Cop%E2%80%9D:%20%E2%80%9Cge%E2%80%9D,%20%E2%80%9Cvalue%E2%80%9D:%222018-06-18%22}]
        }


        public static async Task McServerStat(string apiLink)
        {
            await Task.Delay(1);
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(apiLink);
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
                dynamic results = JsonConvert.DeserializeObject(reponse);
                ServerStatus.McPlayerCount = results.players.online;
                ServerStatus.Port = results.port;
                ServerStatus.Ip = results.ip;
                if (ServerStatus.McPlayerCount == 0)
                {
                    ServerStatus.McPlayersPlaying = "No players are playing";
                }
                else
                {
                    ServerStatus.McPlayersPlaying = null;
                    var a = results.players.list;
                    foreach (string player in a)
                    {
                        if (player != null)
                        {
                            ServerStatus.McPlayersPlaying += player + '\n';
                        }
                    }

                    if (ServerStatus.McPlayersPlaying == null)
                    {
                        ServerStatus.McPlayersPlaying = "No players are currently playing";
                    }
                    
                }
            }
            newStream.Close();
            getreponse.Close();
        }
        
        
        
        [AdminCommands("!kagChange kagName newName","Server status will change your kag name to the new name",true)]
        [Command("kagChange")]
        public async Task nameChange(string kagName, [Remainder] string newName)
        {

            if (GlobalVars.KagNameList.AddNewName(kagName, newName))
            {
                await ReplyAsync("Done!");
            }
            else
            {
                await ReplyAsync("Something went wrong, bad vam");
            }
        }
        
        public static async Task ServerStat(string apiLink)
        {
            await Task.Delay(1);
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(apiLink);
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
                dynamic results = JsonConvert.DeserializeObject(reponse);
                ServerStatus.Day = results.serverStatus.DNCycle;
                ServerStatus.CurrentPlayers = 0;
                ServerStatus.Lastupdate = results.serverStatus.lastUpdate;
                ServerStatus.Name = results.serverStatus.name;
                ServerStatus.Ip = results.serverStatus.IPv4Address;
                ServerStatus.Port = results.serverStatus.port;
                ServerStatus.Playerlist = null;//todo Clears list so names dont repeat
                List<string> newList = new List<string>();
                var a = @results.serverStatus.playerList;
                foreach (string obj in a)
                {
                    if (obj != null)
                    {
                        /*switch (obj.ToString())
                        {
                            case "digga":
                                ServerStatus.Playerlist += "\nOverlady Rajang"; 
                                break;

                            case "TFlippy":
                                ServerStatus.Playerlist += "\n" + @obj + " the Great";
                                break;

                            case "Vamist":
                                ServerStatus.Playerlist += "\nNot " + @obj;
                                break;
                            
                            case "Pirate-rob":
                                ServerStatus.Playerlist += "\n" + @obj + " will rob you";
                                break;

                            default:
                                ServerStatus.Playerlist += "\n" + @obj;
                                if (obj.Contains('*') || obj.Contains('_') || obj.Contains('~'))
                                {
                                    string temp = @obj;
                                    temp = @temp.Replace('*', ' ');
                                    temp = @temp.Replace('_', ' ');
                                    temp = @temp.Replace('~', ' ');
                                }
                                break;
                        }*/
                        string charName = GlobalVars.KagNameList.GetName(obj.ToString());
                        charName.Replace("*", @"\*");
                        charName.Replace("~", @"\~");
                        charName.Replace("_", @"\_");
                        //ServerStatus.Playerlist += "\n" + charName;
                        newList.Add("\n"+charName);
                            

                        ServerStatus.CurrentPlayers += 1;
                    }
                    else
                    {
                        ServerStatus.Playerlist = "No players";
                    }
                }
                newList.Sort();
                foreach (string text in newList)
                {
                    ServerStatus.Playerlist += text;
                }
            }
            newStream.Close();
            getreponse.Close();
        }
    }
}
