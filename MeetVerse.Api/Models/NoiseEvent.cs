namespace MeetVerse.Api.Models;

public class NoiseEvent
{
    public Guid Id { get; set; }

    public Guid MeetingId { get; set; }
    public Meeting Meeting { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string EventType { get; set; } = default!; // loud_noise_suppressed/steady_noise_detected
    public double Intensity { get; set; }
    public string? FrequencyBand { get; set; }

    public string? RawMetricsJson { get; set; }
}


