using System.Windows;
using System.Windows.Controls;
using is_payment_system.Logging;
using is_payment_system.Model;
using is_payment_system.Model.Enums;

namespace is_payment_system.ViewModel
{
    public partial class LoginPage : Page
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordWatermark.Visibility = string.IsNullOrEmpty(PasswordBox.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text?.Trim();
            var password = PasswordBox.Password;

            var user = _viewModel.Login(email, password);

            if (user == null)
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                new HashLogger().Log("ERROR", "User " + user.FirstName + " failed to log in!");
                return;
            }

            if (user.Role == UserRole.ADMIN)
            {
                NavigationService?.Navigate(new AdminDashboardPage(user));
            }
            else
            {
                NavigationService?.Navigate(new DashboardPage());
            }
            
            new HashLogger().Log("INFO", "User " + user.FirstName + " has logged in!");
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegisterPage());
        }
    }
}