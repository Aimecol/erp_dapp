using System.Windows;
using INES.ERP.WPF.ViewModels.Authentication;

namespace INES.ERP.WPF.Views.Authentication;

/// <summary>
/// Interaction logic for LoginWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        
        // Set the data context to the login view model from DI container
        DataContext = App.GetService<LoginViewModel>();
        
        // Initialize the view model
        Loaded += async (s, e) =>
        {
            if (DataContext is LoginViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        };

        // Handle password box changes
        PasswordBox.PasswordChanged += (s, e) =>
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        };
    }

    private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
        {
            DragMove();
        }
    }
}
