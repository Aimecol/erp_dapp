using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Dashboard;

/// <summary>
/// Represents a Key Performance Indicator (KPI) for the dashboard
/// </summary>
public class DashboardKPI : BaseEntity
{
    /// <summary>
    /// KPI name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// KPI display title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// KPI description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Current value of the KPI
    /// </summary>
    public decimal CurrentValue { get; set; }

    /// <summary>
    /// Previous value for comparison
    /// </summary>
    public decimal? PreviousValue { get; set; }

    /// <summary>
    /// Target value for the KPI
    /// </summary>
    public decimal? TargetValue { get; set; }

    /// <summary>
    /// Unit of measurement (e.g., "RWF", "%", "Count")
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// KPI category
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Icon name for display
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Color for the KPI display
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Display order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if the KPI is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Period for which this KPI is calculated
    /// </summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>
    /// End of the period for which this KPI is calculated
    /// </summary>
    public DateTime PeriodEnd { get; set; }

    /// <summary>
    /// Calculates the percentage change from previous value
    /// </summary>
    public decimal? PercentageChange
    {
        get
        {
            if (!PreviousValue.HasValue || PreviousValue.Value == 0)
                return null;

            return ((CurrentValue - PreviousValue.Value) / PreviousValue.Value) * 100;
        }
    }

    /// <summary>
    /// Calculates the progress towards target
    /// </summary>
    public decimal? TargetProgress
    {
        get
        {
            if (!TargetValue.HasValue || TargetValue.Value == 0)
                return null;

            return (CurrentValue / TargetValue.Value) * 100;
        }
    }

    /// <summary>
    /// Indicates if the KPI is trending up
    /// </summary>
    public bool IsTrendingUp => PercentageChange > 0;

    /// <summary>
    /// Indicates if the KPI is trending down
    /// </summary>
    public bool IsTrendingDown => PercentageChange < 0;

    /// <summary>
    /// Indicates if the KPI is on target
    /// </summary>
    public bool IsOnTarget => TargetProgress >= 100;
}

/// <summary>
/// Represents chart data for dashboard analytics
/// </summary>
public class ChartData : BaseEntity
{
    /// <summary>
    /// Chart name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Chart title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Chart type (Line, Bar, Pie, etc.)
    /// </summary>
    public string ChartType { get; set; } = string.Empty;

    /// <summary>
    /// Chart category
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Data points for the chart
    /// </summary>
    public virtual ICollection<ChartDataPoint> DataPoints { get; set; } = new List<ChartDataPoint>();

    /// <summary>
    /// Chart configuration (JSON)
    /// </summary>
    public string? Configuration { get; set; }

    /// <summary>
    /// Display order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if the chart is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Period for which this chart data is calculated
    /// </summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>
    /// End of the period for which this chart data is calculated
    /// </summary>
    public DateTime PeriodEnd { get; set; }
}

/// <summary>
/// Represents a data point in a chart
/// </summary>
public class ChartDataPoint : BaseEntity
{
    /// <summary>
    /// Chart data ID
    /// </summary>
    public Guid ChartDataId { get; set; }

    /// <summary>
    /// Label for the data point
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Value of the data point
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Additional value (for multi-series charts)
    /// </summary>
    public decimal? Value2 { get; set; }

    /// <summary>
    /// Color for the data point
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Order of the data point
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Date/time for the data point
    /// </summary>
    public DateTime? DateTime { get; set; }

    /// <summary>
    /// Navigation property to chart data
    /// </summary>
    public virtual ChartData ChartData { get; set; } = null!;
}

/// <summary>
/// Represents a dashboard alert
/// </summary>
public class DashboardAlert : BaseEntity
{
    /// <summary>
    /// Alert title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Alert message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Alert type (Info, Warning, Error, Success)
    /// </summary>
    public string AlertType { get; set; } = string.Empty;

    /// <summary>
    /// Alert priority (Low, Medium, High, Critical)
    /// </summary>
    public string Priority { get; set; } = string.Empty;

    /// <summary>
    /// Source of the alert
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Icon for the alert
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Action URL or command
    /// </summary>
    public string? ActionUrl { get; set; }

    /// <summary>
    /// Action text
    /// </summary>
    public string? ActionText { get; set; }

    /// <summary>
    /// Indicates if the alert has been read
    /// </summary>
    public bool IsRead { get; set; } = false;

    /// <summary>
    /// Indicates if the alert is dismissed
    /// </summary>
    public bool IsDismissed { get; set; } = false;

    /// <summary>
    /// Alert expiry date
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// User ID for user-specific alerts
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Role for role-specific alerts
    /// </summary>
    public string? Role { get; set; }
}
