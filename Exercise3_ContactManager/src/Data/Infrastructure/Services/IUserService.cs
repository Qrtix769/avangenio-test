using Domain;

namespace Infrastructure.Services;

public interface IUserService : IService<User>
{
	Task<User> GetByUserNameAsync(string userName);
	Task<bool> IsEmailExistAsync(string email, Guid? id = null);
}