using System;
using System.IO;
using Newtonsoft.Json;

using Newtonsoft.Json.Converters;    



namespace DiscordBot.Classes
{
    static class JsonNotUsed
    {
        //used to store everything that i do with jsons, may change later.
        //Need to improve if(file.exist part becuase its way to overkill.


        public static (int bombs, int fire_arrows, int saws, int gold_coins) JsonReturnTuple(ulong id)
        {
            if (File.Exists(GlobalVars.JsonCurrentPath + id + ".json") == false)
                return (0, 0, 0, 0);

            int bombs;
            int fireArrows;
            int saws;
            int goldCoins;
            
            JsonSerializer serializer = new JsonSerializer();
            using (StreamReader sr = new StreamReader(GlobalVars.JsonCurrentPath + id + ".json"))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                dynamic results = serializer.Deserialize(reader);
                bombs = results.Bombs;
                fireArrows = results.Fire_arrows;
                saws = results.Saws;
                goldCoins = results.Gold_coins;
            }

            return (bombs, fireArrows, saws, goldCoins);

        }
        
        //improve below, very badly done >.<
        public static void JsoNsave(ulong id, string name, int bombs = 0, bool pos1 = true, int fireArrows = 0, bool pos2 = true, int saws = 0, bool pos3 = true, int goldCoins = 0, bool pos4 = true)
        {
            JsonSerializer serializer = new JsonSerializer();
            string notes = "";
            if(File.Exists(GlobalVars.JsonCurrentPath + id + ".json") == false)
            {
                int bomb = 0;
                int fire = 0;
                int saw = 0;
                int gold = 0;
                Obj items = new Obj
                {
                    Id = id,
                    Name = name,
                    Bombs = bomb,
                    FireArrows = fire,
                    Saws = saw,
                    GoldCoins = gold,
                    NotesFromOwner = notes
                };

                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(GlobalVars.JsonCurrentPath + id + ".json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    serializer.Serialize(writer, items);
                }
            }

            try
            {
                using (StreamReader sr = new StreamReader(GlobalVars.JsonCurrentPath + id + ".json"))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    dynamic results = serializer.Deserialize(reader);
                    id = results.Id;
                    name = results.Name;

                    if (pos1 == false)
                        bombs = results.Bombs - bombs;
                    else
                        bombs = results.Bombs + bombs;

                    if (pos2 == false)
                        fireArrows = results.Fire_arrows - fireArrows;
                    else
                        fireArrows = results.Fire_arrows + fireArrows;

                    if (pos3 == false)
                        saws = results.Saws - saws;
                    else
                        saws = results.Saws + saws;

                    if (pos4 == false)
                        goldCoins = results.Gold_coins - goldCoins;
                    else
                        goldCoins = results.Gold_coins + goldCoins;

                    notes = results.NotesFromOwner;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Obj product = new Obj
            {
                Id = id,
                Name = name,
                Bombs = bombs,
                FireArrows = fireArrows,
                Saws = saws,
                GoldCoins = goldCoins,
                NotesFromOwner = notes
            };

            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(GlobalVars.JsonCurrentPath + id + ".json")) 
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, product);
            }
        }

        public static string JsonSync(ulong id, string name)
        {
            JsonSerializer serializer = new JsonSerializer();
            int Bombs;
            int Fire_arrows;
            int Gold_coins;
            int Saws;

            try
            {
                using (StreamReader sr = new StreamReader(GlobalVars.JsonOldPath + id + ".json"))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    dynamic results = serializer.Deserialize(reader);
                    if (results.Bombs == null)
                    {
                        Bombs = results.bombs;
                        Fire_arrows = results.fire_arrows;
                        Gold_coins = results.gold_coins;
                        Saws = results.saws;
                        Console.WriteLine("old");
                    }
                    else
                    {
                        Bombs = results.Bombs;
                        Fire_arrows = results.Fire_arrows;
                        Gold_coins = results.Gold_coins;
                        Saws = results.Saws;
                        Console.WriteLine("new" + Gold_coins);
                    }
                }
                File.Move(GlobalVars.JsonOldPath + id + ".json", GlobalVars.JsonDeletedPath + id + ".json");
                JsoNsave(id, name, Bombs, true, Fire_arrows, true, Saws, true, Gold_coins);
                return "Was able to sync correctly.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Was not able to do sync, check console output.";
            }

        }

        public static string JsonNote(string text, ulong id)
        {
            JsonSerializer serializer = new JsonSerializer();
            string name;
            int bombs;
            int fireArrows;
            int goldCoins;
            int saws;
            try
            {
                using (StreamReader sr = new StreamReader(GlobalVars.JsonCurrentPath + id + ".json"))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    dynamic results = serializer.Deserialize(reader);
                    id = results.Id;
                    name = results.Name;
                    bombs = results.Bombs;
                    fireArrows = results.Fire_arrows;
                    saws = results.Saws;
                    goldCoins = results.Gold_coins;
                    //results.NotesFromOwner;
                }
            }
            catch
            {
                return "Unable to load pre-existing json file";
            }

            Obj product = new Obj
            {
                Id = id,
                Name = name,
                Bombs = bombs,
                FireArrows = fireArrows,
                Saws = saws,
                GoldCoins = goldCoins,
                NotesFromOwner = text
            };

            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(GlobalVars.JsonCurrentPath + id + ".json")) 
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, product);
            }
            return "Was able to add \"" + text + " to " + name;
        }

    }
}


