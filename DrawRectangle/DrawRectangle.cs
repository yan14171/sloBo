using System;
using System.Collections.Generic;
using System.IO;



namespace DrawRectangle
{

    [Serializable]
    public class ConsoleRectangle
    {
        private int hWidth;
        private int hHeight;
        private Point hLocation;
        private ConsoleColor hBorderColor;
        public ConsoleRectangle(int width, int height, Point location, ConsoleColor borderColor)
        {
            hWidth = width;
            hHeight = height;
            hLocation = location;
            hBorderColor = borderColor;
            this.TitleColor = ConsoleColor.White;
        }
        public ConsoleRectangle(int width, int height, Point location, ConsoleColor borderColor, StreamReader filedText)
        : this(width, height, location, borderColor)
        {
            // this.FileText = filedText.ReadToEnd();
            this.FileText = filedText.ReadToEnd();
            filedText.Close();
        }
        public ConsoleRectangle(int width, int height, Point location, ConsoleColor borderColor, StreamReader filedText,int TextY)
        :this(width,height,location,borderColor,filedText)
        {
            this.FileTextY = TextY + hLocation.Y;
        }
        public ConsoleRectangle(int width, int height, Point location, ConsoleColor borderColor, IEnumerable<string> Text, int TextY)
        : this(width, height, location, borderColor)
        {
            this.FileTextY = TextY + hLocation.Y;
            foreach (var item in Text)
            {
                this.FileText += item;
            }
        }
        public ConsoleRectangle(int width, int height, Point location, ConsoleColor borderColor, StreamReader filedText, int TextY, ConsoleColor titlecolor)
        :this( width, height, location, borderColor, filedText, TextY)
        {
            this.TitleColor = titlecolor;
        }
        public ConsoleRectangle()
        : this(0, 0, new Point(), ConsoleColor.White)
        {
        }

        public ConsoleColor TitleColor { get; set; }
        
        public string FileText { get; set; }
        public int FileTextY { get; set; }
        public Point Location
            {
                get { return hLocation; }
                set { hLocation = value; }
            } 

            public int Width
            {
                get { return hWidth; }
                set { hWidth = value; }
            }

            public int Height
            {
                get { return hHeight; }
                set { hHeight = value; }
            }

            public ConsoleColor BorderColor
            {
                get { return hBorderColor; }
                set { hBorderColor = (int)value<=15? value : 0; }
            }

            public void Draw()
            {
                string s = "╔";
                string space = "";
                string temp = "";
                for (int i = 0; i < Width; i++)
                {
                    space += " ";
                    s += "═";
                }

                //for (int j = 0; j < Location.X; j++)
                //    temp += " ";

                s += "╗" + "\n";

                for (int i = 0; i < Height; i++)
                    s += "║" + space + "║" + "\n";

                s += "╚";
                for (int i = 0; i < Width; i++)
                    s += "═";

                s += "╝";
            s += " ";
                Console.ForegroundColor = BorderColor;
                Console.CursorTop = hLocation.Y;
                Console.CursorLeft = hLocation.X;
            int count = -1;
            for (int i = 0; i <= Height+1; i++)
            {
                for (int j = 0; j <= Width+2; j++)
                {
                    count++;
                    Console.Write(s[count]);
                    
                }
                Console.SetCursorPosition(hLocation.X,hLocation.Y + i + 1);
            }    

                Console.ResetColor();
            }

        public void AlertWidthStabilize(string message, int Y)
        {
            int MiddleSymbol = message.Length / 2;
            int StartX = this.Width / 2 - MiddleSymbol;
            Console.SetCursorPosition(StartX + 1, Y);
            Console.Write(message);
        }

        public void Alert(string message , int Y)
        {

            if (message.Length > this.Width)
            {
                var FirstMessage = message.Substring(0, this.Width);
                var SecondMessage = message.Substring(this.Width);
                Alert(FirstMessage, Y);
                if (Y == this.Height + this.Location.Y) return;
                Alert(SecondMessage, Y + 1);
            }
            else
            {
                AlertWidthStabilize(message,Y);
            }

        }

        public void Alert(int Y)
        {

            Alert(FileText, Y);

        }
        public void Alert()
        {
            Console.SetCursorPosition(hLocation.X + 1, FileTextY);
            Console.Write(this.FileText);
            
        }

        public void AlertTitle()
        {
            Console.ForegroundColor = this.TitleColor;
            var splited = FileText.Split(new char[] { '\n', '\r' });
            for(int i = 0; i<splited.Length;i++)
            {
                Console.SetCursorPosition(hLocation.X + 1 , FileTextY + i + 1);

                Console.Write(splited[i]);

            }
        }
    }
  
}
