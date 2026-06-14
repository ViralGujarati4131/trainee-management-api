using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Models.TimestampInterface;
using TraineeManagementApi.Submissions.Models;
using TraineeManagementApi.Mentors.Models;

namespace TraineeManagementApi.Reviews.Models;

public class Review : ReviewTimeStamp
{
    [Key]
    public int Id { get; set; }

    public int SubmissionId { get; set; } 
    public Submission? Submission { get; set; }

    public int MentorId { get; set; } 
    public Mentor? Mentor { get; set; }

    [Required(ErrorMessage = "Feedback is required")]
    public string Feedback {get; set;} = string.Empty;

    public int Score {get; set;}

    [EnumDataType(typeof(ReviewStatus), ErrorMessage = "Invalid Status")]
    public ReviewStatus Status { get; set; }
    
    public DateOnly ReviewedDate { get; set; }
}

public enum ReviewStatus
{
    Accepted,
    ChangesRequired,
    Rejected
}