using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Payroll;

/// <summary>
/// Represents an employee in the system
/// </summary>
public class Employee : AuditableEntity
{
    /// <summary>
    /// Employee number/ID
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string EmployeeNumber { get; set; } = string.Empty;

    /// <summary>
    /// Employee's first name
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Employee's last name
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Employee's middle name
    /// </summary>
    [MaxLength(50)]
    public string? MiddleName { get; set; }

    /// <summary>
    /// Employee's email address
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Employee's phone number
    /// </summary>
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gender
    /// </summary>
    [MaxLength(10)]
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Marital status
    /// </summary>
    [MaxLength(20)]
    public string? MaritalStatus { get; set; }

    /// <summary>
    /// Nationality
    /// </summary>
    [MaxLength(50)]
    public string Nationality { get; set; } = "Rwandan";

    /// <summary>
    /// National ID number
    /// </summary>
    [MaxLength(20)]
    public string? NationalId { get; set; }

    /// <summary>
    /// Passport number
    /// </summary>
    [MaxLength(20)]
    public string? PassportNumber { get; set; }

    /// <summary>
    /// Job title
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string JobTitle { get; set; } = string.Empty;

    /// <summary>
    /// Department
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
    /// Employee category (Academic, Administrative, Support)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string EmployeeCategory { get; set; } = string.Empty;

    /// <summary>
    /// Employment type (Permanent, Contract, Part-time)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string EmploymentType { get; set; } = string.Empty;

    /// <summary>
    /// Employment status
    /// </summary>
    public Status EmploymentStatus { get; set; } = Status.Active;

    /// <summary>
    /// Hire date
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// Contract start date
    /// </summary>
    public DateTime? ContractStartDate { get; set; }

    /// <summary>
    /// Contract end date
    /// </summary>
    public DateTime? ContractEndDate { get; set; }

    /// <summary>
    /// Termination date
    /// </summary>
    public DateTime? TerminationDate { get; set; }

    /// <summary>
    /// Basic salary
    /// </summary>
    public decimal BasicSalary { get; set; }

    /// <summary>
    /// Salary currency
    /// </summary>
    [MaxLength(3)]
    public string SalaryCurrency { get; set; } = "RWF";

    /// <summary>
    /// Pay frequency (Monthly, Bi-weekly, etc.)
    /// </summary>
    [MaxLength(20)]
    public string PayFrequency { get; set; } = "Monthly";

    /// <summary>
    /// Bank account number
    /// </summary>
    [MaxLength(50)]
    public string? BankAccountNumber { get; set; }

    /// <summary>
    /// Bank name
    /// </summary>
    [MaxLength(100)]
    public string? BankName { get; set; }

    /// <summary>
    /// Bank branch
    /// </summary>
    [MaxLength(100)]
    public string? BankBranch { get; set; }

    /// <summary>
    /// Tax identification number
    /// </summary>
    [MaxLength(50)]
    public string? TaxId { get; set; }

    /// <summary>
    /// Social security number
    /// </summary>
    [MaxLength(50)]
    public string? SocialSecurityNumber { get; set; }

    /// <summary>
    /// Emergency contact name
    /// </summary>
    [MaxLength(100)]
    public string? EmergencyContactName { get; set; }

    /// <summary>
    /// Emergency contact phone
    /// </summary>
    [MaxLength(20)]
    public string? EmergencyContactPhone { get; set; }

    /// <summary>
    /// Employee photo URL
    /// </summary>
    [MaxLength(500)]
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Navigation property for payroll records
    /// </summary>
    public virtual ICollection<PayrollRecord> PayrollRecords { get; set; } = new List<PayrollRecord>();

    /// <summary>
    /// Navigation property for leave records
    /// </summary>
    public virtual ICollection<LeaveRecord> LeaveRecords { get; set; } = new List<LeaveRecord>();

    /// <summary>
    /// Gets the employee's full name
    /// </summary>
    public string FullName
    {
        get
        {
            var parts = new List<string> { FirstName };
            
            if (!string.IsNullOrWhiteSpace(MiddleName))
                parts.Add(MiddleName);
            
            parts.Add(LastName);
            
            return string.Join(" ", parts);
        }
    }

    /// <summary>
    /// Gets the employee's display name
    /// </summary>
    public string DisplayName => $"{EmployeeNumber} - {FullName}";

    /// <summary>
    /// Calculates the employee's age
    /// </summary>
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    /// <summary>
    /// Calculates years of service
    /// </summary>
    public int YearsOfService
    {
        get
        {
            var endDate = TerminationDate ?? DateTime.Today;
            var years = endDate.Year - HireDate.Year;
            if (HireDate.Date > endDate.AddYears(-years)) years--;
            return Math.Max(0, years);
        }
    }

    /// <summary>
    /// Checks if the employee is currently employed
    /// </summary>
    public bool IsCurrentlyEmployed => EmploymentStatus == Status.Active && !TerminationDate.HasValue;
}

