using System.ComponentModel.DataAnnotations;

namespace HireHub.Application.DTOs;

public class UpdateJobDto
{
    [MaxLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
    public string? Title { get; set; }

    [MinLength(20, ErrorMessage = "Description must be at least 20 characters")]
    public string? Description { get; set; }

    public string? Location { get; set; }

    [Range(0, 1000000, ErrorMessage = "Salary must be between 0 and 1,000,000")]
    public decimal? Salary { get; set; }

    [RegularExpression("^(FullTime|PartTime|Remote|Contract)$",
        ErrorMessage = "Job type must be FullTime, PartTime, Remote, or Contract")]
    public string? JobType { get; set; }
}
