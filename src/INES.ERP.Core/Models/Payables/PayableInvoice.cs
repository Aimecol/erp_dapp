using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Payables;

/// <summary>
/// Represents a payable invoice
/// </summary>
public class PayableInvoice : AuditableEntity
{
    /// <summary>
    /// Invoice number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Supplier ID
    /// </summary>
    public Guid SupplierId { get; set; }

    /// <summary>
    /// Supplier name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string SupplierName { get; set; } = string.Empty;

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
    public Status InvoiceStatus { get; set; } = Status.Pending;

    /// <summary>
    /// Payment terms
    /// </summary>
    [MaxLength(100)]
    public string? PaymentTerms { get; set; }

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
    /// Navigation property for payments
    /// </summary>
    public virtual ICollection<PayablePayment> Payments { get; set; } = new List<PayablePayment>();

    /// <summary>
    /// Checks if invoice is overdue
    /// </summary>
    public bool IsOverdue => DueDate < DateTime.Today && OutstandingAmount > 0;

    /// <summary>
    /// Checks if invoice is fully paid
    /// </summary>
    public bool IsFullyPaid => Math.Abs(OutstandingAmount) < 0.01m;
}

/// <summary>
/// Represents a payment against a payable invoice
/// </summary>
public class PayablePayment : AuditableEntity
{
    /// <summary>
    /// Payment reference
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PaymentReference { get; set; } = string.Empty;

    /// <summary>
    /// Payable invoice ID
    /// </summary>
    public Guid PayableInvoiceId { get; set; }

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
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.BankTransfer;

    /// <summary>
    /// Bank account
    /// </summary>
    [MaxLength(100)]
    public string? BankAccount { get; set; }

    /// <summary>
    /// Check number
    /// </summary>
    [MaxLength(50)]
    public string? CheckNumber { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public new string? Notes { get; set; }

    /// <summary>
    /// Navigation property to payable invoice
    /// </summary>
    public virtual PayableInvoice PayableInvoice { get; set; } = null!;
}

/// <summary>
/// Represents a receivable invoice
/// </summary>
public class ReceivableInvoice : AuditableEntity
{
    /// <summary>
    /// Invoice number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

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
    /// Invoice date
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// Due date
    /// </summary>
    public DateTime DueDate { get; set; }

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
    /// Received amount
    /// </summary>
    public decimal ReceivedAmount { get; set; }

    /// <summary>
    /// Outstanding amount
    /// </summary>
    public decimal OutstandingAmount => TotalAmount - ReceivedAmount;

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
    /// Payment terms
    /// </summary>
    [MaxLength(100)]
    public string? PaymentTerms { get; set; }

    /// <summary>
    /// Department
    /// </summary>
    [MaxLength(100)]
    public string? Department { get; set; }

    /// <summary>
    /// Navigation property for receipts
    /// </summary>
    public virtual ICollection<ReceivableReceipt> Receipts { get; set; } = new List<ReceivableReceipt>();

    /// <summary>
    /// Checks if invoice is overdue
    /// </summary>
    public bool IsOverdue => DueDate < DateTime.Today && OutstandingAmount > 0;

    /// <summary>
    /// Checks if invoice is fully paid
    /// </summary>
    public bool IsFullyPaid => Math.Abs(OutstandingAmount) < 0.01m;
}

/// <summary>
/// Represents a receipt against a receivable invoice
/// </summary>
public class ReceivableReceipt : AuditableEntity
{
    /// <summary>
    /// Receipt reference
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ReceiptReference { get; set; } = string.Empty;

    /// <summary>
    /// Receivable invoice ID
    /// </summary>
    public Guid ReceivableInvoiceId { get; set; }

    /// <summary>
    /// Receipt date
    /// </summary>
    public DateTime ReceiptDate { get; set; }

    /// <summary>
    /// Receipt amount
    /// </summary>
    public decimal ReceiptAmount { get; set; }

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
    /// Check number
    /// </summary>
    [MaxLength(50)]
    public string? CheckNumber { get; set; }

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public new string? Notes { get; set; }

    /// <summary>
    /// Navigation property to receivable invoice
    /// </summary>
    public virtual ReceivableInvoice ReceivableInvoice { get; set; } = null!;
}

/// <summary>
/// Represents a credit note
/// </summary>
public class CreditNote : AuditableEntity
{
    /// <summary>
    /// Credit note number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string CreditNoteNumber { get; set; } = string.Empty;

    /// <summary>
    /// Credit note type (Payable or Receivable)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string CreditNoteType { get; set; } = string.Empty;

    /// <summary>
    /// Related invoice ID
    /// </summary>
    public Guid? RelatedInvoiceId { get; set; }

    /// <summary>
    /// Related invoice number
    /// </summary>
    [MaxLength(50)]
    public string? RelatedInvoiceNumber { get; set; }

