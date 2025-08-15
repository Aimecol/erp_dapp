using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.ProjectAccounts;

/// <summary>
/// Represents a project in the system
/// </summary>
public class Project : AuditableEntity
{
    /// <summary>
    /// Project code/number
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string ProjectCode { get; set; } = string.Empty;

    /// <summary>
    /// Project title
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Project description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Project type (Research, Infrastructure, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ProjectType { get; set; } = string.Empty;

    /// <summary>
    /// Project category
    /// </summary>
    [MaxLength(50)]
    public string? Category { get; set; }

    /// <summary>
    /// Principal investigator or project manager
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string PrincipalInvestigator { get; set; } = string.Empty;

    /// <summary>
    /// Co-investigators
    /// </summary>
    [MaxLength(500)]
    public string? CoInvestigators { get; set; }

    /// <summary>
    /// Department responsible for the project
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// Faculty or school
    /// </summary>
    [MaxLength(100)]
    public string? Faculty { get; set; }

    /// <summary>
    /// Funding source/donor
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string FundingSource { get; set; } = string.Empty;

    /// <summary>
    /// Grant reference number
    /// </summary>
    [MaxLength(50)]
    public string? GrantReference { get; set; }

    /// <summary>
    /// Total project budget
    /// </summary>
    public decimal TotalBudget { get; set; }

    /// <summary>
    /// Currency of the budget
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "RWF";

    /// <summary>
    /// Project start date
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Project end date
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Actual start date
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// Actual end date
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// Project status
    /// </summary>
    public Status ProjectStatus { get; set; } = Status.Draft;

    /// <summary>
    /// Approval date
    /// </summary>
    public DateTime? ApprovalDate { get; set; }

    /// <summary>
    /// Approved by
    /// </summary>
    [MaxLength(100)]
    public string? ApprovedBy { get; set; }

    /// <summary>
    /// Contract start date
    /// </summary>
    public DateTime? ContractStartDate { get; set; }

    /// <summary>
    /// Contract end date
    /// </summary>
    public DateTime? ContractEndDate { get; set; }

    /// <summary>
    /// Reporting frequency
    /// </summary>
    public Frequency ReportingFrequency { get; set; } = Frequency.Quarterly;

    /// <summary>
    /// Next reporting date
    /// </summary>
    public DateTime? NextReportingDate { get; set; }

    /// <summary>
    /// Project objectives
    /// </summary>
    [MaxLength(2000)]
    public string? Objectives { get; set; }

    /// <summary>
    /// Expected outcomes
    /// </summary>
    [MaxLength(2000)]
    public string? ExpectedOutcomes { get; set; }

    /// <summary>
    /// Navigation property for project milestones
    /// </summary>
    public virtual ICollection<ProjectMilestone> Milestones { get; set; } = new List<ProjectMilestone>();

    /// <summary>
    /// Navigation property for project disbursements
    /// </summary>
    public virtual ICollection<ProjectDisbursement> Disbursements { get; set; } = new List<ProjectDisbursement>();

    /// <summary>
    /// Navigation property for project expenses
    /// </summary>
    public virtual ICollection<ProjectExpense> Expenses { get; set; } = new List<ProjectExpense>();

    /// <summary>
    /// Navigation property for project reports
    /// </summary>
    public virtual ICollection<ProjectReport> Reports { get; set; } = new List<ProjectReport>();

    /// <summary>
    /// Gets the project duration in days
    /// </summary>
    public int DurationInDays => (EndDate - StartDate).Days;

    /// <summary>
    /// Gets the project progress percentage
    /// </summary>
    public decimal ProgressPercentage
    {
        get
        {
            if (ProjectStatus == Status.Completed) return 100;
            if (ProjectStatus == Status.Draft || ProjectStatus == Status.Pending) return 0;

            var totalDays = DurationInDays;
            if (totalDays <= 0) return 0;

            var elapsedDays = (DateTime.Today - StartDate).Days;
            if (elapsedDays <= 0) return 0;
            if (elapsedDays >= totalDays) return 100;

            return Math.Round((decimal)elapsedDays / totalDays * 100, 2);
        }
    }

    /// <summary>
    /// Checks if the project is active
    /// </summary>
    public bool IsActive => ProjectStatus == Status.Active || ProjectStatus == Status.InProgress;

    /// <summary>
    /// Checks if the project is overdue
    /// </summary>
    public bool IsOverdue => EndDate < DateTime.Today && ProjectStatus != Status.Completed;
}

/// <summary>
/// Represents a project milestone
/// </summary>
public class ProjectMilestone : AuditableEntity
{
    /// <summary>
    /// Project ID
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Milestone number
    /// </summary>
    public int MilestoneNumber { get; set; }

