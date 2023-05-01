using System.Net;
using Api.CQRS.ContactRequests.GetAllContacts;
using AutoMapper;
using Domain;
using Infrastructure.Services;
using Moq;
using UnitTest.Configuration;

namespace UnitTest.ContactRequestTest;

public class GetAllContactsQueryHandlerTest
{
	private readonly GetAllContactsQueryHandler _getAllContactsQueryHandler;
	private readonly Mock<IUserService> _userServiceMock;
	private readonly Mock<IContactService> _contactServiceMock;
	private readonly Mapper _mapper;

	public GetAllContactsQueryHandlerTest()
	{
		_mapper = Config.Mapper;
		_userServiceMock = new Mock<IUserService>();
		_contactServiceMock = new Mock<IContactService>();
		_getAllContactsQueryHandler = new GetAllContactsQueryHandler(_contactServiceMock.Object, _mapper, _userServiceMock.Object);
	}

	[Fact]
	public async Task GetAllContacts_NotExistingUser_returnNotFound()
	{
		// arrange
		const string userName = "userNameTest";

		_userServiceMock
			.Setup(mock => mock.GetByUserNameAsync(userName, new CancellationToken()))
			.ThrowsAsync(new ArgumentException($"Does not exist user with 'UserName': {userName}"));
		
		// act
		var response =
			await _getAllContactsQueryHandler.Handle(new GetAllContactsQuery(userName), new CancellationToken());
		
		// assert
		Assert.Equal((null, HttpStatusCode.NotFound, $"Does not exist user with name: {userName}"), response);
	}

	[Fact]
	public async Task GetAllContacts_ExistingUser_returnOk()
	{
		// arrange
		const string userName = "userNameTest";
		var userId = Guid.NewGuid();
		_userServiceMock
			.Setup(mock => mock.GetByUserNameAsync(userName, new CancellationToken()))
			.ReturnsAsync((string usName, CancellationToken cancellationToken) =>new User()
			{
				Id = userId,
				UserName = usName,
				Email = "tes@mail.com",
				FirstName = "first",
				LastName = "last",
				Password = "pass"
			});

		var contacts = new List<Contact>();
		for (var i = 0; i < 5; i++)
		{
			contacts.Add(new Contact()
			{
				DateOfBirth = new DateTime(1993, i+2, i + 10),
				Email = "test@mail.com",
				FirstName = "first",
				Id = Guid.NewGuid(),
				LastName = "last",
				Owner = userId,
				Phone = "234234242"
			});
		}

		_contactServiceMock
			.Setup(mock => mock.GetAllByOwnerAsync(userId, new CancellationToken()))
			.ReturnsAsync((Guid id, CancellationToken cancellationToken) => contacts);
		
		// act 
		var response =
			await _getAllContactsQueryHandler.Handle(new GetAllContactsQuery(userName), new CancellationToken());
		
		// assert
		Assert.Null(response.message);
		Assert.Equal(HttpStatusCode.OK, response.status);
		var expected = contacts.ToArray();
		var actual = response.contactDtoList as GetAllContactsDto[] ?? response.contactDtoList?.ToArray();
		Assert.Equal(expected.Length, actual?.Length);
		for (var i = 0; i < expected.Length; i++)
		{
			var e = expected[i];
			var a = actual![i];
			Assert.Equal(e.Email, a.Email);
			Assert.Equal(e.FirstName, a.FirstName);
			Assert.Equal(e.LastName, a.LastName);
			Assert.Equal(e.Owner, a.Owner);
			Assert.Equal(e.Id, a.Id);
			Assert.Equal(e.Phone, a.Phone);
			Assert.Equal(e.DateOfBirth, a.DateOfBirth);
		}
	}
}