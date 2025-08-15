using CommunityToolkit.Mvvm.Input;
using INES.ERP.Core.Interfaces.Services;
using INES.ERP.Core.Models.Dashboard;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace INES.ERP.WPF.ViewModels.Dashboard;

/// <summary>
/// View model for the main dashboard
/// </summary>
public partial class DashboardViewModel : BaseViewModel
{
    private readonly IDashboardService _dashboardService;
    private readonly INotificationService _notificationService;

    public DashboardViewModel(
        IDashboardService dashboardService,
        INotificationService notificationService)
    {
        _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

        Title = "Dashboard";

        // Initialize collections
        KPIs = new ObservableCollection<DashboardKPI>();
        Charts = new ObservableCollection<ChartData>();
        Alerts = new ObservableCollection<DashboardAlert>();

        // Initialize commands
        RefreshCommand = new AsyncRelayCommand(RefreshAsync);
        RefreshKPIsCommand = new AsyncRelayCommand(RefreshKPIsAsync);
        RefreshChartsCommand = new AsyncRelayCommand(RefreshChartsAsync);
        RefreshAlertsCommand = new AsyncRelayCommand(RefreshAlertsAsync);
        MarkAlertAsReadCommand = new AsyncRelayCommand<Guid>(MarkAlertAsReadAsync);
        DismissAlertCommand = new AsyncRelayCommand<Guid>(DismissAlertAsync);
        DrillDownCommand = new RelayCommand<string>(DrillDown);
    }

    #region Properties

    /// <summary>
    /// Collection of KPIs
    /// </summary>
    public ObservableCollection<DashboardKPI> KPIs { get; }

    /// <summary>
    /// Collection of chart data
    /// </summary>
    public ObservableCollection<ChartData> Charts { get; }

    /// <summary>
    /// Collection of alerts
    /// </summary>
    public ObservableCollection<DashboardAlert> Alerts { get; }

    /// <summary>
    /// Financial summary
    /// </summary>
    public FinancialSummary? FinancialSummary { get; private set; }

    /// <summary>
    /// Student accounts summary
    /// </summary>
    public StudentAccountsSummary? StudentAccountsSummary { get; private set; }

    /// <summary>
    /// Inventory summary
    /// </summary>
    public InventorySummary? InventorySummary { get; private set; }

    /// <summary>
    /// Last refresh time
    /// </summary>
    public DateTime LastRefreshTime { get; private set; } = DateTime.Now;

    /// <summary>
    /// Number of unread alerts
    /// </summary>
    public int UnreadAlertsCount => Alerts.Count(a => !a.IsRead);

    #endregion

    #region Commands

    /// <summary>
    /// Command to refresh all dashboard data
    /// </summary>
    public IAsyncRelayCommand RefreshCommand { get; }

    /// <summary>
    /// Command to refresh KPIs
    /// </summary>
    public IAsyncRelayCommand RefreshKPIsCommand { get; }

    /// <summary>
    /// Command to refresh charts
    /// </summary>
    public IAsyncRelayCommand RefreshChartsCommand { get; }

    /// <summary>
    /// Command to refresh alerts
    /// </summary>
    public IAsyncRelayCommand RefreshAlertsCommand { get; }

    /// <summary>
    /// Command to mark alert as read
    /// </summary>
    public IAsyncRelayCommand<Guid> MarkAlertAsReadCommand { get; }

    /// <summary>
    /// Command to dismiss alert
    /// </summary>
    public IAsyncRelayCommand<Guid> DismissAlertCommand { get; }

    /// <summary>
    /// Command for drill-down navigation
    /// </summary>
    public IRelayCommand<string> DrillDownCommand { get; }

    #endregion

    #region Command Implementations

    /// <summary>
    /// Refreshes all dashboard data
    /// </summary>
    private async Task RefreshAsync()
    {
        await ExecuteAsync(async () =>
        {
            await Task.WhenAll(
                LoadKPIsAsync(),
                LoadChartsAsync(),
                LoadAlertsAsync(),
                LoadSummariesAsync()
            );

            LastRefreshTime = DateTime.Now;
            OnPropertyChanged(nameof(LastRefreshTime));
            OnPropertyChanged(nameof(UnreadAlertsCount));

            _notificationService.ShowSuccess("Dashboard refreshed successfully");
        });
    }

    /// <summary>
    /// Refreshes KPIs
    /// </summary>
    private async Task RefreshKPIsAsync()
    {
        await ExecuteAsync(async () =>
        {
            await _dashboardService.RefreshKPIsAsync();
            await LoadKPIsAsync();
            _notificationService.ShowSuccess("KPIs refreshed");
        });
    }

