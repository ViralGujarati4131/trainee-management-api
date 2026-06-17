using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Submissions.Models;

namespace TraineeManagementApi.Submissions.DTOs;

public record SubmissionCreateDto
(
    int TaskAssignmentId,

    [Required(ErrorMessage = "Submissio Url is required")]
    string SubmissionUrl,

    [Required(ErrorMessage = "Notes is required")]
    string Notes,

    [Required(ErrorMessage="SubmittedDate required")]
    DateOnly SubmittedDate,

    [EnumDataType(typeof(SubmissionStatus), ErrorMessage = "Invalid Status")]
    [Required(ErrorMessage = "Status is required")]
    SubmissionStatus? Status
);

public record SubmissionResponseDto
(
    int Id,
    int TaskAssignmentId,
    string SubmissionUrl,
    string Notes,
    DateOnly SubmittedDate,
    SubmissionStatus? Status
);