using TraineeManagement.Api.Data.ReviewModel;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.ReviewDTO;

public record ReviewCreateDto
(
    int SubmissionId,
    
    int MentorId,
    
    [RequiredField]
    string Feedback,
    
    int score,
    
    [ValidEnum(typeof(ReviewStatus))]
    [RequiredField]
    ReviewStatus? ReviewStatus,
    
    [RequiredField]
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