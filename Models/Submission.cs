using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Models.TimestampInterface;
using TraineeManagementApi.TaskAssignments.Models;
using TraineeManagementApi.Reviews.Models;

namespace TraineeManagementApi.Submissions.Models;

public class Submission : SubmissionTimeStamp
{
    [Key]
    public int Id { get; set; }

    public int TaskAssignmentId { get; set; } 
    public TaskAssignment? TaskAssignment { get; set; }

    [Required(ErrorMessage = "Submissio Url is required")]
    public string SubmissionUrl {get; set;} = string.Empty;

    [Required(ErrorMessage = "Notes is required")]
    public string Notes {get; set;} = string.Empty;

    public DateOnly SubmittedDate { get; set; }

    [EnumDataType(typeof(SubmissionStatus), ErrorMessage = "Invalid Status")]
    public SubmissionStatus Status { get; set; }

    public List<Review> Reviews { get; set; } = new List<Review>();
}

public enum SubmissionStatus
{
    Submitted,
    Resubmitted
}