using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Data.TraineeModel;
using TraineeManagement.Api.Data.MentorModel;
using TraineeManagement.Api.Data.LearningTaskModel;
using TraineeManagement.Api.Data.SubmissionModel;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.TaskAssignmentModel;

public class TaskAssignment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id 
    { 
        get; 
        set; 
    }

    public int TraineeId 
    { 
        get; 
        set; 
    }
    public Trainee? Trainee 
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

    public int LearningTaskId 
    { 
        get; 
        set; 
    }
    public LearningTask? LearningTask 
    { 
        get; 
        set; 
    }

    [RequiredField]
    public DateOnly AssignedDate 
    { 
        get; 
        set; 
    }

    [RequiredField]
    [ValidDateRange("AssignedDate")]
    public DateOnly DueDate 
    { 
        get; 
        set; 
    }

   [ValidEnum(typeof(TaskAssignmentStatus))]
   [RequiredField]
    public TaskAssignmentStatus? Status 
    { 
        get; 
        set; 
    }

    public string Remarks 
    { 
        get; 
        set; 
    } = string.Empty;

    public ICollection<Submission> Submissions 
    { 
        get; 
        set; 
    } = new List<Submission>();
}

public enum TaskAssignmentStatus
{
    Assigned,

    Inprogress,

    Submitted,

    Reviewed,
    
    Completed
}