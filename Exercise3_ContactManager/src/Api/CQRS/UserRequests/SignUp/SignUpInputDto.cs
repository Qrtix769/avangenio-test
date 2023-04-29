using System.ComponentModel.DataAnnotations;

namespace Api.CQRS.UserRequests.SignUp;

public class SignUpInputDto
{
	[Required]
	[StringLength(128)]
	public string FirstName { get; set; }

	[Required]
	[StringLength(128)]
	public string LastName { get; set; }

	[Required]
	[StringLength(128)]
	[DataType(DataType.EmailAddress)]
	public string Email { get; set; }

	[Required]
	[StringLength(60)]
	public string UserName { get; set; }

	[Required]
	[StringLength(256)]
	public string Password { get; set; }
}