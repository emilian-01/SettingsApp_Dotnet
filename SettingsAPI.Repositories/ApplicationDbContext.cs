// SettingsAPI.Repositories/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using SettingsAPI.Models;

namespace SettingsAPI.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<BasicSettings> BasicSettings { get; set; } = null!;
        public DbSet<AdvancedSettings> AdvancedSettings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<BasicSettings>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<AdvancedSettings>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);
            
            // Set default admin user
            var adminUser = new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@example.com",
                Role = "Admin"
                // Password will be set via migration
            };
            
            modelBuilder.Entity<User>().HasData(adminUser);
        }
    }
}