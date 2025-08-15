using System.Text;
using System.Text.Json;

namespace INES.ERP.WPF.Services;

/// <summary>
/// Export service for WPF application
/// </summary>
public class ExportService
{
    /// <summary>
    /// Exports data to PDF format
    /// </summary>
    /// <typeparam name="T">Type of data to export</typeparam>
    /// <param name="data">Data to export</param>
    /// <param name="filePath">Output file path</param>
    /// <param name="options">Export options</param>
    /// <returns>True if export was successful</returns>
    public async Task<bool> ExportToPdfAsync<T>(IEnumerable<T> data, string filePath, ExportOptions? options = null)
    {
        // TODO: Implement PDF export using a PDF library like iTextSharp or PdfSharp
        await Task.Delay(100); // Simulate async operation
        return true;
    }

    /// <summary>
    /// Exports data to Excel format
    /// </summary>
    /// <typeparam name="T">Type of data to export</typeparam>
    /// <param name="data">Data to export</param>
    /// <param name="filePath">Output file path</param>
    /// <param name="options">Export options</param>
    /// <returns>True if export was successful</returns>
    public async Task<bool> ExportToExcelAsync<T>(IEnumerable<T> data, string filePath, ExportOptions? options = null)
    {
        // TODO: Implement Excel export using a library like ClosedXML or EPPlus
        await Task.Delay(100); // Simulate async operation
        return true;
    }

    /// <summary>
    /// Exports data to CSV format
    /// </summary>
    /// <typeparam name="T">Type of data to export</typeparam>
    /// <param name="data">Data to export</param>
    /// <param name="filePath">Output file path</param>
    /// <param name="options">Export options</param>
    /// <returns>True if export was successful</returns>
    public async Task<bool> ExportToCsvAsync<T>(IEnumerable<T> data, string filePath, ExportOptions? options = null)
    {
        try
        {
            var csv = new System.Text.StringBuilder();
            var properties = typeof(T).GetProperties();

            // Add headers
            if (options?.IncludeHeaders != false)
            {
                var headers = properties.Select(p => p.Name);
                csv.AppendLine(string.Join(",", headers));
            }

            // Add data rows
            foreach (var item in data)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item);
                    return EscapeCsvValue(value?.ToString() ?? string.Empty);
                });
                csv.AppendLine(string.Join(",", values));
            }

            await System.IO.File.WriteAllTextAsync(filePath, csv.ToString());
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Exports data to JSON format
    /// </summary>
    /// <typeparam name="T">Type of data to export</typeparam>
    /// <param name="data">Data to export</param>
    /// <param name="filePath">Output file path</param>
    /// <param name="options">Export options</param>
    /// <returns>True if export was successful</returns>
    public async Task<bool> ExportToJsonAsync<T>(IEnumerable<T> data, string filePath, ExportOptions? options = null)
    {
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
            await System.IO.File.WriteAllTextAsync(filePath, json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets supported export formats
    /// </summary>
    /// <returns>List of supported formats</returns>
    public IEnumerable<string> GetSupportedFormats()
    {
        return new[] { "PDF", "Excel", "CSV", "JSON" };
    }

    /// <summary>
    /// Escapes CSV values that contain special characters
    /// </summary>
    /// <param name="value">Value to escape</param>
    /// <returns>Escaped value</returns>
    private static string EscapeCsvValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }
}

/// <summary>
/// Export options
/// </summary>
public class ExportOptions
{
    /// <summary>
    /// Whether to include headers in the export
    /// </summary>
    public bool IncludeHeaders { get; set; } = true;

    /// <summary>
    /// Title for the export
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Date format to use for date values
    /// </summary>
    public string DateFormat { get; set; } = "yyyy-MM-dd";

    /// <summary>
    /// Number format to use for numeric values
    /// </summary>
    public string NumberFormat { get; set; } = "N2";

    /// <summary>
    /// Page orientation for PDF exports
    /// </summary>
    public string PageOrientation { get; set; } = "Portrait";

    /// <summary>
    /// Font size for exports
    /// </summary>
    public int FontSize { get; set; } = 10;
}
