using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.LearningTasks.Models;

namespace TraineeManagementApi.LearningTasks.DTOs;

public record LearningTaskCreateDto
(
    [Required]
    string Title,

    [Required]
    string Description,

    [Required]
    string ExpectedTechStack,

    [Required]
    DateOnly DueDate,

    [EnumDataType(typeof(LearningTaskStatus))]
    [Required]
    LearningTaskStatus? Status
);

public record LearningTaskUpdateDto
(
    [Required]
    string Title,

    [Required]
    string Description,

    [Required]
    string ExpectedTechStack,

    [Required]
    DateOnly DueDate,

    [EnumDataType(typeof(LearningTaskStatus))]
    [Required]
    LearningTaskStatus Status
);

public record LearningTaskResposeDto
(
    int Id,
    
    string Title,
    
    string Description,
    
    string ExpectedTechStack,
    
    DateOnly DueDate,
    
    LearningTaskStatus? Status
);