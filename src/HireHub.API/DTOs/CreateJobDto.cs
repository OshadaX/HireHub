using System.ComponentModel.DataAnnotations;

namespace HireHub.API.DTOs;

public class CreateJobDto
{
    [Required(ErrorMessage = "Job title is required")]
    [MaxLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job description is required")]
    [MinLength(20, ErrorMessage = "Description must be at least 20 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required")]
    public string Location { get; set; } = string.Empty;

    [Range(0, 1000000, ErrorMessage = "Salary must be between 0 and 1,000,000")]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "Job type is required")]
    [RegularExpression("^(FullTime|PartTime|Remote|Contract)$",
        ErrorMessage = "Job type must be FullTime, PartTime, Remote, or Contract")]
    public string JobType { get; set; } = string.Empty;
}
