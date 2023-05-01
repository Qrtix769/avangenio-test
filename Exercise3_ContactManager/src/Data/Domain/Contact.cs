using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Contact : BaseEntity
{
	[Required]
	[StringLength(128)]
	public string FirstName { get; set; } = null!;
	
	[StringLength(128)]
	public string? LastName { get; set; }

	[Required]
	[StringLength(128)]
	[DataType(DataType.EmailAddress)]
	public string Email { get; set; } = null!;

	[Required]
	[DataType(DataType.Date)] 
	public DateTime DateOfBirth { get; set; }

	[Required]
	[StringLength(20)]
	public string Phone { get; set; } = null!;

	public Guid Owner { get; set; }
}