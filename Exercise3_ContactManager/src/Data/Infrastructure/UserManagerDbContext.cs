using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
	public class UserManagerDbContext : DbContext
	{
		#region entities
		public virtual DbSet<Contact> Contacts { get; set; }
		public virtual DbSet<User> Users { get; set; }
		#endregion

		#region constructors
		public UserManagerDbContext(DbContextOptions options) : base(options) { }
		#endregion
	}
}
