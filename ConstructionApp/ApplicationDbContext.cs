using ConstructionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructionApp
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ConstructionSite> ConstructionSites { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
    }
}
