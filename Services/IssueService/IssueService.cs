using IssueTracker.Api.Models;
using IssueTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Services.IssueService
{
    public class IssueService : IIssueService
    {
        private readonly IssueTrackerDbContext _context;

        // Constructor to inject the IssueTrackerDbContext through DI
        public IssueService(IssueTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Issue>> GetAllAsync()
        {
            return await _context.Issues.ToListAsync();
        }

        public async Task<Issue?> GetByIdAsync(int id)
        {
            return await _context.Issues
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Issue> CreateAsync(Issue issue)
        {
            _context.Issues.Add(issue);

            await _context.SaveChangesAsync();

            return issue;
        }

        public async Task<bool> UpdateAsync(int id, Issue issue)
        {
            var existingIssue = await GetByIdAsync(id);

            if (existingIssue is null)
            {
                return false;
            }

            existingIssue.Title = issue.Title;
            existingIssue.Description = issue.Description;
            existingIssue.Priority = issue.Priority;
            existingIssue.Type = issue.Type;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingIssue = await GetByIdAsync(id);

            if (existingIssue is null)
            {
                return false;
            }

            _context.Issues.Remove(existingIssue);

            await _context.SaveChangesAsync();

            return true;
        }




    }
}