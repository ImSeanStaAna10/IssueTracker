
using IssueTracker.Api.Models;
using IssueTracker.Services.IssueService;
using IssueTracker.DTOs.Issues;
using IssueTracker.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IssueTracker.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        private int GetCurrentUserId()
        {
            var userIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                throw new UnauthorizedAccessException(
                    "Authenticated user ID is missing or invalid.");
            }

            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search = null,
            [FromQuery] string? status = null,
            [FromQuery] IssueType? type = null,
            [FromQuery] IssuePriority? priority = null)
        {
            var userId = GetCurrentUserId();

            var issues = await _issueService.GetAllAsync(
                userId, search, status, type, priority);

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetCurrentUserId();

            var issue = await _issueService.GetByIdAsync(id, userId);

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

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateIssueDto createIssueDto)
        {
            var userId = GetCurrentUserId();

            var issue = new Issue
            {
                ProjectId = createIssueDto.ProjectId,
                Title = createIssueDto.Title,
                Description = createIssueDto.Description,
                Priority = createIssueDto.Priority,
                Type = createIssueDto.Type
            };

            var createdIssue =
                await _issueService.CreateAsync(issue, userId);

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
                response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateIssueDto updateIssueDto)
        {
            var userId = GetCurrentUserId();

            var issue = new Issue
            {
                ProjectId = updateIssueDto.ProjectId,
                Title = updateIssueDto.Title,
                Description = updateIssueDto.Description,
                Priority = updateIssueDto.Priority,
                Type = updateIssueDto.Type
            };

            var updated = await _issueService.UpdateAsync(
                id, issue, userId);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();

            var deleted = await _issueService.DeleteAsync(id, userId);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var userId = GetCurrentUserId();

            var started = await _issueService.StartAsync(id, userId);

            if (!started)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/resolve")]
        public async Task<IActionResult> Resolve(int id)
        {
            var userId = GetCurrentUserId();

            var resolved = await _issueService.ResolveAsync(id, userId);

            if (!resolved)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/close")]
        public async Task<IActionResult> Close(int id)
        {
            var userId = GetCurrentUserId();

            var closed = await _issueService.CloseAsync(id, userId);

            if (!closed)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
