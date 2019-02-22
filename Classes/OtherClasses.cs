using System.Collections.Generic;
using Discord;
using Discord.WebSocket;

namespace DiscordBot.Classes
{
    public class DiscordUserHandler
    {
        public bool Bot { get; }
        public string Username { get; }
        public bool Moderator { get; }
        public bool Admin { get; }
        public bool Owner { get; }
        public bool Special { get; }
        public bool aaa { get; }
        
        public DiscordUserHandler(SocketGuildUser user)
        {
            Bot = user.IsBot;
            Username = user.Username;
            Moderator = AllAdminCheck(user.Roles);
            
        }

        internal bool AllAdminCheck(IReadOnlyCollection<SocketRole> roles)
        {
            foreach (SocketRole role in roles)
                for(byte a = 0; a <= (GlobalVars.AllAdminRanks.Length - 1); a++)
                    if (role.Id == GlobalVars.AllAdminRanks[a])
                        return true;
            return false;
        }
    }
}