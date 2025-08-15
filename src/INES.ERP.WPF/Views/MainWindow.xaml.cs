using System.Windows;
using INES.ERP.WPF.ViewModels;
using INES.ERP.WPF.Services;

namespace INES.ERP.WPF.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel, NavigationService navigationService)
    {
        InitializeComponent();
        DataContext = viewModel;

        // Set up navigation frame
        navigationService.SetMainFrame(MainContentFrame);

        // Navigate to dashboard by default
        _ = navigationService.NavigateToAsync("Dashboard");
    }
}
