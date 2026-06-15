using HireHub.API.Data;
using HireHub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> CreateApplication([FromBody] Application application)
    {
        application.Id = Guid.NewGuid();
        application.AppliedAt = DateTime.UtcNow;

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllApplications), new { id = application.Id }, application);
    }
}