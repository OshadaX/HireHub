using HireHub.API.Data;
using HireHub.API.DTOs;
using HireHub.Domain.Entities;
using HireHub.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HireHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
        var jobs = await _context.Jobs
            .Include(j => j.Company)
            .Select(j => new JobResponseDto
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                Location = j.Location,
                Salary = j.Salary,
                JobType = j.JobType,
                CompanyName = j.Company != null ? j.Company.Name : "",
                CreatedAt = j.CreatedAt
            })
            .ToListAsync();

        return Ok(jobs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobById(Guid id)
    {
        var job = await _context.Jobs
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (job == null)
            return NotFound();

        var response = new JobResponseDto
        {
            Id = job.Id,
            Title = job.Title,
            Description = job.Description,
            Location = job.Location,
            Salary = job.Salary,
            JobType = job.JobType,
            CompanyName = job.Company != null ? job.Company.Name : "",
            CreatedAt = job.CreatedAt
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob([FromBody] CreateJobDto dto)
    {
        var companyExists = await _context.Companies.AnyAsync(c => c.Id == dto.CompanyId);
        if (!companyExists)
            return BadRequest("Company not found.");

        var job = new Job
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,
            Salary = dto.Salary,
            JobType = dto.JobType,
            CompanyId = dto.CompanyId,
            Status = JobStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJobById), new { id = job.Id }, job);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJob(Guid id, [FromBody] UpdateJobDto dto)
    {
        var job = await _context.Jobs.FindAsync(id);

        if (job == null)
            return NotFound();

        if (dto.Title != null) job.Title = dto.Title;
        if (dto.Description != null) job.Description = dto.Description;
        if (dto.Location != null) job.Location = dto.Location;
        if (dto.Salary != null) job.Salary = dto.Salary.Value;
        if (dto.JobType != null) job.JobType = dto.JobType;

        await _context.SaveChangesAsync();

        return NoContent();
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
