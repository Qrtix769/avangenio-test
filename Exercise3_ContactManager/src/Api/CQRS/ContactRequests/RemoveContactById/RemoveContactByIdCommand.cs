using System.Net;
using MediatR;

namespace Api.CQRS.ContactRequests.RemoveContactById
{
	public class RemoveContactByIdCommand : IRequest<HttpStatusCode>
	{
		public Guid Id { get; set; }

		public RemoveContactByIdCommand(Guid id)
		{
			Id = id;
		}
	}
}
