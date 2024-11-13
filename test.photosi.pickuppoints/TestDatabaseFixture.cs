using Microsoft.EntityFrameworkCore;

using PhotoSi.PickupPoint.Data;
using PhotoSi.PickupPoint.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Test.PhotoSi.PickupPoints
{
    public class TestDatabaseFixture
    {
        private const string ConnectionString = @"Server=localhost,1433; Database=pickuppoints_db; User Id=sa; Password=myPassword1!; TrustServerCertificate=True;";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDatabaseFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        context.AddRange(
                                new PickUpPoint
                                {
                                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                                    Name = "Cartoleria Jolly",
                                    Address = "Via dei quaderni, 1",
                                    City = "Quaderni",
                                    ZipCode = "37046",
                                    Active = "S"
                                },
                                new PickUpPoint
                                {
                                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                                    Name = "IperFamila Cologna Veneta",
                                    Address = "Viale dei fiori, 1",
                                    City = "Cologna Veneta",
                                    ZipCode = "37050",
                                    Active = "S"
                                },
                                new PickUpPoint
                                {
                                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                                    Name = "IperMercato ",
                                    Address = "Viale dei ciliegi, 10",
                                    City = "Cologna Veneta",
                                    ZipCode = "37050",
                                    Active = "N"
                                }
                            );

                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public PickUpPointDbContext CreateContext()
            => new PickUpPointDbContext(
                new DbContextOptionsBuilder<PickUpPointDbContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);
    }
}
