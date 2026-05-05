using System.Windows;
using System.Windows.Controls;
using is_payment_system.Model;

namespace is_payment_system.ViewModel;

public partial class TransactionHistoryPage : Page
{
    private User _currentUser;
    
    // Parameterless constructor for XAML compatibility
    public TransactionHistoryPage()
    {
        InitializeComponent();
    }
    
    // Constructor that accepts user object
    public TransactionHistoryPage(User user)
    {
        InitializeComponent();
        _currentUser = user;
        LoadUserData();
    }
    
    private void LoadUserData()
    {
        if (_currentUser != null)
        {
            // Update the welcome message with the user's name
            WelcomeText.Text = $"Welcome, {_currentUser.FirstName ?? "User"}";
            
            // Update the email field
            EmailText.Text = _currentUser.Email;
        }
    }

    private void Dashboard_Click(object sender, RoutedEventArgs e)
    {
        // Pass the user object back to DashboardPage
        NavigationService?.Navigate(new DashboardPage(_currentUser));
    }

    private void Payments_Click(object sender, RoutedEventArgs e)
    {
        // Pass the user object to PaymentPage
        NavigationService?.Navigate(new PaymentPage(_currentUser));
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new LoginPage());
    }
}