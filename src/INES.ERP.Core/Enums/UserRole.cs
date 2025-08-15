namespace INES.ERP.Core.Enums;

/// <summary>
/// Defines the different user roles in the ERP system
/// </summary>
public enum UserRole
{
    /// <summary>
    /// System administrator with full access
    /// </summary>
    Admin = 1,

    /// <summary>
    /// Financial officer responsible for accounting and finance
    /// </summary>
    Bursar = 2,

    /// <summary>
    /// Store manager responsible for inventory management
    /// </summary>
    StoreManager = 3,

    /// <summary>
    /// Auditor with read-only access for audit purposes
    /// </summary>
    Auditor = 4,

    /// <summary>
    /// Accountant with accounting module access
    /// </summary>
    Accountant = 5,

    /// <summary>
    /// HR manager with payroll and employee management access
    /// </summary>
    HRManager = 6,

    /// <summary>
    /// Project manager with project accounts access
    /// </summary>
    ProjectManager = 7,

    /// <summary>
    /// Student affairs officer with student accounts access
    /// </summary>
    StudentAffairs = 8,

    /// <summary>
    /// Procurement officer with procurement module access
    /// </summary>
    ProcurementOfficer = 9,

    /// <summary>
    /// Regular user with limited access
    /// </summary>
    User = 10
}
