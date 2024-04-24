namespace SohatNotebook.Entities.DTOs.Outcoming.Profile;

public class ProfileDTO
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Country { get; set; }
}