using INES.ERP.Core.Models.Inventory;
using INES.ERP.Core.Models.Common;
using INES.ERP.Core.Enums;

namespace INES.ERP.Core.Interfaces.Services;

/// <summary>
/// Inventory service interface
/// </summary>
public interface IInventoryService
{
    #region Inventory Items

    /// <summary>
    /// Creates a new inventory item
    /// </summary>
    /// <param name="item">Inventory item to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created inventory item</returns>
    Task<InventoryItem> CreateInventoryItemAsync(InventoryItem item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing inventory item
    /// </summary>
    /// <param name="item">Inventory item to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated inventory item</returns>
    Task<InventoryItem> UpdateInventoryItemAsync(InventoryItem item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an inventory item by ID
    /// </summary>
    /// <param name="itemId">Item ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Inventory item or null if not found</returns>
    Task<InventoryItem?> GetInventoryItemByIdAsync(Guid itemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an inventory item by code
    /// </summary>
    /// <param name="itemCode">Item code</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Inventory item or null if not found</returns>
    Task<InventoryItem?> GetInventoryItemByCodeAsync(string itemCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets inventory items with pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="searchTerm">Search term</param>
    /// <param name="category">Category filter</param>
    /// <param name="status">Status filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated inventory items</returns>
    Task<PagedResult<InventoryItem>> GetInventoryItemsAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? searchTerm = null,
        string? category = null,
        Status? status = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets items below reorder level
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Items below reorder level</returns>
    Task<IEnumerable<InventoryItem>> GetItemsBelowReorderLevelAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets expired items
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Expired items</returns>
    Task<IEnumerable<InventoryItem>> GetExpiredItemsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets items expiring soon
    /// </summary>
    /// <param name="daysAhead">Days ahead to check (default: 30)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Items expiring soon</returns>
    Task<IEnumerable<InventoryItem>> GetItemsExpiringSoonAsync(int daysAhead = 30, CancellationToken cancellationToken = default);

    #endregion

    #region Stock Movements

    /// <summary>
    /// Records a stock receipt
    /// </summary>
    /// <param name="itemId">Item ID</param>
    /// <param name="storeId">Store ID</param>
    /// <param name="quantity">Quantity received</param>
    /// <param name="unitCost">Unit cost</param>
    /// <param name="documentReference">Document reference</param>
    /// <param name="reason">Reason for receipt</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Stock movement record</returns>
    Task<StockMovement> RecordStockReceiptAsync(
        Guid itemId,
        Guid storeId,
        decimal quantity,
        decimal unitCost,
        string? documentReference = null,
        string? reason = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Records a stock issue
    /// </summary>
    /// <param name="itemId">Item ID</param>
    /// <param name="storeId">Store ID</param>
    /// <param name="quantity">Quantity issued</param>
    /// <param name="documentReference">Document reference</param>
    /// <param name="reason">Reason for issue</param>
    /// <param name="party">Party receiving the stock</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Stock movement record</returns>
    Task<StockMovement> RecordStockIssueAsync(
        Guid itemId,
        Guid storeId,
        decimal quantity,
        string? documentReference = null,
        string? reason = null,
        string? party = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Records a stock transfer between stores
    /// </summary>
    /// <param name="itemId">Item ID</param>
    /// <param name="fromStoreId">Source store ID</param>
    /// <param name="toStoreId">Destination store ID</param>
    /// <param name="quantity">Quantity to transfer</param>
    /// <param name="documentReference">Document reference</param>
    /// <param name="reason">Reason for transfer</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Transfer movement records</returns>
    Task<(StockMovement IssueMovement, StockMovement ReceiptMovement)> RecordStockTransferAsync(
        Guid itemId,
        Guid fromStoreId,
        Guid toStoreId,
        decimal quantity,
        string? documentReference = null,
        string? reason = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Records a stock adjustment
    /// </summary>
    /// <param name="itemId">Item ID</param>
    /// <param name="storeId">Store ID</param>
    /// <param name="adjustmentQuantity">Adjustment quantity (positive or negative)</param>
    /// <param name="reason">Reason for adjustment</param>
    /// <param name="documentReference">Document reference</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Stock movement record</returns>
    Task<StockMovement> RecordStockAdjustmentAsync(
        Guid itemId,
        Guid storeId,
        decimal adjustmentQuantity,
        string reason,
        string? documentReference = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets stock movements for an item
    /// </summary>
    /// <param name="itemId">Item ID</param>
    /// <param name="storeId">Store ID (optional)</param>
    /// <param name="fromDate">From date (optional)</param>
    /// <param name="toDate">To date (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Stock movements</returns>
    Task<IEnumerable<StockMovement>> GetStockMovementsAsync(
        Guid itemId,
        Guid? storeId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Store Management

    /// <summary>
    /// Creates a new store
    /// </summary>
    /// <param name="store">Store to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created store</returns>
    Task<Store> CreateStoreAsync(Store store, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing store
    /// </summary>
    /// <param name="store">Store to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated store</returns>
    Task<Store> UpdateStoreAsync(Store store, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a store by ID
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Store or null if not found</returns>
    Task<Store?> GetStoreByIdAsync(Guid storeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active stores
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Active stores</returns>
    Task<IEnumerable<Store>> GetActiveStoresAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets store inventory for a specific store
    /// </summary>
    /// <param name="storeId">Store ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Store inventory items</returns>
    Task<IEnumerable<StoreInventory>> GetStoreInventoryAsync(Guid storeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current stock level for an item in a store
    /// </summary>
    /// <param name="itemId">Item ID</param>
    /// <param name="storeId">Store ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Current stock level</returns>
    Task<decimal> GetCurrentStockLevelAsync(Guid itemId, Guid storeId, CancellationToken cancellationToken = default);

    #endregion

    #region Supplier Management

    /// <summary>
    /// Creates a new supplier
    /// </summary>
    /// <param name="supplier">Supplier to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created supplier</returns>
    Task<Supplier> CreateSupplierAsync(Supplier supplier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing supplier
    /// </summary>
    /// <param name="supplier">Supplier to update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated supplier</returns>
    Task<Supplier> UpdateSupplierAsync(Supplier supplier, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a supplier by ID
    /// </summary>
    /// <param name="supplierId">Supplier ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Supplier or null if not found</returns>
    Task<Supplier?> GetSupplierByIdAsync(Guid supplierId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets suppliers with pagination and filtering
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="searchTerm">Search term</param>
    /// <param name="status">Status filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated suppliers</returns>
    Task<PagedResult<Supplier>> GetSuppliersAsync(
        int pageNumber = 1,
        int pageSize = 50,
        string? searchTerm = null,
        Status? status = null,
        CancellationToken cancellationToken = default);

    #endregion

    #region Reporting

    /// <summary>
    /// Gets inventory valuation report
    /// </summary>
    /// <param name="storeId">Store ID (optional)</param>
    /// <param name="category">Category filter (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Inventory valuation report</returns>
    Task<InventoryValuationReport> GetInventoryValuationReportAsync(
        Guid? storeId = null,
        string? category = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets stock movement report
    /// </summary>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="storeId">Store ID (optional)</param>
    /// <param name="itemId">Item ID (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Stock movement report</returns>
    Task<StockMovementReport> GetStockMovementReportAsync(
        DateTime fromDate,
        DateTime toDate,
        Guid? storeId = null,
        Guid? itemId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets reorder report
    /// </summary>
    /// <param name="storeId">Store ID (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Reorder report</returns>
    Task<ReorderReport> GetReorderReportAsync(
        Guid? storeId = null,
        CancellationToken cancellationToken = default);

    #endregion
}

/// <summary>
/// Inventory valuation report
/// </summary>
public class InventoryValuationReport
{
    public DateTime ReportDate { get; set; } = DateTime.Now;
    public Guid? StoreId { get; set; }
    public string? StoreName { get; set; }
    public string? Category { get; set; }
    public decimal TotalValue { get; set; }
    public int TotalItems { get; set; }
    public List<InventoryValuationItem> Items { get; set; } = new();
}

/// <summary>
/// Inventory valuation item
/// </summary>
public class InventoryValuationItem
{
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal CurrentStock { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalValue { get; set; }
}

/// <summary>
/// Stock movement report
/// </summary>
public class StockMovementReport
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public Guid? StoreId { get; set; }
    public string? StoreName { get; set; }
    public Guid? ItemId { get; set; }
    public string? ItemName { get; set; }
    public List<StockMovementSummary> Movements { get; set; } = new();
}

/// <summary>
/// Stock movement summary
/// </summary>
public class StockMovementSummary
{
    public DateTime MovementDate { get; set; }
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string MovementType { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalValue { get; set; }
    public string? DocumentReference { get; set; }
    public string? Reason { get; set; }
}

/// <summary>
/// Reorder report
/// </summary>
public class ReorderReport
{
    public DateTime ReportDate { get; set; } = DateTime.Now;
    public Guid? StoreId { get; set; }
    public string? StoreName { get; set; }
    public List<ReorderItem> Items { get; set; } = new();
}

/// <summary>
/// Reorder item
/// </summary>
public class ReorderItem
{
    public string ItemCode { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal CurrentStock { get; set; }
    public decimal MinimumStockLevel { get; set; }
    public decimal ReorderQuantity { get; set; }
    public decimal Shortage => Math.Max(0, MinimumStockLevel - CurrentStock);
    public string? SupplierName { get; set; }
}
