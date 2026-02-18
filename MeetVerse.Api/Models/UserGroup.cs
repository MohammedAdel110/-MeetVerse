namespace MeetVerse.Api.Models;

public class UserGroup
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid GroupId { get; set; }
    public Group Group { get; set; } = default!;

    /// <summary>owner, admin, or member</summary>
    public string Role { get; set; } = "member";
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
