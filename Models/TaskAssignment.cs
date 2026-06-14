using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Trainees.Models;
using TraineeManagementApi.Mentors.Models;
using TraineeManagementApi.LearningTasks.Models;
using TraineeManagementApi.Models.TimestampInterface;
using TraineeManagementApi.Submissions.Models;

namespace TraineeManagementApi.TaskAssignments.Models;

public class TaskAssignment : TaskAssignmentTimeStamp
{
    [Key]
    public int Id { get; set; }

    public int TraineeId { get; set; } 
    public Trainee? Trainee { get; set; }

    public int MentorId { get; set; } 
    public Mentor? Mentor { get; set; }

    public int LearningTaskId { get; set; } 
    public LearningTask? LearningTask { get; set; }

    public DateOnly AssignedDate { get; set; }

    [Required(ErrorMessage="DueDate required")]
    public DateOnly DueDate { get; set; }

    [EnumDataType(typeof(TaskAssignmentStatus), ErrorMessage = "Invalid Status")]
    public TaskAssignmentStatus Status { get; set; }

    public string Remarks { get; set; } = string.Empty;

    public List<Submission> Submissions { get; set; } = new List<Submission>();
}

public enum TaskAssignmentStatus
{
    Assigned,
    Inprogress,
    Submitted,
    Reviewed,
    Completed
}