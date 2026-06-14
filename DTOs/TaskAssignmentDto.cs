using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.TaskAssignments.Models;

namespace TraineeManagementApi.TaskAssignments.DTOs;

public record TaskAssignmentCreateDto
(
    int TraineeId,

    int MentorId,

    int LearningTaskId,

    [Required(ErrorMessage="DueDate required")]
    DateOnly DueDate,

    [EnumDataType(typeof(TaskAssignmentStatus), ErrorMessage = "Invalid Status")]
    TaskAssignmentStatus Status,
    
    string Remarks
);

public record TaskAssignmentResponseDto
(
    int Id,
    int TraineeId,
    int MentorId,
    int LearningTaskId,
    DateOnly AssignedDate,
    DateOnly DueDate,
    TaskAssignmentStatus Status,
    string Remarks
);

public record TaskAssignmentUpdateDto
(
    [EnumDataType(typeof(TaskAssignmentStatus), ErrorMessage = "Invalid Status")]
    TaskAssignmentStatus Status
);
