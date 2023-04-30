using System.Net;
using Api.Extensions;
using AutoMapper;
using Domain;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.ContactRequests.CreateContact
{
	public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, (CreateContactOutputDto? contactDto, HttpStatusCode status, string? message)>
	{
		#region variables

		private readonly IContactService _contactService;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;

		#endregion

		#region constructors

		public CreateContactCommandHandler(IContactService contactService, IMapper mapper, IUserService userService)
		{
			_contactService = contactService;
			_mapper = mapper;
			_userService = userService;
		}

		#endregion

		#region handler

		public async Task<(CreateContactOutputDto? contactDto, HttpStatusCode status, string? message)> Handle(CreateContactCommand request,
			CancellationToken cancellationToken)
		{
			try
			{
				var user = await _userService.GetByUserNameAsync(request.UserName, cancellationToken);

				if (request.ContactInputInputDto.DateOfBirth.GetAge() < 18)
					return (null, HttpStatusCode.BadRequest, "The age of the contact must be older than 18");
				
				var contact = _mapper.Map<Contact>(request.ContactInputInputDto);
				contact.Owner = user.Id;

				var result = await _contactService.AddAsync(contact, cancellationToken);

				return (_mapper.Map<CreateContactOutputDto>(result), HttpStatusCode.Created, null);
			}
			catch (ArgumentException ex)
			{
				return (null, HttpStatusCode.NotFound, $"Does not exist user with name: {request.UserName}. Therefore the contact could not be created");
			}
		}

		#endregion
	}
}
