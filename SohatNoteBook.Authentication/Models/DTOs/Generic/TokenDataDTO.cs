using System.ComponentModel.DataAnnotations;

namespace SohatNoteBook.Authentication.Models.DTOs.Generic;

public class TokenDataDTO
{
    [Required]
    public string JWTToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}