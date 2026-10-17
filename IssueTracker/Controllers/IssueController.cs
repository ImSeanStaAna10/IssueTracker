
using IssueTracker.Api.Models;
using IssueTracker.Services.IssueService;
using Microsoft.AspNetCore.Mvc;
using IssueTracker.DTOs.Issues;
using IssueTracker.Enums;

namespace IssueTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;

        // Constructor: Injects IIssueService through Dependency Injection.
        public IssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }







        // GET: api/issue?search=keyword&status=OPEN&type=BUG&priority=HIGH
        // Retrieves all issues, with optional search and filtering by title, status, type, and priority.
        // Returns: 200 OK with a collection of IssueResponseDto objects.
        [HttpGet] 
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] IssueType? type = null,
            [FromQuery] IssuePriority? priority = null)
        {
            var issues = await _issueService.GetAllAsync(search, status, type, priority);

            // Manual Mapping: List<Issue> → List<IssueResponseDto>
            var response = issues.Select(i => new IssueResponseDto
            {
                Id = i.Id,
                ProjectId = i.ProjectId,
                Title = i.Title,
                Description = i.Description,
                Status = i.Status,
                Type = i.Type,
                Priority = i.Priority,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            });

            return Ok(response);
        }

        // GET: api/issue/{id}
        // Retrieves a single issue by its ID.
        // Returns: 200 OK with the issue, or 404 Not Found if it does not exist.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var issue = await _issueService.GetByIdAsync(id);

            if (issue is null)
            {
                return NotFound();
            }

            var response = new IssueResponseDto
            {
                Id = issue.Id,
                ProjectId = issue.ProjectId,
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

        // POST: api/issue
        // Creates a new issue using the provided CreateIssueDto.
        // Returns: 201 Created with the created issue and its resource location.
        // Validation and business rules are handled by the DTO, service, and model.
        [HttpPost]
        public async Task<IActionResult> Create(CreateIssueDto createIssueDto)
        {
            // Manual Mapping: CreateIssueDto → Issue
            var issue = new Issue
            {
                ProjectId = createIssueDto.ProjectId,
                Title = createIssueDto.Title,
                Description = createIssueDto.Description,
                Priority = createIssueDto.Priority,
                Type = createIssueDto.Type
            };

            var createdIssue = await _issueService.CreateAsync(issue);

            // Manual Mapping: Issue → IssueResponseDto
            var response = new IssueResponseDto
            {
                Id = createdIssue.Id,
                ProjectId = createdIssue.ProjectId,
                Title = createdIssue.Title,
                Description = createdIssue.Description,
                Status = createdIssue.Status,
                Priority = createdIssue.Priority,
                Type = createdIssue.Type,
                CreatedAt = createdIssue.CreatedAt,
                UpdatedAt = createdIssue.UpdatedAt
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdIssue.Id },
                response
            );
        }

        // PUT: api/issue/{id}
        // Updates an existing issue using the provided UpdateIssueDto.
        // Returns: 204 No Content if updated, or 404 Not Found if the issue does not exist.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateIssueDto updateIssueDto)
        {
            // Manual Mapping: UpdateIssueDto → Issue
            var issue = new Issue
            {
                ProjectId = updateIssueDto.ProjectId,
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

        // DELETE: api/issue/{id}
        // Deletes an existing issue by its ID.
        // Returns: 204 No Content if deleted, or 404 Not Found if the issue does not exist.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _issueService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/issue/{id}/start
        // Moves an issue from OPEN to IN_PROGRESS, subject to the model's business rules.
        // Returns: 204 No Content if successful, or 404 Not Found if the issue does not exist.
        // Invalid state transitions are handled by the exception-handling pipeline.
        [HttpPost("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var started = await _issueService.StartAsync(id);

            if (!started)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/issue/{id}/resolve
        // Moves an issue from IN_PROGRESS to RESOLVED, subject to the model's business rules.
        // Returns: 204 No Content if successful, or 404 Not Found if the issue does not exist.
        // Invalid state transitions are handled by the exception-handling pipeline.
        [HttpPost("{id}/resolve")]
        public async Task<IActionResult> Resolve(int id)
        {
            var resolved = await _issueService.ResolveAsync(id);

            if (!resolved)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/issue/{id}/close
        // Moves an issue from RESOLVED to CLOSED, subject to the model's business rules.
        // Returns: 204 No Content if successful, or 404 Not Found if the issue does not exist.
        // Invalid state transitions are handled by the exception-handling pipeline.
        [HttpPost("{id}/close")]
        public async Task<IActionResult> Close(int id)
        {
            var closed = await _issueService.CloseAsync(id);

            if (!closed)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
