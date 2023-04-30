using System.Net;
using AutoMapper;
using Domain;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.UserRequests.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, (SignUpOutputDto? userDto, HttpStatusCode status, string? message)>
{
	#region variables

	private readonly IUserService _userService;
	private readonly IMapper _mapper;

	#endregion

	#region constructors

	public SignUpCommandHandler(IMapper mapper, IUserService userService)
	{
		_mapper = mapper;
		_userService = userService;
	}

	#endregion


	public async Task<(SignUpOutputDto? userDto, HttpStatusCode status, string? message)> Handle(SignUpCommand request, CancellationToken cancellationToken)
	{
		if (await _userService.IsEmailExistAsync(request.SignUpInputDto.Email, cancellationToken: cancellationToken))
			return (null, HttpStatusCode.BadRequest,
				$"Exist another user with this email: {request.SignUpInputDto.Email}");

		var user = await _userService.AddAsync(_mapper.Map<User>(request.SignUpInputDto), cancellationToken);

		return (_mapper.Map<SignUpOutputDto>(user), HttpStatusCode.Created, null);
	}
}