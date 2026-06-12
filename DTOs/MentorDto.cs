using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Mentors.Models;

namespace TraineeManagementApi.Mentors.DTOs;
public class MentorCreateDto
{
    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

     [Required(ErrorMessage = "Expertise is required")]
    public string Expertise { get; set; } = string.Empty;

    [AllowedValues(MentorStatus.Active, MentorStatus.Inactive)]
    public MentorStatus Status { get; set; }
}

public class MentorUpdateDto
{
    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

     [Required(ErrorMessage = "Expertise is required")]
    public string Expertise { get; set; } = string.Empty;

    [AllowedValues(MentorStatus.Active, MentorStatus.Inactive)]
    public MentorStatus Status { get; set; }
}

public class MentorResponseDto
{
    [Key]
    public int Id {get; set;}

    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; } = string.Empty;
}