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
}