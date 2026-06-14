using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Reviews.Models;

namespace TraineeManagementApi.Reviews.DTOs;

public record ReviewCreateDto
(
    int SubmissionId,
    int MentorId,
    [Required(ErrorMessage = "Feedback is required")]
    string Feedback,
    int score,
    [EnumDataType(typeof(ReviewStatus), ErrorMessage = "Invalid Status")]
    ReviewStatus Status
);

public record ReviewResponseDto
(
    int Id,
    int SubmissionId,
    int MentorId, 
    string Feedback,   
    int score,
    ReviewStatus Status,
    DateOnly ReviewedDate
);