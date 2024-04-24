using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SohatNotebook.Entities.DTOs.Incoming;

public class UserDTO
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string Country { get; set; }
}