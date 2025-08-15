using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Administration;

/// <summary>
/// Represents system configuration settings
/// </summary>
public class SystemConfiguration : AuditableEntity
{
    /// <summary>
    /// Configuration key
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string ConfigurationKey { get; set; } = string.Empty;

    /// <summary>
    /// Configuration value
    /// </summary>
    public string? ConfigurationValue { get; set; }

    /// <summary>
    /// Configuration category
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Configuration description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Data type (String, Integer, Boolean, Decimal, Date)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string DataType { get; set; } = "String";

    /// <summary>
    /// Default value
    /// </summary>
    [MaxLength(1000)]
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Indicates if configuration is system-defined
    /// </summary>
    public bool IsSystemConfiguration { get; set; } = false;

    /// <summary>
    /// Indicates if configuration is read-only
    /// </summary>
    public bool IsReadOnly { get; set; } = false;

    /// <summary>
    /// Indicates if configuration is encrypted
    /// </summary>
    public bool IsEncrypted { get; set; } = false;

    /// <summary>
    /// Display order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Validation rules (JSON)
    /// </summary>
    public string? ValidationRules { get; set; }

    /// <summary>
    /// Possible values (for dropdown configurations)
    /// </summary>
    public string? PossibleValues { get; set; }
}

/// <summary>
/// Represents a system backup record
/// </summary>
public class SystemBackup : AuditableEntity
{
    /// <summary>
    /// Backup name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string BackupName { get; set; } = string.Empty;

    /// <summary>
    /// Backup description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Backup type (Full, Incremental, Differential)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string BackupType { get; set; } = string.Empty;

    /// <summary>
    /// Backup start time
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Backup end time
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Backup status
    /// </summary>
    public Status BackupStatus { get; set; } = Status.InProgress;

    /// <summary>
    /// Backup file path
    /// </summary>
    [MaxLength(500)]
    public string? BackupFilePath { get; set; }

    /// <summary>
    /// Backup file size in bytes
    /// </summary>
    public long? FileSizeBytes { get; set; }

    /// <summary>
    /// Compression ratio
    /// </summary>
    public decimal? CompressionRatio { get; set; }

    /// <summary>
    /// Error message (if backup failed)
    /// </summary>
    [MaxLength(1000)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Backup initiated by
    /// </summary>
    [MaxLength(100)]
    public string? InitiatedBy { get; set; }

    /// <summary>
    /// Indicates if backup is scheduled
    /// </summary>
    public bool IsScheduled { get; set; } = false;

    /// <summary>
    /// Retention period in days
    /// </summary>
    public int RetentionDays { get; set; } = 30;

    /// <summary>
    /// Gets the backup duration
    /// </summary>
    public TimeSpan? BackupDuration
    {
        get
        {
            if (EndTime.HasValue)
                return EndTime.Value - StartTime;
            return null;
        }
    }

    /// <summary>
    /// Checks if backup is completed
    /// </summary>
    public bool IsCompleted => BackupStatus == Status.Completed && EndTime.HasValue;

    /// <summary>
    /// Checks if backup should be deleted (past retention period)
    /// </summary>
    public bool ShouldBeDeleted => CreatedAt.AddDays(RetentionDays) < DateTime.UtcNow;
}

/// <summary>
/// Represents a system notification
/// </summary>
public class SystemNotification : AuditableEntity
{
    /// <summary>
    /// Notification title
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Notification message
    /// </summary>
    [Required]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Notification type (Info, Warning, Error, Success)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string NotificationType { get; set; } = string.Empty;

    /// <summary>
    /// Notification priority (Low, Medium, High, Critical)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Priority { get; set; } = string.Empty;

    /// <summary>
    /// Target audience (All, Role, User)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string TargetAudience { get; set; } = string.Empty;

    /// <summary>
    /// Target value (role name, user ID, etc.)
    /// </summary>
    [MaxLength(100)]
    public string? TargetValue { get; set; }

    /// <summary>
    /// Notification start date
    /// </summary>
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Notification end date
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Indicates if notification is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Indicates if notification is dismissible
    /// </summary>
    public bool IsDismissible { get; set; } = true;

    /// <summary>
    /// Action URL (optional)
    /// </summary>
    [MaxLength(500)]
    public string? ActionUrl { get; set; }

    /// <summary>
    /// Action text (optional)
    /// </summary>
    [MaxLength(50)]
    public string? ActionText { get; set; }

    /// <summary>
    /// Icon name
    /// </summary>
    [MaxLength(50)]
    public string? Icon { get; set; }

