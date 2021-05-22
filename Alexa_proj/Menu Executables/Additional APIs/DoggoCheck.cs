using Alexa_proj.Data_Control.Models;
using DrawRectangle;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Alexa_proj.Additional_APIs
{

    public class DoggoCheck: ExecutableModel
    {
        public override async Task Execute()
        {
            var doggoReport = new DoggoInfo();
            var spinner = new ConsoleSpinner();
            var DrawnRectangle = new DrawRectangle.ConsoleRectangle
                (
             20, 3, new DrawRectangle.Point() { X = 7, Y = 1 },
             ConsoleColor.Black,
              new System.IO.StreamReader(@"Resources/Text/DoggoOpening.txt"),
             0
             );

            StartUp.CurrentMenu.DynamicShow(DrawnRectangle);

            Thread Anim = new Thread(() => spinner.Turn());
            Anim.Start();

            doggoReport = (await GetInfo<DoggoInfo[]>()).First();

            doggoReport.Quality = (DoggoQuality)GetQuality();

            Thread.Sleep(500);

            spinner.RequestToken();

            ShowDoggo(doggoReport);

            Thread.Sleep(1500);

            OpenUrl(doggoReport.url);
        }

        private int GetQuality()
        {
            Random rand = new Random();
            byte num = (byte)rand.Next(byte.MinValue, byte.MaxValue);
            switch (num)
            {
                case < 100:
                    return 0;
                case > 100 and < 180:
                    return 1;
                case > 180 and < 235:
                    return 2;
                case > 235:
                    return 3;
                default:
                    return 4;
            }

        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        private void ShowDoggo(DoggoInfo doggo)
        {
            StartUp.CurrentMenu.DynamicShow(Menu.ClearRectangle);

            var DrawnRectangle = new DrawRectangle.ConsoleRectangle
                (
             12, 3, new DrawRectangle.Point() { X = StartUp.SCREEN_SIZEX / 2 - (doggo?.breeds.FirstOrDefault()?.name?.Length/2+16 ?? 16), Y = 1 },
             ConsoleColor.Black,
              new System.IO.StreamReader(@"Resources/Text/DoggoOpening.txt"),
             0
             );

            DrawnRectangle.TitleColor = (ConsoleColor)GetQuality() + 1;

            DrawnRectangle.FileText = $"You've got a {doggo.Quality.ToString()} {doggo?.breeds.FirstOrDefault()?.name ?? "Null Terier"}";

            StartUp.CurrentMenu.DynamicShow(DrawnRectangle);

            using (var reader = new StreamReader(@"Resources/Text/Doggo.txt"))
            { 
                DrawnRectangle.FileText = reader.ReadToEnd();
            }
            DrawnRectangle.Location.X = 5;
            DrawnRectangle.FileTextY = 6;

            StartUp.CurrentMenu.DynamicShow(DrawnRectangle, isDrawn:false);
        }
       
        }
    public class ConsoleSpinner
        {
            static string[] sequence = null;

            public int Delay { get; set; } = 200;

            int totalSequences = 0;
            int counter;
        public AnimCanceletionToken token { get; private set; }
        public class AnimCanceletionToken
        {
            public bool IsRequested = false;
        }
            public ConsoleSpinner()
            {
            this.token = new AnimCanceletionToken();
                counter = 0;
                sequence = new string[]
                { ".   ", "..  ", "... ", "...." };
        

                totalSequences = sequence.GetLength(0);
            
            }

            public void Turn(string displayMsg = "")
            {
            while(!token.IsRequested)
                {
                counter++;
                if(Thread.CurrentThread.IsAlive)
                Thread.Sleep(Delay);

                int counterValue = counter % 4;

                string fullMessage = displayMsg + sequence[counterValue];
                int msglength = fullMessage.Length;

                Console.Write(fullMessage);

                Console.SetCursorPosition(Console.CursorLeft - msglength, Console.CursorTop);
            }
            }
        public void RequestToken() => this.token.IsRequested = true;
        
}
}
  

