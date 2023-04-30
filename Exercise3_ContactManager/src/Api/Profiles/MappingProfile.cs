using Api.CQRS.ContactRequests.CreateContact;
using Api.CQRS.ContactRequests.GetAllContacts;
using Api.CQRS.ContactRequests.GetContactById;
using Api.CQRS.ContactRequests.UpdateContact;
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
		CreateMap<Contact, UpdateContactOutputDto>().ForMember(
			d => d.Age,
			o => o.MapFrom(s => s.DateOfBirth.GetAge())
		);

		#endregion

		#region user mapping

		CreateMap<User, SignUpInputDto>().ReverseMap();
		CreateMap<User, SignUpOutputDto>().ReverseMap();
		CreateMap<User, GetUserByIdDto>().ReverseMap();

		#endregion
	}
}