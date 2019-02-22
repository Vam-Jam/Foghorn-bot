using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace DiscordBot.Classes
{
    public class Sql
    {
        public void SqlExpriance(ulong id, string username,int messageUp = 0, int expGained = 0, int expLost = 0, int bombs = 0, int fireArrows = 0, int saws = 0, int goldCoins = 0)//improve later
        {
            //SQLiteConnection.CreateFile("D:\\Temp\\Database\\MyDatabase.sqlite"); //use to make the database

            try
            {
                SQLiteConnection sqLiteConn =
                    new SQLiteConnection("Data Source = " + GlobalVars.SqlFileLocation + "Exp.sqlite;Version=3;");
                sqLiteConn.Open();

                SQLiteCommand
                    tweaks = new SQLiteCommand("PRAGMA synchronous = OFF",
                        sqLiteConn); //helps with lower latnecy at the cost of corrupted data if os crashes
                tweaks.ExecuteNonQuery();

                //string Command = "CREATE TABLE EXP (UserID TEXT PRIMARY KEY, Username TEXT, Level INT, Experience INT, MessagesSent INT, Bombs INT, FireArrows INT, Saws INT, GoldCoins INT)"; //Making a new table
                //SQLiteCommand exeCommand = new SQLiteCommand(Command, _sqLiteConn);//Sending the command
                //exeCommand.ExecuteNonQuery(); //Send!
                username = username.Replace('\'', ' ').Replace('"', ' ');

                String iD = null;
                int level = 0;
                int experience = 0;
                int messagesSent = 0;
                int tempBomb = 0; //work around, using bomb makes it return null even though its set to 0
                int tempFireArrows = 0; //even if nothing is there, but this works for now
                int tempSaws = 0;
                int tempGoldCoins = 0;

                try
                {
                    string getData = "SELECT * FROM EXP WHERE UserID ='" + id + "'";
                    SQLiteCommand returnData = new SQLiteCommand(getData, sqLiteConn);
                    SQLiteDataReader reader = returnData.ExecuteReader();
                    while (reader.Read())
                    {
                        iD = reader["UserID"].ToString();
                        level = int.Parse(reader["Level"].ToString());
                        experience = int.Parse(reader["Experience"].ToString());
                        messagesSent = int.Parse(reader["MessagesSent"].ToString());
                        tempBomb = int.Parse(reader["Bombs"].ToString());
                        tempFireArrows = int.Parse(reader["FireArrows"].ToString());
                        tempSaws = int.Parse(reader["Saws"].ToString());
                        tempGoldCoins = int.Parse(reader["GoldCoins"].ToString());
                    }

                    if (string.IsNullOrEmpty(iD))
                        throw new Exception();
                    string upd =
                        $"Update EXP Set Username = '{username}',Experience = {experience + expGained - expLost},MessagesSent = {messagesSent + messageUp},Bombs = {bombs + tempBomb},FireArrows = {fireArrows + tempFireArrows},Saws = {saws + tempSaws},GoldCoins = {goldCoins + tempGoldCoins}  Where UserID = '" +
                        id + "'";
                    SQLiteCommand sqlCommand = new SQLiteCommand(upd, sqLiteConn);
                    sqlCommand.ExecuteNonQuery();
                }
                catch
                {
                    string sql =
                        $"Insert into EXP (UserID,Username,Level, Experience, MessagesSent,Bombs,FireArrows,Saws,GoldCoins) values ('{id}','{username}',{level}, {experience}, {messagesSent + 1},{bombs},{fireArrows},{saws},{goldCoins})";
                    SQLiteCommand liteCommand = new SQLiteCommand(sql, sqLiteConn);
                    liteCommand.ExecuteNonQuery();
                }
                finally
                {
                    sqLiteConn.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
            
            //string sql = "Insert into EXP (UserID,Level, Experience, MesssagesSent) values ("+id+","+Level+", "+(Experience +1)+","+(messagesSent+1)+")";
            //SQLiteCommand liteCommand = new SQLiteCommand(sql, _sqLiteConn);
            //liteCommand.ExecuteNonQuery();
            
            /*string getData = "select * from Exp order by UserID desc";
            SQLiteCommand returnData = new SQLiteCommand(getData,_sqLiteConn);
            SQLiteDataReader reader = returnData.ExecuteReader();
            while(reader.Read())
                Console.WriteLine(reader["UserID"] + " || " + reader["ExpCollected"]);*/

        }
        
        
        public (int message,int exp,int level, int bombs, int fire_arrows, int saws, int goldCoins) SqlReader(ulong id)
        {
            SQLiteConnection sqLiteConn = new SQLiteConnection("Data Source ="+GlobalVars.SqlFileLocation+"Exp.sqlite;Version=3;");
            sqLiteConn.Open();
            
            //string Command = "CREATE TABLE EXP (UserID TEXT PRIMARY KEY, Level INT, Experience INT, MesssagesSent INT)"; //Making a new table
            //SQLiteCommand exeCommand = new SQLiteCommand(Command, _sqLiteConn);//Sending the command
            //exeCommand.ExecuteNonQuery(); //Send!
            
            int level = 0;
            int experience = 0;
            int messagesSent = 0;
            int bomb = 0;
            int fireArrows = 0;
            int saws = 0;
            int goldCoins = 0;

            try
            {
                string getData = "SELECT * FROM EXP WHERE UserID ='" + id + "'";
                SQLiteCommand returnData = new SQLiteCommand(getData, sqLiteConn);
                SQLiteDataReader reader = returnData.ExecuteReader();
                while (reader.Read())
                {
                    level = int.Parse(reader["Level"].ToString());
                    experience = int.Parse(reader["Experience"].ToString());
                    messagesSent = int.Parse(reader["MessagesSent"].ToString());
                    bomb = int.Parse(reader["Bombs"].ToString());
                    fireArrows = int.Parse(reader["FireArrows"].ToString());
                    saws = int.Parse(reader["Saws"].ToString());
                    goldCoins = int.Parse(reader["GoldCoins"].ToString());
                    
                }
                sqLiteConn.Close();
            }
            catch
            {
                sqLiteConn.Close();
                return (0, 0, 0, 0, 0, 0, 0);
            }

            
            return (messagesSent, experience, level, bomb,fireArrows,saws,goldCoins);

        }

        public void SqlBackUp()
        {
            /*string str ="BACKUP DATABASE ["+GlobalVars.SqlFileLocation+"Exp.sqlite] TO  DISK = '"+ DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_T_"+ DateTime.Now.Hour + "_" + DateTime.Now.Minute + ".bak'";
            SQLiteConnection sqLiteConn = new SQLiteConnection("Data Source ="+GlobalVars.SqlFileLocation+"Exp.sqlite;Version=3;");
            sqLiteConn.Open();
            
            SQLiteCommand liteCommand = new SQLiteCommand(str, sqLiteConn);
            liteCommand.ExecuteNonQuery();
            sqLiteConn.Close();*/
            
            using(SQLiteConnection source = new SQLiteConnection("Data Source ="+GlobalVars.SqlFileLocation+"Exp.sqlite;Version=3;"))
            using (SQLiteConnection backup = new SQLiteConnection("Data Source =" + GlobalVars.SqlBackupLocation + "Backup.sqlite;Version=3;"))
            {
                source.Open();
                backup.Open();
                source.BackupDatabase(backup,"main","main",-1,null,0);
                backup.Close();
                source.Close();
            }
        }

        public string SqlRankTuple(int loop)
        {
            SQLiteConnection sqLiteConn = new SQLiteConnection("Data Source ="+GlobalVars.SqlFileLocation+"Exp.sqlite;Version=3;");
            sqLiteConn.Open();
            var str = "";
            int i = 1;
            //List<int> topList = new List<int>();
            //List<string> nameList = new List<string>();
            try
            {
                List<DataHolder> userData = new List<DataHolder>();
                string getData = "Select * from Exp order by GoldCoins desc";
                SQLiteCommand returnData = new SQLiteCommand(getData, sqLiteConn);
                SQLiteDataReader reader = returnData.ExecuteReader();
                while (reader.Read())
                {
                    int coins = int.Parse(reader["GoldCoins"].ToString());
                    int tempBomb = int.Parse(reader["Bombs"].ToString());
                    int tempFireArrows = int.Parse(reader["FireArrows"].ToString());
                    int tempSaws = int.Parse(reader["Saws"].ToString());
                    
                    string name = reader["Username"].ToString();

                    DataHolder user = new DataHolder(coins,tempBomb,tempFireArrows,tempSaws,name);
                    if (user.TotalWealth() != 0)
                        userData.Add(user);
                }

                userData = userData.OrderByDescending(x => x.TotalWealth()).ToList();
                for (int a = 0; a < loop; a++)
                {
                    str = str + "\n" + (a + 1) + ". " + userData[a].TotalWealth() + " Total wealth : " + userData[a].Username;
                }
                
                
             
                sqLiteConn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("SQL RANK TUPLE FAILED, ERROR IS ABOVE");
                throw;
            }
            
            return str;
        }

        public string SqlDataReturn(string select, string order)
        {
            try
            {
                SQLiteConnection sqLiteConn = new SQLiteConnection("Data Source ="+GlobalVars.SqlFileLocation+"Exp.sqlite;Version=3;");
                sqLiteConn.Open();
                var str = "";
                int i = 1;
                try
                {
                    string getData = "Select "+select+" from Exp order by "+order+" desc";
                    SQLiteCommand returnData = new SQLiteCommand(getData, sqLiteConn);
                    SQLiteDataReader reader = returnData.ExecuteReader();
                    while (reader.Read())
                    {
                        int coins = int.Parse(reader[order].ToString());
                        if (coins == 0)
                        {
                        
                        }
                        else if(i < 11)
                        {
                            str = str + "\n"+i+". "+ coins +" ||  " +reader["Username"];
                            i++;
                        }
                    }
                    sqLiteConn.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                return str;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
           
            
        }
    }

    public class DataHolder
    {
        private int GoldCoins { get; }
        private int Bomb { get; }
        private int Fire { get; }
        private int Saw { get; }
        public string Username { get; }

        public DataHolder(int gold, int bomb, int fire, int saw, string username)
        {
            GoldCoins = gold;
            Bomb = bomb;
            Fire = fire;
            Saw = saw;
            Username = username;
        }

        public (int gold, int bomb, int fire, int saw, string username) ReturnAll()
        {
            return (GoldCoins, Bomb, Fire, Saw, Username);
        }

        public int TotalWealth()
        {
            int temp = GoldCoins;
            if (Bomb != 0)
                temp += (Bomb * 10);
            if (Fire != 0)
                temp += (Fire * 15);
            if (Saw != 0)
                temp += (Saw * 50);

            return temp;
        }
    }
    
}


  
/* int temp = 0;
 string tempName = "";
 for (int a = 0; a < topList.Count; a++)
 {
     for (int b = 0; b < topList.Count; b++)
     {
         if(b == topList.Count - 1 )
             break;
         if (topList[b] < topList[b + 1])
         {
             temp = topList[b + 1];
             topList[b + 1] = topList[b];
             topList[b] = temp;
             
             tempName = nameList[b + 1];
             nameList[b + 1] = nameList[b];
             nameList[b] = tempName;
             
         }
     }
 }
 for (int a = 0; a < topList.Count; a++)
 {
     str = str + "\n" + (a + 1) + ". " + topList[a] + " Total wealth : " + nameList[a];
 }*/