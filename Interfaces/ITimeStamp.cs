namespace TraineeManagementApi.Models.TimestampInterface;

public interface ITimestamp
{
    DateTime CreatedDate { get; set; }
    DateTime UpdatedDate { get; set; }
}