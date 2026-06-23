namespace TraineeManagementApi.Messaging.Contracts
{
    public class SubmissionProcessingRequested
    {
        public Guid MessageId 
        { 
            get; 
            init; 
        }
        public Guid CorrelationId 
        { 
            get; 
            init; 
        }
        public int SubmissionId 
        { 
            get; 
            set;
        }
        public int FileId 
        { 
            get; 
            set; 
        }
        public DateTimeOffset RequestedAt 
        { 
            get; 
            init; 
        }
        public string ContractVersion 
        { 
            get; 
            init; 
        } = "1.0";
    }
    
    public class PublishResult
    {
        public Guid MessageId { get; set; }
        public Guid CorrelationId { get; set; }
        public bool Success { get; set; }
    }
}
