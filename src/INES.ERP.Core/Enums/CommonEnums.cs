namespace INES.ERP.Core.Enums;

/// <summary>
/// General status enumeration
/// </summary>
public enum Status
{
    Active = 1,
    Inactive = 2,
    Pending = 3,
    Approved = 4,
    Rejected = 5,
    Cancelled = 6,
    Completed = 7,
    InProgress = 8,
    Draft = 9,
    Archived = 10,
    Closed = 11,
    Failed = 12
}

/// <summary>
/// Priority levels
/// </summary>
public enum Priority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

/// <summary>
/// Document types
/// </summary>
public enum DocumentType
{
    Invoice = 1,
    Receipt = 2,
    Quotation = 3,
    PurchaseOrder = 4,
    DeliveryNote = 5,
    CreditNote = 6,
    DebitNote = 7,
    Statement = 8,
    Report = 9,
    Contract = 10
}

/// <summary>
/// Frequency types for recurring transactions
/// </summary>
public enum Frequency
{
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
    Quarterly = 4,
    SemiAnnually = 5,
    Annually = 6
}

/// <summary>
/// Academic periods
/// </summary>
public enum AcademicPeriod
{
    Semester1 = 1,
    Semester2 = 2,
    Semester3 = 3,
    AcademicYear = 4,
    ShortCourse = 5
}

/// <summary>
/// Student status
/// </summary>
public enum StudentStatus
{
    Active = 1,
    Inactive = 2,
    Graduated = 3,
    Suspended = 4,
    Transferred = 5,
    Withdrawn = 6
}

/// <summary>
/// Employee status
/// </summary>
public enum EmployeeStatus
{
    Active = 1,
    Inactive = 2,
    OnLeave = 3,
    Terminated = 4,
    Retired = 5,
    Suspended = 6
}
