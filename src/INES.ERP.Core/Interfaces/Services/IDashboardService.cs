using INES.ERP.Core.Models.Dashboard;

namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Dashboard service interface
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Gets all KPIs for the dashboard
    /// </summary>
    /// <param name="userId">User ID for personalized KPIs</param>
    /// <param name="category">Optional category filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of KPIs</returns>
    Task<IEnumerable<DashboardKPI>> GetKPIsAsync(
        Guid? userId = null, 
        string? category = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets chart data for the dashboard
    /// </summary>
    /// <param name="chartName">Chart name</param>
    /// <param name="periodStart">Period start date</param>
    /// <param name="periodEnd">Period end date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Chart data</returns>
    Task<ChartData?> GetChartDataAsync(
        string chartName, 
        DateTime periodStart, 
        DateTime periodEnd, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all chart data for the dashboard
    /// </summary>
    /// <param name="category">Optional category filter</param>
    /// <param name="periodStart">Period start date</param>
    /// <param name="periodEnd">Period end date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of chart data</returns>
    Task<IEnumerable<ChartData>> GetAllChartsAsync(
        string? category = null,
        DateTime? periodStart = null,
        DateTime? periodEnd = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets dashboard alerts
    /// </summary>
    /// <param name="userId">User ID for user-specific alerts</param>
    /// <param name="includeRead">Include read alerts</param>
    /// <param name="includeDismissed">Include dismissed alerts</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of alerts</returns>
    Task<IEnumerable<DashboardAlert>> GetAlertsAsync(
        Guid? userId = null,
        bool includeRead = false,
        bool includeDismissed = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks an alert as read
    /// </summary>
    /// <param name="alertId">Alert ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> MarkAlertAsReadAsync(Guid alertId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dismisses an alert
    /// </summary>
    /// <param name="alertId">Alert ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> DismissAlertAsync(Guid alertId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new alert
    /// </summary>
    /// <param name="alert">Alert to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created alert</returns>
    Task<DashboardAlert> CreateAlertAsync(DashboardAlert alert, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes KPI data
    /// </summary>
    /// <param name="kpiName">Optional KPI name to refresh specific KPI</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> RefreshKPIsAsync(string? kpiName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes chart data
    /// </summary>
    /// <param name="chartName">Optional chart name to refresh specific chart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> RefreshChartsAsync(string? chartName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets financial summary for the dashboard
    /// </summary>
    /// <param name="periodStart">Period start date</param>
    /// <param name="periodEnd">Period end date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Financial summary</returns>
    Task<FinancialSummary> GetFinancialSummaryAsync(
        DateTime periodStart, 
        DateTime periodEnd, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets student accounts summary
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Student accounts summary</returns>
    Task<StudentAccountsSummary> GetStudentAccountsSummaryAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets inventory summary
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Inventory summary</returns>
    Task<InventorySummary> GetInventorySummaryAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Financial summary for dashboard
/// </summary>
public class FinancialSummary
{
    /// <summary>
    /// Total income for the period
    /// </summary>
    public decimal TotalIncome { get; set; }

    /// <summary>
    /// Total expenses for the period
    /// </summary>
    public decimal TotalExpenses { get; set; }

    /// <summary>
    /// Net profit/loss for the period
    /// </summary>
    public decimal NetProfit => TotalIncome - TotalExpenses;

    /// <summary>
    /// Total assets
    /// </summary>
    public decimal TotalAssets { get; set; }

    /// <summary>
    /// Total liabilities
    /// </summary>
    public decimal TotalLiabilities { get; set; }

    /// <summary>
    /// Total equity
    /// </summary>
    public decimal TotalEquity => TotalAssets - TotalLiabilities;

    /// <summary>
    /// Cash and bank balance
    /// </summary>
    public decimal CashBalance { get; set; }

    /// <summary>
    /// Accounts receivable
    /// </summary>
    public decimal AccountsReceivable { get; set; }

    /// <summary>
    /// Accounts payable
    /// </summary>
    public decimal AccountsPayable { get; set; }

    /// <summary>
    /// Period start date
    /// </summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>
    /// Period end date
    /// </summary>
    public DateTime PeriodEnd { get; set; }
}

/// <summary>
/// Student accounts summary for dashboard
/// </summary>
public class StudentAccountsSummary
{
    /// <summary>
    /// Total number of active students
    /// </summary>
    public int TotalActiveStudents { get; set; }

    /// <summary>
    /// Total fees billed
    /// </summary>
    public decimal TotalFeesBilled { get; set; }

    /// <summary>
    /// Total fees collected
    /// </summary>
    public decimal TotalFeesCollected { get; set; }

    /// <summary>
    /// Outstanding fees
    /// </summary>
    public decimal OutstandingFees => TotalFeesBilled - TotalFeesCollected;

    /// <summary>
    /// Collection rate percentage
    /// </summary>
    public decimal CollectionRate => TotalFeesBilled > 0 ? (TotalFeesCollected / TotalFeesBilled) * 100 : 0;

    /// <summary>
    /// Number of students with outstanding balances
    /// </summary>
    public int StudentsWithOutstandingBalances { get; set; }

    /// <summary>
    /// Number of students with penalties
    /// </summary>
    public int StudentsWithPenalties { get; set; }

    /// <summary>
    /// Total penalty amount
    /// </summary>
    public decimal TotalPenalties { get; set; }
}

/// <summary>
/// Inventory summary for dashboard
/// </summary>
public class InventorySummary
{
    /// <summary>
    /// Total number of items in inventory
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Total inventory value
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Number of items below reorder level
    /// </summary>
    public int ItemsBelowReorderLevel { get; set; }

    /// <summary>
    /// Number of expired items
    /// </summary>
    public int ExpiredItems { get; set; }

    /// <summary>
    /// Number of items expiring soon
    /// </summary>
    public int ItemsExpiringSoon { get; set; }

    /// <summary>
    /// Number of out of stock items
    /// </summary>
    public int OutOfStockItems { get; set; }

    /// <summary>
    /// Total number of stores
    /// </summary>
    public int TotalStores { get; set; }

    /// <summary>
    /// Average inventory turnover
    /// </summary>
    public decimal AverageInventoryTurnover { get; set; }
}
