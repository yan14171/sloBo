using Alexa_proj.Data_Control.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alexa_proj.Additional_APIs
{
    public class SongCheck : ExecutableModel
    {
        public override async Task Execute()
        {
            this.ExecutableFunction.FunctionResult.ResultValue
                 = CreateSongRequest(this.ExecutableFunction.FunctionResult.ResultValue);

            ExecutableFunction.FunctionEndpoint += $"?q=track:\"{ExecutableFunction.FunctionResult.ResultValue}\"";

            SongInfo songReport = await GetInfo<SongInfo>();

            string outputMessage = $"Playing {songReport.data[0].title_short} by {songReport.data[0].artist.name}";

            StartUp.CurrentMenu.DynamicShow(
             new DrawRectangle.ConsoleRectangle(
                 outputMessage.Length, 1, new DrawRectangle.Point() { X = 1, Y = 1 },
                 ConsoleColor.Green,
                 new[] { outputMessage },
                 0
                 ));

            Thread.Sleep(500);

            OpenUrl(songReport.data[0].preview);
        }

        public string CreateSongRequest(string input)
        {
            string output =
                String.Join(
                    " ",
                 input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .TakeWhile(n => n != "by")
                .Where(n => !Keywords.Any(m => m.KeywordValue == n))
                .Where(n => n != "%HESITATION")
                );

            return output;
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
    }
}
