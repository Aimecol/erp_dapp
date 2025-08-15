using INES.ERP.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Reflection;
using ClosedXML.Excel;

namespace INES.ERP.Services.Common;

/// <summary>
/// Export service implementation
/// </summary>
public class ExportService : IExportService
{
    private readonly ILogger<ExportService> _logger;

    public ExportService(ILogger<ExportService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> ExportToPdfAsync<T>(
        IEnumerable<T> data, 
        ExportOptions options, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var filePath = options.FilePath ?? GenerateFilePath(options.Format, options.Title);
            
            // For now, create a simple text-based PDF
            // In a real implementation, use a proper PDF library like iTextSharp
            var content = GenerateTextContent(data, options);
            
            await File.WriteAllTextAsync(filePath, content, cancellationToken);
            
            _logger.LogInformation("Data exported to PDF: {FilePath}", filePath);
            return filePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to PDF");
            throw;
        }
    }

    public async Task<string> ExportToExcelAsync<T>(
        IEnumerable<T> data, 
        ExportOptions options, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var filePath = options.FilePath ?? GenerateFilePath(options.Format, options.Title);
            
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(options.Title ?? "Data");
            
            var dataList = data.ToList();
            if (dataList.Count == 0)
            {
                worksheet.Cell(1, 1).Value = "No data to export";
            }
            else
            {
                var properties = GetExportProperties<T>(options.Columns);
                
                // Add headers
                if (options.IncludeHeaders)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = properties[i].Name;
                    }
                }
                
                // Add data
                var startRow = options.IncludeHeaders ? 2 : 1;
                for (int row = 0; row < dataList.Count; row++)
                {
                    var item = dataList[row];
                    for (int col = 0; col < properties.Length; col++)
                    {
                        var value = properties[col].GetValue(item);
                        var formattedValue = FormatValue(value, properties[col], options);
                        worksheet.Cell(startRow + row, col + 1).Value = formattedValue?.ToString() ?? string.Empty;
                    }
                }
                
                // Auto-fit columns
                worksheet.Columns().AdjustToContents();
            }
            
            await Task.Run(() => workbook.SaveAs(filePath), cancellationToken);
            
            _logger.LogInformation("Data exported to Excel: {FilePath}", filePath);
            return filePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to Excel");
            throw;
        }
    }

    public async Task<string> ExportToCsvAsync<T>(
        IEnumerable<T> data, 
        ExportOptions options, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var filePath = options.FilePath ?? GenerateFilePath(options.Format, options.Title);
            var csv = new StringBuilder();
            
            var dataList = data.ToList();
            var properties = GetExportProperties<T>(options.Columns);
            
            // Add headers
            if (options.IncludeHeaders)
            {
                csv.AppendLine(string.Join(",", properties.Select(p => EscapeCsvValue(p.Name))));
            }
            
            // Add data
            foreach (var item in dataList)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item);
                    var formattedValue = FormatValue(value, p, options);
                    return EscapeCsvValue(formattedValue?.ToString() ?? string.Empty);
                });
                csv.AppendLine(string.Join(",", values));
            }
            
            await File.WriteAllTextAsync(filePath, csv.ToString(), Encoding.UTF8, cancellationToken);
            
            _logger.LogInformation("Data exported to CSV: {FilePath}", filePath);
            return filePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to CSV");
            throw;
        }
    }

    public async Task<string> ExportReportToPdfAsync(
        object reportData, 
        string templatePath, 
        ExportOptions options, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var filePath = options.FilePath ?? GenerateFilePath(options.Format, options.Title);
            
            // For now, create a simple report
            // In a real implementation, use a proper reporting engine
            var content = $"Report: {options.Title}\n";
            content += $"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n";
            content += $"Template: {templatePath}\n\n";
            content += reportData.ToString();
            
            await File.WriteAllTextAsync(filePath, content, cancellationToken);
            
            _logger.LogInformation("Report exported to PDF: {FilePath}", filePath);
            return filePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting report to PDF");
            throw;
        }
    }

    public IEnumerable<ExportFormat> GetSupportedFormats()
    {
        return new[] { ExportFormat.Pdf, ExportFormat.Excel, ExportFormat.Csv };
    }

    public ExportValidationResult ValidateOptions(ExportOptions options)
    {
        var result = new ExportValidationResult { IsValid = true };
        
        if (string.IsNullOrWhiteSpace(options.Title))
        {
            result.Warnings.Add("Title is not specified. A default title will be used.");
        }
        
        if (!string.IsNullOrEmpty(options.FilePath))
        {
            var directory = Path.GetDirectoryName(options.FilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                result.Errors.Add($"Directory does not exist: {directory}");
                result.IsValid = false;
            }
        }
        
        return result;
    }

    private PropertyInfo[] GetExportProperties<T>(string[]? columns)
    {
        var type = typeof(T);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        if (columns != null && columns.Length > 0)
        {
            return properties.Where(p => columns.Contains(p.Name)).ToArray();
        }
        
        return properties;
    }

    private object? FormatValue(object? value, PropertyInfo property, ExportOptions options)
    {
        if (value == null) return null;
        
        if (value is DateTime dateTime)
        {
            return dateTime.ToString(options.DateFormat);
        }
        
        if (value is decimal || value is double || value is float)
        {
            if (property.Name.ToLower().Contains("amount") || 
                property.Name.ToLower().Contains("price") || 
                property.Name.ToLower().Contains("cost"))
            {
                return Convert.ToDecimal(value).ToString(options.CurrencyFormat);
            }
            return Convert.ToDecimal(value).ToString(options.NumberFormat);
        }
        
        return value;
    }

    private string EscapeCsvValue(string value)
    {
        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }

    private string GenerateFilePath(ExportFormat format, string? title)
    {
        var fileName = $"{title ?? "Export"}_{DateTime.Now:yyyyMMdd_HHmmss}";
        var extension = format switch
        {
            ExportFormat.Pdf => ".pdf",
            ExportFormat.Excel => ".xlsx",
            ExportFormat.Csv => ".csv",
            _ => ".txt"
        };
        
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "INES_ERP_Exports");
        Directory.CreateDirectory(directory);
        
        return Path.Combine(directory, fileName + extension);
    }

    private string GenerateTextContent<T>(IEnumerable<T> data, ExportOptions options)
    {
        var content = new StringBuilder();
        
        if (!string.IsNullOrEmpty(options.Title))
        {
            content.AppendLine(options.Title);
            content.AppendLine(new string('=', options.Title.Length));
            content.AppendLine();
        }
        
        content.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        content.AppendLine();
        
        var dataList = data.ToList();
        var properties = GetExportProperties<T>(options.Columns);
        
        foreach (var item in dataList)
        {
            foreach (var property in properties)
            {
                var value = property.GetValue(item);
                var formattedValue = FormatValue(value, property, options);
                content.AppendLine($"{property.Name}: {formattedValue}");
            }
            content.AppendLine();
        }
        
        return content.ToString();
    }
}
