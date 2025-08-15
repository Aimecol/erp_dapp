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
using INES.ERP.WPF.ViewModels.Authentication;
using INES.ERP.WPF.Views.Authentication;
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

            // Show login window with dependency injection
            var loginViewModel = _host.Services.GetRequiredService<ViewModels.Authentication.LoginViewModel>();
            var loginWindow = new Views.Authentication.LoginWindow
            {
                DataContext = loginViewModel
            };
            loginWindow.Show();

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
        services.AddSingleton<NavigationService>();
        services.AddSingleton<NotificationService>();
        services.AddSingleton<ThemeService>();
        services.AddSingleton<Services.ExportService>();

        // ViewModels
        services.AddTransient<LoginViewModel>();

        // Views
        services.AddTransient<LoginWindow>();

        // AutoMapper (will be configured later)
        // services.AddAutoMapper(typeof(MappingProfile));
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
