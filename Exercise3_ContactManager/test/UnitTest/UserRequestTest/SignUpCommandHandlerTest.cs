using System.Net;
using Api.CQRS.UserRequests.SignUp;
using Api.Profiles;
using Domain;
using Infrastructure.Services;
using Moq;
using UnitTest.Configuration;

namespace UnitTest.UserRequestTest;

public class SignUpCommandHandlerTest
{
	private readonly SignUpCommandHandler _signUpCommandHandler;
	private readonly Mock<IUserService> _userServiceMock;

	public SignUpCommandHandlerTest()
	{
		var mapper = Config.Mapper;
		_userServiceMock = new Mock<IUserService>();
		_userServiceMock
			.Setup(mock =>
				mock.AddAsync(It.IsAny<User>(), new CancellationToken()))
			.ReturnsAsync((User user, CancellationToken cancellationToken) =>
			{
				user.Id = Guid.NewGuid();
				return user;
			});
			
		_signUpCommandHandler = new SignUpCommandHandler(mapper, _userServiceMock.Object);
	}

	[Fact]
	public async Task SignUpCommandHandle_ExistingEmail_returnBadRequest()
	{
		// arrange
		const string email = "test@mail.com";
		_userServiceMock
			.Setup(mock => mock.IsEmailExistAsync(email, null, new CancellationToken()))
			.ReturnsAsync(true);
		
		// act 
		var result = await _signUpCommandHandler.Handle(new SignUpCommand(new SignUpInputDto()
		{
			Email = email,
			FirstName = "name",
			LastName = "last",
			Password = "pass",
			UserName = "username"
		}), new CancellationToken());
		
		// assert
		Assert.Equal((null, HttpStatusCode.BadRequest,
			$"Exist another user with this email: {email}"), result);
	}

	[Fact]
	public async Task SignUpCommandHandler_NotExistingEmail_returnCreated()
	{
		// arrange
		var signUpInputDto = new SignUpInputDto()
		{
			Email = "test@mail.com",
			FirstName = "test",
			LastName = "last",
			Password = "pass",
			UserName = "user name"
		};

		_userServiceMock
			.Setup(mock => mock.IsEmailExistAsync(signUpInputDto.Email, null, new CancellationToken()))
			.ReturnsAsync(false);
		
		// act
		var response = await _signUpCommandHandler.Handle(new SignUpCommand(signUpInputDto), new CancellationToken());
		
		// assert
		Assert.Equal(HttpStatusCode.Created, response.status);
		Assert.Null(response.message);
		Assert.Equal(signUpInputDto.Email, response.userDto?.Email);
		Assert.Equal(signUpInputDto.UserName, response.userDto?.UserName);
		Assert.Equal(signUpInputDto.FirstName, response.userDto?.FirstName);
		Assert.Equal(signUpInputDto.LastName, response.userDto?.LastName);
	}
}