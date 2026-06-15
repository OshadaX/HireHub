using HireHub.API.Data;
using HireHub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HireHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly AppDbContext _context;

    public JobsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllJobs()
    {
        var jobs = await _context.Jobs.ToListAsync();
        return Ok(jobs);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] Job job)
    {
        job.Id = Guid.NewGuid();
        job.CreatedAt = DateTime.UtcNow;

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAllJobs), new { id = job.Id }, job);
    }
    
    [HttpGet("{id}")]
public async Task<IActionResult> GetJobById(Guid id)
{
    var job = await _context.Jobs.FindAsync(id);

    if (job == null)
        return NotFound();

    return Ok(job);
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteJob(Guid id)
{
    var job = await _context.Jobs.FindAsync(id);

    if (job == null)
        return NotFound();

    _context.Jobs.Remove(job);
    await _context.SaveChangesAsync();

    return NoContent();
}
}

