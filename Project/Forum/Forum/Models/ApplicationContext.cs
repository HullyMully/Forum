using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Estimate> Estimates { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
