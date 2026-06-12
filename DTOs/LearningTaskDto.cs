using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.LearningTasks.Models;

namespace TraineeManagementApi.LearningTasks.DTOs;
public class LearningTaskCreateDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title {get; set;} = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    public string Description {get; set;} = string.Empty;

    [Required(ErrorMessage = "ExpectedTechStack is required")]
    public string ExpectedTechStack {get; set;} = string.Empty;

    [Required(ErrorMessage = "DueDate is required")]
    public DateTime DueDate {get; set;}

    [Required(ErrorMessage = "Status is Required")]
    public LearningTaskStatus Status {get; set;}
}

public class LearningTaskUpdateDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title {get; set;} = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    public string Description {get; set;} = string.Empty;

    [Required(ErrorMessage = "ExpectedTechStack is required")]
    public string ExpectedTechStack {get; set;} = string.Empty;

    [Required(ErrorMessage = "DueDate is required")]
    public DateTime DueDate {get; set;}

    [Required(ErrorMessage = "Status is Required")]
    public LearningTaskStatus Status {get; set;}
}

public class LearningTaskResposeDto
{
    [Key]
    public int Id {get; set;}

    [Required(ErrorMessage = "Title is required")]
    public string Title {get; set;} = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    public string Description {get; set;} = string.Empty;

    [Required(ErrorMessage = "ExpectedTechStack is required")]
    public string ExpectedTechStack {get; set;} = string.Empty;

    [Required(ErrorMessage = "DueDate is required")]
    public DateTime DueDate {get; set;}

    [Required(ErrorMessage = "Status is Required")]
    public LearningTaskStatus Status {get; set;}
}
