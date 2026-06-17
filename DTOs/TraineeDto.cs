using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Trainees.Models;

namespace TraineeManagementApi.Trainees.DTOs;

public record TraineeUpdateDto
(
    [Required]
    [MaxLength(50)]
    string FirstName,

    [Required]
    [MaxLength(50)]
    string LastName,

    [EmailAddress]
    string Email,

    [Required]
    string TechStack,

    [EnumDataType(typeof(TraineeStatus))]
    [Required]
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
    [Required]
    [MaxLength(50)]
    string FirstName,

    [Required]
    [MaxLength(50)]
    string LastName,

    [EmailAddress]
    string Email,

    [Required]
    string TechStack,

    [EnumDataType(typeof(TraineeStatus))]
    [Required]
    TraineeStatus? Status
);

public record TraineePaginationSearchDto
(
    int PageNumber,
    int PageSize,
    int TotalRecords,
    IEnumerable<TraineeResponseDto>? Data
);