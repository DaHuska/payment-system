using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Windows.Input;
using is_payment_system.Logging;
using is_payment_system.Model;
using is_payment_system.Model.Enums;
using Microsoft.AspNetCore.Identity;

namespace is_payment_system.ViewModel
{
    /// <summary>
    /// View-model behind <see cref="AddRecordsWindow"/>. Holds the form state for
    /// adding a new User, Card or Transaction, and exposes commands that delegate
    /// to the management VMs from issue #10.
    /// </summary>
    public class AddRecordsViewModel : INotifyPropertyChanged
    {
        private readonly UserManagementViewModel _userManagement;
        private readonly CardManagementViewModel _cardManagement;
        private readonly TransactionManagementViewModel _transactionManagement;

        private string _statusMessage;

        public AddRecordsViewModel()
            : this(new UserManagementViewModel(),
                   new CardManagementViewModel(),
                   new TransactionManagementViewModel())
        {
        }

        public AddRecordsViewModel(
            UserManagementViewModel userManagement,
            CardManagementViewModel cardManagement,
            TransactionManagementViewModel transactionManagement)
        {
            _userManagement = userManagement;
            _cardManagement = cardManagement;
            _transactionManagement = transactionManagement;

            AddUserCommand = new RelayCommand(AddUser);
            AddCardCommand = new RelayCommand(AddCard);
            AddTransactionCommand = new RelayCommand(AddTransaction);

            // Sensible defaults so the form never starts in a "broken" state.
            NewCardExpirationDate = DateTime.Now.AddYears(3);
            NewTransactionTimestamp = DateTime.Now;
        }

        // ─── User form ──────────────────────────────────────────────────
        public string NewUserFirstName { get; set; }
        public string NewUserLastName { get; set; }
        public string NewUserEmail { get; set; }
        public string NewUserPassword { get; set; }
        public UserRole NewUserRole { get; set; } = UserRole.USER;

        public ICommand AddUserCommand { get; }

        private void AddUser()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewUserFirstName)
                    || string.IsNullOrWhiteSpace(NewUserLastName)
                    || string.IsNullOrWhiteSpace(NewUserEmail)
                    || string.IsNullOrWhiteSpace(NewUserPassword))
                {
                    StatusMessage = "All user fields are required.";
                    return;
                }

                var user = new User
                {
                    FirstName = NewUserFirstName,
                    LastName = NewUserLastName,
                    Email = NewUserEmail,
                    Password = hashPass(NewUserPassword),
                    Role = NewUserRole,
                    DateCreated = DateTime.Now,
                    IsActive = true,
                };

                _userManagement.AddUser(user);
                StatusMessage = $"User added successfully (Id={user.Id}).";
                new HashLogger().Log("INFO", StatusMessage);
                
                ClearUserForm();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to add user: {ex.Message}";
                new HashLogger().Log("ERROR", StatusMessage);
            }
        }

        private void ClearUserForm()
        {
            NewUserFirstName = string.Empty;
            NewUserLastName = string.Empty;
            NewUserEmail = string.Empty;
            NewUserPassword = string.Empty;
            NewUserRole = UserRole.USER;

            OnPropertyChanged(nameof(NewUserFirstName));
            OnPropertyChanged(nameof(NewUserLastName));
            OnPropertyChanged(nameof(NewUserEmail));
            OnPropertyChanged(nameof(NewUserPassword));
            OnPropertyChanged(nameof(NewUserRole));
        }

        // ─── Card form ──────────────────────────────────────────────────
        public string NewCardNumber { get; set; }
        public string NewCardCvv { get; set; }
        public string NewCardIban { get; set; }
        public DateTime NewCardExpirationDate { get; set; }
        public int NewCardUserId { get; set; }

        public ICommand AddCardCommand { get; }

        private void AddCard()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewCardNumber)
                    || string.IsNullOrWhiteSpace(NewCardCvv)
                    || string.IsNullOrWhiteSpace(NewCardIban))
                {
                    StatusMessage = "Card number, CVV and IBAN are required.";
                    return;
                }

                if (NewCardUserId <= 0)
                {
                    StatusMessage = "User Id must be a positive number.";
                    return;
                }

                var card = new Card
                {
                    CardNumber = NewCardNumber,
                    CVV = NewCardCvv,
                    Iban = NewCardIban,
                    CreatedDate = DateTime.Now,
                    ExpirationDate = NewCardExpirationDate,
                };

                _cardManagement.AddCard(card, NewCardUserId);
                StatusMessage = $"Card added successfully (Id={card.Id}).";

                new HashLogger().Log("INFO", StatusMessage);
                ClearCardForm();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to add card: {ex.Message}";
                new HashLogger().Log("ERROR", StatusMessage);
            }
        }

        private void ClearCardForm()
        {
            NewCardNumber = string.Empty;
            NewCardCvv = string.Empty;
            NewCardIban = string.Empty;
            NewCardExpirationDate = DateTime.Now.AddYears(3);
            NewCardUserId = 0;

            OnPropertyChanged(nameof(NewCardNumber));
            OnPropertyChanged(nameof(NewCardCvv));
            OnPropertyChanged(nameof(NewCardIban));
            OnPropertyChanged(nameof(NewCardExpirationDate));
            OnPropertyChanged(nameof(NewCardUserId));
        }

        // ─── Transaction form ───────────────────────────────────────────
        public decimal NewTransactionAmount { get; set; }
        public DateTime NewTransactionTimestamp { get; set; }
        public TransactionStatus NewTransactionStatus { get; set; } = TransactionStatus.PENDING;
        public int NewTransactionSenderId { get; set; }
        public int NewTransactionMerchantId { get; set; }

        public ICommand AddTransactionCommand { get; }

        private void AddTransaction()
        {
            try
            {
                if (NewTransactionAmount <= 0)
                {
                    StatusMessage = "Amount must be greater than zero.";
                    return;
                }

                if (NewTransactionSenderId <= 0 || NewTransactionMerchantId <= 0)
                {
                    StatusMessage = "Sender Id and Merchant Id must be positive numbers.";
                    return;
                }

                var transaction = new Transaction
                {
                    Amount = NewTransactionAmount,
                    Timestamp = NewTransactionTimestamp,
                    Status = NewTransactionStatus,
                    Sender = NewTransactionSenderId,
                    Recipient = NewTransactionMerchantId,
                };

                _transactionManagement.AddTransaction(transaction);
                StatusMessage = $"Transaction added successfully (Id={transaction.Id}).";

                new HashLogger().Log("INFO", StatusMessage);
                ClearTransactionForm();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to add transaction: {ex.Message}";
                new HashLogger().Log("ERROR", StatusMessage);
            }
        }

        private void ClearTransactionForm()
        {
            NewTransactionAmount = 0;
            NewTransactionTimestamp = DateTime.Now;
            NewTransactionStatus = TransactionStatus.PENDING;
            NewTransactionSenderId = 0;
            NewTransactionMerchantId = 0;

            OnPropertyChanged(nameof(NewTransactionAmount));
            OnPropertyChanged(nameof(NewTransactionTimestamp));
            OnPropertyChanged(nameof(NewTransactionStatus));
            OnPropertyChanged(nameof(NewTransactionSenderId));
            OnPropertyChanged(nameof(NewTransactionMerchantId));
        }

        // ─── Status & INotifyPropertyChanged plumbing ───────────────────
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage == value) return;
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string hashPass(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
