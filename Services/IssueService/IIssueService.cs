using IssueTracker.Api.Models;

namespace IssueTracker.Services.IssueService
{
    public interface IIssueService
    {
        Task<List<Issue>> GetAllAsync();

        Task<Issue?> GetByIdAsync(int id);

        Task<Issue> CreateAsync(Issue issue);

        Task<bool> UpdateAsync(int id, Issue issue);

        Task<bool> DeleteAsync(int id);
    }
}