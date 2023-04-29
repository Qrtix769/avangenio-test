using System.ComponentModel.DataAnnotations;

namespace Api.CQRS.UserRequests.SignUp;

public class SignUpOutputDto
{
	public Guid Id { get; set; }

	public string GivenName { get; set; }

	public string Surname { get; set; }

	public string UserName { get; set; }

	public string Email { get; set; }
}