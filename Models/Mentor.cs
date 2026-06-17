using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Models.TimestampInterface;
using TraineeManagementApi.TaskAssignments.Models;
using TraineeManagementApi.Reviews.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraineeManagementApi.Mentors.Models;

public class Mentor : ITimestamp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id 
    { 
        get; 
        set; 
    }

    [Required]
    public string FirstName 
    { 
        get; 
        set; 
    } = string.Empty;

    [Required]
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

    [Required]
    public string Expertise 
    { 
        get; 
        set; 
    } = string.Empty;

    [EnumDataType(typeof(MentorStatus))]
    [Required]
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