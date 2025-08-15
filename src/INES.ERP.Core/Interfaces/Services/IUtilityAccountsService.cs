using INES.ERP.Core.Models.UtilityAccounts;
using INES.ERP.Core.Models.Common;
using INES.ERP.Core.Enums;

namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Utility accounts service interface
/// </summary>
public interface IUtilityAccountsService
{
    #region Utility Bills

    /// <summary>
    /// Creates a new utility bill
    /// </summary>
    /// <param name="utilityBill">Utility bill to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created utility bill</returns>
    Task<UtilityBill> CreateUtilityBillAsync(UtilityBill utilityBill, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing utility bill
    /// </summary>
    /// <param name="utilityBill">Utility bill to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated utility bill</returns>
    Task<UtilityBill> UpdateUtilityBillAsync(UtilityBill utilityBill, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a utility bill by ID
    /// </summary>
    /// <param name="billId">Bill ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Utility bill or null if not found</returns>
    Task<UtilityBill?> GetUtilityBillByIdAsync(Guid billId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets utility bills with pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="utilityType">Utility type filter</param>
    /// <param name="provider">Provider filter</param>
    /// <param name="status">Status filter</param>
    /// <param name="fromDate">From date filter</param>
    /// <param name="toDate">To date filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated utility bills</returns>
    Task<PagedResult<UtilityBill>> GetUtilityBillsAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? utilityType = null,
        string? provider = null,
        Status? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes utility bill payment
    /// </summary>
    /// <param name="billId">Bill ID</param>
    /// <param name="paymentReference">Payment reference</param>
    /// <param name="paymentDate">Payment date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> ProcessUtilityPaymentAsync(Guid billId, string paymentReference, DateTime paymentDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Allocates utility bill to departments
    /// </summary>
    /// <param name="billId">Bill ID</param>
    /// <param name="allocations">Department allocations</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> AllocateUtilityBillAsync(Guid billId, IEnumerable<UtilityAllocation> allocations, CancellationToken cancellationToken = default);

    #endregion

    #region Vendor Invoices

    /// <summary>
    /// Creates a new vendor invoice
    /// </summary>
    /// <param name="vendorInvoice">Vendor invoice to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created vendor invoice</returns>
    Task<VendorInvoice> CreateVendorInvoiceAsync(VendorInvoice vendorInvoice, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing vendor invoice
    /// </summary>
    /// <param name="vendorInvoice">Vendor invoice to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated vendor invoice</returns>
    Task<VendorInvoice> UpdateVendorInvoiceAsync(VendorInvoice vendorInvoice, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a vendor invoice by ID
    /// </summary>
    /// <param name="invoiceId">Invoice ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Vendor invoice or null if not found</returns>
    Task<VendorInvoice?> GetVendorInvoiceByIdAsync(Guid invoiceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets vendor invoices with pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="vendorName">Vendor name filter</param>
    /// <param name="status">Status filter</param>
    /// <param name="fromDate">From date filter</param>
    /// <param name="toDate">To date filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated vendor invoices</returns>
    Task<PagedResult<VendorInvoice>> GetVendorInvoicesAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? vendorName = null,
        Status? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Approves a vendor invoice
    /// </summary>
    /// <param name="invoiceId">Invoice ID</param>
    /// <param name="approvedBy">User who approved</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> ApproveVendorInvoiceAsync(Guid invoiceId, string approvedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes vendor invoice payment
    /// </summary>
    /// <param name="invoiceId">Invoice ID</param>
    /// <param name="paymentReference">Payment reference</param>
    /// <param name="paymentDate">Payment date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> ProcessVendorPaymentAsync(Guid invoiceId, string paymentReference, DateTime paymentDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads vendor invoice document
    /// </summary>
    /// <param name="invoiceId">Invoice ID</param>
    /// <param name="documentPath">Document file path</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> UploadInvoiceDocumentAsync(Guid invoiceId, string documentPath, CancellationToken cancellationToken = default);

    #endregion

    #region Recurring Payments

    /// <summary>
    /// Creates a new recurring payment
    /// </summary>
    /// <param name="recurringPayment">Recurring payment to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created recurring payment</returns>
    Task<RecurringPayment> CreateRecurringPaymentAsync(RecurringPayment recurringPayment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing recurring payment
    /// </summary>
    /// <param name="recurringPayment">Recurring payment to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated recurring payment</returns>
    Task<RecurringPayment> UpdateRecurringPaymentAsync(RecurringPayment recurringPayment, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a recurring payment by ID
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Recurring payment or null if not found</returns>
    Task<RecurringPayment?> GetRecurringPaymentByIdAsync(Guid paymentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets recurring payments with pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="payeeName">Payee name filter</param>
    /// <param name="status">Status filter</param>
    /// <param name="frequency">Frequency filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated recurring payments</returns>
    Task<PagedResult<RecurringPayment>> GetRecurringPaymentsAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? payeeName = null,
        Status? status = null,
        Frequency? frequency = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets due recurring payments
    /// </summary>
    /// <param name="dueDate">Due date (default: today)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Due recurring payments</returns>
    Task<IEnumerable<RecurringPayment>> GetDueRecurringPaymentsAsync(DateTime? dueDate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes a recurring payment
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <param name="paymentReference">Payment reference</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> ProcessRecurringPaymentAsync(Guid paymentId, string paymentReference, CancellationToken cancellationToken = default);

    /// <summary>
    /// Suspends a recurring payment
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> SuspendRecurringPaymentAsync(Guid paymentId, CancellationToken cancellationToken = default);

    #endregion

    #region Reporting

    /// <summary>
    /// Gets utility consumption report
    /// </summary>
    /// <param name="utilityType">Utility type</param>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Consumption report</returns>
    Task<UtilityConsumptionReport> GetUtilityConsumptionReportAsync(
        string utilityType,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets vendor payment summary
    /// </summary>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Payment summary</returns>
    Task<VendorPaymentSummary> GetVendorPaymentSummaryAsync(
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets departmental allocation report
    /// </summary>
    /// <param name="department">Department filter</param>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Allocation report</returns>
    Task<DepartmentalAllocationReport> GetDepartmentalAllocationReportAsync(
        string? department,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default);

    #endregion
}

/// <summary>
/// Utility consumption report
/// </summary>
public class UtilityConsumptionReport
{
    public string UtilityType { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal TotalConsumption { get; set; }
    public decimal TotalCost { get; set; }
    public decimal AverageMonthlyConsumption { get; set; }
    public decimal AverageMonthlyCost { get; set; }
    public List<MonthlyConsumption> MonthlyBreakdown { get; set; } = new();
}

/// <summary>
/// Monthly consumption data
/// </summary>
public class MonthlyConsumption
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Consumption { get; set; }
    public decimal Cost { get; set; }
    public string MonthName => new DateTime(Year, Month, 1).ToString("MMM yyyy");
}

/// <summary>
/// Vendor payment summary
/// </summary>
public class VendorPaymentSummary
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal TotalInvoiced { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal OutstandingAmount => TotalInvoiced - TotalPaid;
    public int TotalInvoices { get; set; }
    public int PaidInvoices { get; set; }
    public int PendingInvoices => TotalInvoices - PaidInvoices;
    public List<VendorSummary> VendorBreakdown { get; set; } = new();
}

/// <summary>
/// Vendor summary data
/// </summary>
public class VendorSummary
{
    public string VendorName { get; set; } = string.Empty;
    public decimal TotalInvoiced { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal Outstanding => TotalInvoiced - TotalPaid;
    public int InvoiceCount { get; set; }
}

/// <summary>
/// Departmental allocation report
/// </summary>
public class DepartmentalAllocationReport
{
    public string? Department { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal TotalAllocated { get; set; }
    public List<DepartmentAllocation> DepartmentBreakdown { get; set; } = new();
    public List<UtilityTypeAllocation> UtilityTypeBreakdown { get; set; } = new();
}

/// <summary>
/// Department allocation data
/// </summary>
public class DepartmentAllocation
{
    public string DepartmentName { get; set; } = string.Empty;
    public decimal TotalAllocated { get; set; }
    public decimal Percentage { get; set; }
}

/// <summary>
/// Utility type allocation data
/// </summary>
public class UtilityTypeAllocation
{
    public string UtilityType { get; set; } = string.Empty;
    public decimal TotalAllocated { get; set; }
    public decimal Percentage { get; set; }
}
