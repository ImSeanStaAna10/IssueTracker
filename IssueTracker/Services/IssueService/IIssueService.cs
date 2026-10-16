using IssueTracker.Api.Models;

namespace IssueTracker.Services.IssueService
{
    public interface IIssueService
    {
        Task<List<Issue>> GetAllAsync(string? search = null);
        Task<Issue?> GetByIdAsync(int id);
        Task<Issue> CreateAsync(Issue issue);
        Task<bool> UpdateAsync(int id, Issue issue);
        Task<bool> DeleteAsync(int id);
        Task<bool> StartAsync(int id);
        Task<bool> ResolveAsync(int id);
        Task<bool> CloseAsync(int id);

    }
}