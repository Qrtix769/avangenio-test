using System.ComponentModel.DataAnnotations;

namespace Api.CQRS.UserRequests.SignUp;

public class SignUpInputDto
{
	[Required]
	[StringLength(128, ErrorMessage = "{0} must not exceed {1} characters")]
	[RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "{0} can only contain letters and spaces")]
	public string FirstName { get; set; } = null!;

	[Required]
	[StringLength(128, ErrorMessage = "{0} must not exceed {1} characters")]
	[RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "{0} can only contain letters and spaces")]
	public string LastName { get; set; } = null!;

	[Required]
	[StringLength(128, ErrorMessage = "{0} must not exceed {1} characters")]
	[DataType(DataType.EmailAddress)]
	[RegularExpression(@"^[a-z.]+@[a-z]+.[a-z]+$", ErrorMessage = "Invalid email address")]
	public string Email { get; set; } = null!;

	[Required]
	[StringLength(60, ErrorMessage = "{0} must not exceed {1} characters")]
	[MinLength(3, ErrorMessage = "Must be at least {1} characters long")]
	[RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "{0} can only contain letters, numbers and '_'")]
	public string UserName { get; set; } = null!;

	[Required]
	[StringLength(256, ErrorMessage = "{0} must not exceed {1} characters")]
	[MinLength(8, ErrorMessage = "{0} must be at least 8 characters long")]
	public string Password { get; set; } = null!;
}