using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Trainees.Models;

namespace TraineeManagementApi.Trainees.DTOs;

public record TraineeUpdateDto
(
    [Required(ErrorMessage = "FirstName is required")]
    [MaxLength(50, ErrorMessage = "FirstName can not be exceed 50 character")]
    string FirstName,

    [Required(ErrorMessage = "LastName is required")]
    [MaxLength(50, ErrorMessage = "LastName can not be exceed 50 character")]
    string LastName,

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    string Email,

    [Required(ErrorMessage = "TechStack is required")]
    string TechStack,

    [EnumDataType(typeof(TraineeStatus), ErrorMessage = "Invalid Status")]
    TraineeStatus Status
);

public record TraineeResponseDto
(
    int Id,
    string FirstName,
    string LastName
);

public record TraineeCreateDto
(
    [Required(ErrorMessage = "FirstName is required")]
    [MaxLength(50, ErrorMessage = "FirstName can not be exceed 50 characters")]
    string FirstName,

    [Required(ErrorMessage = "LastName is required")]
    [MaxLength(50, ErrorMessage = "LastName can not be exceed 50 characters")]
    string LastName,

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    string Email,

    [Required(ErrorMessage = "TechStack is required")]
    string TechStack,

    [EnumDataType(typeof(TraineeStatus), ErrorMessage = "Invalid Status")]
    TraineeStatus Status
);

public record TraineePaginationSearchDto
(
    int PageNumber,
    int PageSize,
    int TotalRecords,
    IEnumerable<TraineeResponseDto>? Data
);