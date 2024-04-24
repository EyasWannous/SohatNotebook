using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SohatNotebook.Entities.DbSet;

public class RefreshToken : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string JWTId { get; set; } = string.Empty;
    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime ExpireDate { get; set; }
    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; }
}