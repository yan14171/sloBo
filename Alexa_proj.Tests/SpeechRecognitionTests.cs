using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alexa_proj;
using System.Diagnostics;

namespace Alexa_proj.Tests
{
    class SpeechRecognitionTests
    {
        protected SpeechToTextRequester _requester { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestContext.WriteLine("One time setup");

            _requester = new SpeechToTextRequester();

            // await Task.Run(() => _requester.Recognise(@"Resources/Files/dog.wav"));
        }

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine("Setup");
        }

        [TestCase("weather")]
        [TestCase("dogo")]
        [TestCase("patients")]
        [TestCase("dog")]
        [TestCase("corona")]
        //[TestCase("doggo box")]
        [TestCase("give me a dog")]
        [Test]
        public async Task Requester_Recognise_On_Test_File_Than_Return_File_Name(string fileName)
        {
            var RecogniseResult = await _requester.Recognise($@"Resources/Files/{fileName}.wav");

            Assert.AreEqual(RecogniseResult.First(), fileName);
        }

        [Test]
        public void CreateExecutables_Returns_More_Than_4_Executables()
        {
           var executablesList = SpeechToTextRequester.CreateExecutables();

            Assert.Greater(executablesList.Count(), 3);
        }

        [TearDown]
        public void TearDown()
        {
            TestContext.WriteLine("TearDown");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestContext.WriteLine("One time TearDown");
        }
    }
}
