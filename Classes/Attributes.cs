using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DiscordBot.Classes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NormalCommands  : Attribute
    {
        public string Name {get; }
        public string Desc { get; }
        public NormalCommands(string name, string desc)
        {
            Name = name;
            Desc = desc;
        }

    }


    [AttributeUsage((AttributeTargets.Method))]
    public class AdminCommands : Attribute
    {
        public string Name;
        public string Desc;
        public bool SuperAdminOnly;
        public AdminCommands(string name, string desc, bool superAdminOnly = false)
        {
            Name = name;
            Desc = desc;
            SuperAdminOnly = superAdminOnly;
        }
    }
    
    [AttributeUsage((AttributeTargets.Method))]
    public class GodCommands : Attribute
    {
        public string Name;
        public string Desc;
        public GodCommands(string name, string desc)
        {
            Name = name;
            Desc = desc;
        }
    }
    
}