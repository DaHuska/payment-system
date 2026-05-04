using System.Windows;
using System.Windows.Controls;

namespace is_payment_system.ViewModel;

public partial class TransactionDetailsPage : Page
{
    public TransactionDetailsPage()
    {
        InitializeComponent();
    }
    
    private void Dashboard_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new DashboardPage());
    }

    private void Transactions_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new TransactionHistoryPage());
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new LoginPage());
    }

    private void BackToTransactions_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new TransactionHistoryPage());
    }
}