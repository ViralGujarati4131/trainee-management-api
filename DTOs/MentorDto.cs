using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Mentors.Models;

namespace TraineeManagementApi.Mentors.DTOs;

public record MentorCreateDto
(
    [Required(ErrorMessage = "FirstName is required")]
    string FirstName,

    [Required(ErrorMessage = "LastName is required")]
    string LastName,

    [EmailAddress]
    string Email,

    [Required(ErrorMessage = "Expertise is required")]
    string Expertise,

    [EnumDataType(typeof(MentorStatus), ErrorMessage = "Invalid Status")]
    MentorStatus Status
);

public record MentorUpdateDto
(
    [Required(ErrorMessage = "FirstName is required")]
    string FirstName,

    [Required(ErrorMessage = "LastName is required")]
    string LastName,

    [EmailAddress]
    string Email,

    [Required(ErrorMessage = "Expertise is required")]
    string Expertise,

    [EnumDataType(typeof(MentorStatus), ErrorMessage = "Invalid Status")]
    MentorStatus Status
);

public record MentorResponseDto
(
    int Id,
    string FirstName,
    string LastName
);