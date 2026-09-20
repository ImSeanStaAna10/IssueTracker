using IssueTracker.Api.Models;
using IssueTracker.Services.IssueService;
using Microsoft.AspNetCore.Mvc;
using IssueTracker.DTOs.Issues;

namespace IssueTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;

        // Constructor to inject the IIssueService through DI
        public IssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet] public async Task<IActionResult> GetAll()
        {

            var issues = await _issueService.GetAllAsync();

            // Manual Mapping: List<Issue> → List<IssueResponseDto>
            var response = issues.Select(i => new IssueResponseDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Status = i.Status,
                Priority = i.Priority,
                Type = i.Type,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            });

            return Ok(response);
        }
        [HttpGet("{id}")] public async Task<IActionResult> GetById(int id)
        {
            var issue = await _issueService.GetByIdAsync(id);

            if (issue is null)
            {
                return NotFound();
            }

            var response = new IssueResponseDto
            {

                Id = issue.Id,
                Title = issue.Title,
                Description = issue.Description,
                Status = issue.Status,
                Priority = issue.Priority,
                Type = issue.Type,
                CreatedAt = issue.CreatedAt,
                UpdatedAt = issue.UpdatedAt
            };

            return Ok(response);
        }
        [HttpPost] public async Task<IActionResult> Create(CreateIssueDto createIssueDto)
        {

            // Manual Mapping the CreateIssueDto to an Issue model
            var issue = new Issue
            {
                Title =  createIssueDto.Title,
                Description = createIssueDto.Description,   
                Priority = createIssueDto.Priority,
                Type = createIssueDto.Type  

            };

            var createdIssue = await _issueService.CreateAsync(issue);

            // Manual Mapping: Issue → IssueResponseDto
            var response = new IssueResponseDto
            {
                Id = createdIssue.Id,
                Title = createdIssue.Title,
                Description = createdIssue.Description,
                Status = createdIssue.Status,
                Priority = createdIssue.Priority,
                Type = createdIssue.Type,
                CreatedAt = createdIssue.CreatedAt,
                UpdatedAt = createdIssue.UpdatedAt
            };

            return Ok(response);
        }
        [HttpPut("{id}")] public async Task<IActionResult> Update(int id, UpdateIssueDto updateIssueDto)
        {
            // Manual Mapping the UpdateIssueDto to an Issue model
            var issue = new Issue
            {
                Title = updateIssueDto.Title,
                Description = updateIssueDto.Description,
                Priority = updateIssueDto.Priority,
                Type = updateIssueDto.Type
            };

            var updated = await _issueService.UpdateAsync(id, issue);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _issueService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}