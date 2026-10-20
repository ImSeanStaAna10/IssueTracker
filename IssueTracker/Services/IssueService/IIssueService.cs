
using IssueTracker.Api.Models;
using IssueTracker.Enums;

namespace IssueTracker.Services.IssueService
{
    public interface IIssueService
    {
        Task<List<Issue>> GetAllAsync(
            int userId,
            string? search = null,
            string? status = null,
            IssueType? type = null,
            IssuePriority? priority = null
        );

        Task<Issue?> GetByIdAsync(int id, int userId);

        Task<Issue> CreateAsync(Issue issue, int userId);

        Task<bool> UpdateAsync(int id, Issue issue, int userId);

        Task<bool> DeleteAsync(int id, int userId);

        Task<bool> StartAsync(int id, int userId);

        Task<bool> ResolveAsync(int id, int userId);

        Task<bool> CloseAsync(int id, int userId);
    }
}
