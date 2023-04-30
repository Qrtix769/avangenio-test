using Domain;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public class ContactService : Service<Contact>, IContactService
{
	#region variables

	private readonly IUnitOfWork _unitOfWork;

	#endregion

	#region consntructor

	public ContactService(IServiceProvider provider) : base(provider)
	{
		var scoped = provider.CreateScope();
		_unitOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();
	}
	
	#endregion

	#region IContactService implementation

	public async Task<IEnumerable<Contact>> GetAllByOwnerAsync(Guid ownerId,
		CancellationToken cancellationToken = default)
		=> await _unitOfWork.Repository<Contact>().GetManyAsync(c => c.Owner == ownerId, cancellationToken);

	public async Task<Contact?> GetContactByIdAndOwnerAsync(Guid contactId, Guid ownerId,
		CancellationToken cancellationToken = default)
		=> await _unitOfWork.Repository<Contact>()
			.GetAsync(c => c.Id == contactId && c.Owner == ownerId, cancellationToken);

	#endregion
}
