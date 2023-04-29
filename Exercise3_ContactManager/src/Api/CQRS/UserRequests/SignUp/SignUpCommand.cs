using System.Net;
using MediatR;

namespace Api.CQRS.UserRequests.SignUp;

public class SignUpCommand : IRequest<(SignUpOutputDto? userDto, HttpStatusCode status, string? message)>
{
	public SignUpInputDto SignUpInputDto { get; set; }

	public SignUpCommand(SignUpInputDto signUpInputDto)
	{
		SignUpInputDto = signUpInputDto;
	}

}