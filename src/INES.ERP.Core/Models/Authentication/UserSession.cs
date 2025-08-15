using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Authentication;

/// <summary>
/// Represents a user session
/// </summary>
public class UserSession : BaseEntity
{
    /// <summary>
    /// User ID
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Session token
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string SessionToken { get; set; } = string.Empty;

    /// <summary>
    /// Refresh token
    /// </summary>
    [MaxLength(255)]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Session start time
    /// </summary>
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Session expiry time
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Session end time (when user logs out)
    /// </summary>
    public DateTime? EndedAt { get; set; }

    /// <summary>
    /// IP address from which the session was created
    /// </summary>
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent string
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Device information
    /// </summary>
    [MaxLength(200)]
    public string? DeviceInfo { get; set; }

    /// <summary>
    /// Location information
    /// </summary>
    [MaxLength(200)]
    public string? Location { get; set; }

    /// <summary>
    /// Indicates if the session is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Last activity timestamp
    /// </summary>
    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property for user
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Checks if the session is expired
    /// </summary>
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    /// <summary>
    /// Checks if the session is valid (active and not expired)
    /// </summary>
    public bool IsValid => IsActive && !IsExpired && !EndedAt.HasValue;
}

/// <summary>
/// Represents an audit log entry
/// </summary>
public class AuditLog : BaseEntity
{
    /// <summary>
    /// User ID who performed the action
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Action performed
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Entity or resource affected
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// ID of the entity affected
    /// </summary>
    [MaxLength(50)]
    public string? EntityId { get; set; }

    /// <summary>
    /// Old values (JSON)
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// New values (JSON)
    /// </summary>
    public string? NewValues { get; set; }

    /// <summary>
    /// IP address from which the action was performed
    /// </summary>
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent string
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Additional details about the action
    /// </summary>
    [MaxLength(1000)]
    public string? Details { get; set; }

    /// <summary>
    /// Timestamp when the action was performed
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property for user
    /// </summary>
    public virtual User? User { get; set; }
}