    /// <summary>
    /// Milestone title
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Milestone description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Planned start date
    /// </summary>
    public DateTime PlannedStartDate { get; set; }

    /// <summary>
    /// Planned end date
    /// </summary>
    public DateTime PlannedEndDate { get; set; }

    /// <summary>
    /// Actual start date
    /// </summary>
    public DateTime? ActualStartDate { get; set; }

    /// <summary>
    /// Actual end date
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// Milestone budget
    /// </summary>
    public decimal Budget { get; set; }

    /// <summary>
    /// Disbursement percentage for this milestone
    /// </summary>
    public decimal DisbursementPercentage { get; set; }

    /// <summary>
    /// Milestone status
    /// </summary>
    public Status MilestoneStatus { get; set; } = Status.Pending;

    /// <summary>
    /// Completion percentage
    /// </summary>
    public decimal CompletionPercentage { get; set; }

    /// <summary>
    /// Deliverables for this milestone
    /// </summary>
    [MaxLength(1000)]
    public string? Deliverables { get; set; }

    /// <summary>
    /// Navigation property to project
    /// </summary>
    public virtual Project Project { get; set; } = null!;

    /// <summary>
    /// Checks if the milestone is overdue
    /// </summary>
    public bool IsOverdue => PlannedEndDate < DateTime.Today && MilestoneStatus != Status.Completed;
}

/// <summary>
/// Represents a project disbursement
/// </summary>
public class ProjectDisbursement : AuditableEntity
{
    /// <summary>
    /// Project ID
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Milestone ID (if linked to a milestone)
    /// </summary>
    public Guid? MilestoneId { get; set; }

    /// <summary>
    /// Disbursement reference number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string DisbursementReference { get; set; } = string.Empty;

    /// <summary>
    /// Disbursement amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Disbursement date
    /// </summary>
    public DateTime DisbursementDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Disbursement type (Initial, Milestone, Final, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string DisbursementType { get; set; } = string.Empty;

    /// <summary>
    /// Description or purpose
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Bank reference
    /// </summary>
    [MaxLength(100)]
    public string? BankReference { get; set; }

    /// <summary>
    /// Disbursement status
    /// </summary>
    public Status DisbursementStatus { get; set; } = Status.Pending;

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
    /// Navigation property to project
    /// </summary>
    public virtual Project Project { get; set; } = null!;

    /// <summary>
    /// Navigation property to milestone
    /// </summary>
    public virtual ProjectMilestone? Milestone { get; set; }
}

/// <summary>
/// Represents a project expense
/// </summary>
public class ProjectExpense : AuditableEntity
{
    /// <summary>
    /// Project ID
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Expense reference number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ExpenseReference { get; set; } = string.Empty;

    /// <summary>
    /// Expense category
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Expense description
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Expense amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Expense date
    /// </summary>
    public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Vendor or supplier
    /// </summary>
    [MaxLength(100)]
    public string? Vendor { get; set; }

    /// <summary>
    /// Invoice number
    /// </summary>
    [MaxLength(50)]
    public string? InvoiceNumber { get; set; }

    /// <summary>
    /// Receipt number
    /// </summary>
    [MaxLength(50)]
    public string? ReceiptNumber { get; set; }

    /// <summary>
    /// Payment method
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Expense status
    /// </summary>
    public Status ExpenseStatus { get; set; } = Status.Pending;

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
    /// Navigation property to project
    /// </summary>
    public virtual Project Project { get; set; } = null!;
}

/// <summary>
/// Represents a project report
/// </summary>
public class ProjectReport : AuditableEntity
{
    /// <summary>
    /// Project ID
    /// </summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// Report type (Progress, Financial, Final, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ReportType { get; set; } = string.Empty;

    /// <summary>
    /// Report title
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Reporting period start
    /// </summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>
    /// Reporting period end
    /// </summary>
    public DateTime PeriodEnd { get; set; }

    /// <summary>
    /// Report content
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Report file path
    /// </summary>
    [MaxLength(500)]
    public string? FilePath { get; set; }

    /// <summary>
    /// Report status
    /// </summary>
    public Status ReportStatus { get; set; } = Status.Draft;

    /// <summary>
    /// Submitted date
    /// </summary>
    public DateTime? SubmittedDate { get; set; }

    /// <summary>
    /// Submitted by
    /// </summary>
    [MaxLength(100)]
    public string? SubmittedBy { get; set; }

    /// <summary>
    /// Due date
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Navigation property to project
    /// </summary>
    public virtual Project Project { get; set; } = null!;

    /// <summary>
    /// Checks if the report is overdue
    /// </summary>
    public bool IsOverdue => DueDate < DateTime.Today && ReportStatus != Status.Completed;
}
