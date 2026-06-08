using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Models;
namespace TraineeManagementApi.DTOs;

public class UpdateTraineeDto
{
    [Required]
    [MaxLength(50)]    
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? TechStack { get; set; }

    [Required]
    [AllowedValues(TraineeStatus.Active,TraineeStatus.Inactive,TraineeStatus.Completed)]
    public TraineeStatus Status { get; set; }
}


public class TraineeResponseDto
{
    public int Id { get; set; }
       
    [Required]
    [MaxLength(50)]    
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string? LastName { get; set; }
}


public class CreateTraineeDto
{
    [Required]
    [MaxLength(50)]    
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? TechStack { get; set; }

    [Required]
    [AllowedValues(TraineeStatus.Active,TraineeStatus.Inactive,TraineeStatus.Completed)]
    public TraineeStatus Status { get; set; }
}