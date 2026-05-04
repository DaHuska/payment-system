using System.Windows;
using System.Windows.Controls;

namespace is_payment_system.ViewModel;

public partial class TransactionHistoryPage : Page
{
    public TransactionHistoryPage()
    {
        InitializeComponent();
    }

    private void Dashboard_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new DashboardPage());
    }

    private void Payments_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new PaymentPage());
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new LoginPage());
    }
}