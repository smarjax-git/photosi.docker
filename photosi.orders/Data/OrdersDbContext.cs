using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using PhotoSi.Orders.Models;

using System.Data;

namespace PhotoSi.Orders.Data
{
    public class OrdersDbContext : DbContext
    {
        public DbSet<Ordine> Ordini { get; set; }
        public DbSet<RigaOrdine> OrdiniRighe { get; set; }

        private static string connectionString;

        private IDbContextTransaction _currentTransaction;

        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
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

                    connectionString = config.GetSection("ConnectionStrings")["OrdersDb"];
                }

                return connectionString;
            }
        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();

                await (_currentTransaction?.CommitAsync() ?? Task.CompletedTask);
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
