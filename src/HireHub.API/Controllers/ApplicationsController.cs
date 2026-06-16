using HireHub.API.Data;
using HireHub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HireHub.API.DTOs;

namespace HireHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApplicationsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
public async Task<IActionResult> GetAllApplications()
{
    var applications = await _context.Applications
        .Include(a => a.Job)
        .Include(a => a.User)
        .Select(a => new ApplicationResponseDto
        {
            Id = a.Id,
            CoverLetter = a.CoverLetter,
            Status = a.Status.ToString(),
            AppliedAt = a.AppliedAt,
            JobTitle = a.Job != null ? a.Job.Title : "",
            UserName = a.User != null ? a.User.FullName : "",
            UserEmail = a.User != null ? a.User.Email : ""
        })
        .ToListAsync();

    return Ok(applications);
}

    [HttpPost]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDto dto)
    {
        var jobExists = await _context.Jobs.AnyAsync(j => j.Id == dto.JobId);
        if (!jobExists)
            return BadRequest("Job not found.");

        var userExists = await _context.Users.AnyAsync(u => u.Id == dto.UserId);
        if (!userExists)
            return BadRequest("User not found.");

        var alreadyApplied = await _context.Applications
            .AnyAsync(a => a.JobId == dto.JobId && a.UserId == dto.UserId);
        if (alreadyApplied)
            return Conflict("You have already applied for this job.");

        var application = new Application
        {
            Id = Guid.NewGuid(),
            JobId = dto.JobId,
            UserId = dto.UserId,
            CoverLetter = dto.CoverLetter,
            AppliedAt = DateTime.UtcNow
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllApplications), new { id = application.Id }, application);
    }

[HttpPut("{id}/status")]
public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateApplicationStatusDto dto)
{
    var application = await _context.Applications.FindAsync(id);

    if (application == null)
        return NotFound();

    application.Status = dto.Status;
    await _context.SaveChangesAsync();

    return NoContent();
}
}