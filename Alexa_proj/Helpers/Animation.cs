using DrawRectangle;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Alexa_proj
{
    static class Animation
    {
        private static CancellationTokenSource AnimStop;
        public static CancellationToken AnimStopToken;
        public static Task Anim;
        public static void StartAnimation()
        {
            StartUp.IsWait = true;
            AnimStop = new CancellationTokenSource();
            AnimStopToken = AnimStop.Token;
            Anim = new Task(() => Animate(AnimStopToken), AnimStopToken);
            Anim.Start();
            

        }

        public static void StopAnimation()
        {
            try
            {
                AnimStop.Cancel();
            }
            finally
            {
                AnimStop.Dispose();
            }

            while (true)
                if (Anim.Status == TaskStatus.RanToCompletion || Anim.Status == TaskStatus.Canceled) break;

                StartUp.CurrentMenu.MainWindow.Draw();
            StartUp.IsWait = false;
        }

        private static void Animate(CancellationToken token)
        {
            int OriginalX = 18;
            int OriginalY = 5;
            Point DrawPoint = new Point()
            {
                X = OriginalX,
                Y = OriginalY
            };
            ConsoleRectangle LoadingRectangle =
             new ConsoleRectangle(0, 0, DrawPoint, ConsoleColor.Green, new[] { "" }, 0);



            using (var reader = new StreamReader(@"Resources/Text/Processing.txt"))
            {
                StartUp.CurrentMenu.DynamicShow
                     (
                     new ConsoleRectangle
                     (
                         40, 10, new Point() { X = 5, Y = OriginalY + 2 },
                         ConsoleColor.Black, reader, 0
                     )
                     );
            }
            try
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    while (DrawPoint.X < StartUp.SCREEN_SIZEX - OriginalX - 2)
                    {
                        StartUp.CurrentMenu.DynamicShow
                        (
                           LoadingRectangle
                        );
                        token.ThrowIfCancellationRequested();
                        Thread.Sleep(50);
                        DrawPoint.X += 2;
                    }
                    LoadingRectangle.BorderColor++;
                    DrawPoint.X -= 2;
                    while (DrawPoint.X > OriginalX)
                    {
                        StartUp.CurrentMenu.DynamicShow
                        (
                           LoadingRectangle
                        );
                        token.ThrowIfCancellationRequested();
                        Thread.Sleep(50);
                        DrawPoint.X -= 2;
                    }
                    LoadingRectangle.BorderColor++;

                }
            }
            catch { }

        }



    }
}
