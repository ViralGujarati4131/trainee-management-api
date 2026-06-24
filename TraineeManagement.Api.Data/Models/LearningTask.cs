using TraineeManagement.Api.Data.ModelTimestampInterface;
using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Data.TaskAssignmentModel;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.LearningTaskModel;

public class LearningTask : ICreateTimestamp,IUpdateTimestamp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id 
    { 
        get; 
        set; 
    }

    [RequiredField]
    public string Title 
    { 
        get; 
        set; 
    } = string.Empty;

    [RequiredField]
    public string Description 
    { 
        get; 
        set; 
    } = string.Empty;

    [RequiredField]
    public string ExpectedTechStack 
    { 
        get; 
        set; 
    } = string.Empty;

    [RequiredField]
    public DateOnly DueDate 
    { 
        get; 
        set; 
    }

    [ValidEnum(typeof(LearningTaskStatus))]
    [RequiredField]
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