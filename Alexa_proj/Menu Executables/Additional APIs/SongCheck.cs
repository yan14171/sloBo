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
                =
                String.Join(
                    " ",
            this.ExecutableFunction.FunctionResult.ResultValue
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Where(n => !Keywords.Any(m => m.KeywordValue == n))
                );

            ExecutableFunction.FunctionEndpoint += $"?q=track:\"{ExecutableFunction.FunctionResult.ResultValue}\"";

            SongInfo songReport = await GetInfo<SongInfo>();

            StartUp.CurrentMenu.DynamicShow(
             new DrawRectangle.ConsoleRectangle(
                 45, 5, new DrawRectangle.Point() { X = 1, Y = 8 },
                 ConsoleColor.Green,
                 new[] { $"Playing {songReport.data[0].title_short} by {songReport.data[0].artist.name}" },
                 0
                 ));

            Thread.Sleep(500);

            OpenUrl(songReport.data[0].preview);
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
