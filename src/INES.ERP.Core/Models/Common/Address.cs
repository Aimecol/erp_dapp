using System.ComponentModel.DataAnnotations;

namespace INES.ERP.Core.Models.Common;

/// <summary>
/// Represents a physical address
/// </summary>
public class Address : BaseEntity
{
    /// <summary>
    /// Street address line 1
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Street1 { get; set; } = string.Empty;

    /// <summary>
    /// Street address line 2 (optional)
    /// </summary>
    [MaxLength(200)]
    public string? Street2 { get; set; }

    /// <summary>
    /// City name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State or province
    /// </summary>
    [MaxLength(100)]
    public string? State { get; set; }

    /// <summary>
    /// Postal or ZIP code
    /// </summary>
    [MaxLength(20)]
    public string? PostalCode { get; set; }

    /// <summary>
    /// Country name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = "Rwanda";

    /// <summary>
    /// Address type (Home, Work, Billing, Shipping, etc.)
    /// </summary>
    [MaxLength(50)]
    public string? AddressType { get; set; }

    /// <summary>
    /// Indicates if this is the primary address
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Gets the full formatted address
    /// </summary>
    public string FullAddress
    {
        get
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(Street1))
                parts.Add(Street1);
            
            if (!string.IsNullOrWhiteSpace(Street2))
                parts.Add(Street2);
            
            if (!string.IsNullOrWhiteSpace(City))
                parts.Add(City);
            
            if (!string.IsNullOrWhiteSpace(State))
                parts.Add(State);
            
            if (!string.IsNullOrWhiteSpace(PostalCode))
                parts.Add(PostalCode);
            
            if (!string.IsNullOrWhiteSpace(Country))
                parts.Add(Country);
            
            return string.Join(", ", parts);
        }
    }
}
