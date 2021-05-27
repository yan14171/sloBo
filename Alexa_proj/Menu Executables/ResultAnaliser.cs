using Alexa_proj.Additional_APIs;
using Alexa_proj.Data_Control;
using Alexa_proj.Data_Control.Models;
using Alexa_proj.DataAccess.Repositories;
using Alexa_proj.Repositories;
using IBM.Watson.SpeechToText.v1.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading.Tasks;

namespace Alexa_proj
{
    public class ResultAnaliser : ExecutableModel
    {
        public async override Task Execute()
        {
            List<ExecutableModel> apiExecutables = ReadAvailableExecutables().Result;

            List<string> capturedKeywords = ReadKeywords();

            List<ExecutableModel> sortedApiExecutables = new List<ExecutableModel>();

            sortedApiExecutables = SortRuntimeExecutables(apiExecutables, capturedKeywords);

            Animation.StopAnimation();

            foreach (var item in sortedApiExecutables)
            {
                var m = new Menu();
                m.ExecutionManager = item;
                StartUp.Menus.Add(m);
            }

            StartUp.HardIterate();
        }

        private async Task<List<ExecutableModel>> ReadAvailableExecutables()
        {
            List<ExecutableModel> apiExecutables;

            using (var unitOfWork = new UnitOfWork(StartUp.contextFactory.CreateDbContext()))
            {
                var executableModels = await
                      (unitOfWork.Executables as ExecutableRepository)
                      .GetStaticExecutablesAsync();

                 apiExecutables = executableModels
                    .ToList();
            }

            return apiExecutables;
        }

        private List<string> ReadKeywords()
        {
            List<string> capturedKeywords;
            using (var reader = new StreamReader(@"Resources/Text/RecordingResults.txt"))
            {
                capturedKeywords = JsonConvert.DeserializeObject<List<string>>(reader.ReadToEnd());
            }

            return capturedKeywords;
        }

        private static List<ExecutableModel> SortRuntimeExecutables(IEnumerable<ExecutableModel> executables, IEnumerable<string> keywords)
        {
            var sorted =
                  executables
                  .Where(n => n.Keywords
                      .Any(
                          m => keywords
                          .ToList()
                          .Contains(m.KeywordValue)))
                  .Select(n =>
                  {
                      var m = GetTypeByAssemblyName<ExecutableModel>(n.ExecutableFunction.FunctionName);

                      m.ExecutableFunction = n.ExecutableFunction;

                      m.Keywords = n.Keywords;

                      return m;
                  })
                  .ToList();

            return sorted;

        }

       private static T GetTypeByAssemblyName <T>(string TypeName) where T : ExecutableModel
        {
            var AssemblyName = typeof(ResultAnaliser).Assembly.GetName().Name;
           var handle = Activator.CreateInstance(AssemblyName, TypeName);
            return (T)handle.Unwrap();
        }
    }
}
