using NUnit.Framework;
using Alexa_proj; 
using System;
using System.Collections.Generic;
using Alexa_proj.Additional_APIs;

namespace Alexa_proj.Tests
{
    public class APIsTests
    {

        DoggoCheck doggoCheck;

        [SetUp]
        public void Setup()
        {
           TestContext.WriteLine("Setup");
           doggoCheck = new DoggoCheck();
        }

    

        [TearDown]
        public void TearDown()
        {
            TestContext.WriteLine("TearDown");
        }
    }
}