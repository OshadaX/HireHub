using System.ComponentModel.DataAnnotations;

namespace HireHub.Application.DTOs;

public class CreateApplicationDto
{
    [Required]
    public Guid JobId { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public string CoverLetter { get; set; } = string.Empty;
}