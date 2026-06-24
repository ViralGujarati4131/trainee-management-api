namespace TraineeManagement.Api.Data.ProcessingJobModel;

public class ProcessingJob
{
    public Guid Id 
    { 
        get; 
        set; 
    } 
    public Guid CorrelationId 
    { 
        get; 
        set; 
    }
    public int SubmissionId 
    { 
        get; 
        set; 
    }
    public int Status 
    { 
        get; 
        set; 
    } 
    public int Attempts 
    { 
        get; 
        set; 
    }
    public string? ErrorSummary 
    { 
        get; 
        set; 
    }
    public DateTime StartedAt 
    { 
        get; 
        set; 
    }
    public DateTime? CompletedAt 
    { 
        get; 
        set; 
    }
}