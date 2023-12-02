using Microsoft.EntityFrameworkCore;
using Shelfie.Core.Data;

namespace Shelfie.Infrastructure.Data
{
    public class ShelfieDbContext : DbContext
    {
        public ShelfieDbContext(DbContextOptions<ShelfieDbContext> options) : base(options) { }

        public DbSet<BoardGame> BoardGames => Set<BoardGame>();
    }
}
