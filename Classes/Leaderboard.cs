using System;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;

namespace DiscordBot.Classes
{
    public class Leaderboard
    {
        public Leaderboard()
        {
            
        }

        public void StartVote()
        {
            
        }

        public void SQL()
        {
            
        }
    }

    public class SqlLeaderboard
    {
        private SQLiteConnection sqLiteConn { get; set; }

        public SqlLeaderboard()
        {
             SqlConnect();   
        }


        public void SqlConnect()
        {
            sqLiteConn =
                new SQLiteConnection("Data Source = " + GlobalVars.SqlFileLocation + "LewdLeaderboard.sqlite;Version=3;");
            sqLiteConn.Open();

            SQLiteCommand
                tweaks = new SQLiteCommand("PRAGMA synchronous = OFF",
                    sqLiteConn); //helps with lower latnecy at the cost of corrupted data if os crashes
            tweaks.ExecuteNonQuery();
        }

        public bool SqlSearchPostId(ulong postId)
        {
            string getData = "SELECT * FROM LewdPlace WHERE PostID ='" + postId + "'";
            SQLiteCommand cmd = new SQLiteCommand(getData, sqLiteConn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            if(reader.HasRows)
                return true;
            return false;
        }

        public void SqlReactionAdded(ulong postId, byte type)
        {

            if (SqlSearchPostId(postId))
            {
                var result = SqlReadPostIdAllData(postId);
                byte num = 0;
                switch(type)
                {
                        case 1:
                            result.one += 1;
                            num = 1;
                            break;
                        
                        case 2:
                            result.two += 1;
                            num = 2;
                            break;
                        
                        case 3:
                            result.three += 1;
                            num = 3;
                            break;
                        
                        case 4:
                            result.four += 1;
                            num = 4;
                            break;
                        
                        case 5:
                            result.five += 1;
                            num = 5;
                            break;
                        
                        case 11:
                            result.one -= 1;
                            num = 1;
                            break;
                        
                        case 12:
                            result.two -= 1;
                            num = 2;
                            break;
                        
                        case 13:
                            result.three -= 1;
                            num = 3;
                            break;
                        
                        case 14:
                            result.four -= 1;
                            num = 4;
                            break;
                        
                        case 15:
                            result.five -= 1;
                            num = 5;
                            break;
                }

                result.avaragerating = AvarageTotal(result.one, result.two, result.three, result.four, result.five);

                switch (num)
                {
                        case 1:
                            SqlUpdateValue(postId, result.username, num, result.one, result.avaragerating);
                            break;
                        
                        case 2:
                            SqlUpdateValue(postId, result.username, num, result.two, result.avaragerating);
                            break;
                        
                        case 3:
                            SqlUpdateValue(postId, result.username, num, result.three, result.avaragerating);
                            break;
                        
                        case 4:
                            SqlUpdateValue(postId, result.username, num, result.four, result.avaragerating);
                            break;
                        
                        case 5:
                            SqlUpdateValue(postId, result.username, num, result.five, result.avaragerating);
                            break;
                }
            }

        }

        private float AvarageTotal(int one, int two, int three, int four, int five)
        {
            return ((5 * five ) + (4 * four ) + (3 * three ) + (2 * two ) + (1 * one )) / ((one ) + (two ) + (three ) + (four ) + (five ));
        }
        //23
        private void SqlUpdateValue(ulong postId, string username, byte number, int amount, float avarageNum)
        {
            string numToName = "One";
            if (number == 1)
                numToName = "One";
            else if (number == 2)
                numToName = "Two";
            else if (number == 3)
                numToName = "Three";
            else if (number == 4)
                numToName = "Four";
            else if (number == 5)
                numToName = "Five";
            
            string upd =
                $"Update LewdPlace Set Username = '{username}', {numToName} = '{amount}', AvarageRating = '{avarageNum}'  Where PostID = '{postId}'";
            SQLiteCommand sqlCommand = new SQLiteCommand(upd, sqLiteConn);
            sqlCommand.ExecuteNonQuery();
        }

        private void UpdateAvarage()
        {
            
        }

        public (String username, Int32 one, Int32 two, Int32 three, Int32 four, Int32 five,float avaragerating, string content) SqlReadPostIdAllData(ulong postId)
        {
            string username = "";
            Int32 one = 0;
            Int32 two = 0;
            Int32 three = 0;
            Int32 four = 0;
            Int32 five = 0;
            float avaScore = 0;
            string content = "";
            
            string getData = "SELECT * FROM LewdPlace WHERE PostID ='" +postId + "'";
            SQLiteCommand returnData = new SQLiteCommand(getData, sqLiteConn);
            SQLiteDataReader reader = returnData.ExecuteReader();
            while (reader.Read())
            {
                username = reader["Username"].ToString();
                one = int.Parse(reader["One"].ToString());
                two = int.Parse(reader["Two"].ToString());
                three = int.Parse(reader["Three"].ToString());
                four = int.Parse(reader["Four"].ToString());
                five = int.Parse(reader["Five"].ToString());
                avaScore = float.Parse(reader["AvarageRating"].ToString());
                content = reader["Content"].ToString();
              
                    
            }

            return (username, one, two, three, four, five, avaScore, content);
        }

        public void SqlAddNewPost(ulong postId, string username, string contentUrl)
        {
            string sql =
                $"Insert into LewdPlace (PostID,Username,One,Two,Three,Four,Five,AvarageRating,Content) values ('{postId}','{username}', 0, 0, 0, 0, 0, 3,'{contentUrl}')";
            SQLiteCommand liteCommand = new SQLiteCommand(sql, sqLiteConn);
            liteCommand.ExecuteNonQuery();
        }

        public void SqlClose()
        {
            sqLiteConn.Close();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        //public SqlUpdatePost(ulong postId, )
        
        public Top5Content SqlTop5()
        {
            
            string username = "";
            int upvotes = 0;
            int downvotes = 0;
            string content = "";

            string getData = "SELECT * FROM LewdPlace ORDER BY AvarageRating DESC LIMIT 5";
            SQLiteCommand returnData = new SQLiteCommand(getData, sqLiteConn);
            //SQLiteDataReader reader = returnData.ExecuteReader();
            SQLiteDataReader reader = returnData.ExecuteReader();
            //SQLiteDataAdapter da = new SQLiteDataAdapter();
            //DataTable data = new DataTable(returnData.ToString());
            //da.Fill(data);
            Top5Content T5 = new Top5Content();
            T5.One = new LeaderboardContent();
            T5.Two = new LeaderboardContent();
            T5.Three = new LeaderboardContent();
            T5.Five = new LeaderboardContent();
            T5.Four = new LeaderboardContent();

            int rowCount = 1;
            while (reader.Read())
            {
                if (rowCount == 1)
                {
                    T5.One.Username = reader["Username"].ToString();
                    T5.One.AvarageRating = float.Parse(reader["AvarageRating"].ToString());
                    T5.One.Content = reader["Content"].ToString();
                    rowCount += 1;
                }
                else if(rowCount == 2)
                {
                    T5.Two.Username = reader["Username"].ToString();
                    T5.Two.AvarageRating = float.Parse(reader["AvarageRating"].ToString());
                    T5.Two.Content = reader["Content"].ToString();
                    rowCount += 1;
                }
                else if (rowCount == 3)
                {
                    T5.Three.Username = reader["Username"].ToString();
                    T5.Three.AvarageRating = float.Parse(reader["AvarageRating"].ToString());
                    T5.Three.Content = reader["Content"].ToString();
                    rowCount += 1;
                }
                else if (rowCount == 4)
                {
                    T5.Four.Username = reader["Username"].ToString();
                    T5.Four.AvarageRating = float.Parse(reader["AvarageRating"].ToString());
                    T5.Four.Content = reader["Content"].ToString();
                    rowCount += 1;
                }
                else if (rowCount == 5)
                {
                    T5.Five.Username = reader["Username"].ToString();
                    T5.Five.AvarageRating = float.Parse(reader["AvarageRating"].ToString());
                    T5.Five.Content = reader["Content"].ToString();
                    rowCount += 1;
                }
            }  
            
            
            reader.Close();
            return T5;
        }
        
    }
}