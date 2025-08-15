using System.ComponentModel.DataAnnotations;

namespace INES.ERP.Core.Models.Common;

/// <summary>
/// Represents contact information
/// </summary>
public class Contact : BaseEntity
{
    /// <summary>
    /// Contact type (Phone, Email, Fax, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ContactType { get; set; } = string.Empty;

    /// <summary>
    /// Contact value (phone number, email address, etc.)
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string ContactValue { get; set; } = string.Empty;

    /// <summary>
    /// Contact label (Home, Work, Mobile, etc.)
    /// </summary>
    [MaxLength(50)]
    public string? Label { get; set; }

    /// <summary>
    /// Indicates if this is the primary contact
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Indicates if this contact is verified
    /// </summary>
    public bool IsVerified { get; set; } = false;

    /// <summary>
    /// Date when the contact was verified
    /// </summary>
    public DateTime? VerifiedAt { get; set; }
}

/// <summary>
/// Common contact types
/// </summary>
public static class ContactTypes
{
    public const string Phone = "Phone";
    public const string Email = "Email";
    public const string Fax = "Fax";
    public const string Website = "Website";
    public const string SocialMedia = "SocialMedia";
}

/// <summary>
/// Common contact labels
/// </summary>
public static class ContactLabels
{
    public const string Home = "Home";
    public const string Work = "Work";
    public const string Mobile = "Mobile";
    public const string Personal = "Personal";
    public const string Business = "Business";
    public const string Emergency = "Emergency";
}
