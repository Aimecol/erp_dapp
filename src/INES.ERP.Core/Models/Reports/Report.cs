using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Reports;

/// <summary>
/// Represents a report definition
/// </summary>
public class Report : AuditableEntity
{
    /// <summary>
    /// Report code
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ReportCode { get; set; } = string.Empty;

    /// <summary>
    /// Report name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string ReportName { get; set; } = string.Empty;

    /// <summary>
    /// Report description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Report category
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Report type (Standard, Custom, Dashboard)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string ReportType { get; set; } = string.Empty;

    /// <summary>
    /// Data source (SQL query, stored procedure, etc.)
    /// </summary>
    public string? DataSource { get; set; }

    /// <summary>
    /// Report template path
    /// </summary>
    [MaxLength(500)]
    public string? TemplatePath { get; set; }

    /// <summary>
    /// Report parameters (JSON)
    /// </summary>
    public string? Parameters { get; set; }

    /// <summary>
    /// Report layout configuration (JSON)
    /// </summary>
    public string? LayoutConfiguration { get; set; }

    /// <summary>
    /// Indicates if report is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Indicates if report is system-defined
    /// </summary>
    public bool IsSystemReport { get; set; } = false;

    /// <summary>
    /// Required permissions to access this report
    /// </summary>
    [MaxLength(500)]
    public string? RequiredPermissions { get; set; }

    /// <summary>
    /// Display order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Navigation property for report executions
    /// </summary>
    public virtual ICollection<ReportExecution> ReportExecutions { get; set; } = new List<ReportExecution>();
}

/// <summary>
/// Represents a report execution instance
/// </summary>
public class ReportExecution : AuditableEntity
{
    /// <summary>
    /// Report ID
    /// </summary>
    public Guid ReportId { get; set; }

    /// <summary>
    /// Execution reference
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ExecutionReference { get; set; } = string.Empty;

    /// <summary>
    /// Execution start time
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Execution end time
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Execution status
    /// </summary>
    public Status ExecutionStatus { get; set; } = Status.InProgress;

    /// <summary>
    /// Parameters used for this execution (JSON)
    /// </summary>
    public string? ExecutionParameters { get; set; }

    /// <summary>
    /// Output format (PDF, Excel, CSV, etc.)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string OutputFormat { get; set; } = string.Empty;

    /// <summary>
    /// Output file path
    /// </summary>
    [MaxLength(500)]
    public string? OutputFilePath { get; set; }

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long? FileSizeBytes { get; set; }

    /// <summary>
    /// Number of records in the report
    /// </summary>
    public int? RecordCount { get; set; }

    /// <summary>
    /// Execution duration in milliseconds
    /// </summary>
    public long? ExecutionDurationMs { get; set; }

    /// <summary>
    /// Error message (if execution failed)
    /// </summary>
    [MaxLength(1000)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// User who executed the report
    /// </summary>
    [MaxLength(100)]
    public string? ExecutedBy { get; set; }

    /// <summary>
    /// Navigation property to report
    /// </summary>
    public virtual Report Report { get; set; } = null!;

    /// <summary>
    /// Gets the execution duration
    /// </summary>
    public TimeSpan? ExecutionDuration
    {
        get
        {
            if (ExecutionDurationMs.HasValue)
                return TimeSpan.FromMilliseconds(ExecutionDurationMs.Value);
            
            if (EndTime.HasValue)
                return EndTime.Value - StartTime;
            
            return null;
        }
    }

    /// <summary>
    /// Checks if execution is completed
    /// </summary>
    public bool IsCompleted => ExecutionStatus == Status.Completed && EndTime.HasValue;

    /// <summary>
    /// Checks if execution failed
    /// </summary>
    public bool IsFailed => ExecutionStatus == Status.Failed;
}

/// <summary>
/// Represents a scheduled report
/// </summary>
public class ScheduledReport : AuditableEntity
{
    /// <summary>
    /// Report ID
    /// </summary>
    public Guid ReportId { get; set; }

    /// <summary>
    /// Schedule name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string ScheduleName { get; set; } = string.Empty;

    /// <summary>
    /// Schedule description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Schedule frequency
    /// </summary>
    public Frequency Frequency { get; set; } = Frequency.Monthly;

    /// <summary>
    /// Schedule parameters (JSON)
    /// </summary>
    public string? ScheduleParameters { get; set; }

    /// <summary>
    /// Output format
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string OutputFormat { get; set; } = string.Empty;

    /// <summary>
    /// Email recipients (comma-separated)
    /// </summary>
    [MaxLength(1000)]
    public string? EmailRecipients { get; set; }

