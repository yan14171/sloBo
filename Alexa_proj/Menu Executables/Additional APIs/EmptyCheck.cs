using Alexa_proj.Data_Control;
using Alexa_proj.Data_Control.Models;
using Alexa_proj.Repositories;
using DrawRectangle;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alexa_proj.Additional_APIs
{
    public class EmptyCheck: ExecutableModel
    {
        public override async Task Execute()
        {

            var notFoundPage =
                 new ConsoleRectangle(
                        20, 3, new DrawRectangle.Point() { X = 7, Y = 1 },
                        ConsoleColor.Black,
                         new string[] { "Sorry, I couldn't find anything on your request.\nHere are my current functions:" },
                        0
                        );

            SetUpTable(notFoundPage);

            notFoundPage.BorderColor = ConsoleColor.Green;

            var Executables = await GetExecutables();

            SetUpExecutables(notFoundPage, Executables);
        }

        private void SetUpExecutables(ConsoleRectangle notFoundPage, IEnumerable<ExecutableModel> executables)
        {
            foreach (var item in executables)
            {
                notFoundPage.Width = 25;
                notFoundPage.Location.X = 1;
                notFoundPage.FileText =
                    $"{item.ExecutableName}";
                StartUp.CurrentMenu.DynamicShow(
                notFoundPage
                               );
                notFoundPage.Width = 20;
                notFoundPage.Location.X = 25;
                notFoundPage.FileText =
                    $"{item.Keywords[0].KeywordValue}";
                StartUp.CurrentMenu.DynamicShow(
                notFoundPage
                               );
                notFoundPage.Location.Y += 5;
                notFoundPage.FileTextY += 5;
            }
        }

        private void SetUpTable(ConsoleRectangle notFoundPage)
        {
            notFoundPage.BorderColor = ConsoleColor.Black;
            notFoundPage.Width = 25;
            notFoundPage.Location.X = 1;
            notFoundPage.FileText =
                $"Name:";
            StartUp.CurrentMenu.DynamicShow(
            notFoundPage
                           );
            notFoundPage.Width = 30;
            notFoundPage.Location.X = 25;
            notFoundPage.FileText =
                $"Supreme keyword:";
            StartUp.CurrentMenu.DynamicShow(
            notFoundPage
                           );
            notFoundPage.Location.Y += 4;
            notFoundPage.FileTextY += 4;
        }

        private async Task<List<ExecutableModel>> GetExecutables()
        {
            List<ExecutableModel> models;

             using (var unitOfWork = new UnitOfWork(new FunctionalContextFactory().CreateDbContext()))
            {
                models = (await (unitOfWork.Executables as ExecutableRepository)
                    .GetStaticExecutablesWithKeywordsAsync())
                    .ToList();
            }

            return models;
        }
    }

}
