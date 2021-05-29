 
using Alexa_proj.Data_Control;
using Alexa_proj.Data_Control.Models;
using Alexa_proj.Repositories;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Cloud.SDK.Core.Http;
using IBM.Watson.SpeechToText.v1;
using IBM.Watson.SpeechToText.v1.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alexa_proj
{
    public class SpeechToTextRequester : ExecutableModel
    {
        const string API_KEY = "3oIocpwef11ts4l95M0Er9LvjRYYsW7Aka-tC5J3AZyf";

        const string SERVICE_INSTANCE_URL = "https://api.eu-gb.speech-to-text.watson.cloud.ibm.com";

        static string _watsonConcreteId = string.Empty;

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

        public async override Task Execute()
        {

            Animation.StartAnimation();

           var recognitionResults = await Recognise(@"Resources/Files/play(1).wav");

            await SearchEngineSetup();

            using (var writer = new StreamWriter(@"Resources/Text/RecordingResults.txt"))
            {
                writer.Write(JsonConvert.SerializeObject(recognitionResults));
                writer.Flush();
            }

           StartUp.HardIterate();
        }

        public async Task<IEnumerable<string>> Recognise( string filename = @"Resources/Files/RecordingFile.wav", string fileType = "wav" )
        {
            List<string> keywords = new List<string>();

            using (var unitOfWork = new UnitOfWork(new FunctionalContextFactory().CreateDbContext()))
            {
                var Executables =
                    await
                (unitOfWork.Executables as ExecutableRepository)
                .GetStaticExecutablesWithKeywordsAsync();

                keywords = Executables
                .SelectMany(n => n.Keywords.Select(m => m.KeywordValue))
                .ToList();
            }

            var WatsonRecognitonJob = Task.Run(() => stt.CreateJob
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

            _watsonConcreteId = (await WatsonRecognitonJob).Result.Id;

            List<string> RecognitionResults =
            await Task.Run(()=> GetWatsonRecognitionResults(_watsonConcreteId));

            return RecognitionResults;
        }

        private static string GetWatsonAlternativeTranscript(string ConcreteId)
        {
            DetailedResponse<RecognitionJob> WatsonResponse;

            while (true)
            {
                WatsonResponse = stt.CheckJob(ConcreteId);
                if (WatsonResponse.Result.Status == "completed") break;
            }

            var LastJobResults = WatsonResponse.Result.Results[0].Results;

            var RecognitionResult =
                LastJobResults[0].Alternatives[0].Transcript;

            return RecognitionResult;
        }

        private static List<string> GetWatsonRecognitionResults(string ConcreteId)
        {
            DetailedResponse<RecognitionJob> WatsonResponse;

            while (true)
            {
                WatsonResponse = stt.CheckJob(ConcreteId);
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

            return RecognitionResults;
        }

        public async static Task<List<ExecutableModel>> SearchEngineSetup()
        {
            List<ExecutableModel> returnedExecutables = CreateExecutables();

            using (var unitOfWork = new UnitOfWork(StartUp.contextFactory.CreateDbContext()))
            {
                unitOfWork.Executables.RemoveRange(unitOfWork.Executables.GetAll());

                unitOfWork.Executables.AddRange(returnedExecutables);

                await unitOfWork.CompleteAsync();

                return unitOfWork.Executables as List<ExecutableModel>;
            }
        }

        public static List<ExecutableModel> CreateExecutables()
        {
            var AvailableFeatures = new List<ExecutableModel>();

            AvailableFeatures.AddRange(CreateStaticExecutables());

            AvailableFeatures.AddRange(CreateDynamicExecutables());

            return AvailableFeatures;
        }

        private static List<ExecutableModel> CreateStaticExecutables()
        {
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
                    FunctionEndpoint = "http://api.openweathermap.org/data/2.5/weather?q=Kyiw",
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
                Doggomodel,
            };
            return returnedExecutables;
        }

        private static List<ExecutableModel> CreateDynamicExecutables()
        {
            var SongKeywords = new List<string>(new string[] {
            "play a song",
            "play the song",
            "listen to",
            "listen",
            "play",
            "turn on",
            "turn", });

            var SongModel = new ExecutableModel()
            {
                ExecutableName = "SongExecutable",

                Keywords = new List<Keyword>(),

                ExecutableFunction = new Function()
                {
                    FunctionEndpoint = "https://api.deezer.com/search",
                    FunctionName = "Alexa_proj.Additional_APIs.SongCheck",
                    FunctionResult = new FunctionResult() { ResultValue = string.Empty }
                },

            };

            foreach (var item in SongKeywords)
            {
                SongModel.Keywords.Add(new Keyword { KeywordValue = item });
            }

            SongModel.ExecutableFunction.FunctionResult.ResultValue = GetWatsonAlternativeTranscript(_watsonConcreteId);

            var returnedExecutables = new List<ExecutableModel>()
            {
                SongModel
            };

            return returnedExecutables;
        }
    }
}