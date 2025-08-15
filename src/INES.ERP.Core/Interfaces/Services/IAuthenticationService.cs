using INES.ERP.Core.Models.Authentication;

namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Authentication service interface
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates a user with username and password
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <param name="ipAddress">IP address</param>
    /// <param name="userAgent">User agent</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    Task<AuthenticationResult> AuthenticateAsync(
        string username, 
        string password, 
        string? ipAddress = null, 
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Authenticates a user with two-factor authentication
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <param name="twoFactorCode">Two-factor authentication code</param>
    /// <param name="ipAddress">IP address</param>
    /// <param name="userAgent">User agent</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    Task<AuthenticationResult> AuthenticateWithTwoFactorAsync(
        string username, 
        string password, 
        string twoFactorCode,
        string? ipAddress = null, 
        string? userAgent = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes an authentication token
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result</returns>
    Task<AuthenticationResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs out a user
    /// </summary>
    /// <param name="sessionToken">Session token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    Task LogoutAsync(string sessionToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs out all sessions for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    Task LogoutAllSessionsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates a session token
    /// </summary>
    /// <param name="sessionToken">Session token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User if valid, null otherwise</returns>
    Task<User?> ValidateSessionAsync(string sessionToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes a user's password
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="currentPassword">Current password</param>
    /// <param name="newPassword">New password</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initiates password reset process
    /// </summary>
    /// <param name="email">User email</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> InitiatePasswordResetAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets password using reset token
    /// </summary>
    /// <param name="resetToken">Reset token</param>
    /// <param name="newPassword">New password</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> ResetPasswordAsync(string resetToken, string newPassword, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enables two-factor authentication for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Two-factor setup information</returns>
    Task<TwoFactorSetupResult> EnableTwoFactorAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disables two-factor authentication for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> DisableTwoFactorAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Authentication result
/// </summary>
public class AuthenticationResult
{
    /// <summary>
    /// Indicates if authentication was successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Error message if authentication failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Authenticated user
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Session token
    /// </summary>
    public string? SessionToken { get; set; }

    /// <summary>
    /// Refresh token
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Token expiry time
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Indicates if two-factor authentication is required
    /// </summary>
    public bool RequiresTwoFactor { get; set; }
}

/// <summary>
/// Two-factor authentication setup result
/// </summary>
public class TwoFactorSetupResult
{
    /// <summary>
    /// Secret key for two-factor authentication
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// QR code URL for setting up authenticator app
    /// </summary>
    public string QrCodeUrl { get; set; } = string.Empty;

    /// <summary>
    /// Manual entry key for authenticator app
    /// </summary>
    public string ManualEntryKey { get; set; } = string.Empty;
}
