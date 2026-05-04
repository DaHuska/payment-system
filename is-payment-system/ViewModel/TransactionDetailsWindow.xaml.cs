using System.Windows;

namespace is_payment_system.ViewModel
{
    public partial class TransactionDetailsWindow : Window
    {
        public TransactionDetailsWindow(TransactionDetailsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}