    /// <summary>
    /// Refreshes charts
    /// </summary>
    private async Task RefreshChartsAsync()
    {
        await ExecuteAsync(async () =>
        {
            await _dashboardService.RefreshChartsAsync();
            await LoadChartsAsync();
            _notificationService.ShowSuccess("Charts refreshed");
        });
    }

    /// <summary>
    /// Refreshes alerts
    /// </summary>
    private async Task RefreshAlertsAsync()
    {
        await ExecuteAsync(async () =>
        {
            await LoadAlertsAsync();
            OnPropertyChanged(nameof(UnreadAlertsCount));
            _notificationService.ShowSuccess("Alerts refreshed");
        });
    }

    /// <summary>
    /// Marks an alert as read
    /// </summary>
    private async Task MarkAlertAsReadAsync(Guid alertId)
    {
        await ExecuteAsync(async () =>
        {
            var success = await _dashboardService.MarkAlertAsReadAsync(alertId);
            if (success)
            {
                var alert = Alerts.FirstOrDefault(a => a.Id == alertId);
                if (alert != null)
                {
                    alert.IsRead = true;
                    OnPropertyChanged(nameof(UnreadAlertsCount));
                }
            }
        });
    }

    /// <summary>
    /// Dismisses an alert
    /// </summary>
    private async Task DismissAlertAsync(Guid alertId)
    {
        await ExecuteAsync(async () =>
        {
            var success = await _dashboardService.DismissAlertAsync(alertId);
            if (success)
            {
                var alert = Alerts.FirstOrDefault(a => a.Id == alertId);
                if (alert != null)
                {
                    Alerts.Remove(alert);
                    OnPropertyChanged(nameof(UnreadAlertsCount));
                }
            }
        });
    }

    /// <summary>
    /// Handles drill-down navigation
    /// </summary>
    private void DrillDown(string? target)
    {
        if (string.IsNullOrEmpty(target)) return;

        // Navigate to specific module based on target
        switch (target.ToLower())
        {
            case "studentaccounts":
                // Navigate to student accounts
                break;
            case "financial":
                // Navigate to financial reports
                break;
            case "inventory":
                // Navigate to inventory
                break;
            default:
                _notificationService.ShowInfo($"Navigation to {target} not implemented yet");
                break;
        }
    }

    #endregion

    #region Data Loading Methods

    /// <summary>
    /// Loads KPIs from the service
    /// </summary>
    private async Task LoadKPIsAsync()
    {
        var kpis = await _dashboardService.GetKPIsAsync();
        
        KPIs.Clear();
        foreach (var kpi in kpis)
        {
            KPIs.Add(kpi);
        }
    }

    /// <summary>
    /// Loads chart data from the service
    /// </summary>
    private async Task LoadChartsAsync()
    {
        var charts = await _dashboardService.GetAllChartsAsync();
        
        Charts.Clear();
        foreach (var chart in charts)
        {
            Charts.Add(chart);
        }
    }

    /// <summary>
    /// Loads alerts from the service
    /// </summary>
    private async Task LoadAlertsAsync()
    {
        var alerts = await _dashboardService.GetAlertsAsync();
        
        Alerts.Clear();
        foreach (var alert in alerts)
        {
            Alerts.Add(alert);
        }
    }

    /// <summary>
    /// Loads summary data from the service
    /// </summary>
    private async Task LoadSummariesAsync()
    {
        var periodStart = DateTime.Now.AddMonths(-1);
        var periodEnd = DateTime.Now;

        FinancialSummary = await _dashboardService.GetFinancialSummaryAsync(periodStart, periodEnd);
        StudentAccountsSummary = await _dashboardService.GetStudentAccountsSummaryAsync();
        InventorySummary = await _dashboardService.GetInventorySummaryAsync();

        OnPropertyChanged(nameof(FinancialSummary));
        OnPropertyChanged(nameof(StudentAccountsSummary));
        OnPropertyChanged(nameof(InventorySummary));
    }

    #endregion

    #region Overrides

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        await RefreshAsync();
    }

    protected override void RefreshCommands()
    {
        RefreshCommand.NotifyCanExecuteChanged();
        RefreshKPIsCommand.NotifyCanExecuteChanged();
        RefreshChartsCommand.NotifyCanExecuteChanged();
        RefreshAlertsCommand.NotifyCanExecuteChanged();
    }

    #endregion
}
