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


        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestContext.WriteLine("One time setup");
        }

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine("Setup");
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
