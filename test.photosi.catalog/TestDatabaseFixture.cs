using Microsoft.EntityFrameworkCore;

using PhotoSi.Catalog.Data;
using PhotoSi.Catalog.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Test.PhotoSi.Catalog
{
    public class TestDatabaseFixture
    {
        private const string ConnectionString = "Server=localhost,1433; Database=catalog_db; User Id=sa; Password=myPassword1!; TrustServerCertificate=True;";
            //@"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Catalog;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

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
                                new Categoria
                                {
                                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                                    Name = "Foto",
                                    Active = "S"
                                },
                                new Categoria
                                {
                                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                                    Name = "FotoLibro",
                                    Active = "S"
                                },
                                new Categoria
                                {
                                    Id = new Guid("10000000-0000-0000-0000-000000000002"),
                                    Name = "FotoLibro Vecchio",
                                    Active = "N"
                                }
                            );

                        context.AddRange(
                                new Prodotto
                                {
                                    Id = new Guid("00000000-0000-0000-0000-100000000001"),
                                    Name = "Foto 13x18",
                                    Codice = "FOTO13X18",
                                    Price = 0.5m,
                                    CategoriaId = new Guid("00000000-0000-0000-0000-000000000001"),
                                    Active = "S"
                                },
                                new Prodotto
                                {
                                    Id = new Guid("00000000-0000-0000-0000-100000000002"),
                                    Name = "Foto 10x15",
                                    Codice = "FOTO10X15",
                                    Price = 0.4m,
                                    CategoriaId = new Guid("00000000-0000-0000-0000-000000000001"),
                                    Active = "S"
                                },
                                new Prodotto
                                {
                                    Id = new Guid("00000000-0000-0000-0000-200000000001"),
                                    Name = "FotoLibro 13x18 10 pagine",
                                    Codice = "FOTOLIBRO13X18-10",
                                    Price = 15m,
                                    CategoriaId = new Guid("00000000-0000-0000-0000-000000000002"),
                                    Active = "S"
                                },
                                new Prodotto
                                {
                                    Id = new Guid("00000000-0000-0000-0000-200000000002"),
                                    Name = "FotoLibro 13x18 20 pagine",
                                    Codice = "FOTOLIBRO13X18-20",
                                    Price = 18m,
                                    CategoriaId = new Guid("00000000-0000-0000-0000-000000000002"),
                                    Active = "S"
                                }
                            );

                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public CatalogDbContext CreateContext()
            => new CatalogDbContext(
                new DbContextOptionsBuilder<CatalogDbContext>()
                    .UseSqlServer(ConnectionString)
                    .Options);
    }
}
