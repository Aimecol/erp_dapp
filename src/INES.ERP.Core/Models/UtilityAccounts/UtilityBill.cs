using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.UtilityAccounts;

/// <summary>
/// Represents a utility bill
/// </summary>
public class UtilityBill : AuditableEntity
{
    /// <summary>
    /// Bill reference number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string BillReference { get; set; } = string.Empty;

    /// <summary>
    /// Utility provider
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string UtilityProvider { get; set; } = string.Empty;

    /// <summary>
    /// Utility type (Electricity, Water, Gas, Internet, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string UtilityType { get; set; } = string.Empty;

    /// <summary>
    /// Account number with utility provider
    /// </summary>
    [MaxLength(50)]
    public string? AccountNumber { get; set; }

    /// <summary>
    /// Meter number
    /// </summary>
    [MaxLength(50)]
    public string? MeterNumber { get; set; }

    /// <summary>
    /// Billing period start date
    /// </summary>
    public DateTime BillingPeriodStart { get; set; }

    /// <summary>
    /// Billing period end date
    /// </summary>
    public DateTime BillingPeriodEnd { get; set; }

    /// <summary>
    /// Previous meter reading
    /// </summary>
    public decimal? PreviousReading { get; set; }

    /// <summary>
    /// Current meter reading
    /// </summary>
    public decimal? CurrentReading { get; set; }

    /// <summary>
    /// Units consumed
    /// </summary>
    public decimal UnitsConsumed { get; set; }

    /// <summary>
    /// Rate per unit
    /// </summary>
    public decimal RatePerUnit { get; set; }

    /// <summary>
    /// Base charge
    /// </summary>
    public decimal BaseCharge { get; set; }

    /// <summary>
    /// Usage charge
    /// </summary>
    public decimal UsageCharge { get; set; }

    /// <summary>
    /// Tax amount
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Other charges
    /// </summary>
    public decimal OtherCharges { get; set; }

    /// <summary>
    /// Total bill amount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Bill due date
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Bill status
    /// </summary>
    public Status BillStatus { get; set; } = Status.Pending;

    /// <summary>
    /// Payment date
    /// </summary>
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// Payment reference
    /// </summary>
    [MaxLength(100)]
    public string? PaymentReference { get; set; }

    /// <summary>
    /// Department allocation
    /// </summary>
    [MaxLength(100)]
    public string? Department { get; set; }

    /// <summary>
    /// Cost center
    /// </summary>
    [MaxLength(50)]
    public string? CostCenter { get; set; }

    /// <summary>
    /// Bill document path
    /// </summary>
    [MaxLength(500)]
    public string? DocumentPath { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public new string? Notes { get; set; }

    /// <summary>
    /// Navigation property for allocations
    /// </summary>
    public virtual ICollection<UtilityAllocation> Allocations { get; set; } = new List<UtilityAllocation>();

    /// <summary>
    /// Checks if bill is overdue
    /// </summary>
    public bool IsOverdue => DueDate < DateTime.Today && BillStatus != Status.Completed;

    /// <summary>
    /// Gets the billing period description
    /// </summary>
    public string BillingPeriodDescription => $"{BillingPeriodStart:MMM yyyy} - {BillingPeriodEnd:MMM yyyy}";
}

/// <summary>
/// Represents utility bill allocation to departments
/// </summary>
public class UtilityAllocation : BaseEntity
{
    /// <summary>
    /// Utility bill ID
    /// </summary>
    public Guid UtilityBillId { get; set; }

    /// <summary>
    /// Department code
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string DepartmentCode { get; set; } = string.Empty;

    /// <summary>
    /// Department name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// Allocation percentage
    /// </summary>
    public decimal AllocationPercentage { get; set; }

    /// <summary>
    /// Allocated amount
    /// </summary>
    public decimal AllocatedAmount { get; set; }

    /// <summary>
    /// Cost center
    /// </summary>
    [MaxLength(50)]
    public string? CostCenter { get; set; }

    /// <summary>
    /// Account code for posting
    /// </summary>
    [MaxLength(20)]
    public string? AccountCode { get; set; }

    /// <summary>
    /// Navigation property to utility bill
    /// </summary>
    public virtual UtilityBill UtilityBill { get; set; } = null!;
}

/// <summary>
/// Represents a vendor invoice
/// </summary>
public class VendorInvoice : AuditableEntity
{
    /// <summary>
    /// Invoice number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Vendor ID
    /// </summary>
    public Guid VendorId { get; set; }

    /// <summary>
    /// Vendor name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string VendorName { get; set; } = string.Empty;

    /// <summary>
    /// Invoice date
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// Due date
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Purchase order number
    /// </summary>
    [MaxLength(50)]
    public string? PurchaseOrderNumber { get; set; }

