namespace SohatNotebook.Entities.DTOs.Incoming.Profile;

public class UpdateProfileDTO
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Phone { get; set; }
    public required string Country { get; set; }
    public required string Address { get; set; }
    public required string MobileNumber { get; set; }
    public required string Sex { get; set; }
}