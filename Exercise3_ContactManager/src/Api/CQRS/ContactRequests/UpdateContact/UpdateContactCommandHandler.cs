using System.Net;
using AutoMapper;
using Domain;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.ContactRequests.UpdateContact
{
	public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, HttpStatusCode>
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


		public async Task<HttpStatusCode> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
		{
			var contact = await _service.GetByIdAsync(request.Id, cancellationToken);

			if (contact == null)
				return HttpStatusCode.BadRequest;

			contact.DateOfBirth = request.ContactDto.DateOfBirth;
			contact.Email = request.ContactDto.Email;
			contact.FirstName = request.ContactDto.FirstName;
			contact.LastName = request.ContactDto.LastName;
			contact.Phone = request.ContactDto.Phone;
			
			await _service.UpdateAsync(contact, cancellationToken);
			return HttpStatusCode.OK;
		}
	}
}
