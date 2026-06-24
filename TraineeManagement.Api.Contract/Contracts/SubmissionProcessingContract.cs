namespace TraineeManagement.Api.Contract.SubmissionProcessingContarct;

public record SubmissionProcessingContract
(
    Guid MessageId,

    Guid CorrelationId,

    int SubmissionId,

    int FileId,
    
    DateTimeOffset RequestedAt, 
  
    string ContractVersion 
);
    
public record SubmissionPublishResult
(
    Guid MessageId,

    Guid CorrelationId,

    bool Success
);

