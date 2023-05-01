using System.ComponentModel.DataAnnotations;

namespace Api.CQRS.ContactRequests.UpdateContact;

public class UpdateContactInputDto
{
	
	[Required]
	[StringLength(128, ErrorMessage = "{0} must not exceed {1} characters")]
	[RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "{0} can only contain letters and spaces")]
	public string FirstName { get; set; } = null!;

	[StringLength(128, ErrorMessage = "{0} must not exceed {1} characters")]
	[RegularExpression(@"^[a-z A-Z]+$", ErrorMessage = "{0} can only contain letters and spaces")]
	public string? LastName { get; set; }

	[Required]
	[StringLength(128, ErrorMessage = "{0} must not exceed {1} characters")]
	[DataType(DataType.EmailAddress)]
	[RegularExpression(@"^[a-z.]+@[a-z]+.[a-z]+$", ErrorMessage = "Invalid email address")]
	public string Email { get; set; } = null!;

	[Required]
	[DataType(DataType.Date, ErrorMessage = "{0} must be in the format 'yyyy-mm-dd'")]
	public DateTime DateOfBirth { get; set; }

	[Required]
	[StringLength(20, ErrorMessage = "{0} must not exceed {1} characters")]
	[RegularExpression(@"^\d+$", ErrorMessage = "{0} can only contain numbers")]
	public string Phone { get; set; } = null!;
}