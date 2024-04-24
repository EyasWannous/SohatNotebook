using System.ComponentModel.DataAnnotations;

namespace SohatNoteBook.Authentication.Models.DTOs.Incoming;

public class TokenRequestDTO
{
    [Required]
    public string Token { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}