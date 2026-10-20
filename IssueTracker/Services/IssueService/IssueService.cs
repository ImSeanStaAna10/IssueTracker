
using IssueTracker.Api.Models;
using IssueTracker.Data;
using IssueTracker.Enums;
using IssueTracker.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Services.IssueService
{
    public class IssueService : IIssueService
    {
        private readonly IssueTrackerDbContext _context;

        public IssueService(IssueTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Issue>> GetAllAsync(
            int userId,
            string? search = null,
            string? status = null,
            IssueType? type = null,
            IssuePriority? priority = null)
        {
            var query = _context.Issues
                .Where(i => i.Project.UserId == userId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i => i.Title.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(i => i.Status == status);
            }

            if (type.HasValue)
            {
                query = query.Where(i => i.Type == type.Value);
            }

            if (priority.HasValue)
            {
                query = query.Where(i => i.Priority == priority.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Issue?> GetByIdAsync(int id, int userId)
        {
            return await _context.Issues
                .FirstOrDefaultAsync(i =>
                    i.Id == id &&
                    i.Project.UserId == userId);
        }

        public async Task<Issue> CreateAsync(Issue issue, int userId)
        {
            var projectExists = await _context.Projects
                .AnyAsync(p =>
                    p.Id == issue.ProjectId &&
                    p.UserId == userId);

            if (!projectExists)
            {
                throw new ProjectNotFoundException(
                    "Project not found.");
            }

            _context.Issues.Add(issue);

            await _context.SaveChangesAsync();

            return issue;
        }

        public async Task<bool> UpdateAsync(
            int id,
            Issue issue,
            int userId)
        {
            var existingIssue = await GetByIdAsync(id, userId);

            if (existingIssue is null)
            {
                return false;
            }

            var projectExists = await _context.Projects
                .AnyAsync(p =>
                    p.Id == issue.ProjectId &&
                    p.UserId == userId);

            if (!projectExists)
            {
                throw new ProjectNotFoundException(
                    "Project not found.");
            }

            existingIssue.ProjectId = issue.ProjectId;
            existingIssue.Title = issue.Title;
            existingIssue.Description = issue.Description;
            existingIssue.Priority = issue.Priority;
            existingIssue.Type = issue.Type;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var existingIssue = await GetByIdAsync(id, userId);

            if (existingIssue is null)
            {
                return false;
            }

            _context.Issues.Remove(existingIssue);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> StartAsync(int id, int userId)
        {
            var issue = await GetByIdAsync(id, userId);

            if (issue is null)
            {
                return false;
            }

            issue.Start();

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ResolveAsync(int id, int userId)
        {
            var issue = await GetByIdAsync(id, userId);

            if (issue is null)
            {
                return false;
            }

            issue.Resolve();

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CloseAsync(int id, int userId)
        {
            var issue = await GetByIdAsync(id, userId);

            if (issue is null)
            {
                return false;
            }

            issue.Close();

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
