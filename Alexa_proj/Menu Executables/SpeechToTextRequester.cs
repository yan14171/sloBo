
//#define Mine

using Alexa_proj.Additional_APIs;
using Alexa_proj.Data_Control;
using Alexa_proj.Data_Control.Models;
using Alexa_proj.Repositories;
using DrawRectangle;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Cloud.SDK.Core.Http;
using IBM.Watson.SpeechToText.v1;
using IBM.Watson.SpeechToText.v1.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;

namespace Alexa_proj
{
    class SpeechToTextRequester : Executable
    {
        const string API_KEY = "3oIocpwef11ts4l95M0Er9LvjRYYsW7Aka-tC5J3AZyf";

        const string SERVICE_INSTANCE_URL = "https://api.eu-gb.speech-to-text.watson.cloud.ibm.com";

        static readonly SpeechToTextService stt;

        static SpeechToTextRequester()
        {
            IamAuthenticator auth = new IamAuthenticator(
                apikey: API_KEY
                );
            stt = new SpeechToTextService(auth);
            stt.SetServiceUrl(SERVICE_INSTANCE_URL);
            stt.DisableSslVerification(true);
            stt.WithHeader("X-Watson-Learning-Opt-Out", "true");


        }

        public async override void Execute()
        {
            Animation.StartAnimation();

            //Uncomment to add new features
            //await SearchEngineSetup();

            await Recognise( @"Resources/Files/RecordingFile (8).wav");

            StartUp.HardIterate();
        }

        public static async Task Recognise( string filename = @"Resources/Files/RecordingFile.wav", string fileType = "wav" )
        {
            List<string> keywords = new List<string>();

            using (var unitOfWork = new UnitOfWork(new FunctionalContextFactory().CreateDbContext()))
            {
                var Executables =
                    await
                (unitOfWork.Executables as ExecutableRepository)
                .GetStaticExecutablesAsync();

                keywords = Executables
                .SelectMany(n => n.Keywords.Select(m => m.KeywordValue))
                .ToList();
            }

                var result = await Task.Run(() => stt.CreateJob
              (
          audio: new MemoryStream(File.ReadAllBytes(filename)),
          contentType: $"audio/{fileType}",
          keywords: keywords,
          model: "en-US_NarrowbandModel",
          keywordsThreshold: 0.2F,
          speechDetectorSensitivity: 0.8F,
          backgroundAudioSuppression: 0.5F,
          timestamps: true
          ));
            string ConcreteId = result.Result.Id;

            DetailedResponse<RecognitionJob> WatsonResponse;
            while (true)
            {
                WatsonResponse = await Task.Run(() => stt.CheckJob(ConcreteId));
                if (WatsonResponse.Result.Status == "completed") break;
            }
            var LastJobResults = WatsonResponse.Result.Results[0].Results;
            var RecognitionResults = new List<string>();
            foreach (var item in LastJobResults)
            {
                if (item.KeywordsResult.Count > 0)
                    foreach (var Keyw in item.KeywordsResult)
                    {
                        RecognitionResults.Add(Keyw.Key);
                    }
            }


            using (var writer = new StreamWriter(@"Resources/Text/RecordingResults.txt"))
            {

                writer.Write(JsonConvert.SerializeObject(RecognitionResults));
                writer.Flush();


            }
        }

        private async static Task<List<ExecutableModel>> SearchEngineSetup()
        {
            var AvailableFeatures = new List<ExecutableModel>();
            var WeatherKeywords = new List<string>(new string[] {
                "what's the weather",
                "window",
                "what's outside",
                "what's outside the window",
                "outside",
                "what is outside",
                "weather",
                "temperature",
                "how hot",
                "how warm",
                "how cold",
                "warm",
                "cold"
            });
            var CoronaKeywords = new List<string>(new string[] {
                "corona info",
                "corona",
                "virus news",
                "news about the virus",
                "news about corona virus",
                "situation on corona",
                "covid stats",
                "covid patients",
                "number of covid patients",
                "covid statistic",
                "covid patients",
                "covid",
                "patients",
                "sick"
            });
            var DoggoKeywords = new List<string>(new string[] {
                "play doggo",
                "doggo",
                "dog game",
                "that dog game",
                "roll a dog",
                "roll",
                "dog",
                "dogo",
                "give me a dog",
                "doggo",
                "doggo box",
                "doggobox",
                "open the doggo box"
            });

            ExecutableModel Weathermodel = new ExecutableModel()
            {
                ExecutableName = "WeatherExecutable",

                Keywords = new List<Keyword>(),

                ExecutableFunction = new Function()
                {
                    FunctionEndpoint = $"http://api.openweathermap.org/data/2.5/weather?q=Kyiw&appid={API_KEY}",
                    FunctionName = "Alexa_proj.Additional_APIs.WeatherCheck",
                    FunctionResult = new FunctionResult() { ResultValue = string.Empty }
                },

            };
            ExecutableModel Coronamodel = new ExecutableModel()
            {
                ExecutableName = "CoronaExecutable",

                Keywords = new List<Keyword>(),

                ExecutableFunction = new Function()
                {
                    FunctionEndpoint = $"https://api.covid19api.com/summary",
                    FunctionName = "Alexa_proj.Additional_APIs.CoronaCheck",
                    FunctionResult = new FunctionResult() { ResultValue = string.Empty }
                },

            };
            ExecutableModel Doggomodel = new ExecutableModel()
            {
                ExecutableName = "DoggoExecutable",

                Keywords = new List<Keyword>(),

                ExecutableFunction = new Function()
                {
                    FunctionEndpoint = "https://api.thedogapi.com/v1/images/search",
                    FunctionName = "Alexa_proj.Additional_APIs.DoggoCheck",
                    FunctionResult = new FunctionResult() { ResultValue = string.Empty }
                },

            };

            foreach (var item in WeatherKeywords)
            {
                Weathermodel.Keywords.Add(new Keyword { KeywordValue = item });
            }
            foreach (var item in CoronaKeywords)
            {
                Coronamodel.Keywords.Add(new Keyword { KeywordValue = item });
            }
            foreach (var item in DoggoKeywords)
            {
                Doggomodel.Keywords.Add(new Keyword { KeywordValue = item });
            }

            var returnedExecutables = new List<ExecutableModel>()
            {
                Weathermodel,
                Coronamodel,
                Doggomodel
            };

            using (var unitOfWork = new UnitOfWork(new FunctionalContextFactory().CreateDbContext()))
            {
                unitOfWork.Executables.AddRange(returnedExecutables);

                await unitOfWork.CompleteAsync();

                return unitOfWork.Executables as List<ExecutableModel>;
            }
        }
    }
}