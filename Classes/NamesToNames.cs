using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiscordBot.Classes;

namespace DiscordBotAttempt10000.Classes
{
    public class NamesToNames
    {
        private List<string> KagUsername { get; }
        private List<string> NameUsername { get; set; }

        public NamesToNames()
        {
            KagUsername = new List<string>();
            NameUsername = new List<string>();
            FileToList();
        }


        private string[] ReadFile()
        {
            return File.ReadAllLines(GlobalVars.JsonCurrentPath + @"\NamesToNames.txt");     
        }

        private void FileToList()
        {
            string[] names = ReadFile();
            foreach (string line in names)
            {
                
                string firstName = "";
                string secondName = "";
                int a = 0;
                foreach (char letter in line)
                {
                    if (letter == ',')
                        a++;
                    else if (a == 0)
                        firstName += letter;
                    else
                        secondName += letter;
                }
                KagUsername.Add(firstName);
                NameUsername.Add(secondName);
            }
        }

        private void UpdateList(string kagName, string newName)
        {
            string[] names = ReadFile();
            int stringNum = 0;
            foreach (string line in names)
            {
                string firstName = "";
                string secondName = "";
                int a = 0;
                foreach (char letter in line)
                {
                    if (letter == ',')
                        a++;

                    if (a == 0)
                        firstName += letter;
                    else
                        secondName += letter;
                }

                if (firstName == kagName)
                {
                    names[stringNum] = kagName + ',' + newName;
                    break;
                }
                
                stringNum++;
            }
            File.WriteAllLines(GlobalVars.JsonCurrentPath + @"\NamesToNames.txt",names);
        }

        private bool WriteNewNames(string kagName, string fakeName)
        {
            try
            {
                File.AppendAllText(GlobalVars.JsonCurrentPath + @"\NamesToNames.txt",Environment.NewLine+kagName+","+fakeName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public bool AddNewName(string kagName, string newName)
        {
            if (string.IsNullOrEmpty(kagName) || string.IsNullOrEmpty(newName))
                return false;

            int listNum = IsNameHereAll(kagName);
            if (listNum != -1)
            {
                NameUsername[listNum] = newName;
                UpdateList(kagName,newName);
                return true;
            }

            WriteNewNames(kagName, newName);
            KagUsername.Add(kagName);
            NameUsername.Add(newName);
            return true;

        }

        public int IsNameHereAll(string kagName)
        {
            for (int a = 0; a < KagUsername.Count; a++)
            {
                if (KagUsername[a].ToLower() == kagName.ToLower())
                    return a;
            }

            return -1;
        }


        public string GetName(string kagName)
        {
            for(int a = 0; a < KagUsername.Count; a++)
            {
                if (kagName.ToLower() == KagUsername[a].ToLower())
                    return NameUsername[a];
            }

            return kagName;
        }
    }
}