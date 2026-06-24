using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Data.MentorModel;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.MentorDTO;

public record MentorCreateDto
(
    [RequiredField]
    string FirstName,

    [RequiredField]
    string LastName,

    [RequiredField]
    [EmailAddress]
    string Email,

    [RequiredField]
    string Expertise,

    [ValidEnum(typeof(MentorStatus))]
    [RequiredField]
    MentorStatus Status
);

public record MentorUpdateDto
(
    [RequiredField]
    string FirstName,

    [RequiredField]
    string LastName,

    [EmailAddress]
    string Email,

    [RequiredField]
    string Expertise,

    [ValidEnum(typeof(MentorStatus))]
    [RequiredField]
    MentorStatus? Status
);

public record MentorResponseDto
(
    int Id,

    string FirstName,
    
    string LastName
);