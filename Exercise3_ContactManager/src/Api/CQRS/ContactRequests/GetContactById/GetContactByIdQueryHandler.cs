using System.Net;
using AutoMapper;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.ContactRequests.GetContactById;

public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, (GetContactByIdDto? contactDto, HttpStatusCode status)>
{
	#region variables

	private readonly IContactService _service;
	private readonly IMapper _mapper;

	#endregion

	#region constructors

	public GetContactByIdQueryHandler(IContactService service, IMapper mapper)
	{
		_service = service;
		_mapper = mapper;
	}

	#endregion

	#region handle

	public async Task<(GetContactByIdDto? contactDto, HttpStatusCode status)> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
	{
		var contact = await _service.GetByIdAsync(request.Id, cancellationToken);

		return contact is not null
			? (_mapper.Map<GetContactByIdDto>(contact), HttpStatusCode.OK)
			: (null, HttpStatusCode.NotFound);
	}

	#endregion
}