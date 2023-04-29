using System.Net;
using Api.CQRS.UserRequests.GetUserById;
using MediatR;

namespace Api.CQRS.UserRequests.GetContactById;

public class GetUserByIdQuery : IRequest<(HttpStatusCode status, GetUserByIdDto? userDto)>
{
	public Guid UserId { get; set; }

	public GetUserByIdQuery(Guid userId)
	{
		UserId = userId;
	}

}