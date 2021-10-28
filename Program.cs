using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text; 

namespace the_voice
{
    class Program
    {
        static void Main(string[] args)
        {
            useDb();    

            Console.Write("Login: ");            
            string textLogin = Console.ReadLine();

            Console.Write("Pass: ");            
            var pass = string.Empty;

            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);


            string name = Login(textLogin, pass);

            if( name != null)
            {
                var msg = $"\n\nHello {name}, qual loucura vai fazer hojê!";
                Console.WriteLine(msg);
                Speak(msg);
            } 
            else
            {
                var msg = $"\n\nOpa, tu não está na lista {textLogin}, quem sabe na próxima!";
                Console.WriteLine(msg);
                Speak(msg);
            }
        }

        public static string Login(string userName, string pass)
        {
            string name = null;
            using (var dbContext = new MyDbContext())
            {
                foreach (var user in dbContext.Users)
                {                    
                    if(user.UserName == userName && user.Password == pass)
                    {
                        name = user.FullName;
                    }
                }
            }
            return name;            
        }

        static void useDb()
        {
            string dbName = "Database.db";            
            using (var dbContext = new MyDbContext())
            {                
                if (!File.Exists(dbName))
                {
                    //File.Delete(dbName);
                    //Ensure database is created
                    dbContext.Database.EnsureCreated();
                    if (!dbContext.Users.Any())
                    {
                        dbContext.Users.AddRange(new User[]
                        {
                            new User{ UserId=1, UserName="m4rkos", Password="m007", FullName="Marcos Eduardo" },
                        });
                        dbContext.SaveChanges();
                    }
                }
                foreach (var user in dbContext.Users)
                {
                    Console.WriteLine($"UserID={user.UserId}\tUserName={user.UserName}\tDateTimeAdd={user.DateTimeAdd}");
                }
            }
            Console.ReadLine();
        }

        private static void Speak(string textToSpeech, bool wait = false)  
        {  
            // Command to execute PS  
            Execute($@"Add-Type -AssemblyName System.speech;  
            $speak = New-Object System.Speech.Synthesis.SpeechSynthesizer;                           
            $speak.Speak(""{textToSpeech}"");"); // Embedd text  
  
            void Execute(string command)  
            {  
                // create a temp file with .ps1 extension  
                var cFile = System.IO.Path.GetTempPath() + Guid.NewGuid() + ".ps1";  
  
                //Write the .ps1  
                using var tw = new System.IO.StreamWriter(cFile, false, Encoding.UTF8);  
                tw.Write(command);  
  
                // Setup the PS  
                var start =  
                    new System.Diagnostics.ProcessStartInfo()  
                    {  
                        FileName = "C:\\windows\\system32\\windowspowershell\\v1.0\\powershell.exe",  // CHUPA MICROSOFT 02-10-2019 23:45                    
                        LoadUserProfile = false,  
                        UseShellExecute = false,  
                        CreateNoWindow = true,  
                        Arguments = $"-executionpolicy bypass -File {cFile}",  
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden  
                    };  
  
                //Init the Process  
                var p = System.Diagnostics.Process.Start(start);  
                // The wait may not work! :(  
                if (wait) p.WaitForExit();  
            }  
        }  
    }
}
