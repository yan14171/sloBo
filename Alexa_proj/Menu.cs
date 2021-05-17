﻿
using DrawRectangle;
using System;
using System.Collections.Generic;

namespace Alexa_proj
{
    

    [Serializable]
    class Menu 
    {
        public static ConsoleRectangle ClearRectangle;
       static Menu()
        {
         ClearRectangle = new DrawRectangle.ConsoleRectangle(
                   StartUp.SCREEN_SIZEX - 10, StartUp.SCREEN_SIZEY - 10, new DrawRectangle.Point() { X = 1, Y = 1 },
                    ConsoleColor.Black,
                     new string[] { "" },
                    0
                    );
        }
       public static event EventHandler OnScreenChange;

        public ConsoleRectangle MainWindow
            = new ConsoleRectangle(60, 30, new Point(), ConsoleColor.White);

        public List<ConsoleRectangle> Windows = new List<ConsoleRectangle>();

        public bool IsShown { get; set; }

        public Executable ExecutionManager; 

        public void Show()
        {

            if (!this.IsShown)
            {
              
                Console.SetCursorPosition(0, 0);
                this.MainWindow?.Draw();
                foreach (var item in Windows)
                {
                    Console.SetCursorPosition(0, 0);
                    item?.Draw();
                    item?.AlertTitle();
                }
              this.ExecutionManager?.Execute();
                Console.SetCursorPosition(0, 0);
                OnScreenChange?.Invoke(this, new EventArgs());
              
            }
        }
        public void DynamicShow(ConsoleRectangle injectedScreen, bool isDrawn = true)
            {      
            if(isDrawn)
            injectedScreen.Draw();
            injectedScreen.AlertTitle();
        }
    }
}
