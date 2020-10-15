using Microsoft.EntityFrameworkCore;

namespace RandomUserCodeFirst.Model
{

    public class UserContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=users.db");

        public DbSet<User> Users { get; set; }
    }
}