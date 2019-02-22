using System;
using System.Collections.Generic;
using System.Reflection;
using DiscordBot.Commands;
using DiscordBotAttempt10000.Classes;

namespace DiscordBot.Classes
{
    static class GlobalVars
    {
        public static int Bombs { get; set; }
        public static string LastBombUser { get; set; }
        public static int Firearrows { get; set; }
        public static string LastFireArrowUser { get; set; }
        public static int Saws { get; set; }
        public static string LastSawUser { get; set; }
        public static int Goldcoins { get; set; }
        public static string LastGoldUser { get; set; }
        public static bool Spawntrue { get; set; }
        public static string PlayersPlaying { get; set; }
        public static string ServerName { get; set; }
        public static string Players { get; set; }
        public static int Caltrops { get; set; }
        

        public static readonly Random Rnum = new Random();
        
        public static int TerrariaPid { get; set; }

        public const string JsonCurrentPath = @"";       
        public const string JsonOldPath = @"";   
        public const string JsonDeletedPath = @"";      
        public const string JsonBackUpLocation = @"";    

        public const string SqlFileLocation = @""; 
        public const string SqlBackupLocation = "";
        
        
        public static ulong[] ModeratorRanks = {381131703042965504, 361262968199184397 };
        public static ulong[] AdminRanks = {364040665149079564,474289311206866944};
        public const ulong TrustedAdminId = 474289311206866944;
        public const ulong OwnerId = 361262801387520000;

        public static ulong[] AllAdminRanks =
            {381131703042965504, 361262968199184397, 364040665149079564, 474289311206866944, 361262801387520000};
        
        public static ulong[] PrivateDiscordChannel = {458292308325040128, 381190850157215755, 385864005480349706,
            439908706373599245, 419506659476504577, 416946867117621263, 474303759074459678, 476460822604808223};
        
        public static JsonItems Items { get; set; }
        public static IgnoreChannels Ic = new IgnoreChannels(@"");
        
        public static List<string> DiscordGroupNames = new List<string>();
        //public static AutoRestartTC AutoTC = new AutoRestartTC();
        public static AutoRestart TC = new AutoRestart(@"D:\TempKAG\King Arthur's Gold - TC\","TC.exe","","50309");
        public static AutoRestart RoS = new AutoRestart(@"D:\TempKAG\ROS\King Arthur's Gold - rosIsBack\","RoS.exe","","50314");
        public static AutoRestart WW2 = new AutoRestart(@"","WW2.exe","","50303");
        public static NamesToNames KagNameList = new NamesToNames();


    }
}
