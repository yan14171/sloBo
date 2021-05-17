using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Alexa_proj.Data_Control.Models;

namespace Alexa_proj.Data_Control
{
#nullable enable

    public class FunctionalContext : DbContext
    {
        public FunctionalContext(DbContextOptions<FunctionalContext> options)
            : base(options)
        { }

        public DbSet<Keyword> Keywords { get; set; }

        public DbSet<ExecutableModel> Executables { get; set; }

        public DbSet<Function> Functions { get; set; }

        public DbSet<FunctionResult> FunctionResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Used to provide information about things like inheritance
            //modelBuilder.Entity<MinifigHead>().HasBaseType<Brick>();
            //modelBuilder.Entity<BasePlate>().HasBaseType<Brick>();

        }

    }

    public class FunctionalContextFactory : IDesignTimeDbContextFactory<FunctionalContext>
    {
        public FunctionalContext CreateDbContext(string[]? args = null)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var optionsBuilder = new DbContextOptionsBuilder<FunctionalContext>();
            optionsBuilder
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
                .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

            return new FunctionalContext(optionsBuilder.Options);
        }
    }

#nullable disable
}
