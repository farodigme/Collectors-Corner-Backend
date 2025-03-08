using Microsoft.EntityFrameworkCore;

namespace Collectors_Corner_Backend.Models.DataBase
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
