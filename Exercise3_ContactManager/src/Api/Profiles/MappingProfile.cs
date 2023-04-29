using Api.CQRS.ContactRequests.CreateContact;
using Api.CQRS.ContactRequests.GetAllContacts;
using Api.CQRS.ContactRequests.GetContactById;
using Api.CQRS.ContactRequests.UpdateContact;
using Api.CQRS.UserRequests.GetContactById;
using Api.CQRS.UserRequests.GetUserById;
using Api.CQRS.UserRequests.SignUp;
using Api.Extensions;
using AutoMapper;
using Domain;

namespace Api.Profiles;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		#region contact mapping

		CreateMap<Contact, CreateContactInputDto>().ReverseMap();
		CreateMap<Contact, GetAllContactsDto>().ForMember(
			d => d.Age,
			o => o.MapFrom(s => s.DateOfBirth.GetAge())
		);
		CreateMap<Contact, GetContactByIdDto>().ForMember(
			d => d.Age,
			o => o.MapFrom(s => s.DateOfBirth.GetAge())
		);
		CreateMap<Contact, CreateContactOutputDto>().ForMember(
			d => d.Age,
			o => o.MapFrom(s => s.DateOfBirth.GetAge())
		);
		CreateMap<Contact, UpdateContactInputDto>().ReverseMap();

		#endregion

		#region user mapping

		CreateMap<User, SignUpInputDto>().ReverseMap();
		CreateMap<User, SignUpOutputDto>()
			.ForMember(d => d.GivenName, o => o.MapFrom(s => s.FirstName))
			.ForMember(d => d.Surname, o => o.MapFrom(s => s.LastName));
		CreateMap<User, GetUserByIdDto>().ReverseMap();

		#endregion
	}
}