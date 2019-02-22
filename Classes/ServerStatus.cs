using System;

namespace DiscordBot.Classes
{
    class ServerStatus
    {
        public static int CurrentPlayers { get; set; }
        public static bool Day { get; set; }
        public static DateTime Lastupdate { get; set; }
        public static string Name { get; set; }
        public static string Ip { get; set; }
        public static int Port { get; set; }
        public static string Playerlist { get; set; }
        
        public static string McPlayersPlaying { get; set; }
        public static int McPlayerCount { get; set; }
    }
    
}
