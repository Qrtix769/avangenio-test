using Domain;
using Infrastructure.Repo;

namespace Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
	IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
	Task<int> SaveAsync(CancellationToken cancellationToken);
}