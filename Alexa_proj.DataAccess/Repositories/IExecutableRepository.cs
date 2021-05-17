using Alexa_proj.Data_Control.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa_proj.DataAccess.Repositories
{
    public interface IExecutableRepository : IRepository<ExecutableModel>
    {
        IEnumerable<ExecutableModel> GetStaticExecutables();
    }
}
