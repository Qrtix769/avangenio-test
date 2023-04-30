using System.Net;
using System.Text.Json;
using Api.CQRS.UserRequests.GetUserById;
using Api.CQRS.UserRequests.SignUp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : Controller
{
	#region variables

	private readonly IMediator _mediator;

	#endregion

	#region constructors

	public UserController(IMediator mediator)
	{
		_mediator = mediator;
	}

	#endregion

	#region POST endpoints

	[HttpPost]
	[Route("")]
	public async Task<IActionResult> SignUp(SignUpInputDto userDto)
	{
		var command = new SignUpCommand(userDto);
		var response = await _mediator.Send(command);

		return response switch
		{
			{ status: HttpStatusCode.BadRequest } => BadRequest(JsonSerializer.Serialize(new
				{ ErrorMessage = response.message, User = userDto })),

			{ status: HttpStatusCode.Created, userDto: not null } => Created($"/api/users/{response.userDto.Id}",
				response.userDto),

			_ => BadRequest()
		};
	}

	#endregion

	#region GET endpoints

	[HttpGet]
	[Authorize]
	[Route("{id:guid}")]
	public async Task<IActionResult> GetUserById(Guid id)
	{
		var query = new GetUserByIdQuery(id);
		var response = await _mediator.Send(query);

		return response switch
		{
			{status: HttpStatusCode.NotFound, userDto: null} => NotFound(),
			{status:HttpStatusCode.OK, userDto: not null} => Ok(response.userDto),
			_ => BadRequest()
		};
	}

	#endregion
}