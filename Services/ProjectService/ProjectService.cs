
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

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);

    }
    public async Task<Project> CreateAsync(Project project)
    {
        _context.Projects.Add(project);

        await _context.SaveChangesAsync();
        return project;
    }
    public async Task<bool> UpdateAsync(int id , Project project)
    {
        var existingProject = await GetByIdAsync(id);

        if (existingProject is null)
        {
            return false;
        }

        existingProject.Update(
            project.Name,
            project.Description
        );

        await _context.SaveChangesAsync();

        return true;

    }
    public async Task<bool> DeleteAsync(int id)
    {
        var existingProject = await GetByIdAsync(id);

        if(existingProject is null)
        {
            return false;
        }

        _context.Projects.Remove(existingProject);

        await _context.SaveChangesAsync();

        return true;

    }

}

   
