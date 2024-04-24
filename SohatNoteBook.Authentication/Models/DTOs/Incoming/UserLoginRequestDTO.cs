using System.ComponentModel.DataAnnotations;

namespace SohatNoteBook.Authentication.Models.DTOs.Incoming;

public class UserLoginRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}