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
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "ExpectedTechStack is required")]
    public string ExpectedTechStack { get; set; } = string.Empty;

    [Required(ErrorMessage = "DueDate is required")]
    public DateOnly DueDate { get; set; }

    [EnumDataType(typeof(LearningTaskStatus), ErrorMessage = "Invalid Status")]
    [Required(ErrorMessage = "Status is required")]
    public LearningTaskStatus? Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
}
public enum LearningTaskStatus
{
    Draft,
    Published,
    Closed
}