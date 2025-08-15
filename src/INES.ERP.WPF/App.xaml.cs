using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using INES.ERP.Data;
using INES.ERP.Core.Interfaces.Repositories;
using INES.ERP.Data.Repositories;
using INES.ERP.Core.Interfaces.Services;
using INES.ERP.Services.Authentication;
using INES.ERP.WPF.Services;
using INES.ERP.WPF.ViewModels;
using INES.ERP.WPF.ViewModels.Authentication;
using INES.ERP.WPF.ViewModels.Dashboard;
using INES.ERP.WPF.ViewModels.StudentAccounts;
using INES.ERP.WPF.ViewModels.ProjectAccounts;
using INES.ERP.WPF.ViewModels.Inventory;
using INES.ERP.WPF.ViewModels.Accounting;
using INES.ERP.WPF.ViewModels.Payroll;
using INES.ERP.WPF.ViewModels.Reports;
using INES.ERP.WPF.Views;
using INES.ERP.WPF.Views.Authentication;
using INES.ERP.WPF.Views.Dashboard;
using INES.ERP.WPF.Views.StudentAccounts;
using INES.ERP.WPF.Views.ProjectAccounts;
using INES.ERP.WPF.Views.Inventory;
using INES.ERP.WPF.Views.Accounting;
using INES.ERP.WPF.Views.Payroll;
using INES.ERP.WPF.Views.Reports;
using Serilog;

