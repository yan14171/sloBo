using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa_proj.DataAccess.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IExecutableRepository Executables { get; }

        int Complete();
    }
}
