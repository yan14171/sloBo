using Alexa_proj.Data_Control.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alexa_proj.Additional_APIs
{
    public class CoronaCheck: ExecutableModel
    {
        public override async Task Execute()

        {
            CoronaInfo coronaReport = await GetInfo<CoronaInfo>();

            StartUp.CurrentMenu.DynamicShow(
                new DrawRectangle.ConsoleRectangle(
                    30, 5, new DrawRectangle.Point() { X = 1, Y = 1 },
                    ConsoleColor.Green,
                    new[] { $"Total amount of infected: \n{coronaReport.Global.TotalConfirmed}\n", $"Total Deaths: \n{coronaReport.Global.TotalDeaths}\n" },
                    0
                    )
                );


           var UkraineInfo = coronaReport.Countries.Find(n => n.Slug == "ukraine");


            StartUp.CurrentMenu.DynamicShow(
              new DrawRectangle.ConsoleRectangle(
                  30, 5, new DrawRectangle.Point() { X = 1, Y = 8 },
                  ConsoleColor.Green,
                  new[] { $"Ukraine\nTotal amount of infected: \n{UkraineInfo.TotalConfirmed}\n", $"Total Deaths: \n{UkraineInfo.TotalDeaths}" },
                  0
                  )
              );

        }


    }
}
