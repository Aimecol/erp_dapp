using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Accounting;

/// <summary>
/// Represents a chart of accounts entry
/// </summary>
public class Account : AuditableEntity
{
    /// <summary>
    /// Account code/number
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string AccountCode { get; set; } = string.Empty;

    /// <summary>
    /// Account name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Account description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Account type (Asset, Liability, Equity, Income, Expense)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string AccountType { get; set; } = string.Empty;

    /// <summary>
    /// Account category (Current Asset, Fixed Asset, etc.)
    /// </summary>
    [MaxLength(50)]
    public string? AccountCategory { get; set; }

    /// <summary>
    /// Parent account ID (for hierarchical structure)
    /// </summary>
    public Guid? ParentAccountId { get; set; }

    /// <summary>
    /// Account level in hierarchy
    /// </summary>
    public int AccountLevel { get; set; } = 1;

    /// <summary>
    /// Normal balance (Debit or Credit)
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string NormalBalance { get; set; } = string.Empty;

    /// <summary>
    /// Current balance
    /// </summary>
    public decimal CurrentBalance { get; set; }

    /// <summary>
    /// Opening balance for the current period
    /// </summary>
    public decimal OpeningBalance { get; set; }

    /// <summary>
    /// Indicates if this is a control account
    /// </summary>
    public bool IsControlAccount { get; set; } = false;

    /// <summary>
    /// Indicates if this account allows direct posting
    /// </summary>
    public bool AllowDirectPosting { get; set; } = true;

    /// <summary>
    /// Indicates if this account is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Tax code associated with this account
    /// </summary>
    [MaxLength(20)]
    public string? TaxCode { get; set; }

    /// <summary>
    /// Currency code
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "RWF";

    /// <summary>
    /// Navigation property to parent account
    /// </summary>
    public virtual Account? ParentAccount { get; set; }

    /// <summary>
    /// Navigation property to child accounts
    /// </summary>
    public virtual ICollection<Account> ChildAccounts { get; set; } = new List<Account>();

    /// <summary>
    /// Navigation property to journal entries
    /// </summary>
    public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();

    /// <summary>
    /// Gets the full account path
    /// </summary>
    public string FullAccountPath
    {
        get
        {
            var path = AccountName;
            var parent = ParentAccount;
            while (parent != null)
            {
                path = $"{parent.AccountName} > {path}";
                parent = parent.ParentAccount;
            }
            return path;
        }
    }

    /// <summary>
    /// Checks if account has child accounts
    /// </summary>
    public bool HasChildren => ChildAccounts.Any();
}

/// <summary>
/// Represents a journal entry
/// </summary>
public class JournalEntry : AuditableEntity
{
    /// <summary>
    /// Journal entry number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string JournalEntryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Entry date
    /// </summary>
    public DateTime EntryDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Posting date
    /// </summary>
    public DateTime PostingDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Reference number
    /// </summary>
    [MaxLength(50)]
    public new string? ReferenceNumber { get; set; }

    /// <summary>
    /// Description/narration
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Total debit amount
    /// </summary>
    public decimal TotalDebit { get; set; }

    /// <summary>
    /// Total credit amount
    /// </summary>
    public decimal TotalCredit { get; set; }

    /// <summary>
    /// Entry status
    /// </summary>
    public Status EntryStatus { get; set; } = Status.Draft;

    /// <summary>
    /// Posted by user
    /// </summary>
    [MaxLength(100)]
    public string? PostedBy { get; set; }

    /// <summary>
    /// Posted date
    /// </summary>
    public DateTime? PostedDate { get; set; }

    /// <summary>
    /// Approved by user
    /// </summary>
    [MaxLength(100)]
    public string? ApprovedBy { get; set; }

    /// <summary>
    /// Approval date
    /// </summary>
    public DateTime? ApprovalDate { get; set; }

    /// <summary>
    /// Source document type
    /// </summary>
    [MaxLength(50)]
    public string? SourceDocumentType { get; set; }

    /// <summary>
    /// Source document ID
    /// </summary>
    public Guid? SourceDocumentId { get; set; }

    /// <summary>
    /// Reversal entry ID (if this entry is reversed)
    /// </summary>
    public Guid? ReversalEntryId { get; set; }

    /// <summary>
    /// Indicates if this entry is a reversal
    /// </summary>
    public bool IsReversal { get; set; } = false;

    /// <summary>
    /// Navigation property to journal entry lines
    /// </summary>
    public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();

