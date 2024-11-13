using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using PhotoSi.Users.Models;

using System.Data;

namespace PhotoSi.Users.Data
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPickupPoint> UserPickupPoints { get; set; }

        //private static string connectionString;

        private IDbContextTransaction _currentTransaction;

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {

        }

        //static string GetConnectionString
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(connectionString))
        //        {
        //            var config = new ConfigurationBuilder()
        //                                .AddJsonFile(Path.Combine(Environment.CurrentDirectory, "appsettings.json"))
        //                                .Build();

        //            connectionString = config.GetSection("ConnectionStrings")["UsersDb"];
        //        }

        //        return connectionString;
        //    }
        //}

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
