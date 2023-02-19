using JwtTest.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication.Server.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }
        public DbSet<LoginModel>? LoginModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginModel>().HasData(new LoginModel
            {
                UserID = 1,
                UserName = "Shalitha",
                Password = "def@shali"
            });
        }
    }
}