using INES.ERP.Core.Models.StudentAccounts;
using INES.ERP.Core.Models.Common;
using INES.ERP.Core.Enums;

namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Student accounts service interface
/// </summary>
public interface IStudentAccountsService
{
    #region Student Management

    /// <summary>
    /// Creates a new student
    /// </summary>
    /// <param name="student">Student to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created student</returns>
    Task<Student> CreateStudentAsync(Student student, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing student
    /// </summary>
    /// <param name="student">Student to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated student</returns>
    Task<Student> UpdateStudentAsync(Student student, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a student by ID
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Student or null if not found</returns>
    Task<Student?> GetStudentByIdAsync(Guid studentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a student by student number
    /// </summary>
    /// <param name="studentNumber">Student number</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Student or null if not found</returns>
    Task<Student?> GetStudentByNumberAsync(string studentNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets students with pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="searchTerm">Search term</param>
    /// <param name="program">Program filter</param>
    /// <param name="faculty">Faculty filter</param>
    /// <param name="status">Status filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated students</returns>
    Task<PagedResult<Student>> GetStudentsAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? searchTerm = null,
        string? program = null,
        string? faculty = null,
        StudentStatus? status = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Billing Management

    /// <summary>
    /// Creates billing for a student
    /// </summary>
    /// <param name="billing">Billing record to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created billing record</returns>
    Task<StudentBilling> CreateBillingAsync(StudentBilling billing, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets billing records for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="academicYear">Optional academic year filter</param>
    /// <param name="semester">Optional semester filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Billing records</returns>
    Task<IEnumerable<StudentBilling>> GetStudentBillingAsync(
        Guid studentId,
        int? academicYear = null,
        int? semester = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Bulk creates billing for multiple students
    /// </summary>
    /// <param name="studentIds">Student IDs</param>
    /// <param name="feeType">Fee type</param>
    /// <param name="amount">Amount</param>
    /// <param name="academicYear">Academic year</param>
    /// <param name="semester">Semester</param>
    /// <param name="dueDate">Due date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of billing records created</returns>
    Task<int> BulkCreateBillingAsync(
        IEnumerable<Guid> studentIds,
        string feeType,
        decimal amount,
        int academicYear,
        int semester,
        DateTime dueDate,
        CancellationToken cancellationToken = default);

    #endregion

    #region Payment Management

    /// <summary>
    /// Records a payment for a student
    /// </summary>
    /// <param name="payment">Payment to record</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Recorded payment</returns>
    Task<StudentPayment> RecordPaymentAsync(StudentPayment payment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets payments for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="academicYear">Optional academic year filter</param>
    /// <param name="semester">Optional semester filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment records</returns>
    Task<IEnumerable<StudentPayment>> GetStudentPaymentsAsync(
        Guid studentId,
        int? academicYear = null,
        int? semester = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets payment by reference number
    /// </summary>
    /// <param name="paymentReference">Payment reference</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment or null if not found</returns>
    Task<StudentPayment?> GetPaymentByReferenceAsync(string paymentReference, CancellationToken cancellationToken = default);

    #endregion

    #region Penalty Management

    /// <summary>
    /// Applies a penalty to a student
    /// </summary>
    /// <param name="penalty">Penalty to apply</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Applied penalty</returns>
    Task<StudentPenalty> ApplyPenaltyAsync(StudentPenalty penalty, CancellationToken cancellationToken = default);

    /// <summary>
    /// Waives a penalty
    /// </summary>
    /// <param name="penaltyId">Penalty ID</param>
    /// <param name="reason">Waiver reason</param>
    /// <param name="waivedBy">User who waived the penalty</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> WaivePenaltyAsync(Guid penaltyId, string reason, string waivedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets penalties for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="includeWaived">Include waived penalties</param>
    /// <param name="includePaid">Include paid penalties</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Penalty records</returns>
    Task<IEnumerable<StudentPenalty>> GetStudentPenaltiesAsync(
        Guid studentId,
        bool includeWaived = false,
        bool includePaid = false,
        CancellationToken cancellationToken = default);

    #endregion

    #region Account Summary

    /// <summary>
    /// Gets account summary for a student
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Account summary</returns>
    Task<StudentAccountSummary> GetAccountSummaryAsync(Guid studentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets students with outstanding balances
    /// </summary>
    /// <param name="minimumAmount">Minimum outstanding amount</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Students with outstanding balances</returns>
    Task<IEnumerable<StudentAccountSummary>> GetStudentsWithOutstandingBalancesAsync(
        decimal minimumAmount = 0,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a student statement
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <param name="fromDate">Statement start date</param>
    /// <param name="toDate">Statement end date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Student statement</returns>
    Task<StudentStatement> GenerateStatementAsync(
        Guid studentId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    #endregion
}

/// <summary>
/// Student account summary
/// </summary>
public class StudentAccountSummary
{
    /// <summary>
    /// Student information
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// Total amount billed
    /// </summary>
    public decimal TotalBilled { get; set; }

    /// <summary>
    /// Total amount paid
    /// </summary>
    public decimal TotalPaid { get; set; }

    /// <summary>
    /// Outstanding balance
    /// </summary>
    public decimal OutstandingBalance => TotalBilled - TotalPaid + TotalPenalties;

    /// <summary>
    /// Total penalties
    /// </summary>
    public decimal TotalPenalties { get; set; }

    /// <summary>
    /// Last payment date
    /// </summary>
    public DateTime? LastPaymentDate { get; set; }

    /// <summary>
    /// Last payment amount
    /// </summary>
    public decimal? LastPaymentAmount { get; set; }

    /// <summary>
    /// Number of overdue items
    /// </summary>
    public int OverdueItems { get; set; }

    /// <summary>
    /// Current academic year
    /// </summary>
    public int CurrentAcademicYear { get; set; }

    /// <summary>
    /// Current semester
    /// </summary>
    public int CurrentSemester { get; set; }
}

/// <summary>
/// Student statement
/// </summary>
public class StudentStatement
{
    /// <summary>
    /// Student information
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// Statement period start
    /// </summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>
    /// Statement period end
    /// </summary>
    public DateTime PeriodEnd { get; set; }

    /// <summary>
    /// Opening balance
    /// </summary>
    public decimal OpeningBalance { get; set; }

    /// <summary>
    /// Billing transactions
    /// </summary>
    public IEnumerable<StudentBilling> BillingTransactions { get; set; } = new List<StudentBilling>();

    /// <summary>
    /// Payment transactions
    /// </summary>
    public IEnumerable<StudentPayment> PaymentTransactions { get; set; } = new List<StudentPayment>();

    /// <summary>
    /// Penalty transactions
    /// </summary>
    public IEnumerable<StudentPenalty> PenaltyTransactions { get; set; } = new List<StudentPenalty>();

    /// <summary>
    /// Closing balance
    /// </summary>
    public decimal ClosingBalance { get; set; }

    /// <summary>
    /// Statement generation date
    /// </summary>
    public DateTime GeneratedDate { get; set; } = DateTime.UtcNow;
}
