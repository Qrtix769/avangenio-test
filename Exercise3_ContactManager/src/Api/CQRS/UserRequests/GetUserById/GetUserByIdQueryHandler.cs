using System.Net;
using Api.CQRS.UserRequests.GetUserById;
using AutoMapper;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.UserRequests.GetContactById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, (HttpStatusCode status, GetUserByIdDto? userDto)>
{
	#region variables

	private readonly IUserService _userService;
	private readonly IMapper _mapper;

	#endregion

	#region constructors

	public GetUserByIdQueryHandler(IUserService userService, IMapper mapper)
	{
		_userService = userService;
		_mapper = mapper;
	}

	#endregion

	public async Task<(HttpStatusCode status, GetUserByIdDto? userDto)> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var user = await _userService.GetByIdAsync(request.UserId, cancellationToken);

		return user == null ? (HttpStatusCode.NotFound, null) : (HttpStatusCode.OK, _mapper.Map<GetUserByIdDto>(user));
	}
}