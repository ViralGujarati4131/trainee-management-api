using TraineeManagement.Api.Data.TaskAssignmentModel;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.TaskAssignmentDTO;

public record TaskAssignmentCreateDto
(
    int TraineeId,

    int MentorId,

    int LearningTaskId,

    [RequiredField]
    DateOnly AssignedDate,

    [RequiredField]
    [ValidDateRange("AssignedDate")]
    DateOnly DueDate,

    [ValidEnum(typeof(TaskAssignmentStatus))]
    [RequiredField]
    TaskAssignmentStatus? Status,

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

    TaskAssignmentStatus? Status,

    string Remarks
);

public record TaskAssignmentUpdateDto
(
    [ValidEnum(typeof(TaskAssignmentStatus))]
    [RequiredField]
    TaskAssignmentStatus? Status
);
