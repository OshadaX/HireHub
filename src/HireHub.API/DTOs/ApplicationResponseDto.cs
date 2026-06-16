namespace HireHub.API.DTOs;

public class ApplicationResponseDto
{
    public Guid Id { get; set; }
    public string CoverLetter { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime AppliedAt { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
}