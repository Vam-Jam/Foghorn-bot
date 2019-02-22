using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DiscordBot.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace DiscordBot.Commands
{
    public class Json
    {
        protected JsonSerializer Serializer { get; }
        protected string Filepath { get; }

        public Json(string filePath)
        {
            Serializer = new JsonSerializer();
            Filepath = filePath;
            
        }
    }


    public class TokenId : Json
    {
        private string Token { get; set; }

        public TokenId(string filepath) : base(filepath)
        {
            ReadToken();
        }


        public string ReturnToken()
        {
            return Token;
        }
        
        private void ReadToken()
        {
            using (StreamReader sr = new StreamReader(Filepath))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                dynamic results = Serializer.Deserialize(reader);
                Token = results.Token;
            }
        }
    }


    public class IgnoreChannels : Json
    {
        private List<ulong> IgnoreList { get; set; }

        public IgnoreChannels(string filePath) : base(filePath)
        {
            IgnoreList = new List<ulong>();
            ReadList();
        }

        public void SaveIgnoreList()
        {
            IgnoreList product = new IgnoreList
            {
                  IgnoreIds = IgnoreList.ToArray()
            };

            Serializer.Converters.Add(new JavaScriptDateTimeConverter());
            Serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(Filepath)) 
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                Serializer.Serialize(writer, product);
            }
        }

        public void ReadList()
        {
            JArray arrayList;
            using (StreamReader sr = new StreamReader(Filepath))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                dynamic results = Serializer.Deserialize(reader);
                arrayList = results.IgnoreIds;
            }

            IgnoreList = new List<ulong>(arrayList.ToObject<ulong[]>());
        }

        public List<ulong> ReturnList()
        {
            return IgnoreList;
        }

        public bool IdFoundInList(ulong id)
        {
            ulong[] array = IgnoreList.ToArray();
            foreach (ulong channelId in array)
            {
                if (channelId == id)
                    return true;

            }

            return false;
        }

        public void AddChannel(ulong id)
        {
            IgnoreList.Add(id);
        }

        public void RemoveChannel(ulong id)
        {
            IgnoreList.Remove(id);
            
        }
    }
    

    public class JsonItems : Json
    {
        public Int32 Bombs { get; set; }
        public Int32 Saws { get; set; }
        public Int32 Coins { get; set; }
        public Int32 FArrows { get; set; }
        
        
        public JsonItems(string filePath) : base(filePath)
        {
            ReadSaved();
        }

        public void SaveData(char type, Int32 amount)
        {
        }

        public void SaveAll()
        {
            Items product = new Items
            {
                Bombs = Bombs,
                FArrows = FArrows,
                Gold_coins = Coins,
                Saws = Saws
                
            };

            Serializer.Converters.Add(new JavaScriptDateTimeConverter());
            Serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(Filepath)) 
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                Serializer.Serialize(writer, product);
            }
        }


        public void ReadSaved()
        {
            using (StreamReader sr = new StreamReader(Filepath))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                dynamic results = Serializer.Deserialize(reader);
                Bombs = results.Bombs;
                FArrows = results.FArrows;
                Saws = results.Saws;
                Coins = results.Gold_coins;
            }
        }
        
        
    }
}