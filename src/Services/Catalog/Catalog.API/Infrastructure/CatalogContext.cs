using System.IO;
using Microsoft.EntityFrameworkCore;
using Catalog.API.Model;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Catalog.API.Infrastructure
{
    public class CatalogContext:DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
            
        }
        public  DbSet<CatalogItem>CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public  DbSet<CatalogType> CatalogTypes { get; set; }
    }

    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration["ConnectionString"];
            
            var optionsBuilder =  new DbContextOptionsBuilder<CatalogContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new CatalogContext(optionsBuilder.Options);
        }
    }
}