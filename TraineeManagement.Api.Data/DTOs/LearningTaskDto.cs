using TraineeManagement.Api.Data.LearningTaskModel;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.LearningTaskDTO;

public record LearningTaskCreateDto
(
    [RequiredField]
    string Title,

    [RequiredField]
    string Description,

    [RequiredField]
    string ExpectedTechStack,

    [RequiredField]
    DateOnly DueDate,

    [ValidEnum(typeof(LearningTaskStatus))]
    [RequiredField]
    LearningTaskStatus? Status
);

public record LearningTaskUpdateDto
(
    [RequiredField]
    string Title,

    [RequiredField]
    string Description,

    [RequiredField]
    string ExpectedTechStack,

    [RequiredField]
    DateOnly DueDate,

    [ValidEnum(typeof(LearningTaskStatus))]
    [RequiredField]
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