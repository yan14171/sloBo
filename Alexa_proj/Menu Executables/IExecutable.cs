using System;
namespace Alexa_proj
{
    [Serializable]
    public class Executable
    {
        
        public virtual void Execute()
        {
            throw new NotImplementedException();
        }
    }
    
    [Serializable]
    public class ApiExecutable : Executable
    {

        public override void Execute()
        {

            throw new NotImplementedException();
        
        }
    
    }
}
