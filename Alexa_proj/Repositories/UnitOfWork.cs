using Alexa_proj.Data_Control;
using Alexa_proj.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alexa_proj.Repositories
{ 
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FunctionalContext _context;

        public UnitOfWork(FunctionalContext context)
        {
            _context = context;
            Executables = new ExecutableRepository(_context);
        }

        public IExecutableRepository Executables { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
