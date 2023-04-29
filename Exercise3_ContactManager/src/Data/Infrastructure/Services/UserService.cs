using Domain;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public class UserService : Service<User>, IUserService
{
	#region variables

	private readonly IUnitOfWork _unitOfWork;

	#endregion

	#region consntructor

	public UserService(IServiceProvider provider) : base(provider)
	{
		var scoped = provider.CreateScope();
		_unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();
	}

	#endregion

	#region IUserService impl

	public async Task<User> GetByUserNameAsync(string userName)
		=> await _unitOfWork.Repository<User>().GetAsync(u => u.UserName == userName) 
		   ?? throw new ArgumentException($"Does not exist user with 'UserName': {userName}");

	public async Task<bool> IsEmailExistAsync(string email, Guid? id = null)
		=> await _unitOfWork.Repository<User>()
			.GetAsync(u => id == null ? u.Email == email : u.Email == email && u.Id != id) != null;

	#endregion
}