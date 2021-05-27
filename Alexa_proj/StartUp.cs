using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alexa_proj.Data_Control;
using Newtonsoft.Json;


namespace Alexa_proj
{
    public class StartUp
    {
        static StartUp()
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(SCREEN_SIZEX, SCREEN_SIZEY);
            contextFactory = new FunctionalContextFactory();

            Task.Run(() => new SpeechToTextRequester().Recognise(@"Resources/Files/dog.wav"));
        }

        public static bool IsWait = false;

        public static event EventHandler OnEnterPressed;

        public const int SCREEN_SIZEX = 63;

        public const int SCREEN_SIZEY = 33;

        public static List<Menu> Menus = new List<Menu>();

        static int _menuiterator = 0;

        public static FunctionalContextFactory contextFactory;

        public static int MenuIterator {
            get { return _menuiterator; }
            private set { _menuiterator = value <= Menus.Count() - 1 ? value : 0; }
        }

        static Menu _currentMenu;

        public static Menu CurrentMenu
        {
            get { return _currentMenu ?? new Menu(); }
            set { _currentMenu = value;}
        }

        static void KeyLoop()
        {
            ConsoleKeyInfo Pressed;

            while (true)
            {
                
                    Pressed = Console.ReadKey(intercept: true);

                if (!IsWait)
                {
                    switch (Pressed.Key)
                    {
                        case ConsoleKey.Enter:
                            {
                                MenuIterator++;
                                OnEnterPressed?.Invoke(new object(), new EventArgs());
                                break;
                            }
                        case ConsoleKey.Escape:
                            {
                                Environment.Exit(0);
                                break;
                            }

                    }
                }
   
            }
        }

        static async Task MenuControl()
        {
            while (true)
            {
                if (!Menus[MenuIterator].IsShown)
                    await Menus[MenuIterator].Show();
            }
        
        }

        public static void HardIterate()
        {
            MenuIterator++;
        }

        static void Main(string[] args)
        {
            #region SetUp


            Menu MainMenu;
            using (var read = new StreamReader(@"Resources/Menus/MainMenu.txt"))
            {
                MainMenu = JsonConvert.DeserializeObject<Menu>(read.ReadToEnd());
            }
            Menus.Add(MainMenu);


            Menu RecordMenu;
            using (var read = new StreamReader(@"Resources/Menus/RecordMenu.txt"))
            {
                RecordMenu = JsonConvert.DeserializeObject<Menu>(read.ReadToEnd());
            }
            RecordMenu.ExecutionManager = new SoundCapturer();
            Menus.Add(RecordMenu);


            var TranslateMenu = new Menu();
            TranslateMenu.ExecutionManager = new SpeechToTextRequester();
            Menus.Add(TranslateMenu);


            Menu ResultsMenu = new Menu();
            ResultsMenu.ExecutionManager = new ResultAnaliser();
            Menus.Add(ResultsMenu);
            #endregion

            try
            {
                Menu.OnScreenChange += Menu_OnScreenChange;

                Menus[MenuIterator].Show();
                
                Thread KeyL = new Thread(() =>
             
                KeyLoop());
                
                KeyL.Start();
                
                MenuControl().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.Write("An exception was thrown in MenuControl. Message:" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private static void Menu_OnScreenChange(object sender, EventArgs e)
        {
            (sender as Menu).IsShown = true;
            CurrentMenu.IsShown = false;
            CurrentMenu = sender as Menu;
        }
    }
}

