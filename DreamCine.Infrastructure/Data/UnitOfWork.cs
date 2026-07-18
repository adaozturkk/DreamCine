using DreamCine.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Storage;

namespace DreamCine.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No active transaction found.");
            }

            try
            {
                await _transaction.CommitAsync();

                _transaction.Dispose();
                _transaction = null;
            }
            catch (Exception)
            {
                await RollbackTransactionAsync();
                throw;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();

                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}
