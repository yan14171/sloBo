using Alexa_proj.Additional_APIs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexa_proj.Tests
{
    class STT_With_AnaliserTests
    {
        public void OneTimeSetup()
        {
            TestContext.WriteLine("One time setup");
        }

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine("Setup");
        }

        [Test]
        public void Get_Executables_Than_Sort_With_DogAndWeather_Keywords_Must_Return_DoggoCheck_AndWeatheCheck()
        {
            var executables =
            SpeechToTextRequester.CreateExecutables();

            var sortedExecutables =
            ResultAnaliser.SortRuntimeExecutables(executables, new List<string>() { "dog", "weather" });

            Assert.IsTrue(sortedExecutables.Count() == 2);

            Assert.IsInstanceOf<DoggoCheck>(sortedExecutables.Last());

            Assert.IsInstanceOf<WeatherCheck>(sortedExecutables.First());
        }

        [Test]
        public async Task Recognise_patientsSpeech_Than_Analised_Must_Be_CoronaCheck()
        {
            var availableExecutables =await new ResultAnaliser().ReadAvailableExecutables();

            var recognisedKeywords = await new SpeechToTextRequester().Recognise(@"Resources/Files/patients.wav");

            var sortedExecutables = ResultAnaliser.SortRuntimeExecutables(availableExecutables, recognisedKeywords);

            CollectionAssert.AllItemsAreInstancesOfType(sortedExecutables, typeof(CoronaCheck));
        }

        [TearDown]
        public void TearDown()
        {
            TestContext.WriteLine("Tear down");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestContext.WriteLine("One time tear down");
        }
    }
}
