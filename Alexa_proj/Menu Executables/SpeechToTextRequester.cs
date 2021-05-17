
//#define Mine

using Alexa_proj.Additional_APIs;
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
           await Recognise(SearchEngineSetup(),@"Resources/Files/RecordingFile (8).wav");
            StartUp.HardIterate();
        }

        public static async Task Recognise
            (
            Dictionary<ApiExecutable,
            List<string>> AvailableFeatures,
            string filename = @"Resources/Files/RecordingFile.wav",
            string fileType = "wav"
            )
        {
            var result =await Task.Run(()=> stt.CreateJob
         (
     audio: new MemoryStream(File.ReadAllBytes(filename)),
     contentType: $"audio/{fileType}",
     keywords: AvailableFeatures.Values.SelectMany(n => n).ToList(),
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
            var RecognitionResults = new Dictionary<string, List<KeywordResult>>();
            foreach (var item in LastJobResults)
            {
                if (item.KeywordsResult.Count > 0)
                    foreach (var Keyw in item.KeywordsResult)
                    {
                        RecognitionResults.Add(Keyw.Key, Keyw.Value);
                    }
            }


            using (var writer = new StreamWriter(@"Resources/Text/RecordingResults.txt"))
            {

                writer.Write(JsonConvert.SerializeObject(RecognitionResults));
                writer.Flush();


            }
            using (var writer = new StreamWriter(@"Resources/Text/Functions.txt"))
            {
                writer.Write(JsonConvert.SerializeObject(AvailableFeatures));
                writer.Flush();
            }
        }

        private static Dictionary<ApiExecutable, List<string>> SearchEngineSetup()
        {
            var AvailableFeatures = new Dictionary<ApiExecutable, List<string>>();
            AvailableFeatures.Add(

                new WeatherCheck(),

                     new List<string>(new string[] {
                          "what's the weather", "window", "what's outside", "what's outside the window", "outside","what is outside",
                         "weather", "temperature",  "how hot", "how warm", "how cold", "warm", "cold"
                     })

                     );

            AvailableFeatures.Add(

               new CoronaCheck(),

                    new List<string>(new string[] {
                         "corona info", "corona", "virus news", "news about the virus","news about corona virus",
                         "situation on corona", "covid stats", "covid patients", "number of covid patients", "covid statistic",
                         "covid patients", "covid","patients", "sick"
                    })

                    );


            AvailableFeatures.Add(

               new DoggoCheck(),

                    new List<string>(new string[] {
                         "play doggo",  "doggo", "dog game", "that dog game", "roll a dog","roll","dog","dogo",
                         "give me a dog", "doggo", "doggo box","doggobox", "open the doggo box"
                    })

                    );
            return AvailableFeatures;
        }
    }
}