using Microsoft.EntityFrameworkCore;

// Name: Kai Jie
// Admin No: 234412H

namespace MushroomPocket
{
    public class MushroomDbContext : DbContext
    {
        public DbSet<MushroomHeroes> MushroomHeroes { get; set; }
        public DbSet<EnemyCharacters> EnemyCharacters { get; set; }
        public DbSet<Users> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MushroomDb.db");

        }

        public bool IsDatabaseConnected()
        {
            try
            {
                return this.Database.CanConnect();
            }
            catch
            {
                return false;
            }
        }
    }
}