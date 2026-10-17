using IssueTracker.DTOs.Projects;
using IssueTracker.Models;
using IssueTracker.Services.ProjectService;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        //constructor
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        [HttpGet] public async Task<IActionResult> GetAll(string? search = null)
        {
            var projects = await _projectService.GetAllAsync(search);
            // Manual Mapping 
            var response = projects.Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt ,
                UpdatedAt = p.UpdatedAt ,   
            });
            return Ok(response);
        }
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);

            if(project is null)
            {
                return NotFound();
            }
            // Manual Mapping 
            var response = new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,

            };
            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> Create(CreateProjectDto createProjectDto)
        {
            // mapping
            var projects = new Project
            {
                Name = createProjectDto.Name,
                Description = createProjectDto.Description,
            };
            var createdProject = await _projectService.CreateAsync(projects);

            // mapping 
            var response = new ProjectResponseDto
            {

                Id = createdProject.Id,
                Name = createdProject.Name,
                Description = createdProject.Description,
                CreatedAt = createdProject.CreatedAt,
                UpdatedAt = createdProject.UpdatedAt,

            };
            return CreatedAtAction(
                nameof(GetById),
                new {id = createdProject.Id},
                response
                );
        }
        [HttpPut("{id}")] public async Task<IActionResult> Update(int id, UpdateProjectDto updateProjectDto)
        {
            //mapping UpdateProjectDTO to an project model  
            var project = new Project
            {
                Name = updateProjectDto.Name,
                Description = updateProjectDto.Description,
            };

            var updated = await _projectService.UpdateAsync(id , project);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();


        }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id)
        {

            var deleted = await _projectService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();


        }

    }

}