    /// <summary>
    /// Party ID (Supplier or Customer)
    /// </summary>
    public Guid PartyId { get; set; }

    /// <summary>
    /// Party name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string PartyName { get; set; } = string.Empty;

    /// <summary>
    /// Credit note date
    /// </summary>
    public DateTime CreditNoteDate { get; set; }

    /// <summary>
    /// Reason for credit note
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Credit amount
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "RWF";

    /// <summary>
    /// Credit note status
    /// </summary>
    public Status CreditNoteStatus { get; set; } = Status.Pending;

    /// <summary>
    /// Applied amount
    /// </summary>
    public decimal AppliedAmount { get; set; }

    /// <summary>
    /// Remaining credit
    /// </summary>
    public decimal RemainingCredit => CreditAmount - AppliedAmount;

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
    /// Checks if credit note is approved
    /// </summary>
    public bool IsApproved => CreditNoteStatus == Status.Approved;

    /// <summary>
    /// Checks if credit note is fully applied
    /// </summary>
    public bool IsFullyApplied => Math.Abs(RemainingCredit) < 0.01m;
}

/// <summary>
/// Represents a debit note
/// </summary>
public class DebitNote : AuditableEntity
{
    /// <summary>
    /// Debit note number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string DebitNoteNumber { get; set; } = string.Empty;

    /// <summary>
    /// Debit note type (Payable or Receivable)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string DebitNoteType { get; set; } = string.Empty;

    /// <summary>
    /// Related invoice ID
    /// </summary>
    public Guid? RelatedInvoiceId { get; set; }

    /// <summary>
    /// Related invoice number
    /// </summary>
    [MaxLength(50)]
    public string? RelatedInvoiceNumber { get; set; }

    /// <summary>
    /// Party ID (Supplier or Customer)
    /// </summary>
    public Guid PartyId { get; set; }

    /// <summary>
    /// Party name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string PartyName { get; set; } = string.Empty;

    /// <summary>
    /// Debit note date
    /// </summary>
    public DateTime DebitNoteDate { get; set; }

    /// <summary>
    /// Reason for debit note
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Debit amount
    /// </summary>
    public decimal DebitAmount { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "RWF";

    /// <summary>
    /// Debit note status
    /// </summary>
    public Status DebitNoteStatus { get; set; } = Status.Pending;

    /// <summary>
    /// Applied amount
    /// </summary>
    public decimal AppliedAmount { get; set; }

    /// <summary>
    /// Remaining debit
    /// </summary>
    public decimal RemainingDebit => DebitAmount - AppliedAmount;

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
    /// Checks if debit note is approved
    /// </summary>
    public bool IsApproved => DebitNoteStatus == Status.Approved;

    /// <summary>
    /// Checks if debit note is fully applied
    /// </summary>
    public bool IsFullyApplied => Math.Abs(RemainingDebit) < 0.01m;
}

/// <summary>
/// Represents a bank reconciliation
/// </summary>
public class BankReconciliation : AuditableEntity
{
    /// <summary>
    /// Bank account
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// Statement date
    /// </summary>
    public DateTime StatementDate { get; set; }

    /// <summary>
    /// Statement balance
    /// </summary>
    public decimal StatementBalance { get; set; }

    /// <summary>
    /// Book balance
    /// </summary>
    public decimal BookBalance { get; set; }

    /// <summary>
    /// Reconciliation status
    /// </summary>
    public Status ReconciliationStatus { get; set; } = Status.InProgress;

    /// <summary>
    /// Reconciled by
    /// </summary>
    [MaxLength(100)]
    public string? ReconciledBy { get; set; }

    /// <summary>
    /// Reconciliation date
    /// </summary>
    public DateTime? ReconciliationDate { get; set; }

    /// <summary>
    /// Outstanding deposits
    /// </summary>
    public decimal OutstandingDeposits { get; set; }

    /// <summary>
    /// Outstanding checks
    /// </summary>
    public decimal OutstandingChecks { get; set; }

    /// <summary>
    /// Bank charges
    /// </summary>
    public decimal BankCharges { get; set; }

    /// <summary>
    /// Interest earned
    /// </summary>
    public decimal InterestEarned { get; set; }

    /// <summary>
    /// Adjusted book balance
    /// </summary>
    public decimal AdjustedBookBalance => BookBalance - BankCharges + InterestEarned;

    /// <summary>
    /// Adjusted bank balance
    /// </summary>
    public decimal AdjustedBankBalance => StatementBalance + OutstandingDeposits - OutstandingChecks;

    /// <summary>
    /// Difference
    /// </summary>
    public decimal Difference => AdjustedBookBalance - AdjustedBankBalance;

    /// <summary>
    /// Checks if reconciliation is balanced
    /// </summary>
    public bool IsBalanced => Math.Abs(Difference) < 0.01m;
}
