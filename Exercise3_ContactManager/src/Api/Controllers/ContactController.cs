using System.Net;
using System.Security.Claims;
using Api.CQRS.ContactRequests.CreateContact;
using Api.CQRS.ContactRequests.RemoveContactById;
using Api.CQRS.ContactRequests.GetAllContacts;
using Api.CQRS.ContactRequests.GetContactById;
using Api.CQRS.ContactRequests.UpdateContact;
using Api.Identity;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/contacts")]
public class ContactController : Controller
{
	#region variables

	private readonly IMediator _mediator;

	#endregion

	#region constructors

	public ContactController(IMediator mediator)
	{
		_mediator = mediator;
	}

	#endregion

	#region POST endpoints

	[HttpPost]
	[Route("")]
	public async Task<IActionResult> CreateContact(CreateContactInputDto contactInputInputDto)
	{
		var command = new CreateContactCommand(contactInputInputDto);
		var response = await _mediator.Send(command);

		return response switch
		{
			{ contactDto: not null, status: HttpStatusCode.Created } => Created(
				$"/api/contacts/{response.contactDto.Id}", response.contactDto),

			_ => BadRequest()
		};
	}

	#endregion

	#region GET endpoints

	[HttpGet]
	[Route("")]
	public async Task<IActionResult> GetAllContacts()
	{

		var query = new GetAllContactsQuery();
		var response = await _mediator.Send(query);

		return Ok(response.contactDtoList);
	}

	[HttpGet]
	[Route("{id:guid}")]
	public async Task<IActionResult> GetContactById(Guid id)
	{
		var query = new GetContactByIdQuery(id);
		var response = await _mediator.Send(query);

		return response switch
		{
			{ contactDto: not null, status: HttpStatusCode.OK } => Ok(response.contactDto),
			{ contactDto: null, status: HttpStatusCode.NotFound } => NotFound(),
			_ => BadRequest()
		};
	}

	#endregion

	#region DELET endpoints

	[HttpDelete]
	[Authorize(Policy = IdentityData.CubanAdminUserPolicyName)]
	[Route("{id:guid}")]
	public async Task<IActionResult> RemoveContactById(Guid id)
	{
		var query = new RemoveContactByIdCommand(id);
		var response = await _mediator.Send(query);

		return response switch
		{
			HttpStatusCode.OK => Ok(),
			_ => BadRequest()
		};
	}

	#endregion

	#region PUT endpoints

	[HttpPut]
	[Route("{id:guid}")]
	public async Task<IActionResult> UpdateContact(Guid id, UpdateContactInputDto contactDto)
	{
		var command = new UpdateContactCommand(contactDto, id);
		var response = await _mediator.Send(command);

		return response switch
		{
			HttpStatusCode.OK => Ok(),
			_ => BadRequest()
		};
	}

	#endregion
}