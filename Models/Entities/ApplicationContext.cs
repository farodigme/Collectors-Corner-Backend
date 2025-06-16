using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Models.Entities
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<ResetToken> ResetTokens { get; set; }
        public DbSet<Collection> Collections { get; set; }
		public DbSet<CollectionCategory> CollectionCategories { get; set; }
        public DbSet<Card> Cards { get; set; }
		public DbSet<CardCategory> CardCategories { get; set; }
		public DbSet<FavoriteCollections> FavoriteCollections { get; set; }
		public DbSet<Rating> Ratings { get; set; }
	}
}
