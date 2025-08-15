using Microsoft.EntityFrameworkCore;
using INES.ERP.Core.Models.Authentication;
using INES.ERP.Core.Models.Common;
using System.Linq.Expressions;

namespace INES.ERP.Data;

/// <summary>
/// Entity Framework DbContext for the ERP system
/// </summary>
public class ErpDbContext : DbContext
{
    public ErpDbContext(DbContextOptions<ErpDbContext> options) : base(options)
    {
    }

    #region Authentication DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    #endregion

    #region Common DbSets
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ErpDbContext).Assembly);

        // Configure global query filters for soft delete
        ConfigureGlobalQueryFilters(modelBuilder);

        // Configure indexes
        ConfigureIndexes(modelBuilder);

        // Seed data
        SeedData(modelBuilder);
    }

    private void ConfigureGlobalQueryFilters(ModelBuilder modelBuilder)
    {
        // Apply soft delete filter to all entities that inherit from BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                var filter = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }

    private void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        // User indexes
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Permission indexes
        modelBuilder.Entity<Permission>()
            .HasIndex(p => new { p.Category, p.Action, p.Resource })
            .IsUnique();

        // UserPermission indexes
        modelBuilder.Entity<UserPermission>()
            .HasIndex(up => new { up.UserId, up.PermissionId })
            .IsUnique();

        // RolePermission indexes
        modelBuilder.Entity<RolePermission>()
            .HasIndex(rp => new { rp.RoleId, rp.PermissionId })
            .IsUnique();

        // UserSession indexes
        modelBuilder.Entity<UserSession>()
            .HasIndex(us => us.SessionToken)
            .IsUnique();

        modelBuilder.Entity<UserSession>()
            .HasIndex(us => us.RefreshToken);

        // AuditLog indexes
        modelBuilder.Entity<AuditLog>()
            .HasIndex(al => al.Timestamp);

        modelBuilder.Entity<AuditLog>()
            .HasIndex(al => new { al.EntityName, al.EntityId });
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed default permissions
        var permissions = GetDefaultPermissions();
        modelBuilder.Entity<Permission>().HasData(permissions);

        // Seed default roles
        var roles = GetDefaultRoles();
        modelBuilder.Entity<Role>().HasData(roles);

        // Seed default admin user
        var adminUser = GetDefaultAdminUser();
        modelBuilder.Entity<User>().HasData(adminUser);
    }

    private List<Permission> GetDefaultPermissions()
    {
        var permissions = new List<Permission>();
        var categories = new[]
        {
            "Authentication", "Dashboard", "StudentAccounts", "ProjectAccounts",
            "Inventory", "Accounting", "Payroll", "Reports", "Settings", "Audit"
        };

        var actions = new[] { "Create", "Read", "Update", "Delete", "Export", "Import" };

        foreach (var category in categories)
        {
            foreach (var action in actions)
            {
                permissions.Add(new Permission
                {
                    Id = Guid.NewGuid(),
                    Name = $"{category}.{action}",
                    Description = $"{action} permission for {category} module",
                    Category = category,
                    Action = action,
                    Resource = category,
                    IsSystemPermission = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        return permissions;
    }

    private List<Role> GetDefaultRoles()
    {
        return new List<Role>
        {
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Administrator",
                Description = "Full system access",
                IsSystemRole = true,
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Bursar",
                Description = "Financial management access",
                IsSystemRole = true,
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "StoreManager",
                Description = "Inventory management access",
                IsSystemRole = true,
                CreatedAt = DateTime.UtcNow
            },
            new Role
            {
                Id = Guid.NewGuid(),
                Name = "Auditor",
                Description = "Read-only audit access",
                IsSystemRole = true,
                CreatedAt = DateTime.UtcNow
            }
        };
    }

    private User GetDefaultAdminUser()
    {
        // Note: In production, this should be configured securely
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin@123", salt);

        return new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            Email = "admin@ines.ac.rw",
            PasswordHash = hashedPassword,
            PasswordSalt = salt,
            FirstName = "System",
            LastName = "Administrator",
            Role = Core.Enums.UserRole.Admin,
            IsActive = true,
            IsEmailVerified = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update audit fields before saving
        UpdateAuditFields();
        
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}
