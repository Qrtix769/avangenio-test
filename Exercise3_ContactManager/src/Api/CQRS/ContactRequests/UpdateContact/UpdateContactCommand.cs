using System.Net;
using MediatR;

namespace Api.CQRS.ContactRequests.UpdateContact
{
	public class UpdateContactCommand : IRequest<(UpdateContactOutputDto? contactOutputDto, HttpStatusCode status, string? message)>
	{
		public UpdateContactInputDto ContactDto { get; }
		public Guid Id { get; }

		public UpdateContactCommand(UpdateContactInputDto contactDto, Guid id)
		{
			ContactDto = contactDto;
			Id = id;
		}
	}
}
