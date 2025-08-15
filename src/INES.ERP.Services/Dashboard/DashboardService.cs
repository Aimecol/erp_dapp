using Microsoft.Extensions.Logging;
using INES.ERP.Core.Interfaces.Repositories;
using INES.ERP.Core.Interfaces.Services;
using INES.ERP.Core.Models.Dashboard;

namespace INES.ERP.Services.Dashboard;

/// <summary>
/// Dashboard service implementation
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(IUnitOfWork unitOfWork, ILogger<DashboardService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<DashboardKPI>> GetKPIsAsync(
        Guid? userId = null, 
        string? category = null, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // For now, return sample KPIs
            // In a real implementation, this would query the database and calculate actual KPIs
            var kpis = new List<DashboardKPI>
            {
                new DashboardKPI
                {
                    Id = Guid.NewGuid(),
                    Name = "TotalRevenue",
                    Title = "Total Revenue",
                    Description = "Total revenue for the current period",
                    CurrentValue = 125000000, // 125M RWF
                    PreviousValue = 118000000, // 118M RWF
                    TargetValue = 130000000, // 130M RWF
                    Unit = "RWF",
                    Category = "Financial",
                    Icon = "CurrencyUsd",
                    Color = "#4CAF50",
                    DisplayOrder = 1,
                    IsActive = true,
                    PeriodStart = DateTime.Now.AddMonths(-1),
                    PeriodEnd = DateTime.Now
                },
                new DashboardKPI
                {
                    Id = Guid.NewGuid(),
                    Name = "ActiveStudents",
                    Title = "Active Students",
                    Description = "Number of currently enrolled students",
                    CurrentValue = 2450,
                    PreviousValue = 2380,
                    TargetValue = 2500,
                    Unit = "Count",
                    Category = "Academic",
                    Icon = "AccountGroup",
                    Color = "#2196F3",
                    DisplayOrder = 2,
                    IsActive = true,
                    PeriodStart = DateTime.Now.AddMonths(-1),
                    PeriodEnd = DateTime.Now
                },
                new DashboardKPI
                {
                    Id = Guid.NewGuid(),
                    Name = "CollectionRate",
                    Title = "Fee Collection Rate",
                    Description = "Percentage of fees collected",
                    CurrentValue = 87.5m,
                    PreviousValue = 85.2m,
                    TargetValue = 90.0m,
                    Unit = "%",
                    Category = "Financial",
                    Icon = "Percent",
                    Color = "#FF9800",
                    DisplayOrder = 3,
                    IsActive = true,
                    PeriodStart = DateTime.Now.AddMonths(-1),
                    PeriodEnd = DateTime.Now
                },
                new DashboardKPI
                {
                    Id = Guid.NewGuid(),
                    Name = "OutstandingReceivables",
                    Title = "Outstanding Receivables",
                    Description = "Total amount of outstanding receivables",
                    CurrentValue = 45000000, // 45M RWF
                    PreviousValue = 48000000, // 48M RWF
                    TargetValue = 40000000, // 40M RWF
                    Unit = "RWF",
                    Category = "Financial",
                    Icon = "AccountClock",
                    Color = "#F44336",
                    DisplayOrder = 4,
                    IsActive = true,
                    PeriodStart = DateTime.Now.AddMonths(-1),
                    PeriodEnd = DateTime.Now
                }
            };

            if (!string.IsNullOrEmpty(category))
            {
                kpis = kpis.Where(k => k.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return kpis.OrderBy(k => k.DisplayOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting KPIs");
            return new List<DashboardKPI>();
        }
    }

    public async Task<ChartData?> GetChartDataAsync(
        string chartName, 
        DateTime periodStart, 
        DateTime periodEnd, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // For now, return sample chart data
            // In a real implementation, this would query the database
            return chartName.ToLower() switch
            {
                "monthlyrevenue" => await GetMonthlyRevenueChartAsync(periodStart, periodEnd),
                "studentenrollment" => await GetStudentEnrollmentChartAsync(periodStart, periodEnd),
                "expensebreakdown" => await GetExpenseBreakdownChartAsync(periodStart, periodEnd),
                _ => null
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting chart data for {ChartName}", chartName);
            return null;
        }
    }

    public async Task<IEnumerable<ChartData>> GetAllChartsAsync(
        string? category = null,
        DateTime? periodStart = null,
        DateTime? periodEnd = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var start = periodStart ?? DateTime.Now.AddMonths(-12);
            var end = periodEnd ?? DateTime.Now;

            var charts = new List<ChartData>();

            var monthlyRevenue = await GetChartDataAsync("monthlyrevenue", start, end, cancellationToken);
            if (monthlyRevenue != null) charts.Add(monthlyRevenue);

            var studentEnrollment = await GetChartDataAsync("studentenrollment", start, end, cancellationToken);
            if (studentEnrollment != null) charts.Add(studentEnrollment);

            var expenseBreakdown = await GetChartDataAsync("expensebreakdown", start, end, cancellationToken);
            if (expenseBreakdown != null) charts.Add(expenseBreakdown);

            if (!string.IsNullOrEmpty(category))
            {
                charts = charts.Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return charts.OrderBy(c => c.DisplayOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all charts");
            return new List<ChartData>();
        }
    }

    public async Task<IEnumerable<DashboardAlert>> GetAlertsAsync(
        Guid? userId = null,
        bool includeRead = false,
        bool includeDismissed = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // For now, return sample alerts
            var alerts = new List<DashboardAlert>
            {
                new DashboardAlert
                {
                    Id = Guid.NewGuid(),
                    Title = "Low Inventory Alert",
                    Message = "15 items are below reorder level",
                    AlertType = "Warning",
                    Priority = "Medium",
                    Source = "Inventory",
                    Icon = "AlertCircle",
                    ActionUrl = "/inventory/reorder",
                    ActionText = "View Items",
                    IsRead = false,
                    IsDismissed = false,
                    CreatedAt = DateTime.Now.AddHours(-2)
                },
                new DashboardAlert
                {
                    Id = Guid.NewGuid(),
                    Title = "Payment Overdue",
                    Message = "25 students have overdue payments",
                    AlertType = "Error",
                    Priority = "High",
                    Source = "StudentAccounts",
                    Icon = "CurrencyUsd",
                    ActionUrl = "/students/overdue",
                    ActionText = "View Students",
                    IsRead = false,
                    IsDismissed = false,
                    CreatedAt = DateTime.Now.AddHours(-4)
                },
                new DashboardAlert
                {
                    Id = Guid.NewGuid(),
                    Title = "Monthly Report Ready",
                    Message = "Financial report for last month is ready for review",
                    AlertType = "Info",
                    Priority = "Low",
                    Source = "Reports",
                    Icon = "FileDocument",
                    ActionUrl = "/reports/monthly",
                    ActionText = "View Report",
                    IsRead = true,
                    IsDismissed = false,
                    CreatedAt = DateTime.Now.AddDays(-1)
                }
            };

            var filteredAlerts = alerts.AsQueryable();

            if (!includeRead)
            {
                filteredAlerts = filteredAlerts.Where(a => !a.IsRead);
            }

            if (!includeDismissed)
            {
                filteredAlerts = filteredAlerts.Where(a => !a.IsDismissed);
            }

            if (userId.HasValue)
            {
                filteredAlerts = filteredAlerts.Where(a => a.UserId == userId.Value || a.UserId == null);
            }

            return filteredAlerts.OrderByDescending(a => a.CreatedAt).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting alerts");
            return new List<DashboardAlert>();
        }
    }

    public async Task<bool> MarkAlertAsReadAsync(Guid alertId, CancellationToken cancellationToken = default)
    {
        try
        {
            // In a real implementation, this would update the database
            _logger.LogInformation("Alert marked as read: {AlertId}", alertId);
            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking alert as read: {AlertId}", alertId);
            return false;
        }
    }

    public async Task<bool> DismissAlertAsync(Guid alertId, CancellationToken cancellationToken = default)
    {
        try
        {
            // In a real implementation, this would update the database
            _logger.LogInformation("Alert dismissed: {AlertId}", alertId);
            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error dismissing alert: {AlertId}", alertId);
            return false;
        }
    }

    public async Task<DashboardAlert> CreateAlertAsync(DashboardAlert alert, CancellationToken cancellationToken = default)
    {
        try
        {
            alert.Id = Guid.NewGuid();
            alert.CreatedAt = DateTime.UtcNow;
            
            // In a real implementation, this would save to the database
            _logger.LogInformation("Alert created: {AlertTitle}", alert.Title);
            return await Task.FromResult(alert);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating alert: {AlertTitle}", alert.Title);
            throw;
        }
    }

    public async Task<bool> RefreshKPIsAsync(string? kpiName = null, CancellationToken cancellationToken = default)
    {
        try
        {
            // In a real implementation, this would recalculate KPIs from the database
            _logger.LogInformation("KPIs refreshed: {KpiName}", kpiName ?? "All");
            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing KPIs: {KpiName}", kpiName);
            return false;
        }
    }

    public async Task<bool> RefreshChartsAsync(string? chartName = null, CancellationToken cancellationToken = default)
    {
        try
        {
            // In a real implementation, this would recalculate chart data from the database
            _logger.LogInformation("Charts refreshed: {ChartName}", chartName ?? "All");
            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing charts: {ChartName}", chartName);
            return false;
        }
    }

    public async Task<FinancialSummary> GetFinancialSummaryAsync(
        DateTime periodStart, 
        DateTime periodEnd, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // In a real implementation, this would calculate from actual financial data
            return await Task.FromResult(new FinancialSummary
            {
                TotalIncome = 125000000, // 125M RWF
                TotalExpenses = 98000000, // 98M RWF
                TotalAssets = 850000000, // 850M RWF
                TotalLiabilities = 320000000, // 320M RWF
                CashBalance = 75000000, // 75M RWF
                AccountsReceivable = 45000000, // 45M RWF
                AccountsPayable = 28000000, // 28M RWF
                PeriodStart = periodStart,
                PeriodEnd = periodEnd
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting financial summary");
            throw;
        }
    }

    public async Task<StudentAccountsSummary> GetStudentAccountsSummaryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // In a real implementation, this would calculate from actual student data
            return await Task.FromResult(new StudentAccountsSummary
            {
                TotalActiveStudents = 2450,
                TotalFeesBilled = 180000000, // 180M RWF
                TotalFeesCollected = 157500000, // 157.5M RWF
                StudentsWithOutstandingBalances = 312,
                StudentsWithPenalties = 45,
                TotalPenalties = 2250000 // 2.25M RWF
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student accounts summary");
            throw;
        }
    }

    public async Task<InventorySummary> GetInventorySummaryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // In a real implementation, this would calculate from actual inventory data
            return await Task.FromResult(new InventorySummary
            {
                TotalItems = 1250,
                TotalValue = 45000000, // 45M RWF
                ItemsBelowReorderLevel = 15,
                ExpiredItems = 3,
                ItemsExpiringSoon = 8,
                OutOfStockItems = 5,
                TotalStores = 4,
                AverageInventoryTurnover = 6.5m
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting inventory summary");
            throw;
        }
    }

    private async Task<ChartData> GetMonthlyRevenueChartAsync(DateTime periodStart, DateTime periodEnd)
    {
        var chartData = new ChartData
        {
            Id = Guid.NewGuid(),
            Name = "MonthlyRevenue",
            Title = "Monthly Revenue Trend",
            ChartType = "Line",
            Category = "Financial",
            DisplayOrder = 1,
            IsActive = true,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd
        };

        // Generate sample data for the last 12 months
        var dataPoints = new List<ChartDataPoint>();
        for (int i = 11; i >= 0; i--)
        {
            var month = DateTime.Now.AddMonths(-i);
            var revenue = 10000000 + (new Random().Next(-2000000, 3000000)); // Random revenue between 8M-13M RWF
            
            dataPoints.Add(new ChartDataPoint
            {
                Id = Guid.NewGuid(),
                ChartDataId = chartData.Id,
                Label = month.ToString("MMM yyyy"),
                Value = revenue,
                Order = 11 - i,
                DateTime = month
            });
        }

        chartData.DataPoints = dataPoints;
        return await Task.FromResult(chartData);
    }

    private async Task<ChartData> GetStudentEnrollmentChartAsync(DateTime periodStart, DateTime periodEnd)
    {
        var chartData = new ChartData
        {
            Id = Guid.NewGuid(),
            Name = "StudentEnrollment",
            Title = "Student Enrollment by Program",
            ChartType = "Pie",
            Category = "Academic",
            DisplayOrder = 2,
            IsActive = true,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd
        };

        var programs = new[]
        {
            ("Computer Science", 650, "#2196F3"),
            ("Business Administration", 580, "#4CAF50"),
            ("Engineering", 420, "#FF9800"),
            ("Education", 380, "#9C27B0"),
            ("Health Sciences", 320, "#F44336"),
            ("Agriculture", 100, "#795548")
        };

        var dataPoints = new List<ChartDataPoint>();
        for (int i = 0; i < programs.Length; i++)
        {
            dataPoints.Add(new ChartDataPoint
            {
                Id = Guid.NewGuid(),
                ChartDataId = chartData.Id,
                Label = programs[i].Item1,
                Value = programs[i].Item2,
                Color = programs[i].Item3,
                Order = i
            });
        }

        chartData.DataPoints = dataPoints;
        return await Task.FromResult(chartData);
    }

    private async Task<ChartData> GetExpenseBreakdownChartAsync(DateTime periodStart, DateTime periodEnd)
    {
        var chartData = new ChartData
        {
            Id = Guid.NewGuid(),
            Name = "ExpenseBreakdown",
            Title = "Monthly Expense Breakdown",
            ChartType = "Bar",
            Category = "Financial",
            DisplayOrder = 3,
            IsActive = true,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd
        };

        var categories = new[]
        {
            ("Salaries", 35000000),
            ("Utilities", 8000000),
            ("Maintenance", 5500000),
            ("Supplies", 4200000),
            ("Marketing", 2800000),
            ("Other", 3500000)
        };

        var dataPoints = new List<ChartDataPoint>();
        for (int i = 0; i < categories.Length; i++)
        {
            dataPoints.Add(new ChartDataPoint
            {
                Id = Guid.NewGuid(),
                ChartDataId = chartData.Id,
                Label = categories[i].Item1,
                Value = categories[i].Item2,
                Order = i
            });
        }

        chartData.DataPoints = dataPoints;
        return await Task.FromResult(chartData);
    }
}