namespace INES.ERP.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/ines-erp-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            // Build host
            _host = CreateHostBuilder(e.Args).Build();

            // Database is already initialized
            Log.Information("Application started successfully");

            // Start host
            await _host.StartAsync();

            // Configure navigation service
            var navigationService = _host.Services.GetRequiredService<NavigationService>();
            ConfigureNavigationService(navigationService);

            // Show main window with dependency injection
            var mainWindowViewModel = _host.Services.GetRequiredService<MainWindowViewModel>();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application failed to start");
            MessageBox.Show($"Application failed to start: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host != null)
        {
            await _host.StopAsync();
            _host.Dispose();
        }

        Log.CloseAndFlush();
        base.OnExit(e);
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services, context.Configuration);
            });

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            // Default to SQL Server LocalDB for development
            connectionString = "Server=(localdb)\\mssqllocaldb;Database=INES_ERP;Trusted_Connection=true;MultipleActiveResultSets=true";
        }

        services.AddDbContext<ErpDbContext>(options =>
        {
            // Use SQLite for development (no server installation required)
            var sqliteConnectionString = "Data Source=ines_erp.db";
            options.UseSqlite(sqliteConnectionString);
            options.EnableSensitiveDataLogging(false);
            options.EnableServiceProviderCaching();
        });

        // Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Services
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<NavigationService>();
        services.AddSingleton<NotificationService>();
        services.AddSingleton<ThemeService>();
        services.AddSingleton<Services.ExportService>();

        // ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<DashboardViewModel>();

        // Student Accounts ViewModels
        services.AddTransient<StudentBillingViewModel>();
        services.AddTransient<PaymentTrackingViewModel>();
        services.AddTransient<PenaltyManagementViewModel>();
        services.AddTransient<ReceiptsStatementsViewModel>();

        // Project Accounts ViewModels
        services.AddTransient<ProjectSetupViewModel>();
        services.AddTransient<GrantManagementViewModel>();
        services.AddTransient<MilestoneDisbursementViewModel>();
        services.AddTransient<ProjectExpensesViewModel>();

        // Inventory ViewModels
        services.AddTransient<GoodsReceiptIssueViewModel>();
        services.AddTransient<StockManagementViewModel>();
        services.AddTransient<StockValuationViewModel>();
        services.AddTransient<ReorderAlertsViewModel>();

        // Accounting ViewModels
        services.AddTransient<ChartOfAccountsViewModel>();
        services.AddTransient<JournalEntriesViewModel>();
        services.AddTransient<TrialBalanceViewModel>();
        services.AddTransient<PayablesReceivablesViewModel>();

        // Payroll ViewModels
        services.AddTransient<EmployeeManagementViewModel>();
        services.AddTransient<PayslipGeneratorViewModel>();
        services.AddTransient<LeaveManagementViewModel>();
        services.AddTransient<PayrollReportsViewModel>();

        // Reports ViewModels
        services.AddTransient<FinancialReportsViewModel>();
        services.AddTransient<CustomReportsViewModel>();
        services.AddTransient<AnalyticsDashboardViewModel>();
        services.AddTransient<AuditTrailViewModel>();

        // Views
        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();
        services.AddTransient<DashboardView>();

        // Student Accounts Views
        services.AddTransient<StudentBillingView>();
        services.AddTransient<PaymentTrackingView>();
        services.AddTransient<PenaltyManagementView>();
        services.AddTransient<ReceiptsStatementsView>();

        // Project Accounts Views
        services.AddTransient<ProjectSetupView>();
        services.AddTransient<GrantManagementView>();
        services.AddTransient<MilestoneDisbursementView>();
        services.AddTransient<ProjectExpensesView>();

        // Inventory Views
        services.AddTransient<GoodsReceiptIssueView>();
        services.AddTransient<StockManagementView>();
        services.AddTransient<StockValuationView>();
        services.AddTransient<ReorderAlertsView>();

        // Accounting Views
        services.AddTransient<ChartOfAccountsView>();
        services.AddTransient<JournalEntriesView>();
        services.AddTransient<TrialBalanceView>();
        services.AddTransient<PayablesReceivablesView>();

        // Payroll Views
        services.AddTransient<EmployeeManagementView>();
        services.AddTransient<PayslipGeneratorView>();
        services.AddTransient<LeaveManagementView>();
        services.AddTransient<PayrollReportsView>();

        // Reports Views
        services.AddTransient<FinancialReportsView>();
        services.AddTransient<CustomReportsView>();
        services.AddTransient<AnalyticsDashboardView>();
        services.AddTransient<AuditTrailView>();

        // Register views with NavigationService
        RegisterNavigationViews(services);

        // AutoMapper (will be configured later)
        // services.AddAutoMapper(typeof(MappingProfile));
    }

    private static void RegisterNavigationViews(IServiceCollection services)
    {
        // This method will be called after the service provider is built
        // We'll register the views with NavigationService in OnStartup
    }

    private static void ConfigureNavigationService(NavigationService navigationService)
    {
        // Register all views with the navigation service
        navigationService.RegisterPage("Dashboard", typeof(DashboardView));

        // Student Accounts
        navigationService.RegisterPage("StudentBilling", typeof(StudentBillingView));
        navigationService.RegisterPage("PaymentTracking", typeof(PaymentTrackingView));
        navigationService.RegisterPage("PenaltyManagement", typeof(PenaltyManagementView));
        navigationService.RegisterPage("ReceiptsStatements", typeof(ReceiptsStatementsView));

        // Project Accounts
        navigationService.RegisterPage("ProjectSetup", typeof(ProjectSetupView));
        navigationService.RegisterPage("GrantManagement", typeof(GrantManagementView));
        navigationService.RegisterPage("MilestoneDisbursement", typeof(MilestoneDisbursementView));
        navigationService.RegisterPage("ProjectExpenses", typeof(ProjectExpensesView));

        // Inventory
        navigationService.RegisterPage("GoodsReceiptIssue", typeof(GoodsReceiptIssueView));
        navigationService.RegisterPage("StockManagement", typeof(StockManagementView));
        navigationService.RegisterPage("StockValuation", typeof(StockValuationView));
        navigationService.RegisterPage("ReorderAlerts", typeof(ReorderAlertsView));

        // Accounting
        navigationService.RegisterPage("ChartOfAccounts", typeof(ChartOfAccountsView));
        navigationService.RegisterPage("JournalEntries", typeof(JournalEntriesView));
        navigationService.RegisterPage("TrialBalance", typeof(TrialBalanceView));
        navigationService.RegisterPage("PayablesReceivables", typeof(PayablesReceivablesView));

        // Payroll
        navigationService.RegisterPage("EmployeeManagement", typeof(EmployeeManagementView));
        navigationService.RegisterPage("PayslipGenerator", typeof(PayslipGeneratorView));
        navigationService.RegisterPage("LeaveManagement", typeof(LeaveManagementView));
        navigationService.RegisterPage("PayrollReports", typeof(PayrollReportsView));

        // Reports
        navigationService.RegisterPage("FinancialReports", typeof(FinancialReportsView));
        navigationService.RegisterPage("CustomReports", typeof(CustomReportsView));
        navigationService.RegisterPage("AnalyticsDashboard", typeof(AnalyticsDashboardView));
        navigationService.RegisterPage("AuditTrail", typeof(AuditTrailView));
    }

    private async Task InitializeDatabaseAsync()
    {
        try
        {
            using var scope = _host!.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ErpDbContext>();
            
            Log.Information("Initializing database...");

            // Apply any pending migrations (this will create the database if it doesn't exist)
            if (context.Database.GetPendingMigrations().Any())
            {
                Log.Information("Applying pending migrations...");
                await context.Database.MigrateAsync();
            }
            else
            {
                Log.Information("Database is up to date");
            }

            Log.Information("Database initialized successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to initialize database");
            throw;
        }
    }

    public static T GetService<T>() where T : class
    {
        if (Current is App app && app._host != null)
        {
            return app._host.Services.GetRequiredService<T>();
        }
        throw new InvalidOperationException("Host is not available");
    }

    public static T? GetOptionalService<T>() where T : class
    {
        if (Current is App app && app._host != null)
        {
            return app._host.Services.GetService<T>();
        }
        return null;
    }
}
