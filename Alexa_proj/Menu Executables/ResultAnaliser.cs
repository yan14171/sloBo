using Alexa_proj.Additional_APIs;
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




            //Analising before this point

            Animation.StopAnimation();

            //has to execute all functions here!!


        }         

       private static T GetTypeByAssemblyName <T>(string TypeName)
        {
            var AssemblyName = typeof(T).Assembly.GetName().Name;
           var handle = Activator.CreateInstance(AssemblyName, TypeName);
            return (T)handle.Unwrap();
        }
    }
}
