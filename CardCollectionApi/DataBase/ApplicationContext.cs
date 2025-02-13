using CardCollectionApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CardCollectionApi.DataBase
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}

		public DbSet<Cards> Cards { get; set; }
		public DbSet<Collections> Collections { get; set; }
		public DbSet<Users> Users { get; set; }
		public DbSet<Storage> Storage { get; set; }
	}
}
