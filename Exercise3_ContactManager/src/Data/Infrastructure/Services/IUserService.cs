using Domain;

namespace Infrastructure.Services;

public interface IUserService : IService<User>
{
	Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
	Task<bool> IsEmailExistAsync(string email, Guid? id = null, CancellationToken cancellationToken = default);
}