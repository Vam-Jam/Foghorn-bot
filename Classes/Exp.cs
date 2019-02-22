using System;

namespace DiscordBot.Classes
{
    internal struct Exp // internal means this program can only use this? or something along the lines, struct is for items such as this.
    {
        public Int32 MessageSentTotal { get; set; }
        public Int32 Xp { get; set; }
        public Int32 Level { get; set; }
    } 

}
