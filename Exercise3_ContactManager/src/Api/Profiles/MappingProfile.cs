using Api.CQRS.ContactRequests.CreateContact;
using Api.CQRS.ContactRequests.GetAllContacts;
using Api.CQRS.ContactRequests.GetContactById;
using Api.CQRS.ContactRequests.UpdateContact;
using Api.Extensions;
using AutoMapper;
using Domain;

namespace Api.Profiles;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
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
	}
}