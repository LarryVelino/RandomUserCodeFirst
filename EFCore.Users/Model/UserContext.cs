namespace EFCore.Users.Model
{
    using Microsoft.EntityFrameworkCore;

    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> context) : base(context)
        {
        }

        //Server=localhost;Database=EFSandbox;Trusted_Connection=True;

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlServer("Server=localhost;Database=EFSandbox;Trusted_Connection=True;");

        public DbSet<User> Users { get; set; }
    }
}