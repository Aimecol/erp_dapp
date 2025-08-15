using CommunityToolkit.Mvvm.Input;
using INES.ERP.WPF.Services;
using System.Windows.Input;

namespace INES.ERP.WPF.ViewModels;

/// <summary>
/// ViewModel for the main window
/// </summary>
public class MainWindowViewModel : BaseViewModel
{
    private readonly NavigationService _navigationService;
    private string _currentViewTitle = "Dashboard";

    public MainWindowViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        Title = "INES-Ruhengeri ERP System";
        
        // Initialize navigation commands
        NavigateToDashboardCommand = new AsyncRelayCommand(NavigateToDashboardAsync);
        
        // Student Accounts Commands
        NavigateToStudentBillingCommand = new AsyncRelayCommand(NavigateToStudentBillingAsync);
        NavigateToPaymentTrackingCommand = new AsyncRelayCommand(NavigateToPaymentTrackingAsync);
        NavigateToPenaltyManagementCommand = new AsyncRelayCommand(NavigateToPenaltyManagementAsync);
        NavigateToReceiptsStatementsCommand = new AsyncRelayCommand(NavigateToReceiptsStatementsAsync);
        
        // Project Accounts Commands
        NavigateToProjectSetupCommand = new AsyncRelayCommand(NavigateToProjectSetupAsync);
        NavigateToGrantManagementCommand = new AsyncRelayCommand(NavigateToGrantManagementAsync);
        NavigateToMilestoneDisbursementCommand = new AsyncRelayCommand(NavigateToMilestoneDisbursementAsync);
        NavigateToProjectExpensesCommand = new AsyncRelayCommand(NavigateToProjectExpensesAsync);
        
        // Stores & Inventory Commands
        NavigateToGoodsReceiptIssueCommand = new AsyncRelayCommand(NavigateToGoodsReceiptIssueAsync);
        NavigateToStockManagementCommand = new AsyncRelayCommand(NavigateToStockManagementAsync);
        NavigateToStockValuationCommand = new AsyncRelayCommand(NavigateToStockValuationAsync);
        NavigateToReorderAlertsCommand = new AsyncRelayCommand(NavigateToReorderAlertsAsync);
        
        // Accounting Commands
        NavigateToChartOfAccountsCommand = new AsyncRelayCommand(NavigateToChartOfAccountsAsync);
        NavigateToJournalEntriesCommand = new AsyncRelayCommand(NavigateToJournalEntriesAsync);
        NavigateToTrialBalanceCommand = new AsyncRelayCommand(NavigateToTrialBalanceAsync);
        NavigateToPayablesReceivablesCommand = new AsyncRelayCommand(NavigateToPayablesReceivablesAsync);
        
        // Payroll & HR Commands
        NavigateToEmployeeManagementCommand = new AsyncRelayCommand(NavigateToEmployeeManagementAsync);
        NavigateToPayslipGeneratorCommand = new AsyncRelayCommand(NavigateToPayslipGeneratorAsync);
        NavigateToLeaveManagementCommand = new AsyncRelayCommand(NavigateToLeaveManagementAsync);
        NavigateToPayrollReportsCommand = new AsyncRelayCommand(NavigateToPayrollReportsAsync);
        