    /// <summary>
    /// Checks if notification is currently active
    /// </summary>
    public bool IsCurrentlyActive
    {
        get
        {
            var now = DateTime.UtcNow;
            return IsActive && 
                   StartDate <= now && 
                   (!EndDate.HasValue || EndDate.Value >= now);
        }
    }
}

/// <summary>
/// Represents a system maintenance window
/// </summary>
public class MaintenanceWindow : AuditableEntity
{
    /// <summary>
    /// Maintenance title
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Maintenance description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Maintenance type (Scheduled, Emergency, Upgrade)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string MaintenanceType { get; set; } = string.Empty;

    /// <summary>
    /// Planned start time
    /// </summary>
    public DateTime PlannedStartTime { get; set; }

    /// <summary>
    /// Planned end time
    /// </summary>
    public DateTime PlannedEndTime { get; set; }

    /// <summary>
    /// Actual start time
    /// </summary>
    public DateTime? ActualStartTime { get; set; }

    /// <summary>
    /// Actual end time
    /// </summary>
    public DateTime? ActualEndTime { get; set; }

    /// <summary>
    /// Maintenance status
    /// </summary>
    public Status MaintenanceStatus { get; set; } = Status.Pending;

    /// <summary>
    /// Impact level (Low, Medium, High, Critical)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string ImpactLevel { get; set; } = string.Empty;

    /// <summary>
    /// Affected systems
    /// </summary>
    [MaxLength(500)]
    public string? AffectedSystems { get; set; }

    /// <summary>
    /// Maintenance notes
    /// </summary>
    public new string? Notes { get; set; }

    /// <summary>
    /// Performed by
    /// </summary>
    [MaxLength(100)]
    public string? PerformedBy { get; set; }

    /// <summary>
    /// Notification sent
    /// </summary>
    public bool NotificationSent { get; set; } = false;

    /// <summary>
    /// Gets the planned duration
    /// </summary>
    public TimeSpan PlannedDuration => PlannedEndTime - PlannedStartTime;

    /// <summary>
    /// Gets the actual duration
    /// </summary>
    public TimeSpan? ActualDuration
    {
        get
        {
            if (ActualStartTime.HasValue && ActualEndTime.HasValue)
                return ActualEndTime.Value - ActualStartTime.Value;
            return null;
        }
    }

    /// <summary>
    /// Checks if maintenance is currently active
    /// </summary>
    public bool IsCurrentlyActive
    {
        get
        {
            var now = DateTime.UtcNow;
            return MaintenanceStatus == Status.InProgress &&
                   ActualStartTime.HasValue &&
                   ActualStartTime.Value <= now &&
                   (!ActualEndTime.HasValue || ActualEndTime.Value >= now);
        }
    }
}

/// <summary>
/// Represents system performance metrics
/// </summary>
public class SystemMetrics : BaseEntity
{
    /// <summary>
    /// Metric timestamp
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// CPU usage percentage
    /// </summary>
    public decimal CpuUsagePercentage { get; set; }

    /// <summary>
    /// Memory usage percentage
    /// </summary>
    public decimal MemoryUsagePercentage { get; set; }

    /// <summary>
    /// Disk usage percentage
    /// </summary>
    public decimal DiskUsagePercentage { get; set; }

    /// <summary>
    /// Active user sessions
    /// </summary>
    public int ActiveUserSessions { get; set; }

    /// <summary>
    /// Database connections
    /// </summary>
    public int DatabaseConnections { get; set; }

    /// <summary>
    /// Average response time in milliseconds
    /// </summary>
    public decimal AverageResponseTimeMs { get; set; }

    /// <summary>
    /// Error count in the last hour
    /// </summary>
    public int ErrorCount { get; set; }

    /// <summary>
    /// Warning count in the last hour
    /// </summary>
    public int WarningCount { get; set; }

    /// <summary>
    /// System uptime in hours
    /// </summary>
    public decimal SystemUptimeHours { get; set; }
}

/// <summary>
/// Represents a system log entry
/// </summary>
public class SystemLog : BaseEntity
{
    /// <summary>
    /// Log timestamp
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Log level (Debug, Info, Warning, Error, Fatal)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string LogLevel { get; set; } = string.Empty;

    /// <summary>
    /// Logger name/category
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Logger { get; set; } = string.Empty;

    /// <summary>
    /// Log message
    /// </summary>
    [Required]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Exception details (if any)
    /// </summary>
    public string? Exception { get; set; }

    /// <summary>
    /// User ID (if applicable)
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Session ID (if applicable)
    /// </summary>
    [MaxLength(100)]
    public string? SessionId { get; set; }

    /// <summary>
    /// IP address
    /// </summary>
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Request URL (if applicable)
    /// </summary>
    [MaxLength(500)]
    public string? RequestUrl { get; set; }

    /// <summary>
    /// Additional properties (JSON)
    /// </summary>
    public string? Properties { get; set; }
}
