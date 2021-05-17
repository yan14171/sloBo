﻿using Alexa_proj.Data_Control;
using Alexa_proj.Data_Control.Models;
using Alexa_proj.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Alexa_proj.Repositories
{
    public class ExecutableRepository : Repository<ExecutableModel>, IExecutableRepository
    {
        private readonly FunctionalContext _functionalContext;

        public ExecutableRepository(FunctionalContext context) : base(context)
        {
            _functionalContext = context;
        }

        public IEnumerable<ExecutableModel> GetStaticExecutables()
        {
            var returnedExecutables = 
                _functionalContext.Executables
                 .Include(n => n.ExecutableFunction)
                 .Where(n => n.ExecutableFunction != null);

            return returnedExecutables;
        }
    }
}
