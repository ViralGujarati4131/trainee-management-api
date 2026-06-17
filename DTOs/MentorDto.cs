using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Mentors.Models;

namespace TraineeManagementApi.Mentors.DTOs;

public record MentorCreateDto
(
    [Required]
    string FirstName,

    [Required]
    string LastName,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    string Expertise,

    [EnumDataType(typeof(MentorStatus))]
    [Required]
    MentorStatus Status
);

public record MentorUpdateDto
(
    [Required]
    string FirstName,

    [Required]
    string LastName,

    [EmailAddress]
    string Email,

    [Required]
    string Expertise,

    [EnumDataType(typeof(MentorStatus))]
    [Required]
    MentorStatus? Status
);

public record MentorResponseDto
(
    int Id,

    string FirstName,
    
    string LastName
);