    /// <summary>
    /// Invoice description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Subtotal amount
    /// </summary>
    public decimal SubtotalAmount { get; set; }

    /// <summary>
    /// Tax amount
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Discount amount
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Total amount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "RWF";

    /// <summary>
    /// Invoice status
    /// </summary>
    public Status InvoiceStatus { get; set; } = Status.Pending;

    /// <summary>
    /// Approval status
    /// </summary>
    public Status ApprovalStatus { get; set; } = Status.Pending;

    /// <summary>
    /// Approved by
    /// </summary>
    [MaxLength(100)]
    public string? ApprovedBy { get; set; }

    /// <summary>
    /// Approval date
    /// </summary>
    public DateTime? ApprovalDate { get; set; }

    /// <summary>
    /// Payment date
    /// </summary>
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// Payment reference
    /// </summary>
    [MaxLength(100)]
    public string? PaymentReference { get; set; }

    /// <summary>
    /// Department
    /// </summary>
    [MaxLength(100)]
    public string? Department { get; set; }

    /// <summary>
    /// Cost center
    /// </summary>
    [MaxLength(50)]
    public string? CostCenter { get; set; }

    /// <summary>
    /// Invoice document path
    /// </summary>
    [MaxLength(500)]
    public string? DocumentPath { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public new string? Notes { get; set; }

    /// <summary>
    /// Navigation property for line items
    /// </summary>
    public virtual ICollection<VendorInvoiceLineItem> LineItems { get; set; } = new List<VendorInvoiceLineItem>();

    /// <summary>
    /// Checks if invoice is overdue
    /// </summary>
    public bool IsOverdue => DueDate < DateTime.Today && InvoiceStatus != Status.Completed;

    /// <summary>
    /// Checks if invoice is approved
    /// </summary>
    public bool IsApproved => ApprovalStatus == Status.Approved;
}

/// <summary>
/// Represents a line item in a vendor invoice
/// </summary>
public class VendorInvoiceLineItem : BaseEntity
{
    /// <summary>
    /// Vendor invoice ID
    /// </summary>
    public Guid VendorInvoiceId { get; set; }

    /// <summary>
    /// Line number
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// Item description
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Quantity
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Unit price
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Line total
    /// </summary>
    public decimal LineTotal => Quantity * UnitPrice;

    /// <summary>
    /// Account code
    /// </summary>
    [MaxLength(20)]
    public string? AccountCode { get; set; }

    /// <summary>
    /// Cost center
    /// </summary>
    [MaxLength(50)]
    public string? CostCenter { get; set; }

    /// <summary>
    /// Navigation property to vendor invoice
    /// </summary>
    public virtual VendorInvoice VendorInvoice { get; set; } = null!;
}

/// <summary>
/// Represents a recurring payment schedule
/// </summary>
public class RecurringPayment : AuditableEntity
{
    /// <summary>
    /// Payment name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string PaymentName { get; set; } = string.Empty;

    /// <summary>
    /// Payee name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string PayeeName { get; set; } = string.Empty;

    /// <summary>
    /// Payment amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "RWF";

    /// <summary>
    /// Payment frequency
    /// </summary>
    public Frequency Frequency { get; set; } = Frequency.Monthly;

    /// <summary>
    /// Start date
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date (optional)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Next payment date
    /// </summary>
    public DateTime NextPaymentDate { get; set; }

    /// <summary>
    /// Last payment date
    /// </summary>
    public DateTime? LastPaymentDate { get; set; }

    /// <summary>
    /// Payment method
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.BankTransfer;

    /// <summary>
    /// Account code for posting
    /// </summary>
    [MaxLength(20)]
    public string? AccountCode { get; set; }

    /// <summary>
    /// Department
    /// </summary>
    [MaxLength(100)]
    public string? Department { get; set; }

    /// <summary>
    /// Cost center
    /// </summary>
    [MaxLength(50)]
    public string? CostCenter { get; set; }

    /// <summary>
    /// Payment status
    /// </summary>
    public Status PaymentStatus { get; set; } = Status.Active;

    /// <summary>
    /// Auto-process payment
    /// </summary>
    public bool AutoProcess { get; set; } = false;

    /// <summary>
    /// Description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public new string? Notes { get; set; }

    /// <summary>
    /// Checks if payment is due
    /// </summary>
    public bool IsDue => NextPaymentDate <= DateTime.Today && PaymentStatus == Status.Active;

    /// <summary>
    /// Checks if payment is active
    /// </summary>
    public bool IsActive => PaymentStatus == Status.Active && (!EndDate.HasValue || EndDate.Value >= DateTime.Today);
}
