using BookEvent.Core.Domain.Common;
using BookEvent.Core.Domain.Contracts.Specifications;

namespace BookEvent.Core.Domain.Contracts.Persestence
{
    public interface IGenericRepository<TEntity, TKey>
       where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {


        IQueryable<TEntity> GetQuarable();
        Task<IEnumerable<TEntity>> GetAllAsync(bool WithTraching = false);

        Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> Spec, bool WithTraching = false, CancellationToken cancellationToken = default);

        Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, TKey> spec, CancellationToken cancellationToken);

        Task<int> GetCountAsync(ISpecification<TEntity, TKey> spec, CancellationToken cancellationToken);


        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

    }
}
