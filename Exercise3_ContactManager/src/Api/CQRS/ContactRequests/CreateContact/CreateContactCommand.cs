using System.Net;
using MediatR;

namespace Api.CQRS.ContactRequests.CreateContact;

public class CreateContactCommand : IRequest<(CreateContactOutputDto? contactDto, HttpStatusCode status, string? message)>
{
	public CreateContactInputDto ContactInputInputDto { get; set; }
	public string UserName { get; set; }

	public CreateContactCommand(CreateContactInputDto contactInputInputDto, string userName)
	{
		ContactInputInputDto = contactInputInputDto;
		UserName = userName;
	}
}