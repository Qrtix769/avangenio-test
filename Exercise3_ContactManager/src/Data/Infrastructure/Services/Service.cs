using Domain;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public class Service<TEntity> : IService<TEntity> where TEntity : BaseEntity
{
	#region variables
	private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region constructors
    public Service(IServiceProvider serviceProvider)
    {
		var scoped = serviceProvider.CreateScope();
		_unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();
	}
	#endregion

	#region IService implementation
	public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		var result = await _unitOfWork.Repository<TEntity>().AddAsync(entity, cancellationToken);
		await _unitOfWork.SaveAsync(cancellationToken);
		return result;
	}

	public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
		=> await _unitOfWork.Repository<TEntity>().GetManyAsync(cancellationToken: cancellationToken);

	public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		=> await _unitOfWork.Repository<TEntity>().GetAsync(e => e.Id == id, cancellationToken: cancellationToken);
	
	public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var entity = await _unitOfWork.Repository<TEntity>().GetAsync(e => e.Id == id, cancellationToken) 
			?? throw new ArgumentException($"Does not exist {nameof(TEntity)} with Id {id}");

		await _unitOfWork.Repository<TEntity>().RemoveAsync(entity, cancellationToken);
		await _unitOfWork.SaveAsync(cancellationToken);
	}

	public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		var result = await _unitOfWork.Repository<TEntity>().UpdateAsync(entity, cancellationToken);
		await _unitOfWork.SaveAsync(cancellationToken);
		return result;
	}
	#endregion

}