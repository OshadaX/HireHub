using System.ComponentModel.DataAnnotations;
using HireHub.Domain.Enums;

namespace HireHub.API.DTOs;

public class UpdateApplicationStatusDto
{
    [Required]
    public ApplicationStatus Status { get; set; }
}