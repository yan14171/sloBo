using NUnit.Framework;
using Alexa_proj; 
using System;
using System.Collections.Generic;
using Alexa_proj.Additional_APIs;
using System.Threading.Tasks;
using System.Reflection;
using Alexa_proj.Data_Control.Models;
using System.Linq;

namespace Alexa_proj.Tests
{
    public class APIsTests
    {
        ExecutableModel Weathermodel;

        public void OneTimeSetup()
        {
            TestContext.WriteLine("One time setup");
        }

        [SetUp]
        public void Setup()
        {
           TestContext.WriteLine("Setup");

            Weathermodel = new WeatherCheck()
            {
                ExecutableName = "WeatherExecutable",

                Keywords = new List<Keyword>(),

                ExecutableFunction = new Function()
                {
                    FunctionEndpoint = "http://api.openweathermap.org/data/2.5/weather?q=Kyiw",
                    FunctionName = "Alexa_proj.Additional_APIs.WeatherCheck",
                    FunctionResult = new FunctionResult() { ResultValue = string.Empty }
                },

            };
        }

        [Test]
        public async Task Get_WeatherExecutable_Unfo_Than_There_Should_Be_No_Nulls()
        {
            var info = await
            Weathermodel
            .GetInfo < WeatherInfo > ();

            Assert.IsTrue(!IsAnyNullOrEmpty(info));
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

        public static bool IsAnyNullOrEmpty(object obj)
        {
            //Step 1: Set the result variable to false;
            bool result = false;

            try
            {
                //Step 2: Check if the incoming object has values or not.
                if (obj != null)
                {
                    //Step 3: Iterate over the properties and check for null values based on the type.
                    foreach (PropertyInfo pi in obj.GetType().GetProperties())
                    {
                        //Step 4: The null check condition only works if the value of the result is false, whenever the result gets true, the value is returned from the method.
                        if (result == false)
                        {
                            //Step 5: Different conditions to satisfy different types
                            dynamic value;
                            if (pi.PropertyType == typeof(string))
                            {
                                value = (string)pi.GetValue(obj);
                                result = (string.IsNullOrEmpty(value) ? true : false || string.IsNullOrWhiteSpace(value) ? true : false);
                            }
                            else if (pi.PropertyType == typeof(int))
                            {
                                value = (int)pi.GetValue(obj);
                                result = (value <= 0 ? true : false || value == null ? true : false);
                            }
                            else if (pi.PropertyType == typeof(bool))
                            {
                                value = pi.GetValue(obj);
                                result = (value == null ? true : false);
                            }
                            else if (pi.PropertyType == typeof(Guid))
                            {
                                value = pi.GetValue(obj);
                                result = (value == Guid.Empty ? true : false || value == null ? true : false);
                            }
                        }
                        //Step 6 - If the result becomes true, the value is returned from the method.
                        else
                            return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Step 7: If the value doesn't become true at the end of foreach loop, the value is returned.
            return result;
        }
    }
}