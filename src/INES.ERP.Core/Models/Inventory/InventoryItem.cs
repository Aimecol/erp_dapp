using System.ComponentModel.DataAnnotations;
using INES.ERP.Core.Enums;
using INES.ERP.Core.Models.Common;

namespace INES.ERP.Core.Models.Inventory;

/// <summary>
/// Represents an inventory item
/// </summary>
public class InventoryItem : AuditableEntity
{
    /// <summary>
    /// Item code/SKU
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// Item name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    /// <summary>
    /// Item description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Item category
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Item subcategory
    /// </summary>
    [MaxLength(100)]
    public string? Subcategory { get; set; }

    /// <summary>
    /// Unit of measurement
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string UnitOfMeasurement { get; set; } = string.Empty;

    /// <summary>
    /// Barcode
    /// </summary>
    [MaxLength(50)]
    public string? Barcode { get; set; }

    /// <summary>
    /// QR code
    /// </summary>
    [MaxLength(100)]
    public string? QRCode { get; set; }

    /// <summary>
    /// Manufacturer
    /// </summary>
    [MaxLength(100)]
    public string? Manufacturer { get; set; }

    /// <summary>
    /// Brand
    /// </summary>
    [MaxLength(100)]
    public string? Brand { get; set; }

    /// <summary>
    /// Model number
    /// </summary>
    [MaxLength(100)]
    public string? ModelNumber { get; set; }

    /// <summary>
    /// Serial number (for unique items)
    /// </summary>
    [MaxLength(100)]
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Current stock quantity
    /// </summary>
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// Minimum stock level (reorder point)
    /// </summary>
    public decimal MinimumStockLevel { get; set; }

    /// <summary>
    /// Maximum stock level
    /// </summary>
    public decimal MaximumStockLevel { get; set; }

    /// <summary>
    /// Reorder quantity
    /// </summary>
    public decimal ReorderQuantity { get; set; }

    /// <summary>
    /// Unit cost
    /// </summary>
    public decimal UnitCost { get; set; }

    /// <summary>
    /// Unit selling price
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Valuation method (FIFO, LIFO, Average)
    /// </summary>
    [MaxLength(20)]
    public string ValuationMethod { get; set; } = "FIFO";

    /// <summary>
    /// Location in store
    /// </summary>
    [MaxLength(100)]
    public string? Location { get; set; }

    /// <summary>
    /// Shelf/bin location
    /// </summary>
    [MaxLength(50)]
    public string? ShelfLocation { get; set; }

    /// <summary>
    /// Expiry date (for perishable items)
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Batch number
    /// </summary>
    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Supplier ID
    /// </summary>
    public Guid? SupplierId { get; set; }

    /// <summary>
    /// Item status
    /// </summary>
    public Status ItemStatus { get; set; } = Status.Active;

    /// <summary>
    /// Indicates if item is trackable (serialized)
    /// </summary>
    public bool IsTrackable { get; set; } = false;

    /// <summary>
    /// Indicates if item is perishable
    /// </summary>
    public bool IsPerishable { get; set; } = false;

    /// <summary>
    /// Indicates if item is consumable
    /// </summary>
    public bool IsConsumable { get; set; } = true;

    /// <summary>
    /// Item image URL
    /// </summary>
    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Navigation property for stock movements
    /// </summary>
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    /// <summary>
    /// Gets the total stock value
    /// </summary>
    public decimal StockValue => CurrentStock * UnitCost;

    /// <summary>
    /// Checks if item is below reorder level
    /// </summary>
    public bool IsBelowReorderLevel => CurrentStock <= MinimumStockLevel;

    /// <summary>
    /// Checks if item is out of stock
    /// </summary>
    public bool IsOutOfStock => CurrentStock <= 0;

    /// <summary>
    /// Checks if item is expired
    /// </summary>
    public bool IsExpired => IsPerishable && ExpiryDate.HasValue && ExpiryDate.Value < DateTime.Today;

    /// <summary>
    /// Checks if item is expiring soon (within 30 days)
    /// </summary>
    public bool IsExpiringSoon => IsPerishable && ExpiryDate.HasValue && 
                                  ExpiryDate.Value >= DateTime.Today && 
                                  ExpiryDate.Value <= DateTime.Today.AddDays(30);
}

/// <summary>
/// Represents a stock movement transaction
/// </summary>
public class StockMovement : AuditableEntity
{
    /// <summary>
    /// Inventory item ID
    /// </summary>
    public Guid InventoryItemId { get; set; }

    /// <summary>
    /// Store ID
    /// </summary>
    public Guid StoreId { get; set; }

    /// <summary>
    /// Movement reference number
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string MovementReference { get; set; } = string.Empty;

    /// <summary>
    /// Movement type (Receipt, Issue, Transfer, Adjustment)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string MovementType { get; set; } = string.Empty;

    /// <summary>
    /// Quantity moved (positive for receipts, negative for issues)
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Unit cost at time of movement
    /// </summary>
    public decimal UnitCost { get; set; }

    /// <summary>
    /// Total value of movement
    /// </summary>
    public decimal TotalValue => Math.Abs(Quantity) * UnitCost;

