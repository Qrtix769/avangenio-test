using System.Net;
using Api.CQRS.ContactRequests.UpdateContact;
using Api.Extensions;
using AutoMapper;
using Domain;
using Infrastructure.Services;
using Moq;
using UnitTest.Configuration;

namespace UnitTest.ContactRequestTest;

public class UpdateContactCommandHandlerTest
{
	private readonly UpdateContactCommandHandler _updateContactCommandHandler;
	private readonly Mock<IContactService> _contactServiceMock;
	private readonly Mapper _mapper;

	public UpdateContactCommandHandlerTest()
	{
		_mapper = Config.Mapper;
		_contactServiceMock = new Mock<IContactService>();
		_updateContactCommandHandler = new UpdateContactCommandHandler(_contactServiceMock.Object, _mapper);
	}

	[Fact]
	public async Task UpdateContact_AgeContactYoungerThan18_returnBadRequest()
	{
		// arrange
		var updateContactInputDto = new UpdateContactInputDto()
		{
			DateOfBirth = new DateTime(2020, 5, 18),
			Email = "test@mail.com",
			FirstName = "first",
			Phone = "534543"
		};
		
		// act
		var response =
			await _updateContactCommandHandler.Handle(new UpdateContactCommand(updateContactInputDto, Guid.Empty),
				new CancellationToken());
		
		// assert
		Assert.Equal((null, HttpStatusCode.BadRequest, "The age of the contact must be older than 18"), response);
	}

	[Fact]
	public async Task UpdateContact_AgeContactOlderThan18_returnOk()
	{
		// arrange
		var updateContactInputDto = new UpdateContactInputDto()
		{
			DateOfBirth = new DateTime(2000, 5, 18),
			Email = "test@mail.com",
			FirstName = "first",
			Phone = "534543"
		};
		var oldContact = new Contact()
		{
			DateOfBirth = new DateTime(2002, 5, 10),
			Email = "old@mail.com",
			Id = Guid.NewGuid(),
			LastName = "old",
			FirstName = "old",
			Owner = Guid.NewGuid(),
			Phone = "5545554555"
		};
		var newContact = new Contact()
		{
			DateOfBirth = updateContactInputDto.DateOfBirth,
			Email = updateContactInputDto.Email,
			FirstName = updateContactInputDto.FirstName,
			Phone = updateContactInputDto.Phone,
			Id = oldContact.Id,
			LastName = oldContact.LastName,
			Owner = oldContact.Owner
		};

		_contactServiceMock
			.Setup(mock =>
				mock.GetByIdAsync(oldContact.Id, new CancellationToken()))
			.ReturnsAsync(() => oldContact);

		_contactServiceMock
			.Setup(mock =>
				mock.UpdateAsync(It.IsAny<Contact>(), new CancellationToken()))
			.ReturnsAsync((Contact contact, CancellationToken cancellationToken) => newContact);
		
		// act
		var response =
			await _updateContactCommandHandler.Handle(new UpdateContactCommand(updateContactInputDto, oldContact.Id),
				new CancellationToken());
		
		// assert
		Assert.Equal(HttpStatusCode.OK, response.status);
		Assert.Null(response.message);
		Assert.Equal(newContact.Id, response.contactOutputDto!.Id);
		Assert.Equal(newContact.Email, response.contactOutputDto.Email);
		Assert.Equal(newContact.FirstName, response.contactOutputDto.FirstName);
		Assert.Equal(newContact.DateOfBirth, response.contactOutputDto.DateOfBirth);
		Assert.Equal(newContact.DateOfBirth.GetAge(), response.contactOutputDto.Age);
		Assert.Equal(newContact.Owner, response.contactOutputDto.Owner);
		Assert.Equal(newContact.Phone, response.contactOutputDto.Phone);
		Assert.Equal(newContact.LastName, response.contactOutputDto.LastName);
	}

	[Fact]
	public async Task UpdateContact_NotExistingContact_returnNotFound()
	{
		// arrange
		var updateContactInputDto = new UpdateContactInputDto()
		{
			DateOfBirth = new DateTime(2000, 5, 18),
			Email = "test@mail.com",
			FirstName = "first",
			Phone = "534543"
		};

		_contactServiceMock
			.Setup(mock =>
				mock.GetByIdAsync(Guid.Empty, new CancellationToken()))
			.ReturnsAsync(null as Contact);
		
		// act
		var response =
			await _updateContactCommandHandler.Handle(new UpdateContactCommand(updateContactInputDto, Guid.Empty),
				new CancellationToken());
		
		// assert
		Assert.Equal((null, HttpStatusCode.NotFound, $"Does not exist contact with id: {Guid.Empty}"), response);
	}
}