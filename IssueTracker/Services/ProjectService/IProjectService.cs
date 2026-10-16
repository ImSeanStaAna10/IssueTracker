
using IssueTracker.Models;

namespace IssueTracker.Services.ProjectService;

public interface IProjectService
{
    Task<List<Project>> GetAllAsync(string? search = null);
    Task<Project?> GetByIdAsync(int id);
    Task<Project> CreateAsync(Project project);
    Task<bool> UpdateAsync(int id, Project project);
    Task<bool> DeleteAsync(int id); 








}