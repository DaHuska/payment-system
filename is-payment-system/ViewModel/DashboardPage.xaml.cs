using System.Windows;
using System.Windows.Controls;
using is_payment_system.Model;
using is_payment_system.Repository;

namespace is_payment_system.ViewModel;

public partial class DashboardPage : Page
{
    private User _currentUser;
    private TransactionRepository _transactionRepo;
    private CardRepository _cardRepo;
    
    public DashboardPage()
    {
        InitializeComponent();
        _transactionRepo = new TransactionRepository();
        _cardRepo = new CardRepository();
    }
    
    public DashboardPage(User user)
    {
        InitializeComponent();
        _currentUser = user;
        _transactionRepo = new TransactionRepository();
        _cardRepo = new CardRepository();
        LoadUserData();
        LoadPaymentStats();
        LoadPendingStats();
        LoadBalance();
    }
    
    private void LoadUserData()
    {
        if (_currentUser != null)
        {
            WelcomeText.Text = $"Welcome, {_currentUser.FirstName ?? "User"}";
            
            EmailText.Text = _currentUser.Email;
        }
    }
    
    private void LoadPaymentStats()
    {
        if (_currentUser != null)
        {
            int paymentCount = _transactionRepo.CountPaymentsByUser(_currentUser.Id);
            
            PaymentsCountText.Text = paymentCount.ToString();
        }
    }
    
    private void LoadPendingStats()
    {
        if (_currentUser != null)
        {
            int pendingCount = _transactionRepo.CountPendingPaymentsByUser(_currentUser.Id);
            
            PendingCountText.Text = pendingCount.ToString();
        }
    }
    
    private void LoadBalance()
    {
        int userid = _currentUser.Id;
        
        if (_currentUser != null)
        {
            var card = _cardRepo.FindCardByUserId(_currentUser.Id);
            decimal balance = card.Balance;
            
            BalanceText.Text = balance.ToString();
        }
    }
    
    private void Payments_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new PaymentPage(_currentUser));
    }

    private void MakePayment_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new PaymentPage(_currentUser));
    }

    private void ViewHistory_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new TransactionHistoryPage(_currentUser));
    }

    private void Transactions_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new TransactionHistoryPage(_currentUser));
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new LoginPage());
    }
}