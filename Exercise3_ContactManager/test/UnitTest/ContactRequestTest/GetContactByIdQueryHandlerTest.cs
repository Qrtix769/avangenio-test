using System.Net;
using Api.CQRS.ContactRequests.GetContactById;
using Api.Extensions;
using AutoMapper;
using Domain;
using Infrastructure.Services;
using Moq;
using UnitTest.Configuration;

namespace UnitTest.ContactRequestTest;

public class GetContactByIdQueryHandlerTest
{
	private readonly GetContactByIdQueryHandler _getContactByIdQueryHandler;
	private readonly Mock<IUserService> _userServiceMock;
	private readonly Mock<IContactService> _contactServiceMock;
	private readonly Mapper _mapper;

	public GetContactByIdQueryHandlerTest()
	{
		_mapper = Config.Mapper;
		_userServiceMock = new Mock<IUserService>();
		_contactServiceMock = new Mock<IContactService>();
		_getContactByIdQueryHandler = new GetContactByIdQueryHandler(_contactServiceMock.Object, _mapper, _userServiceMock.Object);
	}

	[Fact]
	public async Task GetContactById_NotExistingUser_returnNotFound()
	{
		// arrange
		const string userName = "userNameTest";

		_userServiceMock
			.Setup(mock => mock.GetByUserNameAsync(userName, new CancellationToken()))
			.ThrowsAsync(new ArgumentException($"Does not exist user with 'UserName': {userName}"));
		
		// act
		var response =
			await _getContactByIdQueryHandler.Handle(new GetContactByIdQuery(Guid.Empty, userName),
				new CancellationToken());
		
		// assert
		Assert.Equal((null, HttpStatusCode.NotFound, $"Does not exist user with name: {userName}"), response);
	}

	[Fact]
	public async Task GetContactById_ExistingUser_returnOk()
	{
		// arrange
		const string userName = "userNameTest";
		var userId = Guid.NewGuid();
		_userServiceMock
			.Setup(mock => mock.GetByUserNameAsync(userName, new CancellationToken()))
			.ReturnsAsync(new User()
			{
				Id = userId,
				UserName = userName,
				Email = "tes@mail.com",
				FirstName = "first",
				LastName = "last",
				Password = "pass"
			});
		var contact = new Contact()
		{
			DateOfBirth = new DateTime(1993, 2, 10),
			Email = "test@mail.com",
			FirstName = "first",
			Id = Guid.NewGuid(),
			LastName = "last",
			Owner = userId,
			Phone = "234234242"
		};
		_contactServiceMock
			.Setup(mock => 
				mock.GetContactByIdAndOwnerAsync(contact.Id, userId, new CancellationToken()))
			.ReturnsAsync(() => new Contact()
			{
				DateOfBirth = contact.DateOfBirth,
				Email = contact.Email,
				FirstName = contact.FirstName,
				Id = contact.Id,
				LastName = contact.LastName,
				Owner = userId,
				Phone = contact.Phone
			});
		
		// act
		var response =
			await _getContactByIdQueryHandler.Handle(new GetContactByIdQuery(contact.Id, userName),
				new CancellationToken());
		
		// assert
		Assert.Null(response.message);
		Assert.Equal(HttpStatusCode.OK, response.status);
		Assert.Equal(contact.Id, response.contactDto!.Id);
		Assert.Equal(contact.Email, response.contactDto.Email);
		Assert.Equal(contact.FirstName, response.contactDto.FirstName);
		Assert.Equal(contact.DateOfBirth, response.contactDto.DateOfBirth);
		Assert.Equal(contact.DateOfBirth.GetAge(), response.contactDto.Age);
		Assert.Equal(contact.Owner, response.contactDto.Owner);
		Assert.Equal(contact.Phone, response.contactDto.Phone);
		Assert.Equal(contact.LastName, response.contactDto.LastName);
	}

	[Fact]
	public async Task GetContactById_ExistingUserAndNotExistingContact_returnNotFound()
	{
		// arrange
		const string userName = "userNameTest";
		var userId = Guid.NewGuid();
		_userServiceMock
			.Setup(mock => mock.GetByUserNameAsync(userName, new CancellationToken()))
			.ReturnsAsync(new User()
			{
				Id = userId,
				UserName = userName,
				Email = "tes@mail.com",
				FirstName = "first",
				LastName = "last",
				Password = "pass"
			});
		_contactServiceMock
			.Setup(mock =>
				mock.GetContactByIdAndOwnerAsync(Guid.Empty, userId, new CancellationToken()))
			.ReturnsAsync(null as Contact);
		
		// act
		var response =
			await _getContactByIdQueryHandler.Handle(new GetContactByIdQuery(Guid.Empty, userName),
				new CancellationToken());
		
		// assert
		Assert.Equal((null, HttpStatusCode.NotFound, $"Does not exist contact with id: {Guid.Empty}"), response);
	}
}