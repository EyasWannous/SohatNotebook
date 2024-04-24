namespace SohatNotebook.Authentication.Configuration;

public class JWTConfig
{
    public string SigningKey { get; set; } = string.Empty;
    public TimeSpan ExpireTime { get; set; }
}