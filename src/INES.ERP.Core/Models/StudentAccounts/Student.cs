using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.StudentAccounts;

/// <summary>
/// Represents a student in the system
/// </summary>
public class Student : AuditableEntity
{
    /// <summary>
    /// Student registration number
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string StudentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Student's first name
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Student's last name
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Student's middle name
    /// </summary>
    [MaxLength(50)]
    public string? MiddleName { get; set; }

    /// <summary>
    /// Student's email address
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Student's phone number
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
    /// Program of study
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Program { get; set; } = string.Empty;

    /// <summary>
    /// Faculty or school
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Faculty { get; set; } = string.Empty;

    /// <summary>
    /// Department
    /// </summary>
    [MaxLength(100)]
    public string? Department { get; set; }

    /// <summary>
    /// Academic year of enrollment
    /// </summary>
    public int EnrollmentYear { get; set; }

    /// <summary>
    /// Current academic year
    /// </summary>
    public int CurrentYear { get; set; }

    /// <summary>
    /// Current semester
    /// </summary>
    public int CurrentSemester { get; set; }

    /// <summary>
    /// Student status
    /// </summary>
    public StudentStatus StudentStatus { get; set; } = StudentStatus.Active;

    /// <summary>
    /// Enrollment date
    /// </summary>
    public DateTime EnrollmentDate { get; set; }

    /// <summary>
    /// Expected graduation date
    /// </summary>
    public DateTime? ExpectedGraduationDate { get; set; }

    /// <summary>
    /// Actual graduation date
    /// </summary>
    public DateTime? GraduationDate { get; set; }

    /// <summary>
    /// Guardian/parent name
    /// </summary>
    [MaxLength(100)]
    public string? GuardianName { get; set; }

    /// <summary>
    /// Guardian/parent phone
    /// </summary>
    [MaxLength(20)]
    public string? GuardianPhone { get; set; }

    /// <summary>
    /// Guardian/parent email
    /// </summary>
    [EmailAddress]
    [MaxLength(100)]
    public string? GuardianEmail { get; set; }

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
    /// Student photo URL
    /// </summary>
    [MaxLength(500)]
    public string? PhotoUrl { get; set; }

    /// <summary>
    /// Navigation property for student billing records
    /// </summary>
    public virtual ICollection<StudentBilling> BillingRecords { get; set; } = new List<StudentBilling>();

    /// <summary>
    /// Navigation property for student payments
    /// </summary>
    public virtual ICollection<StudentPayment> Payments { get; set; } = new List<StudentPayment>();

    /// <summary>
    /// Navigation property for student penalties
    /// </summary>
    public virtual ICollection<StudentPenalty> Penalties { get; set; } = new List<StudentPenalty>();

    /// <summary>
    /// Gets the student's full name
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
    /// Gets the student's display name
    /// </summary>
    public string DisplayName => $"{StudentNumber} - {FullName}";

    /// <summary>
    /// Calculates the student's age
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
    /// Checks if the student is currently enrolled
    /// </summary>
    public bool IsEnrolled => StudentStatus == StudentStatus.Active;

    /// <summary>
    /// Checks if the student has graduated
    /// </summary>
    public bool HasGraduated => StudentStatus == StudentStatus.Graduated && GraduationDate.HasValue;
}

/// <summary>
/// Represents a billing record for a student
/// </summary>
public class StudentBilling : AuditableEntity
{
    /// <summary>
    /// Student ID
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// Billing period (e.g., "2024 Semester 1")
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string BillingPeriod { get; set; } = string.Empty;

    /// <summary>
    /// Academic year
    /// </summary>
    public int AcademicYear { get; set; }

    /// <summary>
    /// Semester
    /// </summary>
    public int Semester { get; set; }

    /// <summary>
    /// Fee type (Tuition, Accommodation, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FeeType { get; set; } = string.Empty;

    /// <summary>
    /// Fee description
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Billed amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Due date for payment
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Billing date
    /// </summary>
    public DateTime BillingDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates if this billing is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property to student
    /// </summary>
    public virtual Student Student { get; set; } = null!;
}

/// <summary>
/// Represents a payment made by a student
/// </summary>
public class StudentPayment : AuditableEntity
{
    /// <summary>
    /// Student ID
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// Payment reference number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PaymentReference { get; set; } = string.Empty;

    /// <summary>
    /// Payment amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment date
    /// </summary>
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Payment method
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Payment description
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Academic year this payment applies to
    /// </summary>
    public int AcademicYear { get; set; }

    /// <summary>
    /// Semester this payment applies to
    /// </summary>
    public int Semester { get; set; }

    /// <summary>
    /// Receipt number
    /// </summary>
    [MaxLength(50)]
    public string? ReceiptNumber { get; set; }

    /// <summary>
    /// Bank reference (for bank transfers)
    /// </summary>
    [MaxLength(100)]
    public string? BankReference { get; set; }

    /// <summary>
    /// Mobile money reference (for mobile payments)
    /// </summary>
    [MaxLength(100)]
    public string? MobileMoneyReference { get; set; }

    /// <summary>
    /// Payment status
    /// </summary>
    public Status PaymentStatus { get; set; } = Status.Completed;

    /// <summary>
    /// Navigation property to student
    /// </summary>
    public virtual Student Student { get; set; } = null!;
}

/// <summary>
/// Represents a penalty applied to a student
/// </summary>
public class StudentPenalty : AuditableEntity
{
    /// <summary>
    /// Student ID
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// Penalty type
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PenaltyType { get; set; } = string.Empty;

    /// <summary>
    /// Penalty description
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Penalty amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Date penalty was applied
    /// </summary>
    public DateTime PenaltyDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Due date for penalty payment
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// Indicates if penalty has been paid
    /// </summary>
    public bool IsPaid { get; set; } = false;

    /// <summary>
    /// Date penalty was paid
    /// </summary>
    public DateTime? PaidDate { get; set; }

    /// <summary>
    /// Payment reference for penalty payment
    /// </summary>
    [MaxLength(50)]
    public string? PaymentReference { get; set; }

    /// <summary>
    /// Indicates if penalty is waived
    /// </summary>
    public bool IsWaived { get; set; } = false;

    /// <summary>
    /// Date penalty was waived
    /// </summary>
    public DateTime? WaivedDate { get; set; }

    /// <summary>
    /// Reason for waiving penalty
    /// </summary>
    [MaxLength(500)]
    public string? WaiverReason { get; set; }

    /// <summary>
    /// User who waived the penalty
    /// </summary>
    [MaxLength(100)]
    public string? WaivedBy { get; set; }

    /// <summary>
    /// Navigation property to student
    /// </summary>
    public virtual Student Student { get; set; } = null!;
}
