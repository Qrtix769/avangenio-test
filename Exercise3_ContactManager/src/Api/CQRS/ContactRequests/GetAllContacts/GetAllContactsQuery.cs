using System.Net;
using MediatR;

namespace Api.CQRS.ContactRequests.GetAllContacts;

public class GetAllContactsQuery : IRequest<(IEnumerable<GetAllContactsDto> contactDtoList, HttpStatusCode status)>
{
	//public Guid UserId { get; set; }

	//public GetAllContactsQuery(Guid userId)
	//{
	//	UserId = userId;
	//}
}