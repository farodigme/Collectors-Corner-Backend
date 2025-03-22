using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Models.Entities
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<ResetToken> ResetTokens { get; set; }

	}
}
