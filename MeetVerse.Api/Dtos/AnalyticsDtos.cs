namespace MeetVerse.Api.Dtos;

public class NoiseMetricBatchRequest
{
    public List<NoiseMetricItem> Metrics { get; set; } = new();
}

public class NoiseMetricItem
{
    public DateTime Timestamp { get; set; }
    public double ClarityScore { get; set; }
    public double NoiseLevel { get; set; }
    public bool IsLoudNoiseSuppressed { get; set; }
    public string? FrequencyBand { get; set; }
    public string? RawMetricsJson { get; set; }
}


