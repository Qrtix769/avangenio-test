using Domain;

namespace Infrastructure.Services;

public interface IContactService : IService<Contact>
{
	Task<IEnumerable<Contact>> GetAllByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);
	Task<Contact?> GetContactByIdAndOwnerAsync(Guid contactId, Guid ownerId, CancellationToken cancellationToken = default);
}
