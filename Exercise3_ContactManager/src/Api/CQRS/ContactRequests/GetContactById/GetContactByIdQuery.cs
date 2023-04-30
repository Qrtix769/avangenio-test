using System.Net;
using MediatR;

namespace Api.CQRS.ContactRequests.GetContactById;

public class GetContactByIdQuery : IRequest<(GetContactByIdDto? contactDto, HttpStatusCode status, string? message)>
{
	public Guid Id { get; set; }
	public string UserName { get; set; }

	public GetContactByIdQuery(Guid id, string userName)
	{
		Id = id;
		UserName = userName;
	}
}