namespace MeetVerse.Api.Models;

public class NoiseReport
{
    public Guid Id { get; set; }

    public Guid MeetingId { get; set; }
    public Meeting Meeting { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public double AvgClarityScore { get; set; }
    public double AvgNoiseLevel { get; set; }
    public double PeakNoiseLevel { get; set; }

    public int NoiseEventsCount { get; set; }
    public double SpeakingTimeSeconds { get; set; }

    public string? SegmentsJson { get; set; } // optional per-segment metrics

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}


