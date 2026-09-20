using IssueTracker.Api.Models;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Issue>()
                .Property(i => i.Priority)
                .HasConversion<string>();

            modelBuilder.Entity<Issue>()
                .Property(i => i.Type)
                .HasConversion<string>();
        }
    }
}