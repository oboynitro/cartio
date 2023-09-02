using Microsoft.EntityFrameworkCore;
using Cartio.Entities;

namespace Cartio.Persistance
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }

    }
}
