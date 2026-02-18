namespace MeetVerse.Api.Models;

public class NoiseProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public string ProfileDataJson { get; set; } = "{}"; // frequency bands, signatures, etc.

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}


