using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User : BaseEntity
{
	[Required]
	[StringLength(128)]
	public string FirstName { get; set; }
	
	[Required]
	[StringLength(128)]
	public string LastName { get; set; }
	
	[Required]
	[StringLength(60)]
	public string UserName { get; set; }
	
	[Required]
	[StringLength(256)]
	public string Password { get; set; }

	public IEnumerable<Contact>? Contacts { get; set; }
}