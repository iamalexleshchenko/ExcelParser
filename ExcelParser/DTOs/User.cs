using System.ComponentModel.DataAnnotations;

namespace ExcelParser.DTOs;

public class User
{
    [Required, MaxLength(50)]
    public string Login { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }
}