    /// <summary>
    /// Email subject template
    /// </summary>
    [MaxLength(200)]
    public string? EmailSubject { get; set; }

    /// <summary>
    /// Email body template
    /// </summary>
    [MaxLength(1000)]
    public string? EmailBody { get; set; }

    /// <summary>
    /// Next execution date
    /// </summary>
    public DateTime? NextExecutionDate { get; set; }

    /// <summary>
    /// Last execution date
    /// </summary>
    public DateTime? LastExecutionDate { get; set; }

    /// <summary>
    /// Schedule status
    /// </summary>
    public Status ScheduleStatus { get; set; } = Status.Active;

    /// <summary>
    /// Navigation property to report
    /// </summary>
    public virtual Report Report { get; set; } = null!;

    /// <summary>
    /// Checks if schedule is active
    /// </summary>
    public bool IsActive => ScheduleStatus == Status.Active;

    /// <summary>
    /// Checks if schedule is due for execution
    /// </summary>
    public bool IsDueForExecution => IsActive && NextExecutionDate.HasValue && NextExecutionDate.Value <= DateTime.UtcNow;
}

/// <summary>
/// Represents a report dashboard
/// </summary>
public class ReportDashboard : AuditableEntity
{
    /// <summary>
    /// Dashboard name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string DashboardName { get; set; } = string.Empty;

    /// <summary>
    /// Dashboard description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Dashboard layout configuration (JSON)
    /// </summary>
    public string? LayoutConfiguration { get; set; }

    /// <summary>
    /// Refresh interval in minutes
    /// </summary>
    public int RefreshIntervalMinutes { get; set; } = 30;

    /// <summary>
    /// Indicates if dashboard is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Indicates if dashboard is public
    /// </summary>
    public bool IsPublic { get; set; } = false;

    /// <summary>
    /// Required permissions to access this dashboard
    /// </summary>
    [MaxLength(500)]
    public string? RequiredPermissions { get; set; }

    /// <summary>
    /// Navigation property for dashboard widgets
    /// </summary>
    public virtual ICollection<DashboardWidget> DashboardWidgets { get; set; } = new List<DashboardWidget>();
}

/// <summary>
/// Represents a widget on a dashboard
/// </summary>
public class DashboardWidget : BaseEntity
{
    /// <summary>
    /// Dashboard ID
    /// </summary>
    public Guid DashboardId { get; set; }

    /// <summary>
    /// Report ID (if widget displays a report)
    /// </summary>
    public Guid? ReportId { get; set; }

    /// <summary>
    /// Widget title
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Widget type (Chart, Table, KPI, etc.)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string WidgetType { get; set; } = string.Empty;

    /// <summary>
    /// Widget configuration (JSON)
    /// </summary>
    public string? Configuration { get; set; }

    /// <summary>
    /// Widget position X
    /// </summary>
    public int PositionX { get; set; }

    /// <summary>
    /// Widget position Y
    /// </summary>
    public int PositionY { get; set; }

    /// <summary>
    /// Widget width
    /// </summary>
    public int Width { get; set; } = 1;

    /// <summary>
    /// Widget height
    /// </summary>
    public int Height { get; set; } = 1;

    /// <summary>
    /// Refresh interval in minutes
    /// </summary>
    public int RefreshIntervalMinutes { get; set; } = 30;

    /// <summary>
    /// Indicates if widget is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property to dashboard
    /// </summary>
    public virtual ReportDashboard Dashboard { get; set; } = null!;

    /// <summary>
    /// Navigation property to report
    /// </summary>
    public virtual Report? Report { get; set; }
}

/// <summary>
/// Represents a report template
/// </summary>
public class ReportTemplate : AuditableEntity
{
    /// <summary>
    /// Template name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string TemplateName { get; set; } = string.Empty;

    /// <summary>
    /// Template description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Template category
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Template content (HTML, XML, etc.)
    /// </summary>
    public string? TemplateContent { get; set; }

    /// <summary>
    /// Template file path
    /// </summary>
    [MaxLength(500)]
    public string? TemplateFilePath { get; set; }

    /// <summary>
    /// Template type (HTML, RDLC, Crystal, etc.)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string TemplateType { get; set; } = string.Empty;

    /// <summary>
    /// Supported output formats (JSON array)
    /// </summary>
    [MaxLength(200)]
    public string? SupportedFormats { get; set; }

    /// <summary>
    /// Template parameters (JSON)
    /// </summary>
    public string? Parameters { get; set; }

    /// <summary>
    /// Indicates if template is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Indicates if template is system-defined
    /// </summary>
    public bool IsSystemTemplate { get; set; } = false;
}
