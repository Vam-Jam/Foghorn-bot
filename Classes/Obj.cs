using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace DiscordBot.Classes
{
    class Obj
    {
        //Items from the item drop game
        public ulong Id { get; set; }
        public string Name { get; set; }
        public int Bombs { get; set; }
        public int FireArrows { get; set; }
        public int Saws { get; set; }
        public int GoldCoins { get; set; }
        public string NotesFromOwner { get; set; }
    }

    class Items
    {
        public int Bombs { get; set; }
        public int Saws { get; set; }
        public int FArrows { get; set; }
        public int Gold_coins { get; set; }
    }

    public class LeaderboardContent
    {
        public string Username { get; set; }
        public float AvarageRating { get; set; }
        public string Content { get; set; }
    }

    public class Top5Content 
    {
        public LeaderboardContent One { get; set; }
        public LeaderboardContent Two { get; set; }
        public LeaderboardContent Three { get; set; }
        public LeaderboardContent Four { get; set; }
        public LeaderboardContent Five { get; set; }
    }

    public class IgnoreList
    {
        public ulong[] IgnoreIds { get; set; }
    }
}
