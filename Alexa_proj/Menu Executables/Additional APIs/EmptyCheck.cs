using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Alexa_proj.Additional_APIs
{
    public class EmptyCheck:ApiExecutable
    {
        public override void Execute()
        {
            var DrawnRectangle =
                 new DrawRectangle.ConsoleRectangle(
                        20, 3, new DrawRectangle.Point() { X = 7, Y = 1 },
                        ConsoleColor.Black,
                         new string[] { "Sorry, I couldn't find anything on your request.\nHere are my current functions:" },
                        0
                        );

            StartUp.CurrentMenu.DynamicShow(
               DrawnRectangle
                );
             
            Dictionary<string, List<string>> tempResults;

            tempResults = GetAvailableFunctions();

            #region HardCodedTableTop
            DrawnRectangle.BorderColor = ConsoleColor.Black;
            DrawnRectangle.Width = 25;
            DrawnRectangle.Location.X = 1;
            DrawnRectangle.FileText =
                $"Name:";
            StartUp.CurrentMenu.DynamicShow(
            DrawnRectangle
                           );
            DrawnRectangle.Width = 30;
            DrawnRectangle.Location.X = 25;
            DrawnRectangle.FileText =
                $"Supreme keyword:";
            StartUp.CurrentMenu.DynamicShow(
            DrawnRectangle
                           );
            DrawnRectangle.Location.Y += 4;
            DrawnRectangle.FileTextY += 4;
            #endregion

            DrawnRectangle.BorderColor = ConsoleColor.Green;

            foreach (var item in tempResults)
            {
                DrawnRectangle.Width = 25;
                DrawnRectangle.Location.X = 1;
                DrawnRectangle.FileText =
                    $"{item.Key.Substring(item.Key.Length - 16)}";
                StartUp.CurrentMenu.DynamicShow(
                DrawnRectangle
                               );
                DrawnRectangle.Width = 20;
                DrawnRectangle.Location.X = 25;
                DrawnRectangle.FileText =
                    $"{item.Value[0]}";
                StartUp.CurrentMenu.DynamicShow(
                DrawnRectangle
                               );
                DrawnRectangle.Location.Y += 5;
                DrawnRectangle.FileTextY += 5;
            }
        }

        private static Dictionary<string, List<string>> GetAvailableFunctions()
        {
            Dictionary<string, List<string>> tempResults;
            using (var reader = new StreamReader(@"Resources/Text/Functions.txt"))
            {
                tempResults = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(reader.ReadToEnd());
            }

            return tempResults;
        }
    }

}
