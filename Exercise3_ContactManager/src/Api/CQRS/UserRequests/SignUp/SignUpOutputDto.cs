namespace Api.CQRS.UserRequests.SignUp;

public class SignUpOutputDto
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public string UserName { get; set; } = null!;

	public string Email { get; set; } = null!;
}