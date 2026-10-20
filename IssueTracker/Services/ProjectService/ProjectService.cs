
using IssueTracker.Data;
using IssueTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Services.ProjectService;

public class ProjectService : IProjectService
{
    private readonly IssueTrackerDbContext _context;

    public ProjectService(IssueTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync(
        int userId,
        string? search = null)
    {
        var query = _context.Projects
            .Where(p => p.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search));
        }

        return await query.ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id, int userId)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p =>
                p.Id == id && p.UserId == userId);
    }

    public async Task<Project> CreateAsync(Project project, int userId)
    {
        project.UserId = userId;

        _context.Projects.Add(project);

        await _context.SaveChangesAsync();

        return project;
    }

    public async Task<bool> UpdateAsync(
        int id,
        Project project,
        int userId)
    {
        var existingProject = await GetByIdAsync(id, userId);

        if (existingProject is null)
        {
            return false;
        }

        existingProject.Update(
            project.Name,
            project.Description);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var existingProject = await GetByIdAsync(id, userId);

        if (existingProject is null)
        {
            return false;
        }

        _context.Projects.Remove(existingProject);

        await _context.SaveChangesAsync();

        return true;
    }
}
