
namespace JWTAuthentication.Data
{
    using JWTAuthentication.Entities;
    using Microsoft.EntityFrameworkCore;
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
    }
}
