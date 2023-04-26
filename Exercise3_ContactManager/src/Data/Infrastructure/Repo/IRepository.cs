using System.Linq.Expressions;
using Domain;

namespace Infrastructure.Repo;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
	Task<IQueryable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? expression = null,
		CancellationToken cancellationToken = default);

	Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

	Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

	Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

	Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

	Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

	Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

	Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
}