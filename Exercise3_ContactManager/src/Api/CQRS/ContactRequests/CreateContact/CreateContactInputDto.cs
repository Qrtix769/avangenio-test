using System.ComponentModel.DataAnnotations;

namespace Api.CQRS.ContactRequests.CreateContact;

public class CreateContactInputDto
{
	[Required]
	[StringLength(128)]
	public string FirstName { get; set; }

	[StringLength(128)]
	public string? LastName { get; set; }

	[Required]
	[StringLength(128)]
	[DataType(DataType.EmailAddress)]
	public string Email { get; set; }

	[Required]
	[DataType(DataType.Date)]
	public DateTime DateOfBirth { get; set; }

	[Required]
	[StringLength(20)]
	public string Phone { get; set; }
}