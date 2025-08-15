using INES.ERP.Core.Models.Authentication;
using INES.ERP.Core.Models.StudentAccounts;
using INES.ERP.Core.Models.ProjectAccounts;
using INES.ERP.Core.Models.Inventory;
using INES.ERP.Core.Models.Accounting;
using INES.ERP.Core.Models.Payroll;
using INES.ERP.Core.Models.Reports;
using INES.ERP.Core.Models.Administration;
using INES.ERP.Core.Models.Dashboard;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Interfaces.Repositories;

/// <summary>
/// Unit of Work pattern interface for managing transactions
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// User repository
    /// </summary>
    IRepository<User> Users { get; }

    /// <summary>
    /// Permission repository
    /// </summary>
    IRepository<Permission> Permissions { get; }

    /// <summary>
    /// User permission repository
    /// </summary>
    IRepository<UserPermission> UserPermissions { get; }

    /// <summary>
    /// Role repository
    /// </summary>
    IRepository<Role> Roles { get; }

    /// <summary>
    /// Role permission repository
    /// </summary>
    IRepository<RolePermission> RolePermissions { get; }

    /// <summary>
    /// User session repository
    /// </summary>
    IRepository<UserSession> UserSessions { get; }

    /// <summary>
    /// Audit log repository
    /// </summary>
    IRepository<AuditLog> AuditLogs { get; }

    #region Student Accounts
    /// <summary>
    /// Student repository
    /// </summary>
    IRepository<Student> Students { get; }

    /// <summary>
    /// Student billing repository
    /// </summary>
    IRepository<StudentBilling> StudentBillings { get; }

    /// <summary>
    /// Student payment repository
    /// </summary>
    IRepository<StudentPayment> StudentPayments { get; }

    /// <summary>
    /// Student penalty repository
    /// </summary>
    IRepository<StudentPenalty> StudentPenalties { get; }
    #endregion

    #region Project Accounts
    /// <summary>
    /// Project repository
    /// </summary>
    IRepository<Project> Projects { get; }

    /// <summary>
    /// Project milestone repository
    /// </summary>
    IRepository<ProjectMilestone> ProjectMilestones { get; }

    /// <summary>
    /// Project disbursement repository
    /// </summary>
    IRepository<ProjectDisbursement> ProjectDisbursements { get; }

    /// <summary>
    /// Project expense repository
    /// </summary>
    IRepository<ProjectExpense> ProjectExpenses { get; }

    /// <summary>
    /// Project report repository
    /// </summary>
    IRepository<ProjectReport> ProjectReports { get; }
    #endregion

    /// <summary>
    /// Saves all changes to the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected records</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a database transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Transaction object</returns>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