/// <summary>
/// Represents a payroll record for an employee
/// </summary>
public class PayrollRecord : AuditableEntity
{
    /// <summary>
    /// Employee ID
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Payroll period ID
    /// </summary>
    public Guid PayrollPeriodId { get; set; }

    /// <summary>
    /// Basic salary for this period
    /// </summary>
    public decimal BasicSalary { get; set; }

    /// <summary>
    /// Total allowances
    /// </summary>
    public decimal TotalAllowances { get; set; }

    /// <summary>
    /// Total deductions
    /// </summary>
    public decimal TotalDeductions { get; set; }

    /// <summary>
    /// Gross pay (Basic + Allowances)
    /// </summary>
    public decimal GrossPay => BasicSalary + TotalAllowances;

    /// <summary>
    /// Net pay (Gross - Deductions)
    /// </summary>
    public decimal NetPay => GrossPay - TotalDeductions;

    /// <summary>
    /// Tax amount
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Social security contribution
    /// </summary>
    public decimal SocialSecurityContribution { get; set; }

    /// <summary>
    /// Other statutory deductions
    /// </summary>
    public decimal OtherStatutoryDeductions { get; set; }

    /// <summary>
    /// Payroll status
    /// </summary>
    public Status PayrollStatus { get; set; } = Status.Draft;

    /// <summary>
    /// Payment date
    /// </summary>
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// Payment method
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.BankTransfer;

    /// <summary>
    /// Payment reference
    /// </summary>
    [MaxLength(100)]
    public string? PaymentReference { get; set; }

    /// <summary>
    /// Navigation property to employee
    /// </summary>
    public virtual Employee Employee { get; set; } = null!;

    /// <summary>
    /// Navigation property to payroll period
    /// </summary>
    public virtual PayrollPeriod PayrollPeriod { get; set; } = null!;

    /// <summary>
    /// Navigation property to payroll details
    /// </summary>
    public virtual ICollection<PayrollDetail> PayrollDetails { get; set; } = new List<PayrollDetail>();
}

/// <summary>
/// Represents detailed payroll items (allowances, deductions)
/// </summary>
public class PayrollDetail : BaseEntity
{
    /// <summary>
    /// Payroll record ID
    /// </summary>
    public Guid PayrollRecordId { get; set; }

    /// <summary>
    /// Item type (Allowance, Deduction)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string ItemType { get; set; } = string.Empty;

    /// <summary>
    /// Item code
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// Item description
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Calculation basis (Fixed, Percentage, etc.)
    /// </summary>
    [MaxLength(20)]
    public string? CalculationBasis { get; set; }

    /// <summary>
    /// Rate (for percentage-based calculations)
    /// </summary>
    public decimal? Rate { get; set; }

    /// <summary>
    /// Navigation property to payroll record
    /// </summary>
    public virtual PayrollRecord PayrollRecord { get; set; } = null!;
}

/// <summary>
/// Represents a payroll period
/// </summary>
public class PayrollPeriod : AuditableEntity
{
    /// <summary>
    /// Period name
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PeriodName { get; set; } = string.Empty;

    /// <summary>
    /// Period start date
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Period end date
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Pay date
    /// </summary>
    public DateTime PayDate { get; set; }

    /// <summary>
    /// Period status
    /// </summary>
    public Status PeriodStatus { get; set; } = Status.Draft;

    /// <summary>
    /// Processed date
    /// </summary>
    public DateTime? ProcessedDate { get; set; }

    /// <summary>
    /// Processed by
    /// </summary>
    [MaxLength(100)]
    public string? ProcessedBy { get; set; }

    /// <summary>
    /// Navigation property to payroll records
    /// </summary>
    public virtual ICollection<PayrollRecord> PayrollRecords { get; set; } = new List<PayrollRecord>();

    /// <summary>
    /// Checks if the period is processed
    /// </summary>
    public bool IsProcessed => PeriodStatus == Status.Completed && ProcessedDate.HasValue;
}

/// <summary>
/// Represents a leave record
/// </summary>
public class LeaveRecord : AuditableEntity
{
    /// <summary>
    /// Employee ID
    /// </summary>
    public Guid EmployeeId { get; set; }

    /// <summary>
    /// Leave type (Annual, Sick, Maternity, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LeaveType { get; set; } = string.Empty;

    /// <summary>
    /// Leave start date
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Leave end date
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Number of days
    /// </summary>
    public int NumberOfDays { get; set; }

    /// <summary>
    /// Reason for leave
    /// </summary>
    [MaxLength(500)]
    public string? Reason { get; set; }

    /// <summary>
    /// Leave status
    /// </summary>
    public Status LeaveStatus { get; set; } = Status.Pending;

    /// <summary>
    /// Applied date
    /// </summary>
    public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

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
    /// Comments
    /// </summary>
    [MaxLength(500)]
    public string? Comments { get; set; }

    /// <summary>
    /// Navigation property to employee
    /// </summary>
    public virtual Employee Employee { get; set; } = null!;

    /// <summary>
    /// Checks if leave is approved
    /// </summary>
    public bool IsApproved => LeaveStatus == Status.Approved && ApprovalDate.HasValue;
}
