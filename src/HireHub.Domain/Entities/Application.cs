using HireHub.Domain.Enums;

namespace HireHub.Domain.Entities;

public class Application
{
    public Guid Id { get; set; }
    public string CoverLetter { get; set; } = string.Empty;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public Guid JobId { get; set; }
    public Job? Job { get; set; } 

    public Guid UserId { get; set; }
    public User? User { get; set; } 
}