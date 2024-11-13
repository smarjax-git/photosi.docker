using Microsoft.EntityFrameworkCore;

using PhotoSi.Catalog.Models;

namespace PhotoSi.Catalog.Data
{
    public class CatalogDbContext : DbContext
    {
        public DbSet<Prodotto> Prodotti { get; set; }
        public DbSet<Categoria> Categorie { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {

        }

        private static string connectionString;

        static string GetConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    var config = new ConfigurationBuilder()
                                        .AddJsonFile(Path.Combine(Environment.CurrentDirectory, "appsettings.json"))
                                        .Build();

                    connectionString = config.GetSection("ConnectionStrings")["CatalogDb"];
                }

                return connectionString;
            }
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(GetConnectionString);
        //}
    }
}
