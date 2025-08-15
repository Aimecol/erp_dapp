using Microsoft.Extensions.Logging;
using INES.ERP.Core.Interfaces.Repositories;
using INES.ERP.Core.Interfaces.Services;
using INES.ERP.Core.Models.Authentication;
using INES.ERP.Core.Enums;
using System.Security.Cryptography;
using System.Text;

namespace INES.ERP.Services.Authentication;

/// <summary>
/// Authentication service implementation
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(IUnitOfWork unitOfWork, ILogger<AuthenticationService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<AuthenticationResult> AuthenticateAsync(
        string username, 
        string password, 
        string? ipAddress = null, 
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authentication attempt for user: {Username}", username);

            // Find user by username or email
            var user = await _unitOfWork.Users.GetSingleAsync(
                u => u.Username == username || u.Email == username, 
                cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Authentication failed: User not found - {Username}", username);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid username or password."
                };
            }

            // Check if account is locked
            if (user.IsLocked)
            {
                _logger.LogWarning("Authentication failed: Account locked - {Username}", username);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Account is locked until {user.LockedUntil:yyyy-MM-dd HH:mm:ss}."
                };
            }

            // Check if account is active
            if (!user.IsActive)
            {
                _logger.LogWarning("Authentication failed: Account inactive - {Username}", username);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Account is inactive. Please contact administrator."
                };
            }

            // Verify password
            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                await HandleFailedLoginAsync(user, cancellationToken);
                _logger.LogWarning("Authentication failed: Invalid password - {Username}", username);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid username or password."
                };
            }

            // Check if two-factor authentication is required
            if (user.IsTwoFactorEnabled)
            {
                _logger.LogInformation("Two-factor authentication required for user: {Username}", username);
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    RequiresTwoFactor = true,
                    ErrorMessage = "Two-factor authentication required."
                };
            }

            // Create session
            var session = await CreateSessionAsync(user, ipAddress, userAgent, cancellationToken);

            // Update user login information
            await UpdateUserLoginAsync(user, cancellationToken);

            _logger.LogInformation("Authentication successful for user: {Username}", username);

            return new AuthenticationResult
            {
                IsSuccess = true,
                User = user,
                SessionToken = session.SessionToken,
                RefreshToken = session.RefreshToken,
                ExpiresAt = session.ExpiresAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication for user: {Username}", username);
            return new AuthenticationResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred during authentication. Please try again."
            };
        }
    }

    public async Task<AuthenticationResult> AuthenticateWithTwoFactorAsync(
        string username, 
        string password, 
        string twoFactorCode,
        string? ipAddress = null, 
        string? userAgent = null,
        CancellationToken cancellationToken = default)
    {
        // First authenticate with username and password
        var result = await AuthenticateAsync(username, password, ipAddress, userAgent, cancellationToken);
        
        if (!result.RequiresTwoFactor)
        {
            return result;
        }

        // Verify two-factor code
        var user = await _unitOfWork.Users.GetSingleAsync(
            u => u.Username == username || u.Email == username, 
            cancellationToken);

        if (user == null || !VerifyTwoFactorCode(user.TwoFactorSecret!, twoFactorCode))
        {
            _logger.LogWarning("Two-factor authentication failed for user: {Username}", username);
            return new AuthenticationResult
            {
                IsSuccess = false,
                ErrorMessage = "Invalid two-factor authentication code."
            };
        }

        // Create session
        var session = await CreateSessionAsync(user, ipAddress, userAgent, cancellationToken);

        // Update user login information
        await UpdateUserLoginAsync(user, cancellationToken);

        _logger.LogInformation("Two-factor authentication successful for user: {Username}", username);

        return new AuthenticationResult
        {
            IsSuccess = true,
            User = user,
            SessionToken = session.SessionToken,
            RefreshToken = session.RefreshToken,
            ExpiresAt = session.ExpiresAt
        };
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        try
        {
            var session = await _unitOfWork.UserSessions.GetSingleAsync(
                s => s.RefreshToken == refreshToken && s.IsActive,
                cancellationToken);

            if (session == null || session.IsExpired)
            {
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid or expired refresh token."
                };
            }

            // Get user
            var user = await _unitOfWork.Users.GetByIdAsync(session.UserId, cancellationToken);
            if (user == null || !user.IsActive)
            {
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "User account is not active."
                };
            }

            // Create new session
            var newSession = await CreateSessionAsync(user, session.IpAddress, session.UserAgent, cancellationToken);

            // Deactivate old session
            session.IsActive = false;
            session.EndedAt = DateTime.UtcNow;
            await _unitOfWork.UserSessions.UpdateAsync(session, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthenticationResult
            {
                IsSuccess = true,
                User = user,
                SessionToken = newSession.SessionToken,
                RefreshToken = newSession.RefreshToken,
                ExpiresAt = newSession.ExpiresAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during token refresh");
            return new AuthenticationResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred during token refresh."
            };
        }
    }

    public async Task LogoutAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        try
        {
            var session = await _unitOfWork.UserSessions.GetSingleAsync(
                s => s.SessionToken == sessionToken,
                cancellationToken);

            if (session != null)
            {
                session.IsActive = false;
                session.EndedAt = DateTime.UtcNow;
                await _unitOfWork.UserSessions.UpdateAsync(session, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
        }
    }

    public async Task LogoutAllSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var sessions = await _unitOfWork.UserSessions.GetAsync(
                s => s.UserId == userId && s.IsActive,
                cancellationToken);

            foreach (var session in sessions)
            {
                session.IsActive = false;
                session.EndedAt = DateTime.UtcNow;
                await _unitOfWork.UserSessions.UpdateAsync(session, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout all sessions for user: {UserId}", userId);
        }
    }

    public async Task<User?> ValidateSessionAsync(string sessionToken, CancellationToken cancellationToken = default)
    {
        try
        {
            var session = await _unitOfWork.UserSessions.GetSingleAsync(
                s => s.SessionToken == sessionToken && s.IsActive,
                cancellationToken);

            if (session == null || session.IsExpired)
            {
                return null;
            }

            // Update last activity
            session.LastActivityAt = DateTime.UtcNow;
            await _unitOfWork.UserSessions.UpdateAsync(session, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return await _unitOfWork.Users.GetByIdAsync(session.UserId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during session validation");
            return null;
        }
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("Password change failed: User not found - {UserId}", userId);
                return false;
            }

            // Verify current password
            if (!VerifyPassword(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                _logger.LogWarning("Password change failed: Invalid current password - {UserId}", userId);
                return false;
            }

            // Hash new password
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword, salt);

            // Update user
            user.PasswordHash = hashedPassword;
            user.PasswordSalt = salt;
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Password changed successfully for user: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password change for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> InitiatePasswordResetAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.Users.GetSingleAsync(u => u.Email == email, cancellationToken);
            if (user == null)
            {
                // Don't reveal if email exists for security
                _logger.LogWarning("Password reset requested for non-existent email: {Email}", email);
                return true; // Return true to not reveal email existence
            }

            // Generate reset token
            var resetToken = GenerateSecureToken();
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(15); // 15 minute expiry

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // TODO: Send email with reset token
            _logger.LogInformation("Password reset initiated for user: {Email}", email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset initiation for email: {Email}", email);
            return false;
        }
    }

    public async Task<bool> ResetPasswordAsync(string resetToken, string newPassword, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.Users.GetSingleAsync(
                u => u.PasswordResetToken == resetToken && u.PasswordResetTokenExpiry > DateTime.UtcNow,
                cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Password reset failed: Invalid or expired token");
                return false;
            }

            // Hash new password
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword, salt);

            // Update user
            user.PasswordHash = hashedPassword;
            user.PasswordSalt = salt;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Password reset successfully for user: {UserId}", user.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            return false;
        }
    }

    public async Task<TwoFactorSetupResult> EnableTwoFactorAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Generate secret key for TOTP
            var secretKey = GenerateSecureToken();
            user.TwoFactorSecret = secretKey;
            user.IsTwoFactorEnabled = true;

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TwoFactorSetupResult
            {
                SecretKey = secretKey,
                QrCodeUrl = $"otpauth://totp/INES-ERP:{user.Email}?secret={secretKey}&issuer=INES-ERP",
                ManualEntryKey = secretKey
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enabling two-factor authentication for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> DisableTwoFactorAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                return false;
            }

            user.IsTwoFactorEnabled = false;
            user.TwoFactorSecret = null;

            await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Two-factor authentication disabled for user: {UserId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disabling two-factor authentication for user: {UserId}", userId);
            return false;
        }
    }

    // Private helper methods
    private bool VerifyPassword(string password, string hash, string salt)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    private bool VerifyTwoFactorCode(string secret, string code)
    {
        // Implementation for TOTP verification would go here
        // For now, return true for demonstration
        return true;
    }

    private async Task<UserSession> CreateSessionAsync(User user, string? ipAddress, string? userAgent, CancellationToken cancellationToken)
    {
        var session = new UserSession
        {
            UserId = user.Id,
            SessionToken = GenerateSecureToken(),
            RefreshToken = GenerateSecureToken(),
            StartedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(8), // 8 hour session
            IpAddress = ipAddress,
            UserAgent = userAgent,
            IsActive = true,
            LastActivityAt = DateTime.UtcNow
        };

        await _unitOfWork.UserSessions.AddAsync(session, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return session;
    }

    private async Task UpdateUserLoginAsync(User user, CancellationToken cancellationToken)
    {
        user.LastLoginAt = DateTime.UtcNow;
        user.FailedLoginAttempts = 0;
        user.LockedUntil = null;
        await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task HandleFailedLoginAsync(User user, CancellationToken cancellationToken)
    {
        user.FailedLoginAttempts++;
        
        if (user.FailedLoginAttempts >= 5) // Max attempts
        {
            user.LockedUntil = DateTime.UtcNow.AddMinutes(30); // Lock for 30 minutes
        }

        await _unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private string GenerateSecureToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
