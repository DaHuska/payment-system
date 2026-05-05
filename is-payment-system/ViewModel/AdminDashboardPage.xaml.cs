using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using is_payment_system.Model;

namespace is_payment_system.ViewModel
{
    public partial class AdminDashboardPage : Page
    {
        private readonly AdminViewModel _viewModel;

        public AdminDashboardPage(User admin)
        {
            InitializeComponent();
            _viewModel = new AdminViewModel(admin);
            DataContext = _viewModel;
            Loaded += AdminDashboardPage_Loaded;
        }

        private void AdminDashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadDashboardData();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadDashboardData();
        }

        private void DatabaseTables_Click(object sender, RoutedEventArgs e)
        {
            var window = new DatabaseTablesWindow
            {
                Owner = Window.GetWindow(this)
            };
            window.Show();
        }

        private void AddRecords_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddRecordsWindow
            {
                Owner = Window.GetWindow(this)
            };
            window.Show();
        }

        private void SearchRecords_Click(object sender, RoutedEventArgs e)
        {
            var window = new SearchRecordsWindow
            {
                Owner = Window.GetWindow(this)
            };
            window.Show();
        }

        private void Logs_Click(object sender, RoutedEventArgs e)
        {
            var window = new LogsWindow
            {
                Owner = Window.GetWindow(this)
            };
            window.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new LoginPage());
        }

        private void ViewTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.DataContext is not TransactionItemViewModel item)
                return;
            
            var transaction = _viewModel.TransactionRepository.FindTransactionById(item.TransactionId);
            var merchant = _viewModel.MerchantRepository.FindMerchantById(transaction.Recipient);
            var client = _viewModel.UserRepository.GetUserByID(transaction.Sender);
            var card = _viewModel.CardRepository.FindCardByUserId(transaction.Sender);

            var viewModel = new TransactionDetailsViewModel
            {
                Recipient = merchant.BusinessName,
                Description = $"Sender: {transaction.Sender}",
                TransactionIdText = transaction.Id.ToString(),
                Date = transaction.Timestamp.ToString("yyyy-MM-dd HH:mm"),
                PaymentMethod = card.CardNumber,
                Status = transaction.Status.ToString(),
                Notes = $"Transaction {transaction.Status}. {client.FirstName} {client.LastName} sent transaction to {merchant.BusinessName}",
                Subtotal = transaction.Amount.ToString("0.00"),
                Fee = "0.00",
                Total = transaction.Amount.ToString("0.00")
            };

            var window = new TransactionDetailsWindow(viewModel)
            {
                Owner = Window.GetWindow(this)
            };

            window.ShowDialog();
        }
    }
}