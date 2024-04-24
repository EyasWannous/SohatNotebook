namespace SohatNoteBook.Authentication.Models.DTOs.Outcoming;

public class AuthResult
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool Seccess { get; set; }
    public List<string> Errors { get; set; } = [];
}