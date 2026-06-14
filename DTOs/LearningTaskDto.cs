using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.LearningTasks.Models;

namespace TraineeManagementApi.LearningTasks.DTOs;

public record LearningTaskCreateDto
(
    [Required(ErrorMessage = "Title is required")]
    string Title,

    [Required(ErrorMessage = "Description is required")]
    string Description,

    [Required(ErrorMessage = "ExpectedTechStack is required")]
    string ExpectedTechStack,

    [Required(ErrorMessage = "DueDate is required")]
    DateOnly DueDate,

    [EnumDataType(typeof(LearningTaskStatus), ErrorMessage = "Invalid Status")]
    LearningTaskStatus Status
);

public record LearningTaskUpdateDto
(
    [Required(ErrorMessage = "Title is required")]
    string Title,

    [Required(ErrorMessage = "Description is required")]
    string Description,

    [Required(ErrorMessage = "ExpectedTechStack is required")]
    string ExpectedTechStack,

    [Required(ErrorMessage = "DueDate is required")]
    DateOnly DueDate,

    [EnumDataType(typeof(LearningTaskStatus), ErrorMessage = "Invalid Status")]
    LearningTaskStatus Status
);

public record LearningTaskResposeDto
(
    int Id,
    string Title,
    string Description,
    string ExpectedTechStack,
    DateOnly DueDate,
    LearningTaskStatus Status
);