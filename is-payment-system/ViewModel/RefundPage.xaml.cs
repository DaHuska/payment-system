using System.Windows;
using System.Windows.Controls;

namespace is_payment_system.ViewModel;

public partial class RefundPage : Page
{
    public RefundPage()
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

    private void Transactions_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new TransactionHistoryPage());
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new LoginPage());
    }

    private void CancelRequest_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new DashboardPage());
    }

    private void BackToPaymentDetails_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new TransactionDetailsPage());
    }
}