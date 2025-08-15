using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Billing;

/// <summary>
/// Represents a quotation
/// </summary>
public class Quotation : AuditableEntity
{
    /// <summary>
    /// Quotation number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string QuotationNumber { get; set; } = string.Empty;

    /// <summary>
    /// Customer ID
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Customer name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Quotation date
    /// </summary>
    public DateTime QuotationDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Valid until date
    /// </summary>
    public DateTime ValidUntil { get; set; }

    /// <summary>
    /// Description
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
    /// Quotation status
    /// </summary>
    public Status QuotationStatus { get; set; } = Status.Draft;

    /// <summary>
    /// Terms and conditions
    /// </summary>
    public string? TermsAndConditions { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public new string? Notes { get; set; }

    /// <summary>
    /// Navigation property for line items
    /// </summary>
    public virtual ICollection<QuotationLineItem> LineItems { get; set; } = new List<QuotationLineItem>();

    /// <summary>
    /// Checks if quotation is expired
    /// </summary>
    public bool IsExpired => ValidUntil < DateTime.Today;

    /// <summary>
    /// Checks if quotation is accepted
    /// </summary>
    public bool IsAccepted => QuotationStatus == Status.Approved;
}

/// <summary>
/// Represents a quotation line item
/// </summary>
public class QuotationLineItem : BaseEntity
{
    /// <summary>
    /// Quotation ID
    /// </summary>
    public Guid QuotationId { get; set; }

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
    /// Navigation property to quotation
    /// </summary>
    public virtual Quotation Quotation { get; set; } = null!;
}

/// <summary>
/// Represents a proforma invoice
/// </summary>
public class ProformaInvoice : AuditableEntity
{
    /// <summary>
    /// Proforma invoice number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ProformaNumber { get; set; } = string.Empty;

    /// <summary>
    /// Customer ID
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Customer name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Proforma date
    /// </summary>
    public DateTime ProformaDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Valid until date
    /// </summary>
    public DateTime ValidUntil { get; set; }

    /// <summary>
    /// Description
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
    /// Proforma status
    /// </summary>
    public Status ProformaStatus { get; set; } = Status.Draft;

    /// <summary>
    /// Related quotation ID
    /// </summary>
    public Guid? QuotationId { get; set; }

    /// <summary>
    /// Terms and conditions
    /// </summary>
    public string? TermsAndConditions { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public new string? Notes { get; set; }

    /// <summary>
    /// Navigation property for line items
    /// </summary>
    public virtual ICollection<ProformaLineItem> LineItems { get; set; } = new List<ProformaLineItem>();

    /// <summary>
    /// Navigation property to quotation
    /// </summary>
    public virtual Quotation? Quotation { get; set; }

    /// <summary>
    /// Checks if proforma is expired
    /// </summary>
    public bool IsExpired => ValidUntil < DateTime.Today;
}

/// <summary>
/// Represents a proforma invoice line item
/// </summary>
public class ProformaLineItem : BaseEntity
{
    /// <summary>
    /// Proforma invoice ID
    /// </summary>
    public Guid ProformaInvoiceId { get; set; }

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
    /// Navigation property to proforma invoice
    /// </summary>
    public virtual ProformaInvoice ProformaInvoice { get; set; } = null!;
}

/// <summary>
/// Represents an electronic invoice (RRA compliant)
/// </summary>
public class ElectronicInvoice : AuditableEntity
{
    /// <summary>
    /// Invoice number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// RRA invoice signature
    /// </summary>
    [MaxLength(500)]
    public string? RRASignature { get; set; }

    /// <summary>
    /// RRA internal ID
    /// </summary>
    [MaxLength(100)]
    public string? RRAInternalId { get; set; }

    /// <summary>
    /// Customer ID
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Customer name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Customer TIN
    /// </summary>
    [MaxLength(20)]
    public string? CustomerTIN { get; set; }

    /// <summary>
    /// Invoice date
    /// </summary>
    public DateTime InvoiceDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Due date
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Description
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
    /// Paid amount
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Outstanding amount
    /// </summary>
    public decimal OutstandingAmount => TotalAmount - PaidAmount;

    /// <summary>
    /// Currency
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "RWF";

    /// <summary>
    /// Invoice status
    /// </summary>
    public Status InvoiceStatus { get; set; } = Status.Draft;

    /// <summary>
    /// RRA submission status
    /// </summary>
    public Status RRAStatus { get; set; } = Status.Pending;

    /// <summary>
    /// RRA submission date
    /// </summary>
    public DateTime? RRASubmissionDate { get; set; }

