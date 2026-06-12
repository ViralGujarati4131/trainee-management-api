using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Models.TimestampInterface;

namespace TraineeManagementApi.Mentors.Models;
public class Mentor : ITimestamp
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

     [Required(ErrorMessage = "Expertise is required")]
    public string Expertise { get; set; } = string.Empty;

    [AllowedValues(MentorStatus.Active, MentorStatus.Inactive)]
    public MentorStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
public enum MentorStatus
{
    Active,
    Inactive
}