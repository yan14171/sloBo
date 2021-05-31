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
        [OneTimeSetUp]
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
        public async Task Get_Executables_Than_There_Are_More_Than_Three_Of_Them()
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

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestContext.WriteLine("One time TearDown");
        }
    }
}






