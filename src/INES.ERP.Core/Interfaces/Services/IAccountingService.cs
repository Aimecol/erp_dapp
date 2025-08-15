using INES.ERP.Core.Models.Accounting;
using INES.ERP.Core.Models.Common;
using INES.ERP.Core.Enums;

namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Accounting service interface
/// </summary>
public interface IAccountingService
{
    #region Chart of Accounts

    /// <summary>
    /// Creates a new account
    /// </summary>
    /// <param name="account">Account to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created account</returns>
    Task<Account> CreateAccountAsync(Account account, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing account
    /// </summary>
    /// <param name="account">Account to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated account</returns>
    Task<Account> UpdateAccountAsync(Account account, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an account by ID
    /// </summary>
    /// <param name="accountId">Account ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Account or null if not found</returns>
    Task<Account?> GetAccountByIdAsync(Guid accountId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an account by code
    /// </summary>
    /// <param name="accountCode">Account code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Account or null if not found</returns>
    Task<Account?> GetAccountByCodeAsync(string accountCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets chart of accounts
    /// </summary>
    /// <param name="accountType">Account type filter</param>
    /// <param name="includeInactive">Include inactive accounts</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Chart of accounts</returns>
    Task<IEnumerable<Account>> GetChartOfAccountsAsync(
        string? accountType = null,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets account balance
    /// </summary>
    /// <param name="accountId">Account ID</param>
    /// <param name="asOfDate">As of date (optional, defaults to today)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Account balance</returns>
    Task<decimal> GetAccountBalanceAsync(Guid accountId, DateTime? asOfDate = null, CancellationToken cancellationToken = default);

    #endregion

    #region Journal Entries

    /// <summary>
    /// Creates a new journal entry
    /// </summary>
    /// <param name="journalEntry">Journal entry to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created journal entry</returns>
    Task<JournalEntry> CreateJournalEntryAsync(JournalEntry journalEntry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing journal entry
    /// </summary>
    /// <param name="journalEntry">Journal entry to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated journal entry</returns>
    Task<JournalEntry> UpdateJournalEntryAsync(JournalEntry journalEntry, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a journal entry by ID
    /// </summary>
    /// <param name="entryId">Entry ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Journal entry or null if not found</returns>
    Task<JournalEntry?> GetJournalEntryByIdAsync(Guid entryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets journal entries with pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="fromDate">From date filter</param>
    /// <param name="toDate">To date filter</param>
    /// <param name="status">Status filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated journal entries</returns>
    Task<PagedResult<JournalEntry>> GetJournalEntriesAsync(
        int pageNumber = 1,
        int pageSize = 50,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        Status? status = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Posts a journal entry
    /// </summary>
    /// <param name="entryId">Entry ID</param>
    /// <param name="postedBy">User who posted</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> PostJournalEntryAsync(Guid entryId, string postedBy, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reverses a journal entry
    /// </summary>
    /// <param name="entryId">Entry ID to reverse</param>
    /// <param name="reversalReason">Reason for reversal</param>
    /// <param name="reversedBy">User who reversed</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Reversal journal entry</returns>
    Task<JournalEntry> ReverseJournalEntryAsync(Guid entryId, string reversalReason, string reversedBy, CancellationToken cancellationToken = default);

    #endregion

    #region Financial Periods

    /// <summary>
    /// Creates a new financial period
    /// </summary>
    /// <param name="period">Financial period to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created financial period</returns>
    Task<FinancialPeriod> CreateFinancialPeriodAsync(FinancialPeriod period, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current financial period
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Current financial period</returns>
    Task<FinancialPeriod?> GetCurrentFinancialPeriodAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets financial periods
    /// </summary>
    /// <param name="financialYear">Financial year filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Financial periods</returns>
    Task<IEnumerable<FinancialPeriod>> GetFinancialPeriodsAsync(int? financialYear = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes a financial period
    /// </summary>
    /// <param name="periodId">Period ID</param>
    /// <param name="closedBy">User who closed</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> CloseFinancialPeriodAsync(Guid periodId, string closedBy, CancellationToken cancellationToken = default);

    #endregion

    #region Budget Management

    /// <summary>
    /// Creates a new budget
    /// </summary>
    /// <param name="budget">Budget to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created budget</returns>
    Task<Budget> CreateBudgetAsync(Budget budget, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing budget
    /// </summary>
    /// <param name="budget">Budget to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated budget</returns>
    Task<Budget> UpdateBudgetAsync(Budget budget, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a budget by ID
    /// </summary>
    /// <param name="budgetId">Budget ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Budget or null if not found</returns>
    Task<Budget?> GetBudgetByIdAsync(Guid budgetId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets budgets for a financial year
    /// </summary>
    /// <param name="financialYear">Financial year</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Budgets</returns>
    Task<IEnumerable<Budget>> GetBudgetsAsync(int financialYear, CancellationToken cancellationToken = default);

    /// <summary>
    /// Approves a budget
    /// </summary>
    /// <param name="budgetId">Budget ID</param>
    /// <param name="approvedBy">User who approved</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> ApproveBudgetAsync(Guid budgetId, string approvedBy, CancellationToken cancellationToken = default);

    #endregion

    #region Financial Reports

    /// <summary>
    /// Generates trial balance
    /// </summary>
    /// <param name="asOfDate">As of date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Trial balance</returns>
    Task<TrialBalance> GenerateTrialBalanceAsync(DateTime asOfDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates income statement
    /// </summary>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Income statement</returns>
    Task<IncomeStatement> GenerateIncomeStatementAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates balance sheet
    /// </summary>
    /// <param name="asOfDate">As of date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Balance sheet</returns>
    Task<BalanceSheet> GenerateBalanceSheetAsync(DateTime asOfDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates cash flow statement
    /// </summary>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Cash flow statement</returns>
    Task<CashFlowStatement> GenerateCashFlowStatementAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates budget vs actual report
    /// </summary>
    /// <param name="budgetId">Budget ID</param>
    /// <param name="asOfDate">As of date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Budget vs actual report</returns>
    Task<BudgetVsActualReport> GenerateBudgetVsActualReportAsync(Guid budgetId, DateTime asOfDate, CancellationToken cancellationToken = default);

    #endregion
}

/// <summary>
/// Trial balance report
/// </summary>
public class TrialBalance
{
    public DateTime AsOfDate { get; set; }
    public List<TrialBalanceAccount> Accounts { get; set; } = new();
    public decimal TotalDebits => Accounts.Sum(a => a.DebitBalance);
    public decimal TotalCredits => Accounts.Sum(a => a.CreditBalance);
    public bool IsBalanced => Math.Abs(TotalDebits - TotalCredits) < 0.01m;
}

/// <summary>
/// Trial balance account
/// </summary>
public class TrialBalanceAccount
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public decimal DebitBalance { get; set; }
    public decimal CreditBalance { get; set; }
}

/// <summary>
/// Income statement report
/// </summary>
public class IncomeStatement
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public List<IncomeStatementLine> Revenue { get; set; } = new();
    public List<IncomeStatementLine> Expenses { get; set; } = new();
    public decimal TotalRevenue => Revenue.Sum(r => r.Amount);
    public decimal TotalExpenses => Expenses.Sum(e => e.Amount);
    public decimal NetIncome => TotalRevenue - TotalExpenses;
}

/// <summary>
/// Income statement line item
/// </summary>
public class IncomeStatementLine
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

/// <summary>
/// Balance sheet report
/// </summary>
public class BalanceSheet
{
    public DateTime AsOfDate { get; set; }
    public List<BalanceSheetLine> Assets { get; set; } = new();
    public List<BalanceSheetLine> Liabilities { get; set; } = new();
    public List<BalanceSheetLine> Equity { get; set; } = new();
    public decimal TotalAssets => Assets.Sum(a => a.Amount);
    public decimal TotalLiabilities => Liabilities.Sum(l => l.Amount);
    public decimal TotalEquity => Equity.Sum(e => e.Amount);
    public bool IsBalanced => Math.Abs(TotalAssets - (TotalLiabilities + TotalEquity)) < 0.01m;
}

/// <summary>
/// Balance sheet line item
/// </summary>
public class BalanceSheetLine
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

/// <summary>
/// Cash flow statement report
/// </summary>
public class CashFlowStatement
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public List<CashFlowLine> OperatingActivities { get; set; } = new();
    public List<CashFlowLine> InvestingActivities { get; set; } = new();
    public List<CashFlowLine> FinancingActivities { get; set; } = new();
    public decimal NetCashFromOperating => OperatingActivities.Sum(o => o.Amount);
    public decimal NetCashFromInvesting => InvestingActivities.Sum(i => i.Amount);
    public decimal NetCashFromFinancing => FinancingActivities.Sum(f => f.Amount);
    public decimal NetCashFlow => NetCashFromOperating + NetCashFromInvesting + NetCashFromFinancing;
}

/// <summary>
/// Cash flow line item
/// </summary>
public class CashFlowLine
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

/// <summary>
/// Budget vs actual report
/// </summary>
public class BudgetVsActualReport
{
    public Guid BudgetId { get; set; }
    public string BudgetName { get; set; } = string.Empty;
    public DateTime AsOfDate { get; set; }
    public List<BudgetVsActualLine> Lines { get; set; } = new();
    public decimal TotalBudget => Lines.Sum(l => l.BudgetAmount);
    public decimal TotalActual => Lines.Sum(l => l.ActualAmount);
    public decimal TotalVariance => Lines.Sum(l => l.Variance);
}

/// <summary>
/// Budget vs actual line item
/// </summary>
public class BudgetVsActualLine
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public decimal Variance => BudgetAmount - ActualAmount;
    public decimal VariancePercentage => BudgetAmount != 0 ? (Variance / BudgetAmount) * 100 : 0;
}
