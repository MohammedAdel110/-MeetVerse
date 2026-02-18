using Microsoft.EntityFrameworkCore;
using MeetVerse.Api.Models;

namespace MeetVerse.Api.Data;

public class MeetVerseDbContext : DbContext
{
    public MeetVerseDbContext(DbContextOptions<MeetVerseDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<MeetingParticipant> MeetingParticipants => Set<MeetingParticipant>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<WhiteboardSession> WhiteboardSessions => Set<WhiteboardSession>();
    public DbSet<WhiteboardEvent> WhiteboardEvents => Set<WhiteboardEvent>();
    public DbSet<Recording> Recordings => Set<Recording>();
    public DbSet<Transcript> Transcripts => Set<Transcript>();
    public DbSet<TranscriptLine> TranscriptLines => Set<TranscriptLine>();
    public DbSet<MeetingSummary> MeetingSummaries => Set<MeetingSummary>();
    public DbSet<NoiseProfile> NoiseProfiles => Set<NoiseProfile>();
    public DbSet<NoiseEvent> NoiseEvents => Set<NoiseEvent>();
    public DbSet<NoiseReport> NoiseReports => Set<NoiseReport>();
    public DbSet<EngagementMetric> EngagementMetrics => Set<EngagementMetric>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<GroupMessage> GroupMessages => Set<GroupMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.NoiseProfile)
            .WithOne(np => np.User)
            .HasForeignKey<NoiseProfile>(np => np.UserId);

        modelBuilder.Entity<Meeting>()
            .HasOne(m => m.Host)
            .WithMany(u => u.HostedMeetings)
            .HasForeignKey(m => m.HostId);

        modelBuilder.Entity<MeetingParticipant>()
            .HasOne(mp => mp.Meeting)
            .WithMany(m => m.Participants)
            .HasForeignKey(mp => mp.MeetingId);

        modelBuilder.Entity<MeetingParticipant>()
            .HasOne(mp => mp.User)
            .WithMany(u => u.MeetingParticipants)
            .HasForeignKey(mp => mp.UserId);

        modelBuilder.Entity<Meeting>()
            .HasOne(m => m.Transcript)
            .WithOne(t => t.Meeting)
            .HasForeignKey<Transcript>(t => t.MeetingId);

        modelBuilder.Entity<Meeting>()
            .HasOne(m => m.MeetingSummary)
            .WithOne(ms => ms.Meeting)
            .HasForeignKey<MeetingSummary>(ms => ms.MeetingId);

        modelBuilder.Entity<Recording>()
            .HasOne(r => r.Meeting)
            .WithMany(m => m.Recordings)
            .HasForeignKey(r => r.MeetingId);

        modelBuilder.Entity<Transcript>()
            .HasOne(t => t.Recording)
            .WithOne(r => r.Transcript!)
            .HasForeignKey<Transcript>(t => t.RecordingId);

        modelBuilder.Entity<Group>()
            .HasOne(g => g.CreatedBy)
            .WithMany(u => u.CreatedGroups)
            .HasForeignKey(g => g.CreatedById);

        modelBuilder.Entity<UserGroup>()
            .HasKey(ug => new { ug.UserId, ug.GroupId });
        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.User)
            .WithMany(u => u.UserGroups)
            .HasForeignKey(ug => ug.UserId);
        modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.Group)
            .WithMany(g => g.UserGroups)
            .HasForeignKey(ug => ug.GroupId);

        modelBuilder.Entity<GroupMessage>()
            .HasOne(gm => gm.Group)
            .WithMany(g => g.Messages)
            .HasForeignKey(gm => gm.GroupId);
        modelBuilder.Entity<GroupMessage>()
            .HasOne(gm => gm.Sender)
            .WithMany(u => u.GroupMessagesSent)
            .HasForeignKey(gm => gm.SenderId);

        modelBuilder.Entity<Meeting>()
            .HasOne(m => m.Group)
            .WithMany(g => g.Meetings)
            .HasForeignKey(m => m.GroupId)
            .IsRequired(false);
    }
}


