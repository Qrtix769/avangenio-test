using System.Net;
using AutoMapper;
using Domain;
using Infrastructure.Services;
using MediatR;

namespace Api.CQRS.ContactRequests.CreateContact
{
	public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, (CreateContactOutputDto? contactDto, HttpStatusCode status)>
	{
		#region variables

		private readonly IContactService _service;
		private readonly IMapper _mapper;

		#endregion

		#region constructors

		public CreateContactCommandHandler(IContactService service, IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}

		#endregion

		#region handler

		public async Task<(CreateContactOutputDto? contactDto, HttpStatusCode status)> Handle(CreateContactCommand request,
			CancellationToken cancellationToken)
		{
			var contact = _mapper.Map<Contact>(request.ContactInputInputDto);

			var task = _service.AddAsync(contact, cancellationToken);
			await task;

			return task.IsCompletedSuccessfully
				? (_mapper.Map<CreateContactOutputDto>(contact), HttpStatusCode.Created)
				: (null, HttpStatusCode.BadRequest);
		}

		#endregion
	}
}
