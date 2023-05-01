using System.Net;
using Api.CQRS.ContactRequests.RemoveContactById;
using Domain;
using Infrastructure.Services;
using Moq;
using UnitTest.Configuration;

namespace UnitTest.ContactRequestTest;

public class RemoveContactByIdCommandHandlerTest
{
	private readonly RemoveContactByIdCommandHandler _removeContactByIdCommandHandler;
	private readonly Mock<IContactService> _contactServiceMock;

	public RemoveContactByIdCommandHandlerTest()
	{
		var mapper = Config.Mapper;
		_contactServiceMock = new Mock<IContactService>();
		_removeContactByIdCommandHandler = new RemoveContactByIdCommandHandler(_contactServiceMock.Object, mapper);
	}

	[Fact]
	public async Task RemoveContactById_NotExistingContact_returnNotFound()
	{
		// arrange
		_contactServiceMock
			.Setup(mock =>
				mock.RemoveByIdAsync(Guid.Empty, new CancellationToken()))
			.ThrowsAsync(new ArgumentException($"Does not exist {nameof(Contact)} with Id {Guid.Empty}"));
		
		// act
		var response =
			await _removeContactByIdCommandHandler.Handle(new RemoveContactByIdCommand(Guid.Empty),
				new CancellationToken());
		
		// assert
		Assert.Equal(HttpStatusCode.NotFound, response);
	}

	[Fact]
	public async Task RemoveContactById_ExistingContact_returnOk()
	{
		// arrange
		_contactServiceMock
			.Setup(mock =>
				mock.RemoveByIdAsync(Guid.Empty, new CancellationToken()))
			.Returns(Task.CompletedTask);
		
		// act
		var response =
			await _removeContactByIdCommandHandler.Handle(new RemoveContactByIdCommand(Guid.Empty),
				new CancellationToken());
		
		// assert
		Assert.Equal(HttpStatusCode.OK, response);
	}
}