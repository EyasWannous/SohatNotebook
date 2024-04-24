namespace SohatNotebook.Entities.DbSet;

public class User : BaseEntity
{
    public Guid IdentityId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Country { get; set; }
    public string Address { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;
}