using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using is_payment_system.Model;
using is_payment_system.Model.Enums;
using is_payment_system.Repository;

namespace is_payment_system.ViewModel;

public partial class DashboardPage : Page
{
    private User _currentUser;
    private TransactionRepository _transactionRepo;
    private CardRepository _cardRepo;
    private MerchantRepository _merchantRepo;
    
    public ObservableCollection<RecentTransactionItem> RecentTransactions { get; set; }
    
    public DashboardPage()
    {
        InitializeComponent();
        _transactionRepo = new TransactionRepository();
        _cardRepo = new CardRepository();
        _merchantRepo = new MerchantRepository();
        RecentTransactions = new ObservableCollection<RecentTransactionItem>();
        DataContext = this;
    }
    
    public DashboardPage(User user)
    {
        InitializeComponent();
        _currentUser = user;
        _transactionRepo = new TransactionRepository();
        _cardRepo = new CardRepository();
        _merchantRepo = new MerchantRepository();
        RecentTransactions = new ObservableCollection<RecentTransactionItem>();
        DataContext = this;
        LoadUserData();
        LoadPaymentStats();
        LoadPendingStats();
        LoadBalance();
        LoadRecentTransactions();
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
        if (_currentUser != null)
        {
            var card = _cardRepo.FindCardByUserId(_currentUser.Id);
            if (card != null)
            {
                decimal balance = card.Balance;
                BalanceText.Text = $"{balance:F2}";
            }
            else
            {
                BalanceText.Text = "0.00";
            }
        }
    }
    
    private void LoadRecentTransactions()
    {
        if (_currentUser == null) return;
        
        var transactions = _transactionRepo.FindTransactionsBySenderId(_currentUser.Id);
        var recentTransactions = transactions.Take(5).ToList();
        
        RecentTransactions.Clear();
        
        foreach (var transaction in recentTransactions)
        {
            var merchant = _merchantRepo.FindMerchantById(transaction.Recipient);
            string merchantName = merchant != null ? merchant.BusinessName : $"Merchant #{transaction.Recipient}";
            string amountFormatted = transaction.Amount.ToString("F2");
            string amountDisplay = $"-{amountFormatted} BGN";
            
            RecentTransactions.Add(new RecentTransactionItem
            {
                MerchantName = merchantName,
                Amount = amountDisplay,
                AmountValue = transaction.Amount,
                IsPositive = false,
                TransactionDate = transaction.Timestamp,
                Status = transaction.Status
            });
        }
        
        TransactionsList.ItemsSource = null;
        TransactionsList.ItemsSource = RecentTransactions;
        
        if (RecentTransactions.Count == 0)
        {
            var emptyItem = new RecentTransactionItem
            {
                MerchantName = "No transactions yet",
                Amount = "Make a payment to get started",
                IsPositive = true
            };
            RecentTransactions.Add(emptyItem);
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

public class RecentTransactionItem
{
    public string MerchantName { get; set; }
    public string Amount { get; set; }
    public decimal AmountValue { get; set; }
    public bool IsPositive { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionStatus Status { get; set; }
}