using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Reviews.Models;

namespace TraineeManagementApi.Reviews.DTOs;

public record ReviewCreateDto
(
    int SubmissionId,
    
    int MentorId,
    
    [Required]
    string Feedback,
    
    int score,
    
    [EnumDataType(typeof(ReviewStatus))]
    [Required]
    ReviewStatus? ReviewStatus,
    
    [Required]
    DateOnly ReviewedDate
);

public record ReviewResponseDto
(
    int Id,
    
    int SubmissionId,
    
    int MentorId,
    
    string Feedback,
    
    int score,
    
    ReviewStatus? ReviewStatus,
    
    DateOnly ReviewedDate
);