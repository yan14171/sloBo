using Alexa_proj.Additional_APIs;
using Alexa_proj.Data_Control;
using Alexa_proj.DataAccess.Repositories;
    using Alexa_proj.Repositories;
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
            List<ApiExecutable> apiExecutables = new List<ApiExecutable>();

            List<string> capturedKeywords = new List<string>();

            List<Executable> sortedApiExecutables = new List<Executable>();

            using (var reader = new StreamReader(@"Resources/Text/RecordingResults.txt"))
            { 
                capturedKeywords = JsonConvert.DeserializeObject<List<string>>(reader.ReadToEnd());
            }

            using (var unitOfWork = new UnitOfWork( new FunctionalContextFactory().CreateDbContext()))
            {
                var executableModels = await
                      (unitOfWork.Executables as ExecutableRepository)
                      .GetStaticExecutablesAsync();

                apiExecutables = executableModels
                    .Cast<ApiExecutable>()
                    .ToList();
            }

            sortedApiExecutables = SortRuntimeExecutables(apiExecutables, capturedKeywords);

                Animation.StopAnimation();

            foreach (var item in sortedApiExecutables)
            {
                item.Execute();
            }


        }         

        private static List<Executable> SortRuntimeExecutables(IEnumerable<Executable> executables, IEnumerable<string> keywords)
        {
            var sorted =
                  executables
                  .Where(n => n.Keywords
                      .Any(
                          m => keywords
                          .ToList()
                          .Contains(m.KeywordValue)))
                  .ToList();

            return sorted;

        }

       private static T GetTypeByAssemblyName <T>(string TypeName)
        {
            var AssemblyName = typeof(T).Assembly.GetName().Name;
           var handle = Activator.CreateInstance(AssemblyName, TypeName);
            return (T)handle.Unwrap();
        }
    }
}
