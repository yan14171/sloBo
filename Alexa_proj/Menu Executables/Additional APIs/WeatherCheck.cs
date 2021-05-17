
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alexa_proj.Additional_APIs
{
    [Serializable]
    public class WeatherCheck : ApiExecutable
    {
        public string API_KEY = "4ca8dd9610ce1483dbf82dffa805ab08";
        public override async void Execute()

        {
            WeatherInfo weatherReport = await GetWeather();

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

        private async Task<WeatherInfo> GetWeather()
        {
            var client = new HttpClient();
            var response =
                await client.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q=Kyiw&appid={API_KEY}");
            WeatherInfo weatherReport = JsonConvert.DeserializeObject<WeatherInfo>(await response.Content.ReadAsStringAsync());
            return weatherReport;
        }
    }
}
