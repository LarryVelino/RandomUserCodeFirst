namespace EFCore.Users
{
    using EFCore.Users.Model;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    internal class UserDesignTimeContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        public UserContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<UserContext>();
            builder.UseSqlServer("Server=localhost;Database=EFSandbox;Trusted_Connection=True;");
            return new UserContext(builder.Options);
        }
    }
}