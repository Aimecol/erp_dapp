using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace INES.ERP.WPF.ViewModels.StudentAccounts;

/// <summary>
/// ViewModel for Student Billing management
/// </summary>
public class StudentBillingViewModel : BaseViewModel
{
    private ObservableCollection<StudentBillItem> _bills = new();
    private StudentBillItem? _selectedBill;
    private string _searchText = string.Empty;
    private decimal _totalOutstanding;
    private decimal _totalPaid;

    public StudentBillingViewModel()
    {
        Title = "Student Billing";
        
        // Initialize commands
        SearchCommand = new AsyncRelayCommand(SearchAsync);
        CreateBillCommand = new AsyncRelayCommand(CreateBillAsync);
        EditBillCommand = new AsyncRelayCommand<StudentBillItem>(EditBillAsync);
        DeleteBillCommand = new AsyncRelayCommand<StudentBillItem>(DeleteBillAsync);
        RefreshCommand = new AsyncRelayCommand(RefreshAsync);
        ExportCommand = new AsyncRelayCommand(ExportAsync);
        
        // Load initial data
        _ = InitializeAsync();
    }

    #region Properties

    /// <summary>
    /// Collection of student bills
    /// </summary>
    public ObservableCollection<StudentBillItem> Bills
    {
        get => _bills;
        set => SetProperty(ref _bills, value);
    }

    /// <summary>
    /// Currently selected bill
    /// </summary>
    public StudentBillItem? SelectedBill
    {
        get => _selectedBill;
        set => SetProperty(ref _selectedBill, value);
    }

    /// <summary>
    /// Search text for filtering bills
    /// </summary>
    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    /// <summary>
    /// Total outstanding amount
    /// </summary>
    public decimal TotalOutstanding
    {
        get => _totalOutstanding;
        set => SetProperty(ref _totalOutstanding, value);
    }

    /// <summary>
    /// Total paid amount
    /// </summary>
    public decimal TotalPaid
    {
        get => _totalPaid;
        set => SetProperty(ref _totalPaid, value);
    }

    #endregion

    #region Commands

    public ICommand SearchCommand { get; }
    public ICommand CreateBillCommand { get; }
    public ICommand EditBillCommand { get; }
    public ICommand DeleteBillCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand ExportCommand { get; }

    #endregion

    #region Methods

    public override async Task InitializeAsync()
    {
        await LoadBillsAsync();
    }

    private async Task LoadBillsAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Simulate loading bills from database
            await Task.Delay(500);
            
            var bills = new List<StudentBillItem>
            {
                new() { Id = 1, StudentId = "STU001", StudentName = "John Doe", BillType = "Tuition", Amount = 500000, PaidAmount = 300000, DueDate = DateTime.Now.AddDays(30), Status = "Partial" },
                new() { Id = 2, StudentId = "STU002", StudentName = "Jane Smith", BillType = "Accommodation", Amount = 200000, PaidAmount = 200000, DueDate = DateTime.Now.AddDays(-5), Status = "Paid" },
                new() { Id = 3, StudentId = "STU003", StudentName = "Bob Johnson", BillType = "Tuition", Amount = 500000, PaidAmount = 0, DueDate = DateTime.Now.AddDays(15), Status = "Unpaid" },
                new() { Id = 4, StudentId = "STU004", StudentName = "Alice Brown", BillType = "Lab Fees", Amount = 50000, PaidAmount = 25000, DueDate = DateTime.Now.AddDays(20), Status = "Partial" }
            };

            Bills = new ObservableCollection<StudentBillItem>(bills);
            CalculateTotals();
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
                await LoadBillsAsync();
                return;
            }

            var filteredBills = Bills.Where(b => 
                b.StudentName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                b.StudentId.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                b.BillType.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            Bills = new ObservableCollection<StudentBillItem>(filteredBills);
            CalculateTotals();
        });
    }

    private async Task CreateBillAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Simulate creating a new bill
            await Task.Delay(300);
            await ShowInfoDialogAsync("Create Bill functionality will be implemented here.");
        });
    }

    private async Task EditBillAsync(StudentBillItem? bill)
    {
        if (bill == null) return;

        await ExecuteAsync(async () =>
        {
            // Simulate editing a bill
            await Task.Delay(300);
            await ShowInfoDialogAsync($"Edit Bill functionality for {bill.StudentName} will be implemented here.");
        });
    }

    private async Task DeleteBillAsync(StudentBillItem? bill)
    {
        if (bill == null) return;

        var confirmed = await ShowConfirmationDialogAsync($"Are you sure you want to delete the bill for {bill.StudentName}?");
        if (!confirmed) return;

        await ExecuteAsync(async () =>
        {
            // Simulate deleting a bill
            await Task.Delay(300);
            Bills.Remove(bill);
            CalculateTotals();
        });
    }

    private async Task RefreshAsync()
    {
        await LoadBillsAsync();
    }

    private async Task ExportAsync()
    {
        await ExecuteAsync(async () =>
        {
            // Simulate export operation
            await Task.Delay(500);
            await ShowInfoDialogAsync("Bills exported successfully!");
        });
    }

    private void CalculateTotals()
    {
        TotalOutstanding = Bills.Sum(b => b.Amount - b.PaidAmount);
        TotalPaid = Bills.Sum(b => b.PaidAmount);
    }

    #endregion
}

/// <summary>
/// Represents a student bill item
/// </summary>
public class StudentBillItem
{
    public int Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string BillType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal OutstandingAmount => Amount - PaidAmount;
}
