using System.Linq.Expressions;
using Domain;

namespace Infrastructure.Services;

public interface IService<TEntity> where TEntity : BaseEntity
{
	Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

	Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

	Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

	Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

	Task RemoveByIdAsync(Guid id, CancellationToken cancellationToken = default);
}