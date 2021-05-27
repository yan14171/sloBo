using Alexa_proj.Additional_APIs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alexa_proj.Tests
{
    class AnaliserTests
    {
        public void OneTimeSetup()
        {
            TestContext.WriteLine("One time setup");
        }
        DoggoCheck doggoCheck;

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine("Setup");
        }

        [Test]
        public void Get_Executables_Than_Sort_With_DogAndWeather_Keywords_Must_Return_DoggoCheck_AndWeatheCheck()
        {
            var executables = 
            SpeechToTextRequester.CreateStaticExecutables();

            var sortedExecutables = 
            ResultAnaliser.SortRuntimeExecutables(executables, new List<string>() { "dog", "weather" });

            Assert.IsTrue(sortedExecutables.Count() == 2);

            Assert.IsInstanceOf<DoggoCheck>(sortedExecutables.Last());

            Assert.IsInstanceOf<WeatherCheck>(sortedExecutables.First());
        }

        [Test]
        public async Task Get_Executables_Than_There_Are_More_Than_Four_Of_Them()
        {
            var executables = await
            new ResultAnaliser().ReadAvailableExecutables();

            Assert.IsTrue(executables.Count() >= 4);
        }

        [TearDown]
        public void TearDown()
        {
            TestContext.WriteLine("TearDown");
        }

        [OneTimeSetUp]
        public void OneTimeTearDown()
        {
            TestContext.WriteLine("One time TearDown");
        }
    }
}






