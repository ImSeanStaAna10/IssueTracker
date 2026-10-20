
using IssueTracker.Models;

namespace IssueTracker.Services.ProjectService;

public interface IProjectService
{
    Task<List<Project>> GetAllAsync(int userId, string? search = null);

    Task<Project?> GetByIdAsync(int id, int userId);

    Task<Project> CreateAsync(Project project, int userId);

    Task<bool> UpdateAsync(int id, Project project, int userId);

    Task<bool> DeleteAsync(int id, int userId);
}