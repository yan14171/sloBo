using Alexa_proj.Additional_APIs;
using IBM.Watson.SpeechToText.v1.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;


namespace Alexa_proj
{
    class ResultAnaliser : Executable
    {
        public async override void Execute()
        {

            Dictionary<ApiExecutable, List<string>> AvailableFunctions = new Dictionary<ApiExecutable, List<string>>();
            Dictionary<string, List<KeywordResult>> Recognised;

            ApiExecutable Function;
            string[] ArrayOfFunctions;

            var RecognisedJson = await File.ReadAllTextAsync(@"Resources/Text/RecordingResults.txt");
            Recognised = JsonConvert.DeserializeObject<Dictionary<string, List<KeywordResult>>>(RecognisedJson);
            

            var FunctionsJson = await File.ReadAllTextAsync(@"Resources/Text/Functions.txt");
            var FunctionsDict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(FunctionsJson);

            for (int i = 0; i < FunctionsDict.Count; i++)
            {
                ArrayOfFunctions = FunctionsDict.Keys.ToArray<string>();
                Function = GetTypeByAssemblyName<ApiExecutable>(FunctionsDict.Keys.ToArray()[i]);
                AvailableFunctions.Add
                    (
                    Function,
                    FunctionsDict[ArrayOfFunctions[i]]
                    );
            }
            var AnalysisResult = from keyv in AvailableFunctions
                                 from v in keyv.Value
                                 where Recognised.Keys.Contains(v)
                                 select new { Info = Recognised.GetValueOrDefault(v), Does = keyv.Key };

            Animation.StopAnimation();



            //has to execute all functions!!


            if (AnalysisResult.Count() == 0)
                new EmptyCheck().Execute();
            else
                foreach (var item in AnalysisResult.Take(1))
                    item.Does.Execute();

        }         

       private static T GetTypeByAssemblyName <T>(string TypeName)
        {
            var AssemblyName = typeof(T).Assembly.GetName().Name;
           var handle = Activator.CreateInstance(AssemblyName, TypeName);
            return (T)handle.Unwrap();
        }
    }
}
