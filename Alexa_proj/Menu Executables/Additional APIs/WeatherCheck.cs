
using Alexa_proj.Data_Control.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alexa_proj.Additional_APIs
{
    [Serializable]
    public class WeatherCheck : ExecutableModel
    {
        public string API_KEY = "4ca8dd9610ce1483dbf82dffa805ab08";
        public override async Task Execute()
        {
            WeatherInfo weatherReport = await GetInfo<WeatherInfo>();

            StartUp.CurrentMenu.DynamicShow(
                new DrawRectangle.ConsoleRectangle(
                    20, 3, new DrawRectangle.Point() { X = 31, Y = 1 },
                    ConsoleColor.Green,
                    new[] { ((int)weatherReport.main.temp).ToString() + " degrees Celsium\n" +
                    "Feels like: " + ((int)weatherReport.main.feels_like)},
                    0
                    )
                );

        }

        public override async Task<WeatherInfo> GetInfo<WeatherInfo>()
        {
            var client = new HttpClient();
            var response =
                await client.GetAsync(this.ExecutableFunction.FunctionEndpoint + $"&appid={API_KEY}");
            WeatherInfo Report = JsonConvert.DeserializeObject<WeatherInfo>(await response.Content.ReadAsStringAsync());
            return Report;
        }    
    }
}

