using System.Net;
using Api.Extensions;
using AutoMapper;
using Domain;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.ContactRequests.UpdateContact
{
	public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, (UpdateContactOutputDto? contactOutputDto, HttpStatusCode status, string? message)>
	{
		#region variables

		private readonly IContactService _service;
		private readonly IMapper _mapper;

		#endregion

		#region constructors

		public UpdateContactCommandHandler(IContactService service, IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}

		#endregion


		public async Task<(UpdateContactOutputDto? contactOutputDto, HttpStatusCode status, string? message)> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
		{
			if (request.ContactDto.DateOfBirth.GetAge() < 18)
				return (null, HttpStatusCode.BadRequest, "The age of the contact must be older than 18");

			var contact = await _service.GetByIdAsync(request.Id, cancellationToken);

			if (contact == null)
				return (null, HttpStatusCode.NotFound, $"Does not exist contact with id: {request.Id}");

			contact.DateOfBirth = request.ContactDto.DateOfBirth;
			contact.Email = request.ContactDto.Email;
			contact.FirstName = request.ContactDto.FirstName;
			contact.LastName = request.ContactDto.LastName;
			contact.Phone = request.ContactDto.Phone;
			
			var result = await _service.UpdateAsync(contact, cancellationToken);
			return (_mapper.Map<UpdateContactOutputDto>(result), HttpStatusCode.OK, null);
		}
	}
}
