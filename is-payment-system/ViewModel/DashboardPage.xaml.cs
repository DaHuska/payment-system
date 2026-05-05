using System.Windows;
using System.Windows.Controls;
using is_payment_system.Model;

namespace is_payment_system.ViewModel;

public partial class DashboardPage : Page
{
    private User _currentUser;
    
    // Parameterless constructor for XAML compatibility
    public DashboardPage()
    {
        InitializeComponent();
    }
    
    // Constructor that accepts user object
    public DashboardPage(User user)
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
    
    private void Payments_Click(object sender, RoutedEventArgs e)
    {
        // Pass the user object to PaymentPage
        NavigationService?.Navigate(new PaymentPage(_currentUser));
    }

    private void MakePayment_Click(object sender, RoutedEventArgs e)
    {
        // Pass the user object to PaymentPage
        NavigationService?.Navigate(new PaymentPage(_currentUser));
    }

    private void ViewHistory_Click(object sender, RoutedEventArgs e)
    {
        // Pass the user object to TransactionHistoryPage
        NavigationService?.Navigate(new TransactionHistoryPage(_currentUser));
    }

    private void Transactions_Click(object sender, RoutedEventArgs e)
    {
        // Pass the user object to TransactionHistoryPage
        NavigationService?.Navigate(new TransactionHistoryPage(_currentUser));
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new LoginPage());
    }
}