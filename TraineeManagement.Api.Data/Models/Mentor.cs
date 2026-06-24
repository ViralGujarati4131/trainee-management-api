using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Data.ModelTimestampInterface;
using TraineeManagement.Api.Data.TaskAssignmentModel;
using TraineeManagement.Api.Data.ReviewModel;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.MentorModel;

public class Mentor : ICreateTimestamp,IUpdateTimestamp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id 
    { 
        get; 
        set; 
    }

    [RequiredField]
    public string FirstName 
    { 
        get; 
        set; 
    } = string.Empty;

    [RequiredField]
    public string LastName 
    { 
        get; 
        set; 
    } = string.Empty;

    [EmailAddress]
    public string Email
    { 
        get; 
        set; 
    } = string.Empty;

    [RequiredField]
    public string Expertise 
    { 
        get; 
        set; 
    } = string.Empty;

    [ValidEnum(typeof(MentorStatus))]
    [RequiredField]
    public MentorStatus? Status 
    { 
        get; 
        set; 
    }

    public DateTime CreatedDate 
    { 
        get; 
        set; 
    }

    public DateTime UpdatedDate 
    { 
        get; 
        set; 
    }

    public ICollection<TaskAssignment> TaskAssignments 
    { 
        get; 
        set; 
    } = new List<TaskAssignment>();

    public ICollection<Review> Reviews 
    { 
        get; 
        set; 
    } = new List<Review>();
}
public enum MentorStatus
{
    Active,
    
    Inactive
}