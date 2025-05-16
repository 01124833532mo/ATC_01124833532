using BookEvent.Core.Domain.Common;
using BookEvent.Core.Domain.Contracts.Persestence;
using BookEvent.Infrastructure.Persistence._Data;
using BookEvent.Infrastructure.Persistence.Repositories.Generic_Repository;
using System.Collections.Concurrent;

namespace BookEvent.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ConcurrentDictionary<string, object> _repositories;
        private readonly BookEventDbContext _dbContext;

        public UnitOfWork(BookEventDbContext dbContext)
        {
            _repositories = new ConcurrentDictionary<string, object>();
            _dbContext = dbContext;

        }



        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();

        }

        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : IEquatable<TKey>
        {

            return (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_dbContext));

        }
    }
}