        // Reports & Analytics Commands
        NavigateToFinancialReportsCommand = new AsyncRelayCommand(NavigateToFinancialReportsAsync);
        NavigateToCustomReportsCommand = new AsyncRelayCommand(NavigateToCustomReportsAsync);
        NavigateToAnalyticsDashboardCommand = new AsyncRelayCommand(NavigateToAnalyticsDashboardAsync);
        NavigateToAuditTrailCommand = new AsyncRelayCommand(NavigateToAuditTrailAsync);
    }

    /// <summary>
    /// Current view title
    /// </summary>
    public string CurrentViewTitle
    {
        get => _currentViewTitle;
        set => SetProperty(ref _currentViewTitle, value);
    }

    #region Navigation Commands

    // Dashboard
    public ICommand NavigateToDashboardCommand { get; }

    // Student Accounts
    public ICommand NavigateToStudentBillingCommand { get; }
    public ICommand NavigateToPaymentTrackingCommand { get; }
    public ICommand NavigateToPenaltyManagementCommand { get; }
    public ICommand NavigateToReceiptsStatementsCommand { get; }

    // Project Accounts
    public ICommand NavigateToProjectSetupCommand { get; }
    public ICommand NavigateToGrantManagementCommand { get; }
    public ICommand NavigateToMilestoneDisbursementCommand { get; }
    public ICommand NavigateToProjectExpensesCommand { get; }

    // Stores & Inventory
    public ICommand NavigateToGoodsReceiptIssueCommand { get; }
    public ICommand NavigateToStockManagementCommand { get; }
    public ICommand NavigateToStockValuationCommand { get; }
    public ICommand NavigateToReorderAlertsCommand { get; }

    // Accounting
    public ICommand NavigateToChartOfAccountsCommand { get; }
    public ICommand NavigateToJournalEntriesCommand { get; }
    public ICommand NavigateToTrialBalanceCommand { get; }
    public ICommand NavigateToPayablesReceivablesCommand { get; }

    // Payroll & HR
    public ICommand NavigateToEmployeeManagementCommand { get; }
    public ICommand NavigateToPayslipGeneratorCommand { get; }
    public ICommand NavigateToLeaveManagementCommand { get; }
    public ICommand NavigateToPayrollReportsCommand { get; }

    // Reports & Analytics
    public ICommand NavigateToFinancialReportsCommand { get; }
    public ICommand NavigateToCustomReportsCommand { get; }
    public ICommand NavigateToAnalyticsDashboardCommand { get; }
    public ICommand NavigateToAuditTrailCommand { get; }

    #endregion

    #region Navigation Methods

    private async Task NavigateToDashboardAsync()
    {
        System.Diagnostics.Debug.WriteLine("NavigateToDashboardAsync called");
        await _navigationService.NavigateToAsync("Dashboard");
        CurrentViewTitle = "Dashboard";
    }

    // Student Accounts
    private async Task NavigateToStudentBillingAsync()
    {
        System.Diagnostics.Debug.WriteLine("NavigateToStudentBillingAsync called");
        await _navigationService.NavigateToAsync("StudentBilling");
        CurrentViewTitle = "Student Billing";
    }

    private async Task NavigateToPaymentTrackingAsync()
    {
        await _navigationService.NavigateToAsync("PaymentTracking");
        CurrentViewTitle = "Payment Tracking";
    }

    private async Task NavigateToPenaltyManagementAsync()
    {
        await _navigationService.NavigateToAsync("PenaltyManagement");
        CurrentViewTitle = "Penalty Management";
    }

    private async Task NavigateToReceiptsStatementsAsync()
    {
        await _navigationService.NavigateToAsync("ReceiptsStatements");
        CurrentViewTitle = "Receipts & Statements";
    }

    // Project Accounts
    private async Task NavigateToProjectSetupAsync()
    {
        await _navigationService.NavigateToAsync("ProjectSetup");
        CurrentViewTitle = "Project Setup";
    }

    private async Task NavigateToGrantManagementAsync()
    {
        await _navigationService.NavigateToAsync("GrantManagement");
        CurrentViewTitle = "Grant Management";
    }

    private async Task NavigateToMilestoneDisbursementAsync()
    {
        await _navigationService.NavigateToAsync("MilestoneDisbursement");
        CurrentViewTitle = "Milestone Disbursement";
    }

    private async Task NavigateToProjectExpensesAsync()
    {
        await _navigationService.NavigateToAsync("ProjectExpenses");
        CurrentViewTitle = "Project Expenses";
    }

    // Stores & Inventory
    private async Task NavigateToGoodsReceiptIssueAsync()
    {
        await _navigationService.NavigateToAsync("GoodsReceiptIssue");
        CurrentViewTitle = "Goods Receipt/Issue";
    }

    private async Task NavigateToStockManagementAsync()
    {
        await _navigationService.NavigateToAsync("StockManagement");
        CurrentViewTitle = "Stock Management";
    }

    private async Task NavigateToStockValuationAsync()
    {
        await _navigationService.NavigateToAsync("StockValuation");
        CurrentViewTitle = "Stock Valuation";
    }

    private async Task NavigateToReorderAlertsAsync()
    {
        await _navigationService.NavigateToAsync("ReorderAlerts");
        CurrentViewTitle = "Reorder Alerts";
    }

    // Accounting
    private async Task NavigateToChartOfAccountsAsync()
    {
        await _navigationService.NavigateToAsync("ChartOfAccounts");
        CurrentViewTitle = "Chart of Accounts";
    }

    private async Task NavigateToJournalEntriesAsync()
    {
        await _navigationService.NavigateToAsync("JournalEntries");
        CurrentViewTitle = "Journal Entries";
    }

    private async Task NavigateToTrialBalanceAsync()
    {
        await _navigationService.NavigateToAsync("TrialBalance");
        CurrentViewTitle = "Trial Balance";
    }

    private async Task NavigateToPayablesReceivablesAsync()
    {
        await _navigationService.NavigateToAsync("PayablesReceivables");
        CurrentViewTitle = "Payables & Receivables";
    }

    // Payroll & HR
    private async Task NavigateToEmployeeManagementAsync()
    {
        await _navigationService.NavigateToAsync("EmployeeManagement");
        CurrentViewTitle = "Employee Management";
    }

    private async Task NavigateToPayslipGeneratorAsync()
    {
        await _navigationService.NavigateToAsync("PayslipGenerator");
        CurrentViewTitle = "Payslip Generator";
    }

    private async Task NavigateToLeaveManagementAsync()
    {
        await _navigationService.NavigateToAsync("LeaveManagement");
        CurrentViewTitle = "Leave Management";
    }

    private async Task NavigateToPayrollReportsAsync()
    {
        await _navigationService.NavigateToAsync("PayrollReports");
        CurrentViewTitle = "Payroll Reports";
    }

    // Reports & Analytics
    private async Task NavigateToFinancialReportsAsync()
    {
        await _navigationService.NavigateToAsync("FinancialReports");
        CurrentViewTitle = "Financial Reports";
    }

    private async Task NavigateToCustomReportsAsync()
    {
        await _navigationService.NavigateToAsync("CustomReports");
        CurrentViewTitle = "Custom Reports";
    }

    private async Task NavigateToAnalyticsDashboardAsync()
    {
        await _navigationService.NavigateToAsync("AnalyticsDashboard");
        CurrentViewTitle = "Analytics Dashboard";
    }

    private async Task NavigateToAuditTrailAsync()
    {
        await _navigationService.NavigateToAsync("AuditTrail");
        CurrentViewTitle = "Audit Trail";
    }

    #endregion
}
