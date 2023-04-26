using System.Linq.Expressions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repo;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
	#region variables

	private readonly DbSet<TEntity> _dbSet;
	private readonly DbContext _context;

	#endregion

	#region constructors

	public Repository(DbContext context)
	{
		_context = context;
		_dbSet = context.Set<TEntity>();
	}

	#endregion

	#region IRepository implementation

	public Task<IQueryable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>>? expression = null,
		CancellationToken cancellationToken = default)
		=> Task.Run(() => expression == null ? _dbSet : _dbSet.Where(expression), cancellationToken);
	
	public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
		=> _dbSet.FirstOrDefaultAsync(expression, cancellationToken: cancellationToken);


	public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		var entry = await _dbSet.AddAsync(entity, cancellationToken);
		return entry.Entity;
	}

	public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
		=> _dbSet.AddRangeAsync(entities, cancellationToken);


	public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
		=> Task.Run(() =>
		{
			var entry = _dbSet.Attach(entity);
			entry.State = EntityState.Modified;
			return entry.Entity;
		}, cancellationToken);

	public Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
		=> Task.Run(() =>
		{
			_dbSet.AttachRange(entities);
			foreach (var entity in entities)
			{
				_context.Entry(entity).State = EntityState.Modified;
			}
		}, cancellationToken);

	public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
		=> Task.Run(() => _dbSet.Remove(entity), cancellationToken);

	public Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
		=> Task.Run(() => _dbSet.RemoveRange(entities), cancellationToken);


	#endregion
}