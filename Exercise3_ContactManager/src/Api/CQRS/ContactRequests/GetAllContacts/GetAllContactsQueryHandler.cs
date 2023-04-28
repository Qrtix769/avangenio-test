using System.Net;
using AutoMapper;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.ContactRequests.GetAllContacts;

public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, (IEnumerable<GetAllContactsDto> contactDtoList, HttpStatusCode status)>
{
	#region variables

	private readonly IContactService _service;
	private readonly IMapper _mapper;

	#endregion

	#region constructors

	public GetAllContactsQueryHandler(IContactService service, IMapper mapper)
	{
		_service = service;
		_mapper = mapper;
	}

	#endregion

	#region handle

	public async Task<(IEnumerable<GetAllContactsDto> contactDtoList, HttpStatusCode status)> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
	{
		var contacts = await _service.GetAllAsync(cancellationToken);

		return (contacts.Select(contact => _mapper.Map<GetAllContactsDto>(contact)), HttpStatusCode.OK);
	}

	#endregion


}