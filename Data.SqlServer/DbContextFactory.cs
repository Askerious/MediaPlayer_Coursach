using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.SqlServer
{
    public class DbContextFactory : IDesignTimeDbContextFactory<MediaDbContext>
    {
        public MediaDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.database.json")
            .Build();
            return CreateDbContext(configuration);
        }

        public MediaDbContext CreateDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<MediaDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new MediaDbContext(optionsBuilder.Options);
        }
    }
}
