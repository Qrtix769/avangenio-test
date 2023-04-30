using System.Net;
using AutoMapper;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.ContactRequests.GetAllContacts;

public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, (IEnumerable<GetAllContactsDto>? contactDtoList, HttpStatusCode status, string? message)>
{
	#region variables

	private readonly IContactService _contactService;
	private readonly IUserService _userService;
	private readonly IMapper _mapper;

	#endregion

	#region constructors

	public GetAllContactsQueryHandler(IContactService contactService, IMapper mapper, IUserService userService)
	{
		_contactService = contactService;
		_mapper = mapper;
		_userService = userService;
	}

	#endregion

	#region handle

	public async Task<(IEnumerable<GetAllContactsDto>? contactDtoList, HttpStatusCode status, string? message)> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var user = await _userService.GetByUserNameAsync(request.UserName, cancellationToken);

			var contacts = await _contactService.GetAllByOwnerAsync(user.Id, cancellationToken);

			return (contacts.Select(contact => _mapper.Map<GetAllContactsDto>(contact)), HttpStatusCode.OK, null);
		}
		catch (ArgumentException ex)
		{
			return (null, HttpStatusCode.NotFound, $"Does not exist user with name: {request.UserName}");
		}
	}

	#endregion
}