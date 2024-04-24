using SohatNotebook.Entities.DTOs.Errors;

namespace SohatNotebook.Entities.DTOs.Generic;

public class Result<T>
{
    public T Content { get; set; }
    public Error? Error { get; set; }
    public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
    public bool IsSuccess => Error is null;
}