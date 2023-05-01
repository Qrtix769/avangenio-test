using System.Net;
using Api.CQRS.ContactRequests.CreateContact;
using Api.Extensions;
using Domain;
using Infrastructure.Services;
using Moq;
using UnitTest.Configuration;

namespace UnitTest.ContactRequestTest;

public class CreateContactCommandHandlerTest
{
	private readonly CreateContactCommandHandler _createContactCommandHandler;
	private readonly Mock<IUserService> _userServiceMock;

	public CreateContactCommandHandlerTest()
	{
		var mapper = Config.Mapper;
		_userServiceMock = new Mock<IUserService>();
		var contactServiceMock = new Mock<IContactService>();
		contactServiceMock
			.Setup(mock => mock.AddAsync(It.IsAny<Contact>(), new CancellationToken()))
			.ReturnsAsync((Contact contact, CancellationToken cancellationToken) =>
			{
				contact.Id = Guid.NewGuid();
				return contact;
			});

		_createContactCommandHandler =
			new CreateContactCommandHandler(contactServiceMock.Object, mapper, _userServiceMock.Object);
	}

	[Fact]
	public async Task CreateContact_AgeContactYoungerThan18_returnBadRequest()
	{
		// arrange
		var createContactInputDto = new CreateContactInputDto()
		{
			Email = "tes@mail.com",
			DateOfBirth = DateTime.Now.Subtract(new TimeSpan(365 * 10, 0, 0)),
			FirstName = "firs",
			LastName = "last",
			Phone = "0353059434"
		};
		const string userName = "userNameTest";

		_userServiceMock
			.Setup(mock => mock.GetByUserNameAsync(userName, new CancellationToken()))
			.ReturnsAsync(new User());
		
		// act
		var response =
			await _createContactCommandHandler.Handle(new CreateContactCommand(createContactInputDto, userName),
				new CancellationToken());
		
		// assert
		Assert.Equal((null, HttpStatusCode.BadRequest, "The age of the contact must be older than 18"), response);
	}

	[Fact]
	public async Task CreateContact_NotExistingUser_returnNotFound()
	{
		// arrange
		var createContactInputDto = new CreateContactInputDto()
		{
			Email = "tes@mail.com",
			DateOfBirth = DateTime.Now.Subtract(new TimeSpan(365 * 10, 0, 0)),
			FirstName = "firs",
			LastName = "last",
			Phone = "0353059434"
		};
		const string userName = "userNameTest";

		_userServiceMock
			.Setup(mock => mock.GetByUserNameAsync(userName, new CancellationToken()))
			.ThrowsAsync(new ArgumentException($"Does not exist user with 'UserName': {userName}"));
		
		// act
		var response =
			await _createContactCommandHandler.Handle(new CreateContactCommand(createContactInputDto, userName),
				new CancellationToken());	
		
		// assert
		Assert.Equal(
			(null, HttpStatusCode.NotFound, $"Does not exist user with name: {userName}. Therefore the contact could not be created"), 
			response);
	}

	[Fact]
	public async Task CreateContact_AgeContactOlderThan18_returnCreated()
	{
		// arrange
		var user = new User()
		{
			Email = "user@mail.com",
			FirstName = "first",
			Id = Guid.NewGuid(),
			LastName = "last",
			Password = "pass",
			UserName = "username"
		};
		var createContactInputDto = new CreateContactInputDto()
		{
			DateOfBirth = new DateTime(1993, 5, 26),
			Email = "contact@mail.com",
			FirstName = "first",
			LastName = "last",
			Phone = "30453985234"
		};
		
		_userServiceMock
			.Setup(mock => mock.GetByUserNameAsync(user.UserName, new CancellationToken()))
			.ReturnsAsync(user); 

			
		// act
		var response =
			await _createContactCommandHandler.Handle(new CreateContactCommand(createContactInputDto, user.UserName),
				new CancellationToken());
		
		// assert
		Assert.Equal(HttpStatusCode.Created, response.status);
		Assert.Null(response.message);
		Assert.Equal(createContactInputDto.Email, response.contactDto?.Email);
		Assert.Equal(createContactInputDto.FirstName, response.contactDto?.FirstName);
		Assert.Equal(createContactInputDto.LastName, response.contactDto?.LastName);
		Assert.Equal(createContactInputDto.Phone, response.contactDto?.Phone);
		Assert.Equal(createContactInputDto.DateOfBirth, response.contactDto?.DateOfBirth);
	}
}