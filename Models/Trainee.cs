using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagementApi.Models.TimestampInterface;
using TraineeManagementApi.TaskAssignments.Models;

namespace TraineeManagementApi.Trainees.Models;

public class Trainee : ITimestamp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id 
    { 
        get; set; 
    }

    [Required]
    [MaxLength(50)]
    public string FirstName 
    { 
        get; 
        set; 
    } = string.Empty;

    [Required]
    [MaxLength(50)]
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
    public string TechStack 
    { 
        get; 
        set; 
    } = string.Empty;

    [EnumDataType(typeof(TraineeStatus))]
    [Required]
    public TraineeStatus? Status 
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
}

public enum TraineeStatus
{
    Active,

    Inactive,
    
    Completed
}