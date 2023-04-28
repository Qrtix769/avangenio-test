using System.Net;
using MediatR;

namespace Api.CQRS.ContactRequests.CreateContact;

public class CreateContactCommand : IRequest<(CreateContactOutputDto? contactDto, HttpStatusCode status)>
{
	public CreateContactInputDto ContactInputInputDto { get; set; }

	public CreateContactCommand(CreateContactInputDto contactInputInputDto)
	{
		ContactInputInputDto = contactInputInputDto;
	}
}