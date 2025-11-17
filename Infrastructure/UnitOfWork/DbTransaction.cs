using BookingClinic.Application.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookingClinic.Infrastructure.UnitOfWork
{
    public class DbTransaction : IDbTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public DbTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public Task CommitAsync() => _transaction.CommitAsync();

        public Task RollbackAsync() => _transaction.RollbackAsync();

        public ValueTask DisposeAsync() => _transaction.DisposeAsync();
    }
}
