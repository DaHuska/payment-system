using System.Windows;
using System.Windows.Controls;

namespace is_payment_system.ViewModel
{
    public partial class RegisterPage : Page
    {
        private readonly AddRecordsViewModel _viewModel;

        public RegisterPage()
        {
            InitializeComponent();
            _viewModel = new AddRecordsViewModel();
            DataContext = _viewModel;

            PasswordWatermark.Visibility = string.IsNullOrEmpty(PasswordBox.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordWatermark.Visibility = string.IsNullOrEmpty(PasswordBox.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;

            _viewModel.NewUserPassword = PasswordBox.Password;
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.AddUserCommand.Execute(null);

            if (!string.IsNullOrWhiteSpace(_viewModel.StatusMessage) &&
                _viewModel.StatusMessage.StartsWith("User added successfully"))
            {
                NavigationService?.Navigate(new DashboardPage());
            }
            else
            {
                MessageBox.Show(_viewModel.StatusMessage, "Registration", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
            else
                NavigationService?.Navigate(new LoginPage());
        }
    }
}