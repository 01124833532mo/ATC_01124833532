﻿using BookEvent.Core.Domain.Common;
using BookEvent.Core.Domain.Contracts.Persestence;
using BookEvent.Core.Domain.Contracts.Specifications;
using BookEvent.Infrastructure.Persistence._Common;
using BookEvent.Infrastructure.Persistence._Data;
using Microsoft.EntityFrameworkCore;

namespace BookEvent.Infrastructure.Persistence.Repositories.Generic_Repository
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
         where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>

    {
        private readonly BookEventDbContext _dbContext;

        public GenericRepository(BookEventDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> Spec, bool WithTraching = false, CancellationToken cancellationToken = default)
        {
            return WithTraching ? await ApplySpecifications(Spec).ToListAsync(cancellationToken) : await ApplySpecifications(Spec).AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, TKey> spec, CancellationToken cancellationToken)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(ISpecification<TEntity, TKey> spec, CancellationToken cancellationToken)
        {
            return await ApplySpecifications(spec).CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool WithTraching = false)
        {
            return WithTraching ? await _dbContext.Set<TEntity>().ToListAsync() : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }







        public async Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);


        }


        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }


        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }


        private IQueryable<TEntity> ApplySpecifications(ISpecification<TEntity, TKey> Spec)
        {
            return SpecificationEvaluator<TEntity, TKey>.GetQuery(_dbContext.Set<TEntity>(), Spec);
        }

        public IQueryable<TEntity> GetQuarable()
        {
            return _dbContext.Set<TEntity>();
        }
    }
}
