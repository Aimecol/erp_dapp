using Microsoft.EntityFrameworkCore.Storage;
using INES.ERP.Core.Interfaces.Repositories;
using INES.ERP.Core.Models.Authentication;
using INES.ERP.Core.Models.StudentAccounts;
using INES.ERP.Core.Models.ProjectAccounts;

namespace INES.ERP.Data.Repositories;

/// <summary>
/// Unit of Work implementation
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ErpDbContext _context;
    private IDbContextTransaction? _transaction;
    private bool _disposed = false;

    // Repository instances
    private IRepository<User>? _users;
    private IRepository<Permission>? _permissions;
    private IRepository<UserPermission>? _userPermissions;
    private IRepository<Role>? _roles;
    private IRepository<RolePermission>? _rolePermissions;
    private IRepository<UserSession>? _userSessions;
    private IRepository<AuditLog>? _auditLogs;

    // Student Accounts repositories
    private IRepository<Student>? _students;
    private IRepository<StudentBilling>? _studentBillings;
    private IRepository<StudentPayment>? _studentPayments;
    private IRepository<StudentPenalty>? _studentPenalties;

    // Project Accounts repositories
    private IRepository<Project>? _projects;
    private IRepository<ProjectMilestone>? _projectMilestones;
    private IRepository<ProjectDisbursement>? _projectDisbursements;
    private IRepository<ProjectExpense>? _projectExpenses;
    private IRepository<ProjectReport>? _projectReports;

    public UnitOfWork(ErpDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IRepository<User> Users
    {
        get { return _users ??= new Repository<User>(_context); }
    }

    public IRepository<Permission> Permissions
    {
        get { return _permissions ??= new Repository<Permission>(_context); }
    }

    public IRepository<UserPermission> UserPermissions
    {
        get { return _userPermissions ??= new Repository<UserPermission>(_context); }
    }

    public IRepository<Role> Roles
    {
        get { return _roles ??= new Repository<Role>(_context); }
    }

    public IRepository<RolePermission> RolePermissions
    {
        get { return _rolePermissions ??= new Repository<RolePermission>(_context); }
    }

    public IRepository<UserSession> UserSessions
    {
        get { return _userSessions ??= new Repository<UserSession>(_context); }
    }

    public IRepository<AuditLog> AuditLogs
    {
        get { return _auditLogs ??= new Repository<AuditLog>(_context); }
    }

    #region Student Accounts
    public IRepository<Student> Students
    {
        get { return _students ??= new Repository<Student>(_context); }
    }

    public IRepository<StudentBilling> StudentBillings
    {
        get { return _studentBillings ??= new Repository<StudentBilling>(_context); }
    }

    public IRepository<StudentPayment> StudentPayments
    {
        get { return _studentPayments ??= new Repository<StudentPayment>(_context); }
    }

    public IRepository<StudentPenalty> StudentPenalties
    {
        get { return _studentPenalties ??= new Repository<StudentPenalty>(_context); }
    }
    #endregion

    #region Project Accounts
    public IRepository<Project> Projects
    {
        get { return _projects ??= new Repository<Project>(_context); }
    }

    public IRepository<ProjectMilestone> ProjectMilestones
    {
        get { return _projectMilestones ??= new Repository<ProjectMilestone>(_context); }
    }

    public IRepository<ProjectDisbursement> ProjectDisbursements
    {
        get { return _projectDisbursements ??= new Repository<ProjectDisbursement>(_context); }
    }

    public IRepository<ProjectExpense> ProjectExpenses
    {
        get { return _projectExpenses ??= new Repository<ProjectExpense>(_context); }
    }

    public IRepository<ProjectReport> ProjectReports
    {
        get { return _projectReports ??= new Repository<ProjectReport>(_context); }
    }
    #endregion

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction in progress.");
        }

        try
        {
            await _transaction.CommitAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction in progress.");
        }

        try
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
