using System.Net;
using MediatR;

namespace Api.CQRS.ContactRequests.UpdateContact
{
	public class UpdateContactCommand : IRequest<HttpStatusCode>
	{
		public UpdateContactInputDto ContactDto { get; set; }
		public Guid Id { get; set; }

		public UpdateContactCommand(UpdateContactInputDto contactDto, Guid id)
		{
			ContactDto = contactDto;
			Id = id;
		}
	}
}
