using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User : BaseEntity
{
	[Required] [StringLength(128)] public string FirstName { get; set; } = null!;

	[Required] [StringLength(128)] public string LastName { get; set; } = null!;

	[Required]
	[StringLength(128)]
	[DataType(DataType.EmailAddress)]
	public string Email { get; set; } = null!;

	[Required] [StringLength(60)] public string UserName { get; set; } = null!;

	[Required] [StringLength(256)] public string Password { get; set; } = null!;

	public IEnumerable<Contact>? Contacts { get; set; }
}