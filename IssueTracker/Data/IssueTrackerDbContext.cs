
using IssueTracker.Api.Models;
using IssueTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Data
{
    public class IssueTrackerDbContext : DbContext
    {
        public IssueTrackerDbContext(
            DbContextOptions<IssueTrackerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Issue> Issues { get; set; } = null!;

        public DbSet<Project> Projects { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Issue>()
                .Property(i => i.Priority)
                .HasConversion<string>();

            modelBuilder.Entity<Issue>()
                .Property(i => i.Type)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Fullname)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(254);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
