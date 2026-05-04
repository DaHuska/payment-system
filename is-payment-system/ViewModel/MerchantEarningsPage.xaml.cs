using System.Windows;
using System.Windows.Controls;

namespace is_payment_system.ViewModel;

public partial class MerchantEarningsPage : Page
{
    public MerchantEarningsPage()
    {
        InitializeComponent();
    }
    
    private void Refunds_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new RefundPage());
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new LoginPage());
    }

    private void ViewTransaction_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new TransactionDetailsPage());
    }
    
    private void OrdersButton_Click(object sender, RoutedEventArgs e)
    {
        this.NavigationService?.Navigate(new MerchantOrderPage());
    }
    
    private void DashboardButton_Click(object sender, RoutedEventArgs e)
    {
        this.NavigationService?.Navigate(new MerchantDashboardPage());
    }
    
}