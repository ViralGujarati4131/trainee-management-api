namespace TraineeManagementApi.Models.TimestampInterface;

public interface ITimestamp
{
    DateTime CreatedDate { get; set; }
    DateTime UpdatedDate { get; set; }
}

public interface TaskAssignmentTimeStamp
{
    DateOnly AssignedDate { get; set; }
}

public interface SubmissionTimeStamp
{
    DateOnly SubmittedDate { get; set; }
}

public interface ReviewTimeStamp
{
    DateOnly ReviewedDate { get; set; }
}