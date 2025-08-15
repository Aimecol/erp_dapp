using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Authentication;

/// <summary>
/// Represents a system permission
/// </summary>
public class Permission : BaseEntity
{
    /// <summary>
    /// Permission name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Permission description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Permission category or module
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Permission action (Create, Read, Update, Delete, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Resource or entity the permission applies to
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Resource { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if this is a system permission (cannot be deleted)
    /// </summary>
    public bool IsSystemPermission { get; set; } = false;

    /// <summary>
    /// Navigation property for user permissions
    /// </summary>
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    /// <summary>
    /// Navigation property for role permissions
    /// </summary>
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

/// <summary>
/// Represents the relationship between users and permissions
/// </summary>
public class UserPermission : BaseEntity
{
    /// <summary>
    /// User ID
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Permission ID
    /// </summary>
    public Guid PermissionId { get; set; }

    /// <summary>
    /// Indicates if the permission is granted or denied
    /// </summary>
    public bool IsGranted { get; set; } = true;

    /// <summary>
    /// Permission expiry date (optional)
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Navigation property for user
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Navigation property for permission
    /// </summary>
    public virtual Permission Permission { get; set; } = null!;
}

/// <summary>
/// Represents a role in the system
/// </summary>
public class Role : BaseEntity
{
    /// <summary>
    /// Role name
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Role description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Indicates if this is a system role (cannot be deleted)
    /// </summary>
    public bool IsSystemRole { get; set; } = false;

    /// <summary>
    /// Navigation property for role permissions
    /// </summary>
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

/// <summary>
/// Represents the relationship between roles and permissions
/// </summary>
public class RolePermission : BaseEntity
{
    /// <summary>
    /// Role ID
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Permission ID
    /// </summary>
    public Guid PermissionId { get; set; }

    /// <summary>
    /// Navigation property for role
    /// </summary>
    public virtual Role Role { get; set; } = null!;

    /// <summary>
    /// Navigation property for permission
    /// </summary>
    public virtual Permission Permission { get; set; } = null!;
}
