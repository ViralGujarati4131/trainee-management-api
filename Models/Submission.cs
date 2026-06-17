using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.TaskAssignments.Models;
using TraineeManagementApi.Reviews.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraineeManagementApi.Submissions.Models;

public class Submission
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id 
    { 
        get; 
        set; 
    }

    public int TaskAssignmentId 
    { 
        get; 
        set; 
    }
    public TaskAssignment? TaskAssignment 
    { 
        get; 
        set; 
    }

    [Required]
    public string SubmissionUrl 
    { 
        get; 
        set; 
    } = string.Empty;

    [Required]
    public string Notes 
    { 
        get; 
        set; 
    } = string.Empty;

    [Required]
    public DateOnly SubmittedDate 
    { 
        get; 
        set; 
    }

    [EnumDataType(typeof(SubmissionStatus))]
    [Required]
    public SubmissionStatus? Status 
    { 
        get; 
        set; 
    }

    public ICollection<Review> Reviews 
    { 
        get; 
        set; 
    } = new List<Review>();
}

public enum SubmissionStatus
{
    Submitted,
    Resubmitted
}