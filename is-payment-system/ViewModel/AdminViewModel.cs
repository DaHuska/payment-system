using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using is_payment_system.Model;
using is_payment_system.Model.Enums;
using is_payment_system.Repository;

namespace is_payment_system.ViewModel
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private readonly TransactionRepository _transactionRepository;
        private readonly CardRepository _cardRepository;
        private readonly MerchantRepository _merchantRepository;
        
        public TransactionRepository TransactionRepository => _transactionRepository;
        public UserRepository UserRepository => _userRepository;
        public MerchantRepository MerchantRepository => _merchantRepository;
        public CardRepository CardRepository => _cardRepository;

        private int _totalUsers;
        private string _payments;
        private string _refunds;
        private string _revenue;
        private string _adminName;
        private string _adminEmail;

        public ObservableCollection<TransactionItemViewModel> RecentTransactions { get; } = new();

        public AdminViewModel(User admin)
            : this(admin, new UserRepository(), new TransactionRepository(), new CardRepository(), new MerchantRepository())
        {
        }

        public AdminViewModel(User admin, UserRepository userRepository, TransactionRepository transactionRepository, 
            CardRepository cardRepository, MerchantRepository merchantRepository)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _cardRepository = cardRepository;
            _merchantRepository = merchantRepository;

            AdminName = admin != null ? $"{admin.FirstName} {admin.LastName}" : "Admin";
            AdminEmail = admin?.Email ?? "admin@system.com";

            LoadDashboardData();
        }

        public string AdminName
        {
            get => _adminName;
            set
            {
                if (_adminName == value) return;
                _adminName = value;
                OnPropertyChanged();
            }
        }

        public string AdminEmail
        {
            get => _adminEmail;
            set
            {
                if (_adminEmail == value) return;
                _adminEmail = value;
                OnPropertyChanged();
            }
        }

        public int TotalUsers
        {
            get => _totalUsers;
            set
            {
                if (_totalUsers == value) return;
                _totalUsers = value;
                OnPropertyChanged();
            }
        }

        public string Payments
        {
            get => _payments;
            set
            {
                if (_payments == value) return;
                _payments = value;
                OnPropertyChanged();
            }
        }

        public string Refunds
        {
            get => _refunds;
            set
            {
                if (_refunds == value) return;
                _refunds = value;
                OnPropertyChanged();
            }
        }

        public string Revenue
        {
            get => _revenue;
            set
            {
                if (_revenue == value) return;
                _revenue = value;
                OnPropertyChanged();
            }
        }

        public void LoadDashboardData()
        {
            TotalUsers = _userRepository.Users.Count;

            var transactions = _transactionRepository.Transactions
                .OrderByDescending(t => t.Timestamp)
                .ToList();

            Payments = transactions.Count.ToString();
            Refunds = transactions.Count(t => t.Status == TransactionStatus.COMPLETED).ToString();

            var revenueValue = transactions
                .Where(t => t.Status == TransactionStatus.COMPLETED)
                .Sum(t => (decimal?)t.Amount) ?? 0m;

            Revenue = revenueValue.ToString("C", CultureInfo.GetCultureInfo("en-US"));

            RecentTransactions.Clear();
            foreach (var transaction in transactions.Take(10))
            {
                var sender = _userRepository.GetUserByID(transaction.Sender);
                var senderName = sender != null
                    ? $"{sender.FirstName} {sender.LastName}"
                    : $"User #{transaction.Sender}";

                RecentTransactions.Add(new TransactionItemViewModel
                {
                    Customer = senderName,
                    Amount = transaction.Amount.ToString("C", CultureInfo.GetCultureInfo("en-US")),
                    Status = transaction.Status.ToString(),
                    StatusColor = GetStatusColor(transaction.Status),
                    Date = transaction.Timestamp.ToString("dd MMM"),
                    TransactionId = transaction.Id
                });
            }
        }

        private static string GetStatusColor(TransactionStatus status)
        {
            return status switch
            {
                TransactionStatus.COMPLETED => "#7ee787",
                TransactionStatus.PENDING => "#f9b17a",
                TransactionStatus.FAILED => "#ff7b72",
                _ => "#aab0d6"
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TransactionItemViewModel
    {
        public string Customer { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public string Date { get; set; }
        public int TransactionId { get; set; }
    }
}