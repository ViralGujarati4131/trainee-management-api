using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Mentors.Models;

namespace TraineeManagementApi.Mentors.DTOs;

public record MentorCreateDto
(
    [Required(ErrorMessage = "FirstName is required")]
    string FirstName,

    [Required(ErrorMessage = "LastName is required")]
    string LastName,

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    string Email,

    [Required(ErrorMessage = "Expertise is required")]
    string Expertise,

    [EnumDataType(typeof(MentorStatus), ErrorMessage = "Invalid Status")]
    [Required(ErrorMessage = "Status is required")]
    MentorStatus Status
);

public record MentorUpdateDto
(
    [Required(ErrorMessage = "FirstName is required")]
    string FirstName,

    [Required(ErrorMessage = "LastName is required")]
    string LastName,

    [EmailAddress(ErrorMessage = "Valid Email is required")]
    string Email,

    [Required(ErrorMessage = "Expertise is required")]
    string Expertise,

    [EnumDataType(typeof(MentorStatus), ErrorMessage = "Invalid Status")]
    [Required(ErrorMessage = "Status is required")]
    MentorStatus? Status
);

public record MentorResponseDto
(
    int Id,
    string FirstName,
    string LastName
);