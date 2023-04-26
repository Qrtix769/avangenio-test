using Domain;

namespace Infrastructure.Services;

public class ContactService : Service<Contact>, IContactService
{
	public ContactService(IServiceProvider serviceProvider) : base(serviceProvider) { }
}