    /// <summary>
    /// Checks if the journal entry is balanced
    /// </summary>
    public bool IsBalanced => Math.Abs(TotalDebit - TotalCredit) < 0.01m;

    /// <summary>
    /// Checks if the journal entry is posted
    /// </summary>
    public bool IsPosted => EntryStatus == Status.Completed && PostedDate.HasValue;
}

/// <summary>
/// Represents a line item in a journal entry
/// </summary>
public class JournalEntryLine : BaseEntity
{
    /// <summary>
    /// Journal entry ID
    /// </summary>
    public Guid JournalEntryId { get; set; }

    /// <summary>
    /// Account ID
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// Line number
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// Description for this line
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Debit amount
    /// </summary>
    public decimal DebitAmount { get; set; }

    /// <summary>
    /// Credit amount
    /// </summary>
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// Reference information
    /// </summary>
    [MaxLength(200)]
    public string? Reference { get; set; }

    /// <summary>
    /// Cost center
    /// </summary>
    [MaxLength(50)]
    public string? CostCenter { get; set; }

    /// <summary>
    /// Project code
    /// </summary>
    [MaxLength(50)]
    public string? ProjectCode { get; set; }

    /// <summary>
    /// Navigation property to journal entry
    /// </summary>
    public virtual JournalEntry JournalEntry { get; set; } = null!;

    /// <summary>
    /// Navigation property to account
    /// </summary>
    public virtual Account Account { get; set; } = null!;

    /// <summary>
    /// Gets the net amount (debit - credit)
    /// </summary>
    public decimal NetAmount => DebitAmount - CreditAmount;
}

/// <summary>
/// Represents a financial period
/// </summary>
public class FinancialPeriod : AuditableEntity
{
    /// <summary>
    /// Period name
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PeriodName { get; set; } = string.Empty;

    /// <summary>
    /// Financial year
    /// </summary>
    public int FinancialYear { get; set; }

    /// <summary>
    /// Period start date
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Period end date
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Period status
    /// </summary>
    public Status PeriodStatus { get; set; } = Status.Active;

    /// <summary>
    /// Indicates if this is the current period
    /// </summary>
    public bool IsCurrentPeriod { get; set; } = false;

    /// <summary>
    /// Period close date
    /// </summary>
    public DateTime? CloseDate { get; set; }

    /// <summary>
    /// Closed by user
    /// </summary>
    [MaxLength(100)]
    public string? ClosedBy { get; set; }

    /// <summary>
    /// Checks if the period is closed
    /// </summary>
    public bool IsClosed => PeriodStatus == Status.Closed && CloseDate.HasValue;

    /// <summary>
    /// Checks if the period is active
    /// </summary>
    public bool IsActive => PeriodStatus == Status.Active;
}

/// <summary>
/// Represents a budget entry
/// </summary>
public class Budget : AuditableEntity
{
    /// <summary>
    /// Budget name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string BudgetName { get; set; } = string.Empty;

    /// <summary>
    /// Budget description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Financial year
    /// </summary>
    public int FinancialYear { get; set; }

    /// <summary>
    /// Budget start date
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Budget end date
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Budget status
    /// </summary>
    public Status BudgetStatus { get; set; } = Status.Draft;

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
    /// Navigation property to budget lines
    /// </summary>
    public virtual ICollection<BudgetLine> BudgetLines { get; set; } = new List<BudgetLine>();

    /// <summary>
    /// Gets the total budget amount
    /// </summary>
    public decimal TotalBudgetAmount => BudgetLines.Sum(bl => bl.BudgetAmount);
}

/// <summary>
/// Represents a budget line item
/// </summary>
public class BudgetLine : BaseEntity
{
    /// <summary>
    /// Budget ID
    /// </summary>
    public Guid BudgetId { get; set; }

    /// <summary>
    /// Account ID
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// Budget amount
    /// </summary>
    public decimal BudgetAmount { get; set; }

    /// <summary>
    /// Actual amount spent
    /// </summary>
    public decimal ActualAmount { get; set; }

    /// <summary>
    /// Variance (Budget - Actual)
    /// </summary>
    public decimal Variance => BudgetAmount - ActualAmount;

    /// <summary>
    /// Variance percentage
    /// </summary>
    public decimal VariancePercentage => BudgetAmount != 0 ? (Variance / BudgetAmount) * 100 : 0;

    /// <summary>
    /// Notes
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Navigation property to budget
    /// </summary>
    public virtual Budget Budget { get; set; } = null!;

    /// <summary>
    /// Navigation property to account
    /// </summary>
    public virtual Account Account { get; set; } = null!;
}
