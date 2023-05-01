using System.Net;
using Api.CQRS.UserRequests.GetUserById;
using AutoMapper;
using Domain;
using Infrastructure.Services;
using Moq;
using UnitTest.Configuration;

namespace UnitTest.UserRequestTest;

public class GetUserByIdHandlerTest
{
	private readonly GetUserByIdQueryHandler _getUserByIdQueryHandler;
	private readonly Mock<IUserService> _userServiceMock;
	private readonly Mapper _mapper;

	public GetUserByIdHandlerTest()
	{
		_userServiceMock = new Mock<IUserService>();
		_mapper = Config.Mapper;
		_getUserByIdQueryHandler = new GetUserByIdQueryHandler(_userServiceMock.Object, _mapper);
	}

	[Fact]
	public async Task GetUserByIdQueryHandler_ExistingUserId_returnAnUser()
	{
		// arrange
		var user = new User()
		{
			Email = "test@mail.com",
			FirstName = "first",
			Id = Guid.NewGuid(),
			LastName = "last",
			Password = "pass",
			UserName = "usname"
		};

		_userServiceMock
			.Setup(mock => mock.GetByIdAsync(user.Id, new CancellationToken()))
			.ReturnsAsync(user);
		
		// act 
		var response = await _getUserByIdQueryHandler.Handle(new GetUserByIdQuery(user.Id), new CancellationToken());
		
		// assert
		Assert.Equal(HttpStatusCode.OK, response.status);
		Assert.Equal(user.Id, response.userDto!.Id);
		Assert.Equal(user.UserName, response.userDto.UserName);
		Assert.Equal(user.FirstName, response.userDto.FirstName);
		Assert.Equal(user.LastName, response.userDto.LastName);
		Assert.Equal(user.Email, response.userDto.Email);
	}

	[Fact]
	public async Task GetUserByIdQueryHandler_NotExistingUserId_returnNull()
	{
		// arrange
		var userId = Guid.NewGuid();

		_userServiceMock
			.Setup(mock => mock.GetByIdAsync(userId, new CancellationToken()))
			.ReturnsAsync(null as User);
		
		// act
		var response = await _getUserByIdQueryHandler.Handle(new GetUserByIdQuery(userId), new CancellationToken());
		
		// assert
		Assert.Equal(HttpStatusCode.NotFound, response.status);
		Assert.Null(response.userDto);
	}
}