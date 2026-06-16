using System.ComponentModel.DataAnnotations;
using HireHub.Domain.Enums;

namespace HireHub.Application.DTOs;

public class UpdateApplicationStatusDto
{
    [Required]
    public ApplicationStatus Status { get; set; }
}