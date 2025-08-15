namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Export service interface for exporting data to various formats
/// </summary>
public interface IExportService
{
    /// <summary>
    /// Exports data to PDF format
    /// </summary>
    /// <typeparam name="T">Type of data to export</typeparam>
    /// <param name="data">Data to export</param>
    /// <param name="options">Export options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Exported file path</returns>
    Task<string> ExportToPdfAsync<T>(
        IEnumerable<T> data, 
        ExportOptions options, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports data to Excel format
    /// </summary>
    /// <typeparam name="T">Type of data to export</typeparam>
    /// <param name="data">Data to export</param>
    /// <param name="options">Export options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Exported file path</returns>
    Task<string> ExportToExcelAsync<T>(
        IEnumerable<T> data, 
        ExportOptions options, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports data to CSV format
    /// </summary>
    /// <typeparam name="T">Type of data to export</typeparam>
    /// <param name="data">Data to export</param>
    /// <param name="options">Export options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Exported file path</returns>
    Task<string> ExportToCsvAsync<T>(
        IEnumerable<T> data, 
        ExportOptions options, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports a report to PDF format
    /// </summary>
    /// <param name="reportData">Report data</param>
    /// <param name="templatePath">Report template path</param>
    /// <param name="options">Export options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Exported file path</returns>
    Task<string> ExportReportToPdfAsync(
        object reportData, 
        string templatePath, 
        ExportOptions options, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets supported export formats
    /// </summary>
    /// <returns>List of supported formats</returns>
    IEnumerable<ExportFormat> GetSupportedFormats();

    /// <summary>
    /// Validates export options
    /// </summary>
    /// <param name="options">Export options to validate</param>
    /// <returns>Validation result</returns>
    ExportValidationResult ValidateOptions(ExportOptions options);
}

/// <summary>
/// Export format enumeration
/// </summary>
public enum ExportFormat
{
    Pdf,
    Excel,
    Csv,
    Word,
    Json,
    Xml
}

/// <summary>
/// Export options
/// </summary>
public class ExportOptions
{
    /// <summary>
    /// Export format
    /// </summary>
    public ExportFormat Format { get; set; }

    /// <summary>
    /// Output file path (optional, will generate if not provided)
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// Document title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Document author
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// Document subject
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    /// Include headers in export
    /// </summary>
    public bool IncludeHeaders { get; set; } = true;

    /// <summary>
    /// Include footer in export
    /// </summary>
    public bool IncludeFooter { get; set; } = true;

    /// <summary>
    /// Page orientation (for PDF/Excel)
    /// </summary>
    public PageOrientation Orientation { get; set; } = PageOrientation.Portrait;

    /// <summary>
    /// Page size (for PDF)
    /// </summary>
    public PageSize PageSize { get; set; } = PageSize.A4;

    /// <summary>
    /// Columns to include in export (null for all columns)
    /// </summary>
    public string[]? Columns { get; set; }

    /// <summary>
    /// Custom properties for the export
    /// </summary>
    public Dictionary<string, object> Properties { get; set; } = new();

    /// <summary>
    /// Date format for date columns
    /// </summary>
    public string DateFormat { get; set; } = "yyyy-MM-dd";

    /// <summary>
    /// Number format for numeric columns
    /// </summary>
    public string NumberFormat { get; set; } = "N2";

    /// <summary>
    /// Currency format for currency columns
    /// </summary>
    public string CurrencyFormat { get; set; } = "C2";
}

/// <summary>
/// Page orientation enumeration
/// </summary>
public enum PageOrientation
{
    Portrait,
    Landscape
}

/// <summary>
/// Page size enumeration
/// </summary>
public enum PageSize
{
    A4,
    A3,
    Letter,
    Legal
}

/// <summary>
/// Export validation result
/// </summary>
public class ExportValidationResult
{
    /// <summary>
    /// Indicates if validation passed
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Validation error messages
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Validation warning messages
    /// </summary>
    public List<string> Warnings { get; set; } = new();
}
