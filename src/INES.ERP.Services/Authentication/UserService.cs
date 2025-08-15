using Microsoft.Extensions.Logging;
using INES.ERP.Core.Interfaces.Repositories;
using INES.ERP.Core.Interfaces.Services;
using INES.ERP.Core.Models.Authentication;
using INES.ERP.Core.Models.Common;
using INES.ERP.Core.Enums;

namespace INES.ERP.Services.Authentication;

/// <summary>
/// User management service implementation
/// </summary>
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;

    public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<User> CreateUserAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate username and email availability
            if (!await IsUsernameAvailableAsync(user.Username, cancellationToken: cancellationToken))
            {
                throw new ArgumentException($"Username '{user.Username}' is already taken.");
            }

            if (!await IsEmailAvailableAsync(user.Email, cancellationToken: cancellationToken))
            {
                throw new ArgumentException($"Email '{user.Email}' is already registered.");
            }

            // Hash password
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            user.PasswordHash = hashedPassword;
            user.PasswordSalt = salt;
            user.CreatedAt = DateTime.UtcNow;

            var createdUser = await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User created successfully: {Username}", user.Username);
            return createdUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Username}", user.Username);
            throw;
        }
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate username and email availability (excluding current user)
            if (!await IsUsernameAvailableAsync(user.Username, user.Id, cancellationToken))
            {
                throw new ArgumentException($"Username '{user.Username}' is already taken.");
            }

            if (!await IsEmailAvailableAsync(user.Email, user.Id, cancellationToken))
            {
                throw new ArgumentException($"Email '{user.Email}' is already registered.");
            }

            user.UpdatedAt = DateTime.UtcNow;
            var updatedUser = await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User updated successfully: {UserId}", user.Id);
            return updatedUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {UserId}", user.Id);
            throw;
        }
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
    }

    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Users.GetSingleAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Users.GetSingleAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<PagedResult<User>> GetUsersAsync(
        int pageNumber = 1, 
        int pageSize = 50, 
        string? searchTerm = null, 
        UserRole? role = null,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Users.GetPagedAsync(
            pageNumber,
            pageSize,
            predicate: u => 
                (string.IsNullOrEmpty(searchTerm) || 
                 u.Username.Contains(searchTerm) || 
                 u.Email.Contains(searchTerm) || 
                 u.FirstName.Contains(searchTerm) || 
                 u.LastName.Contains(searchTerm)) &&
                (!role.HasValue || u.Role == role.Value),
            orderBy: q => q.OrderBy(u => u.LastName).ThenBy(u => u.FirstName),
            cancellationToken: cancellationToken);
    }

    public async Task<bool> ActivateUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user == null) return false;

            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User activated: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> DeactivateUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user == null) return false;

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User deactivated: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid userId, string? deletedBy = null, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.Users.SoftDeleteAsync(userId, deletedBy, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User deleted: {UserId} by {DeletedBy}", userId, deletedBy);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> AssignPermissionAsync(
        Guid userId, 
        Guid permissionId, 
        bool isGranted = true, 
        DateTime? expiresAt = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if permission assignment already exists
            var existingPermission = await _unitOfWork.UserPermissions.GetSingleAsync(
                up => up.UserId == userId && up.PermissionId == permissionId,
                cancellationToken);

            if (existingPermission != null)
            {
                // Update existing permission
                existingPermission.IsGranted = isGranted;
                existingPermission.ExpiresAt = expiresAt;
                existingPermission.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.UserPermissions.UpdateAsync(existingPermission, cancellationToken);
            }
            else
            {
                // Create new permission assignment
                var userPermission = new UserPermission
                {
                    UserId = userId,
                    PermissionId = permissionId,
                    IsGranted = isGranted,
                    ExpiresAt = expiresAt,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.UserPermissions.AddAsync(userPermission, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Permission assigned: User {UserId}, Permission {PermissionId}, Granted: {IsGranted}", 
                userId, permissionId, isGranted);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning permission: User {UserId}, Permission {PermissionId}", userId, permissionId);
            return false;
        }
    }

    public async Task<bool> RemovePermissionAsync(Guid userId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var userPermission = await _unitOfWork.UserPermissions.GetSingleAsync(
                up => up.UserId == userId && up.PermissionId == permissionId,
                cancellationToken);

            if (userPermission != null)
            {
                await _unitOfWork.UserPermissions.DeleteAsync(userPermission, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Permission removed: User {UserId}, Permission {PermissionId}", userId, permissionId);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing permission: User {UserId}, Permission {PermissionId}", userId, permissionId);
            return false;
        }
    }

    public async Task<IEnumerable<UserPermission>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.UserPermissions.GetAsync(
            up => up.UserId == userId && up.IsGranted && (!up.ExpiresAt.HasValue || up.ExpiresAt > DateTime.UtcNow),
            cancellationToken);
    }

    public async Task<bool> HasPermissionAsync(Guid userId, string permissionName, CancellationToken cancellationToken = default)
    {
        var permission = await _unitOfWork.Permissions.GetSingleAsync(p => p.Name == permissionName, cancellationToken);
        if (permission == null) return false;

        var userPermission = await _unitOfWork.UserPermissions.GetSingleAsync(
            up => up.UserId == userId && 
                  up.PermissionId == permission.Id && 
                  up.IsGranted && 
                  (!up.ExpiresAt.HasValue || up.ExpiresAt > DateTime.UtcNow),
            cancellationToken);

        return userPermission != null;
    }

    public async Task<IEnumerable<UserSession>> GetActiveSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.UserSessions.GetAsync(
            s => s.UserId == userId && s.IsActive && !s.IsExpired,
            cancellationToken);
    }

    public async Task<bool> IsUsernameAvailableAsync(string username, Guid? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var existingUser = await _unitOfWork.Users.GetSingleAsync(
            u => u.Username == username && (!excludeUserId.HasValue || u.Id != excludeUserId.Value),
            cancellationToken);
        return existingUser == null;
    }

    public async Task<bool> IsEmailAvailableAsync(string email, Guid? excludeUserId = null, CancellationToken cancellationToken = default)
    {
        var existingUser = await _unitOfWork.Users.GetSingleAsync(
            u => u.Email == email && (!excludeUserId.HasValue || u.Id != excludeUserId.Value),
            cancellationToken);
        return existingUser == null;
    }
}
