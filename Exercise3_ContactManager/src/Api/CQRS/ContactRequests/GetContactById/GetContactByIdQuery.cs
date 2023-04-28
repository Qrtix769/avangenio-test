using System.Net;
using MediatR;

namespace Api.CQRS.ContactRequests.GetContactById;

public class GetContactByIdQuery : IRequest<(GetContactByIdDto? contactDto, HttpStatusCode status)>
{
	public Guid Id { get; set; }

	public GetContactByIdQuery(Guid id)
	{
		Id = id;
	}
}