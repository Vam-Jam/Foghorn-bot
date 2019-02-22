using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot.Classes
{

    public class AutoRestart
    {
        private string FileLocation { get; }
        private string ExeName { get; }
        private string Ip { get; }
        private string Port { get; }
        private int PID { get; set; }
        private int CheckFrequency { get; set; }
        private bool ServerCheck { get; set; }
        public DateTime lastTimeChecked { get; set; }
        
        public AutoRestart(string fileLocation, string fileExeName, string ip, string port)
        {
            FileLocation = fileLocation;
            ExeName = fileExeName;
            Ip = ip;
            Port = port;
            CheckFrequency = 250;
            ServerCheck = true;
            Console.WriteLine("starting " + fileExeName + " checker");
            ServerChecker();
        }


        private async Task ServerChecker()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(1000 * CheckFrequency);
                    lastTimeChecked = DateTime.Now;
                    if (await CheckIfServerIsDead() == false)
                    {
                        Console.WriteLine("am ded : " + DateTime.Now + " | " + ExeName);
                        await KillServer();
                        await StartServer();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
        }


        public void StopChecker()
        {
            ServerCheck = false;
        }

        public void StartChecker()
        {
            ServerCheck = true;
            ServerChecker();
        }
        
        public async Task<bool> CheckIfServerIsDead()
        {
            await Task.Delay(1);
            
            DateTime lastupdate;
            
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create($"https://api.kag2d.com/v1/game/thd/kag/server/"+Ip+"/"+Port+"/status");
            getRequest.Method = "GET";
            
            HttpWebResponse getresponse = (HttpWebResponse)getRequest.GetResponse();
            Stream newStream = getresponse.GetResponseStream();
            
            using (StreamReader sr = new StreamReader(newStream))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var reponse = sr.ReadToEnd();
                dynamic results = JsonConvert.DeserializeObject(reponse);
                lastupdate = results.serverStatus.lastUpdate;
            }

            DateTime dateToSend = DateTime.Now;
            dateToSend = dateToSend.AddMinutes(-1);
            
            if (IsLastUpdateOld(dateToSend, lastupdate))
                return true;            
            return false;
        }


        public bool IsLastUpdateOld(DateTime currentTime, DateTime lastUpdate)
        {
            if (currentTime.TimeOfDay > lastUpdate.TimeOfDay)
                return false;
            return true;
        }

        public async Task<bool> KillServer()
        {
            await Task.Delay(1);
            try
            {
                Process process = Process.GetProcessById(PID);
                process.Kill();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                try
                {
                    Process[] processes = Process.GetProcessesByName(ExeName);
                    processes[0].Kill();
                    return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    return false;
                }
            }
        }


        public async Task<bool> StartServer()
        {
            await Task.Delay(1);
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = @FileLocation+ExeName,
                    Arguments = @"start /wait "+ExeName+" nolauncher autostart Scripts/server_autostart.as autoconfig autoconfig.cfg",
                    CreateNoWindow = false,
                    UseShellExecute = true
                };
                Process x = Process.Start(startInfo);
                
                x.PriorityClass = ProcessPriorityClass.High;
                PID = x.Id;
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        
    }
    
    
    
    /*
    public class AutoRestartTC
    {
        private static DateTime TimeLastchecked;
        private static string Ip = "109.228.14.252";
        private static int Port = 50309;
        private static int CurrentPID = 0;
        private static bool HasStarted = false;
        public AutoRestartTC()
        {
            
        }

        public bool HasStart()
        {
            return HasStarted;
        }

        public async Task MainProgram()
        {
            ThreadStart task1 = () => TcCheckThread();
            Thread t1 = new Thread(task1);
            t1.Start();
        }

        private async Task TcCheckThread()
        {
            HasStarted = true;
            await Run();
            start:
           
            await Task.Delay(1000 * 30 * 4);
            //System.Threading.Thread.Sleep(1000 * 30 * 4);
            TimeLastchecked = DateTime.Now;
            TimeLastchecked = TimeLastchecked.AddHours(-1);
            await Task.Delay(1000 * 30 * 1);
            //System.Threading.Thread.Sleep(1000 * 30 * 1);
            try
            {
                DateTime lastupdate;
                JsonSerializer serializer = new JsonSerializer();
                HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create($"https://api.kag2d.com/v1/game/thd/kag/server/"+Ip+"/"+Port+"/status");
                getRequest.Method = "GET";
                var getreponse = (HttpWebResponse)getRequest.GetResponse();
                Stream newStream = getreponse.GetResponseStream();
                using (StreamReader sr = new StreamReader(newStream))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    var reponse = sr.ReadToEnd();
                    dynamic results = JsonConvert.DeserializeObject(reponse);
                    lastupdate = results.serverStatus.lastUpdate;
                }
                if (TimeLastchecked.TimeOfDay > lastupdate.TimeOfDay)
                {
                    Console.WriteLine($"{DateTime.Now} - Restarting server");
                    await Task.Run(Kill);
                    System.Threading.Thread.Sleep(1000);
                    await Task.Run(Run);
                    goto start;
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now} - Server alive");
                    goto start;
                }
            }
            catch
            {
                Console.WriteLine($"{DateTime.Now} - Error with trying to do stuff with api. Check that you typed your correct port");
                goto start;
            }
        }
        
        public async Task Run()
        {
            await Task.Delay(1);
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = @"D:\TempKAG\King Arthur's Gold - TC\TC.exe",
                    Arguments = @"start /wait TC.exe nolauncher autostart Scripts/server_autostart.as autoconfig autoconfig.cfg",
                    CreateNoWindow = false,
                    UseShellExecute = true
                };
                Process x = Process.Start(startInfo);
                
                x.PriorityClass = ProcessPriorityClass.High;
                CurrentPID = x.Id;
            }
            catch(Exception e)
            {
                Console.WriteLine("Could not run for some reason. May be caused by anti-virus protection. (Or Vamist broke something)");
                Console.WriteLine(e);
            }
        }

        public async Task Kill()
        {
            await Task.Delay(1);
            try
            {
                //proc[0].Kill();
                Process k = Process.GetProcessById(CurrentPID);
                k.Kill();
                Console.WriteLine($"{DateTime.Now} - Server closed successfully");
            }
            catch
            {
                Console.WriteLine($"{DateTime.Now} - Can't kill task, may already be closed.");
            }
        }
    }*/
}