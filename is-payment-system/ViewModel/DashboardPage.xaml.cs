using System.Windows;
using System.Windows.Controls;

namespace is_payment_system.ViewModel;

public partial class DashboardPage : Page
{
    public DashboardPage()
    {
        InitializeComponent();
    }
    
    private void Payments_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new PaymentPage());
    }

    private void MakePayment_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new PaymentPage());
    }

    private void ViewHistory_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new TransactionHistoryPage());
    }

    private void Transactions_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new TransactionHistoryPage());
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new LoginPage());
    }
}