using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Data.ModelTimestampInterface;
using TraineeManagement.Api.Data.TaskAssignmentModel;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.TraineeModel;

public class Trainee : ICreateTimestamp,IUpdateTimestamp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id 
    { 
        get; set; 
    }

    [RequiredField]
    [FieldMaxLength(50)]
    public string FirstName 
    { 
        get; 
        set; 
    } = string.Empty;

    [RequiredField]
    [FieldMaxLength(50)]
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
    public string TechStack 
    { 
        get; 
        set; 
    } = string.Empty;

    [ValidEnum(typeof(TraineeStatus))]
    [RequiredField]
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