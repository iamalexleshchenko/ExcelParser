using System.ComponentModel.DataAnnotations;

namespace ExcelParser.Entities;

public class UserEntity : WithId
{
    [Required, MaxLength(50)]
    public string Login { get; set; }
    
    [Required, MaxLength(255), EmailAddress]
    public string Email { get; set; }
    
    [Required, MaxLength(255)]
    public string PasswordHash { get; set; }
}