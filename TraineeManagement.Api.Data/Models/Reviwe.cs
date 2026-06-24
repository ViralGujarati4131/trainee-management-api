using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Data.SubmissionModel;
using TraineeManagement.Api.Data.MentorModel;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.ReviewModel;

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

    [RequiredField]
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

    [ValidEnum(typeof(ReviewStatus))]
    [RequiredField]
    public ReviewStatus? ReviewStatus 
    { 
        get; 
        set; 
    }

    [RequiredField]
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