using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Submissions.Models;
using TraineeManagementApi.Mentors.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraineeManagementApi.Reviews.Models;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id 
    { 
        get; 
        set; 
    }

    public int SubmissionId
    { 
        get; 
        set; 
    }
    public Submission? Submission 
    { 
        get; 
        set; 
    }

    public int MentorId 
    { 
        get; 
        set; 
    }
    public Mentor? Mentor 
    { 
        get; 
        set; 
    }

    [Required]
    public string Feedback 
    { 
        get; 
        set; 
    } = string.Empty;

    public int Score 
    { 
        get; 
        set; 
    }

    [EnumDataType(typeof(ReviewStatus))]
    [Required]
    public ReviewStatus? ReviewStatus 
    { 
        get; 
        set; 
    }

    [Required]
    public DateOnly ReviewedDate 
    { 
        get; 
        set; 
    }
}

public enum ReviewStatus
{
    Accepted,

    ChangesRequired,
    
    Rejected
}