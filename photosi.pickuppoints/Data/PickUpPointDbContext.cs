using Microsoft.EntityFrameworkCore;

using PhotoSi.PickupPoint.Models;

namespace PhotoSi.PickupPoint.Data
{
    public class PickUpPointDbContext : DbContext
    {
        public DbSet<PickUpPoint> PickUpPoints { get; set; }

        private static string connectionString;

        public PickUpPointDbContext(DbContextOptions<PickUpPointDbContext> options) : base(options)
        {

        }

        static string GetConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    var config = new ConfigurationBuilder()
                                        .AddJsonFile(Path.Combine(Environment.CurrentDirectory, "appsettings.json"))
                                        .Build();

                    connectionString = config.GetSection("ConnectionStrings")["PickUpPointsDb"];
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
