using MeetVerse.Api.Models;

namespace MeetVerse.Api.Data;

public interface IDatabaseSeeder
{
    Task SeedAsync();
}

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly MeetVerseDbContext _db;

    public DatabaseSeeder(MeetVerseDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync()
    {
        if (_db.Users.Any())
        {
            return;
        }

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@meetverse.test",
            PasswordHash = "stub",
            Name = "Admin User",
            Roles = "Admin"
        };

        var participant = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@meetverse.test",
            PasswordHash = "stub",
            Name = "Demo User",
            Roles = "Participant"
        };

        _db.Users.AddRange(admin, participant);

        var demoGroup = new Group
        {
            Id = Guid.NewGuid(),
            Name = "Demo Community",
            Description = "Sample group for testing",
            CreatedById = admin.Id,
            CreatedAt = DateTime.UtcNow
        };
        _db.Groups.Add(demoGroup);
        _db.UserGroups.Add(new UserGroup { UserId = admin.Id, GroupId = demoGroup.Id, Role = "owner", JoinedAt = DateTime.UtcNow });
        _db.UserGroups.Add(new UserGroup { UserId = participant.Id, GroupId = demoGroup.Id, Role = "member", JoinedAt = DateTime.UtcNow });

        var meeting = new Meeting
        {
            Id = Guid.NewGuid(),
            HostId = admin.Id,
            GroupId = demoGroup.Id,
            Title = "Demo Meeting",
            Description = "Seeded demo meeting",
            ScheduledStart = DateTime.UtcNow.AddMinutes(-5),
            ScheduledEnd = DateTime.UtcNow.AddMinutes(25),
            Status = "live"
        };

        _db.Meetings.Add(meeting);
        Console.WriteLine("============================================");
        Console.WriteLine($"TEST MEETING ID: {meeting.Id}");
        Console.WriteLine("============================================");
        var participantLink = new MeetingParticipant
        {
            Id = Guid.NewGuid(),
            MeetingId = meeting.Id,
            UserId = participant.Id,
            Role = "participant",
            JoinedAt = DateTime.UtcNow.AddMinutes(-2),
            IsActive = true
        };

        _db.MeetingParticipants.Add(participantLink);

        var noiseProfile = new NoiseProfile
        {
            Id = Guid.NewGuid(),
            UserId = participant.Id,
            ProfileDataJson = "{\"example\":\"seed\"}"
        };

        _db.NoiseProfiles.Add(noiseProfile);

        var noiseReport = new NoiseReport
        {
            Id = Guid.NewGuid(),
            MeetingId = meeting.Id,
            UserId = participant.Id,
            AvgClarityScore = 0.85,
            AvgNoiseLevel = 0.2,
            PeakNoiseLevel = 0.7,
            NoiseEventsCount = 3,
            SpeakingTimeSeconds = 300
        };

        _db.NoiseReports.Add(noiseReport);

        await _db.SaveChangesAsync();
    }
}


