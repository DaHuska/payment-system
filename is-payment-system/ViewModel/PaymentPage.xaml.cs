using System.Windows;
using System.Windows.Controls;
using is_payment_system.Model;

namespace is_payment_system.ViewModel;

public partial class PaymentPage : Page
{
    private User _currentUser;
    
    // Parameterless constructor for XAML compatibility
    public PaymentPage()
    {
        InitializeComponent();
    }
    
    // Constructor that accepts user object
    public PaymentPage(User user)
    {
        InitializeComponent();
        _currentUser = user;
    }

    private void PayNow_Click(object sender, RoutedEventArgs e)
    {
        // Just pass the user object to TransactionDetailsPage
        NavigationService?.Navigate(new DashboardPage(_currentUser));
    }
}