using TraineeManagementApi.Models.TimestampInterface;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TraineeManagementApi.TaskAssignments.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraineeManagementApi.LearningTasks.Models;

public class LearningTask : ITimestamp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id 
    { 
        get; 
        set; 
    }

    [Required]
    public string Title 
    { 
        get; 
        set; 
    } = string.Empty;

    [Required]
    public string Description 
    { 
        get; 
        set; 
    } = string.Empty;

    [Required]
    public string ExpectedTechStack 
    { 
        get; 
        set; 
    } = string.Empty;

    [Required]
    public DateOnly DueDate 
    { 
        get; 
        set; 
    }

    [EnumDataType(typeof(LearningTaskStatus))]
    [Required]
    public LearningTaskStatus? Status 
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
public enum LearningTaskStatus
{
    Draft,

    Published,

    Closed
}