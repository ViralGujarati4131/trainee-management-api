namespace TraineeManagement.Api.Contract.TrainingDirectoryContract;

public class TraineeProfileDto
{
    public int TraineeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CurrentTechStack { get; set; } = string.Empty;
    public string TrainingStatus { get; set; } = string.Empty;
}