    /// <summary>
    /// Related proforma ID
    /// </summary>
    public Guid? ProformaInvoiceId { get; set; }

    /// <summary>
    /// Payment terms
    /// </summary>
    [MaxLength(100)]
    public string? PaymentTerms { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(1000)]
    public new string? Notes { get; set; }

    /// <summary>
    /// Navigation property for line items
    /// </summary>
    public virtual ICollection<ElectronicInvoiceLineItem> LineItems { get; set; } = new List<ElectronicInvoiceLineItem>();

    /// <summary>
    /// Navigation property for payments
    /// </summary>
    public virtual ICollection<InvoicePayment> Payments { get; set; } = new List<InvoicePayment>();

    /// <summary>
    /// Navigation property to proforma invoice
    /// </summary>
    public virtual ProformaInvoice? ProformaInvoice { get; set; }

    /// <summary>
    /// Checks if invoice is overdue
    /// </summary>
    public bool IsOverdue => DueDate < DateTime.Today && OutstandingAmount > 0;

    /// <summary>
    /// Checks if invoice is fully paid
    /// </summary>
    public bool IsFullyPaid => Math.Abs(OutstandingAmount) < 0.01m;

    /// <summary>
    /// Checks if invoice is submitted to RRA
    /// </summary>
    public bool IsSubmittedToRRA => RRAStatus == Status.Completed && RRASubmissionDate.HasValue;
}

/// <summary>
/// Represents an electronic invoice line item
/// </summary>
public class ElectronicInvoiceLineItem : BaseEntity
{
    /// <summary>
    /// Electronic invoice ID
    /// </summary>
    public Guid ElectronicInvoiceId { get; set; }

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
    /// Tax rate
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Line total before tax
    /// </summary>
    public decimal LineSubtotal => Quantity * UnitPrice;

    /// <summary>
    /// Line tax amount
    /// </summary>
    public decimal LineTaxAmount => LineSubtotal * (TaxRate / 100);

    /// <summary>
    /// Line total including tax
    /// </summary>
    public decimal LineTotal => LineSubtotal + LineTaxAmount;

    /// <summary>
    /// Navigation property to electronic invoice
    /// </summary>
    public virtual ElectronicInvoice ElectronicInvoice { get; set; } = null!;
}

/// <summary>
/// Represents a payment against an invoice
/// </summary>
public class InvoicePayment : AuditableEntity
{
    /// <summary>
    /// Payment reference
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PaymentReference { get; set; } = string.Empty;

    /// <summary>
    /// Electronic invoice ID
    /// </summary>
    public Guid ElectronicInvoiceId { get; set; }

    /// <summary>
    /// Payment date
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Payment amount
    /// </summary>
    public decimal PaymentAmount { get; set; }

    /// <summary>
    /// Payment method
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

    /// <summary>
    /// Bank account
    /// </summary>
    [MaxLength(100)]
    public string? BankAccount { get; set; }

    /// <summary>
    /// Mobile money number
    /// </summary>
    [MaxLength(20)]
    public string? MobileMoneyNumber { get; set; }

    /// <summary>
    /// Transaction reference
    /// </summary>
    [MaxLength(100)]
    public string? TransactionReference { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public new string? Notes { get; set; }

    /// <summary>
    /// Navigation property to electronic invoice
    /// </summary>
    public virtual ElectronicInvoice ElectronicInvoice { get; set; } = null!;
}

/// <summary>
/// Represents a receipt
/// </summary>
public class Receipt : AuditableEntity
{
    /// <summary>
    /// Receipt number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ReceiptNumber { get; set; } = string.Empty;

    /// <summary>
    /// Customer ID
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Customer name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Receipt date
    /// </summary>
    public DateTime ReceiptDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Amount received
    /// </summary>
    public decimal AmountReceived { get; set; }

    /// <summary>
    /// Payment method
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

    /// <summary>
    /// Currency
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "RWF";

    /// <summary>
    /// Description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Related invoice ID
    /// </summary>
    public Guid? RelatedInvoiceId { get; set; }

    /// <summary>
    /// Bank account
    /// </summary>
    [MaxLength(100)]
    public string? BankAccount { get; set; }

    /// <summary>
    /// Mobile money number
    /// </summary>
    [MaxLength(20)]
    public string? MobileMoneyNumber { get; set; }

    /// <summary>
    /// Transaction reference
    /// </summary>
    [MaxLength(100)]
    public string? TransactionReference { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public new string? Notes { get; set; }

    /// <summary>
    /// Navigation property to related invoice
    /// </summary>
    public virtual ElectronicInvoice? RelatedInvoice { get; set; }
}
