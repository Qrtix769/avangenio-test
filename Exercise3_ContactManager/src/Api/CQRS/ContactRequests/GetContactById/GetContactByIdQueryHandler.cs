using System.Net;
using AutoMapper;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.ContactRequests.GetContactById;

public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, (GetContactByIdDto? contactDto, HttpStatusCode status, string? message)>
{
	#region variables

	private readonly IContactService _contactService;
	private readonly IUserService _userService;
	private readonly IMapper _mapper;

	#endregion

	#region constructors

	public GetContactByIdQueryHandler(IContactService contactService, IMapper mapper, IUserService userService)
	{
		_contactService = contactService;
		_mapper = mapper;
		_userService = userService;
	}

	#endregion

	#region handle

	public async Task<(GetContactByIdDto? contactDto, HttpStatusCode status, string? message)> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var user = await _userService.GetByUserNameAsync(request.UserName, cancellationToken);

			var contact = await _contactService.GetContactByIdAndOwnerAsync( request.Id, user.Id, cancellationToken);

			return contact is not null
				? (_mapper.Map<GetContactByIdDto>(contact), HttpStatusCode.OK, null)
				: (null, HttpStatusCode.NotFound, $"Does not exist contact with id: {request.Id}");
		}
		catch (ArgumentException ex)
		{
			return (null, HttpStatusCode.NotFound, $"Does not exist user with name: {request.UserName}");
		}
		
		
	}

	#endregion
}