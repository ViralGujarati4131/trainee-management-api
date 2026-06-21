using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Trainees.Models;
using TraineeManagementApi.Utils.CustomValidation;

namespace TraineeManagementApi.Trainees.DTOs;

public record TraineeUpdateDto
(
    [RequiredField]
    [FieldMaxLength(50)]
    string FirstName,

    [RequiredField]
    [FieldMaxLength(50)]
    string LastName,

    [EmailAddress]
    string Email,

    [RequiredField]
    string TechStack,

    [ValidEnum(typeof(TraineeStatus))]
    [RequiredField]
    TraineeStatus? Status
);

public record TraineeResponseDto
(
    int Id,

    string FirstName,

    string LastName
);

public record TraineeCreateDto
(
    [RequiredField]
    [FieldMaxLength(50)]
    string FirstName,

    [RequiredField]
    [FieldMaxLength(50)]
    string LastName,

    [EmailAddress]
    string Email,

    [RequiredField]
    string TechStack,

    [ValidEnum(typeof(TraineeStatus))]
    [RequiredField]
    TraineeStatus? Status
);

public record TraineePaginationSearchDto
(
    int PageNumber,

    int PageSize,

    int TotalRecords,
    
    IEnumerable<TraineeResponseDto>? Data
);