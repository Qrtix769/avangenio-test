using System.Net;
using AutoMapper;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.ContactRequests.RemoveContactById
{
	public class RemoveContactByIdCommandHandler : IRequestHandler<RemoveContactByIdCommand, HttpStatusCode>
	{
		#region variables

		private readonly IContactService _service;
		private readonly IMapper _mapper;

		#endregion

		#region constructors

		public RemoveContactByIdCommandHandler(IContactService service, IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}

		#endregion

		#region handle

		public async Task<HttpStatusCode> Handle(RemoveContactByIdCommand request, CancellationToken cancellationToken)
		{
			try
			{
				await _service.RemoveByIdAsync(request.Id, cancellationToken);
			}
			catch (ArgumentException)
			{
				return HttpStatusCode.NotFound;
			}

			return HttpStatusCode.OK;
		}

		#endregion
	}
}
