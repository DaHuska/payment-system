using System.Windows;
using System.Windows.Controls;

namespace is_payment_system.ViewModel;

public partial class PaymentPage : Page
{
    public PaymentPage()
    {
        InitializeComponent();
    }

    private void PayNow_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new DashboardPage());
    }
}