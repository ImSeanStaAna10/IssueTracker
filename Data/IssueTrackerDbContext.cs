using IssueTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Data
{
    public class IssueTrackerDbContext : DbContext
    {

        public IssueTrackerDbContext(DbContextOptions<IssueTrackerDbContext> options) : base(options)
        {
        }

        public DbSet<Issue> Issues { get; set; } = null!;
    }
}
