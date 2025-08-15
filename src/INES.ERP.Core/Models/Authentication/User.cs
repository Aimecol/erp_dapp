using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Authentication;

/// <summary>
/// Represents a system user
/// </summary>
public class User : AuditableEntity
{
    /// <summary>
    /// Unique username for login
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Salt used for password hashing
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string PasswordSalt { get; set; } = string.Empty;

    /// <summary>
    /// User's first name
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's middle name (optional)
    /// </summary>
    [MaxLength(50)]
    public string? MiddleName { get; set; }

    /// <summary>
    /// User's phone number
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// User's profile picture URL
    /// </summary>
    [MaxLength(500)]
    public string? ProfilePictureUrl { get; set; }

    /// <summary>
    /// User's primary role
    /// </summary>
    public UserRole Role { get; set; } = UserRole.User;

    /// <summary>
    /// Indicates if the user account is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Indicates if the user's email is verified
    /// </summary>
    public bool IsEmailVerified { get; set; } = false;

    /// <summary>
    /// Indicates if two-factor authentication is enabled
    /// </summary>
    public bool IsTwoFactorEnabled { get; set; } = false;

    /// <summary>
    /// Two-factor authentication secret key
    /// </summary>
    [MaxLength(255)]
    public string? TwoFactorSecret { get; set; }

    /// <summary>
    /// Date and time of last login
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Number of failed login attempts
    /// </summary>
    public int FailedLoginAttempts { get; set; } = 0;

    /// <summary>
    /// Date and time when the account was locked
    /// </summary>
    public DateTime? LockedUntil { get; set; }

    /// <summary>
    /// Password reset token
    /// </summary>
    [MaxLength(255)]
    public string? PasswordResetToken { get; set; }

    /// <summary>
    /// Password reset token expiry
    /// </summary>
    public DateTime? PasswordResetTokenExpiry { get; set; }

    /// <summary>
    /// Email verification token
    /// </summary>
    [MaxLength(255)]
    public string? EmailVerificationToken { get; set; }

    /// <summary>
    /// Navigation property for user permissions
    /// </summary>
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    /// <summary>
    /// Navigation property for user sessions
    /// </summary>
    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();



    /// <summary>
    /// Gets the user's full name
    /// </summary>
    public string FullName
    {
        get
        {
            var parts = new List<string> { FirstName };
            
            if (!string.IsNullOrWhiteSpace(MiddleName))
                parts.Add(MiddleName);
            
            parts.Add(LastName);
            
            return string.Join(" ", parts);
        }
    }

    /// <summary>
    /// Gets the user's display name
    /// </summary>
    public string DisplayName => !string.IsNullOrWhiteSpace(FullName) ? FullName : Username;

    /// <summary>
    /// Checks if the account is locked
    /// </summary>
    public bool IsLocked => LockedUntil.HasValue && LockedUntil.Value > DateTime.UtcNow;
}
