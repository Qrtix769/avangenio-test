using System.Net;
using MediatR;

namespace Api.CQRS.ContactRequests.GetAllContacts;

public class GetAllContactsQuery : IRequest<(IEnumerable<GetAllContactsDto>? contactDtoList, HttpStatusCode status, string? message)>
{
	public string UserName { get; set; }

	public GetAllContactsQuery(string userName)
	{
		UserName = userName;
	}
}