    /// <summary>
    /// Movement date
    /// </summary>
    public DateTime MovementDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Document reference (PO, GRN, Issue Note, etc.)
    /// </summary>
    [MaxLength(50)]
    public string? DocumentReference { get; set; }

    /// <summary>
    /// Reason for movement
    /// </summary>
    [MaxLength(500)]
    public string? Reason { get; set; }

    /// <summary>
    /// Supplier/customer/department
    /// </summary>
    [MaxLength(100)]
    public string? Party { get; set; }

    /// <summary>
    /// Batch number
    /// </summary>
    [MaxLength(50)]
    public string? BatchNumber { get; set; }

    /// <summary>
    /// Expiry date for this batch
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Balance after this movement
    /// </summary>
    public decimal BalanceAfterMovement { get; set; }

    /// <summary>
    /// Movement status
    /// </summary>
    public Status MovementStatus { get; set; } = Status.Completed;

    /// <summary>
    /// Navigation property to inventory item
    /// </summary>
    public virtual InventoryItem InventoryItem { get; set; } = null!;

    /// <summary>
    /// Navigation property to store
    /// </summary>
    public virtual Store Store { get; set; } = null!;
}

/// <summary>
/// Represents a store/warehouse
/// </summary>
public class Store : AuditableEntity
{
    /// <summary>
    /// Store code
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string StoreCode { get; set; } = string.Empty;

    /// <summary>
    /// Store name
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string StoreName { get; set; } = string.Empty;

    /// <summary>
    /// Store description
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Store type (Main, Branch, Department, etc.)
    /// </summary>
    [MaxLength(50)]
    public string StoreType { get; set; } = "Main";

    /// <summary>
    /// Store location
    /// </summary>
    [MaxLength(200)]
    public string? Location { get; set; }

    /// <summary>
    /// Store manager
    /// </summary>
    [MaxLength(100)]
    public string? StoreManager { get; set; }

    /// <summary>
    /// Contact phone
    /// </summary>
    [MaxLength(20)]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// Contact email
    /// </summary>
    [EmailAddress]
    [MaxLength(100)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// Store status
    /// </summary>
    public Status StoreStatus { get; set; } = Status.Active;

    /// <summary>
    /// Navigation property for stock movements
    /// </summary>
    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    /// <summary>
    /// Navigation property for store inventory
    /// </summary>
    public virtual ICollection<StoreInventory> StoreInventory { get; set; } = new List<StoreInventory>();
}

/// <summary>
/// Represents inventory levels per store
/// </summary>
public class StoreInventory : BaseEntity
{
    /// <summary>
    /// Store ID
    /// </summary>
    public Guid StoreId { get; set; }

    /// <summary>
    /// Inventory item ID
    /// </summary>
    public Guid InventoryItemId { get; set; }

    /// <summary>
    /// Current stock in this store
    /// </summary>
    public decimal CurrentStock { get; set; }

    /// <summary>
    /// Reserved stock (allocated but not issued)
    /// </summary>
    public decimal ReservedStock { get; set; }

    /// <summary>
    /// Available stock (current - reserved)
    /// </summary>
    public decimal AvailableStock => CurrentStock - ReservedStock;

    /// <summary>
    /// Last stock count date
    /// </summary>
    public DateTime? LastStockCountDate { get; set; }

    /// <summary>
    /// Last movement date
    /// </summary>
    public DateTime? LastMovementDate { get; set; }

    /// <summary>
    /// Navigation property to store
    /// </summary>
    public virtual Store Store { get; set; } = null!;

    /// <summary>
    /// Navigation property to inventory item
    /// </summary>
    public virtual InventoryItem InventoryItem { get; set; } = null!;
}

/// <summary>
/// Represents a supplier
/// </summary>
public class Supplier : AuditableEntity
{
    /// <summary>
    /// Supplier code
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string SupplierCode { get; set; } = string.Empty;

    /// <summary>
    /// Supplier name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string SupplierName { get; set; } = string.Empty;

    /// <summary>
    /// Contact person
    /// </summary>
    [MaxLength(100)]
    public string? ContactPerson { get; set; }

    /// <summary>
    /// Contact phone
    /// </summary>
    [MaxLength(20)]
    public string? ContactPhone { get; set; }

    /// <summary>
    /// Contact email
    /// </summary>
    [EmailAddress]
    [MaxLength(100)]
    public string? ContactEmail { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    [MaxLength(500)]
    public string? Address { get; set; }

    /// <summary>
    /// Tax identification number
    /// </summary>
    [MaxLength(50)]
    public string? TaxId { get; set; }

    /// <summary>
    /// Payment terms
    /// </summary>
    [MaxLength(100)]
    public string? PaymentTerms { get; set; }

    /// <summary>
    /// Credit limit
    /// </summary>
    public decimal CreditLimit { get; set; }

    /// <summary>
    /// Supplier status
    /// </summary>
    public Status SupplierStatus { get; set; } = Status.Active;

    /// <summary>
    /// Supplier rating (1-5)
    /// </summary>
    public int Rating { get; set; } = 3;
}
