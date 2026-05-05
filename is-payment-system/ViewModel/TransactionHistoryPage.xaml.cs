﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using is_payment_system.Model;
using is_payment_system.Model.Enums;
using is_payment_system.Repository;

namespace is_payment_system.ViewModel;

public partial class TransactionHistoryPage : Page
{
    private User _currentUser;
    private TransactionRepository _transactionRepository;
    private MerchantRepository _merchantRepository;
    
    public ObservableCollection<TransactionDisplay> Transactions { get; set; }
    
    public TransactionHistoryPage()
    {
        InitializeComponent();
        _transactionRepository = new TransactionRepository();
        _merchantRepository = new MerchantRepository();
        Transactions = new ObservableCollection<TransactionDisplay>();
        DataContext = this;
    }
    
    public TransactionHistoryPage(User user) : this()
    {
        _currentUser = user;
        LoadUserData();
        LoadUserTransactions();
    }
    
    private void LoadUserData()
    {
        if (_currentUser != null)
        {
            WelcomeText.Text = $"Welcome, {_currentUser.FirstName ?? "User"}";
            EmailText.Text = _currentUser.Email;
        }
    }
    
    private void LoadUserTransactions()
    {
        if (_currentUser == null) return;
        
        var transactions = _transactionRepository.FindTransactionsBySenderId(_currentUser.Id);
        
        Transactions.Clear();
        
        foreach (var transaction in transactions)
        {
            var merchant = _merchantRepository.FindMerchantById(transaction.Recipient);
            Transactions.Add(new TransactionDisplay
            {
                Id = transaction.Id,
                Date = transaction.Timestamp.ToString("yyyy-MM-dd HH:mm"),
                Amount = $"{transaction.Amount:F2} BGN",
                Recipient = merchant.BusinessName,
                Status = GetStatusText(transaction.Status),
                StatusColor = GetStatusColor(transaction.Status)
            });
        }
    }
    
    private string GetStatusText(TransactionStatus status)
    {
        return status switch
        {
            TransactionStatus.COMPLETED => "Completed",
            TransactionStatus.PENDING => "Pending",
            TransactionStatus.FAILED => "Failed",
            _ => "Unknown"
        };
    }
    
    private string GetStatusColor(TransactionStatus status)
    {
        return status switch
        {
            TransactionStatus.COMPLETED => "#4CAF50",
            TransactionStatus.PENDING => "#FFC107",
            TransactionStatus.FAILED => "#F44336",
            _ => "#9E9E9E"
        };
    }
    
    private void FilterTransactions(string statusFilter)
    {
        if (_currentUser == null) return;
        
        var transactions = _transactionRepository.FindTransactionsBySenderId(_currentUser.Id);
        
        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
        {
            var statusEnum = statusFilter.ToUpper() switch
            {
                "COMPLETED" => TransactionStatus.COMPLETED,
                "PENDING" => TransactionStatus.PENDING,
                "FAILED" => TransactionStatus.FAILED,
                _ => (TransactionStatus?)null
            };
            
            if (statusEnum.HasValue)
            {
                transactions = transactions.Where(t => t.Status == statusEnum.Value).ToList();
            }
        }
        
        Transactions.Clear();
        
        foreach (var transaction in transactions)
        {
            Transactions.Add(new TransactionDisplay
            {
                Id = transaction.Id,
                Date = transaction.Timestamp.ToString("yyyy-MM-dd HH:mm"),
                Amount = $"{transaction.Amount:F2} BGN",
                Status = GetStatusText(transaction.Status),
                StatusColor = GetStatusColor(transaction.Status)
            });
        }
    }

    private void Dashboard_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new DashboardPage(_currentUser));
    }

    private void Payments_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new PaymentPage(_currentUser));
    }
    
    private void AllFilter_Click(object sender, RoutedEventArgs e)
    {
        FilterTransactions("All");
        UpdateActiveFilterButton(sender as Button);
    }
    
    private void CompletedFilter_Click(object sender, RoutedEventArgs e)
    {
        FilterTransactions("COMPLETED");
        UpdateActiveFilterButton(sender as Button);
    }
    
    private void PendingFilter_Click(object sender, RoutedEventArgs e)
    {
        FilterTransactions("PENDING");
        UpdateActiveFilterButton(sender as Button);
    }
    
    private void FailedFilter_Click(object sender, RoutedEventArgs e)
    {
        FilterTransactions("FAILED");
        UpdateActiveFilterButton(sender as Button);
    }
    
    private void UpdateActiveFilterButton(Button clickedButton)
    {
        var filterButtons = new[] { AllFilterBtn, CompletedFilterBtn, PendingFilterBtn, FailedFilterBtn };
        foreach (var btn in filterButtons)
        {
            if (btn != null)
            {
                btn.Style = (Style)FindResource("FilterButtonStyle");
            }
        }
        
        if (clickedButton != null)
        {
            clickedButton.Style = (Style)FindResource("ActiveFilterButtonStyle");
        }
    }

    private void Logout_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new LoginPage());
    }
}

public class TransactionDisplay
{
    public int Id { get; set; }
    public string Date { get; set; }
    public string Recipient { get; set; }
    public string Amount { get; set; }
    public string Status { get; set; }
    public string StatusColor { get; set; }
}