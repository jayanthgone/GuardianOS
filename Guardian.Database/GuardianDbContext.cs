using Guardian.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Guardian.Database;

/// <summary>
/// Entity Framework Core database context for GuardianOS.
/// </summary>
public class GuardianDbContext : DbContext
{
    /// <summary>
    /// Initialize database context with options.
    /// </summary>
    public GuardianDbContext(DbContextOptions<GuardianDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Users DbSet.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Devices DbSet.
    /// </summary>
    public DbSet<Device> Devices { get; set; }

    /// <summary>
    /// Blocked Applications DbSet.
    /// </summary>
    public DbSet<BlockedApplication> BlockedApplications { get; set; }

    /// <summary>
    /// Audit Events DbSet.
    /// </summary>
    public DbSet<AuditEvent> AuditEvents { get; set; }

    /// <summary>
    /// Remote Commands DbSet.
    /// </summary>
    public DbSet<RemoteCommand> RemoteCommands { get; set; }

    /// <summary>
    /// Sessions DbSet.
    /// </summary>
    public DbSet<Session> Sessions { get; set; }

    /// <summary>
    /// Configure model relationships and constraints.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
        });

        // Device configuration
        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DeviceId).IsRequired().HasMaxLength(256);
            entity.HasIndex(e => e.DeviceId).IsUnique();
            entity.Property(e => e.ComputerName).IsRequired().HasMaxLength(256);
            entity.Property(e => e.OSVersion).IsRequired().HasMaxLength(256);

            // Foreign key relationship
            entity.HasOne(e => e.User)
                .WithMany(u => u.Devices)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Blocked Application configuration
        modelBuilder.Entity<BlockedApplication>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ApplicationName).IsRequired().HasMaxLength(512);

            entity.HasOne(e => e.Device)
                .WithMany(d => d.BlockedApplications)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Audit Event configuration
        modelBuilder.Entity<AuditEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EventType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired();
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => e.EventType);

            entity.HasOne(e => e.Device)
                .WithMany(d => d.AuditEvents)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Remote Command configuration
        modelBuilder.Entity<RemoteCommand>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CommandType).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);

            entity.HasOne(e => e.Device)
                .WithMany(d => d.RemoteCommands)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Session configuration
        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RefreshTokenHash).IsRequired();
            entity.HasIndex(e => e.ExpiresAt);
            entity.HasIndex(e => e.IsActive);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Sessions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
