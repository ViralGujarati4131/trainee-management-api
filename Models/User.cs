using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagementApi.Models.TimestampInterface;

namespace TraineeManagementApi.Users.Models;

public class User : ITimestamp
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id 
    { 
        get; 
        set; 
    }

    [Required]
    public string Username 
    { 
        get; 
        set; 
    } = string.Empty;

    [Required]
    public string PasswordHash 
    { 
        get; 
        set; 
    } = string.Empty;

    [EnumDataType(typeof(UserRole))]
    [Required]
    public UserRole? Role 
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
}

public enum UserRole
{
    Admin,

    Mentor,
    
    Trainee
}