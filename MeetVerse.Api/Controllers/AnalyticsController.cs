using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetVerse.Api.Data;
using MeetVerse.Api.Dtos;
using MeetVerse.Api.Models;

namespace MeetVerse.Api.Controllers;

[ApiController]
[Route("api/meetings/{meetingId:guid}/analytics")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly MeetVerseDbContext _db;

    public AnalyticsController(MeetVerseDbContext db)
    {
        _db = db;
    }

    [HttpPost("noise-metrics")]
    public async Task<IActionResult> PostNoiseMetrics(Guid meetingId, [FromBody] NoiseMetricBatchRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (!await _db.Meetings.AnyAsync(m => m.Id == meetingId))
        {
            return NotFound("Meeting not found");
        }

        double totalClarity = 0;
        double totalNoise = 0;

        foreach (var metric in request.Metrics)
        {
            totalClarity += metric.ClarityScore;
            totalNoise += metric.NoiseLevel;

            if (metric.IsLoudNoiseSuppressed)
            {
                var ev = new NoiseEvent
                {
                    Id = Guid.NewGuid(),
                    MeetingId = meetingId,
                    UserId = userId,
                    Timestamp = metric.Timestamp,
                    EventType = "loud_noise_suppressed",
                    Intensity = metric.NoiseLevel,
                    FrequencyBand = metric.FrequencyBand,
                    RawMetricsJson = metric.RawMetricsJson
                };
                _db.NoiseEvents.Add(ev);
            }
        }

        int count = request.Metrics.Count;
        if (count > 0)
        {
            var report = await _db.NoiseReports
                .FirstOrDefaultAsync(r => r.MeetingId == meetingId && r.UserId == userId);

            double avgClarity = totalClarity / count;
            double avgNoise = totalNoise / count;

            if (report is null)
            {
                report = new NoiseReport
                {
                    Id = Guid.NewGuid(),
                    MeetingId = meetingId,
                    UserId = userId,
                    AvgClarityScore = avgClarity,
                    AvgNoiseLevel = avgNoise,
                    PeakNoiseLevel = request.Metrics.Max(m => m.NoiseLevel),
                    NoiseEventsCount = request.Metrics.Count(m => m.IsLoudNoiseSuppressed),
                    SpeakingTimeSeconds = 0
                };
                _db.NoiseReports.Add(report);
            }
            else
            {
                report.AvgClarityScore = (report.AvgClarityScore + avgClarity) / 2.0;
                report.AvgNoiseLevel = (report.AvgNoiseLevel + avgNoise) / 2.0;
                report.PeakNoiseLevel = Math.Max(report.PeakNoiseLevel, request.Metrics.Max(m => m.NoiseLevel));
                report.NoiseEventsCount += request.Metrics.Count(m => m.IsLoudNoiseSuppressed);
            }
        }

        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("noise-reports")]
    [Authorize(Roles = "Admin,Host")]
    public async Task<ActionResult<IEnumerable<NoiseReport>>> GetNoiseReports(Guid meetingId)
    {
        var reports = await _db.NoiseReports
            .Where(r => r.MeetingId == meetingId)
            .ToListAsync();
        return reports;
    }
}


