using System.Net;
using System.Security.Claims;
using Api.CQRS.ContactRequests.CreateContact;
using Api.CQRS.ContactRequests.RemoveContactById;
using Api.CQRS.ContactRequests.GetAllContacts;
using Api.CQRS.ContactRequests.GetContactById;
using Api.CQRS.ContactRequests.UpdateContact;
using Api.Identity;
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
		if (HttpContext.User.Identity is not ClaimsIdentity identity) 
			return BadRequest("Invalid Claims");
		
		var userName = identity.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;

		if (userName == null) return BadRequest("User name do not received");
		
		var command = new CreateContactCommand(contactInputInputDto, userName);
		var response = await _mediator.Send(command);

		return response switch
		{
			{ contactDto: not null, status: HttpStatusCode.Created } => Created(
				$"/api/contacts/{response.contactDto.Id}", response.contactDto),
			
			{contactDto: null, status: HttpStatusCode.NotFound} => NotFound(response.message),
			
			_ => BadRequest(response.message)
		};
	}

	#endregion

	#region GET endpoints

	[HttpGet]
	[Route("")]
	public async Task<IActionResult> GetAllContacts()
	{
		if (HttpContext.User.Identity is not ClaimsIdentity identity) 
			return BadRequest("Invalid Claims");
		
		var userName = identity.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;

		if (userName == null) return BadRequest("User name do not received");

		var query = new GetAllContactsQuery(userName);
		var response = await _mediator.Send(query);

		return response switch
		{
			{contactDtoList: null, status: HttpStatusCode.NotFound} => NotFound(response.message),
			{contactDtoList: not null, status: HttpStatusCode.OK} => Ok(response.contactDtoList),
			_ => BadRequest()
		};

	}

	[HttpGet]
	[Route("{id:guid}")]
	public async Task<IActionResult> GetContactById(Guid id)
	{
		if (HttpContext.User.Identity is not ClaimsIdentity identity) 
			return BadRequest("Invalid Claims");
		
		var userName = identity.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;

		if (userName == null) return BadRequest("User name do not received");

		var query = new GetContactByIdQuery(id, userName);
		var response = await _mediator.Send(query);

		return response switch
		{
			{ contactDto: not null, status: HttpStatusCode.OK } => Ok(response.contactDto),
			{ contactDto: null, status: HttpStatusCode.NotFound } => NotFound(response.message),
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
			HttpStatusCode.NotFound => NotFound($"Does not exist Contact with Id {id}"),
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
			{status: HttpStatusCode.OK} => Ok(response.contactOutputDto),
			{status: HttpStatusCode.NotFound} => NotFound(response.message),
			_ => BadRequest(response.message)
		};
	}

	#endregion

}