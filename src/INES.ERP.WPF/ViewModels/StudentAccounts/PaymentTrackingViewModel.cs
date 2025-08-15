using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace INES.ERP.WPF.ViewModels.StudentAccounts;

/// <summary>
/// ViewModel for Payment Tracking
/// </summary>
public class PaymentTrackingViewModel : BaseViewModel
{
    private ObservableCollection<PaymentItem> _payments = new();
    private PaymentItem? _selectedPayment;
    private string _searchText = string.Empty;
    private decimal _totalPayments;
    private DateTime _fromDate = DateTime.Now.AddMonths(-1);
    private DateTime _toDate = DateTime.Now;

    public PaymentTrackingViewModel()
    {
        Title = "Payment Tracking";
        
        // Initialize commands
        SearchCommand = new AsyncRelayCommand(SearchAsync);
        RecordPaymentCommand = new AsyncRelayCommand(RecordPaymentAsync);
        ViewPaymentCommand = new AsyncRelayCommand<PaymentItem>(ViewPaymentAsync);
        RefreshCommand = new AsyncRelayCommand(RefreshAsync);
        ExportCommand = new AsyncRelayCommand(ExportAsync);
        
        // Load initial data
        _ = InitializeAsync();
    }

    #region Properties

    /// <summary>
    /// Collection of payments
    /// </summary>
    public ObservableCollection<PaymentItem> Payments
    {
        get => _payments;
        set => SetProperty(ref _payments, value);
    }

    /// <summary>
    /// Currently selected payment
    /// </summary>
    public PaymentItem? SelectedPayment
    {
        get => _selectedPayment;
        set => SetProperty(ref _selectedPayment, value);
    }

    /// <summary>
    /// Search text for filtering payments
    /// </summary>
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    /// <summary>
    /// Total payments amount
    /// </summary>
    public decimal TotalPayments
    {
        get => _totalPayments;
        set => SetProperty(ref _totalPayments, value);
    }

    /// <summary>
    /// From date for filtering
    /// </summary>
    public DateTime FromDate
    {
        get => _fromDate;
        set => SetProperty(ref _fromDate, value);
    }

    /// <summary>
    /// To date for filtering
    /// </summary>
    public DateTime ToDate
    {
        get => _toDate;
        set => SetProperty(ref _toDate, value);
    }

    #endregion

    #region Commands

    public ICommand SearchCommand { get; }
    public ICommand RecordPaymentCommand { get; }
    public ICommand ViewPaymentCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ExportCommand { get; }

    #endregion

    #region Methods

    public override async Task InitializeAsync()
    {
        await LoadPaymentsAsync();
    }

    private async Task LoadPaymentsAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Simulate loading payments from database
            await Task.Delay(500);
            
            var payments = new List<PaymentItem>
            {
                new() { Id = 1, StudentId = "STU001", StudentName = "John Doe", Amount = 300000, PaymentDate = DateTime.Now.AddDays(-2), PaymentMethod = "Bank Transfer", Reference = "TXN001", Status = "Confirmed" },
                new() { Id = 2, StudentId = "STU002", StudentName = "Jane Smith", Amount = 200000, PaymentDate = DateTime.Now.AddDays(-5), PaymentMethod = "Cash", Reference = "CSH001", Status = "Confirmed" },
                new() { Id = 3, StudentId = "STU003", StudentName = "Bob Johnson", Amount = 150000, PaymentDate = DateTime.Now.AddDays(-1), PaymentMethod = "Mobile Money", Reference = "MM001", Status = "Pending" },
                new() { Id = 4, StudentId = "STU004", StudentName = "Alice Brown", Amount = 25000, PaymentDate = DateTime.Now.AddDays(-3), PaymentMethod = "Bank Transfer", Reference = "TXN002", Status = "Confirmed" }
            };

            Payments = new ObservableCollection<PaymentItem>(payments);
            CalculateTotal();
        });
    }

    private async Task SearchAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Simulate search operation
            await Task.Delay(200);
            
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadPaymentsAsync();
                return;
            }

            var filteredPayments = Payments.Where(p => 
                p.StudentName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                p.StudentId.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                p.Reference.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Payments = new ObservableCollection<PaymentItem>(filteredPayments);
            CalculateTotal();
        });
    }

    private async Task RecordPaymentAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Simulate recording a new payment
            await Task.Delay(300);
            await ShowInfoDialogAsync("Record Payment functionality will be implemented here.");
        });
    }

    private async Task ViewPaymentAsync(PaymentItem? payment)
    {
        if (payment == null) return;

        await ExecuteAsync(async () =>
        {
            // Simulate viewing payment details
            await Task.Delay(300);
            await ShowInfoDialogAsync($"Payment details for {payment.StudentName} will be displayed here.");
        });
    }

    private async Task RefreshAsync()
    {
        await LoadPaymentsAsync();
    }

    private async Task ExportAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Simulate export operation
            await Task.Delay(500);
            await ShowInfoDialogAsync("Payments exported successfully!");
        });
    }

    private void CalculateTotal()
    {
        TotalPayments = Payments.Sum(p => p.Amount);
    }

    #endregion
}

/// <summary>
/// Represents a payment item
/// </summary>
public class PaymentItem
{
    public int Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
