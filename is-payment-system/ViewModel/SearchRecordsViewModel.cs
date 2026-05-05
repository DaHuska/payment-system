using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using is_payment_system.Logging;
using is_payment_system.Model;

namespace is_payment_system.ViewModel
{
    /// <summary>
    /// View-model behind <see cref="SearchRecordsWindow"/>. Holds the search
    /// criteria for each entity and exposes commands that delegate to the
    /// management VMs from issue #10. Results are placed in ObservableCollections
    /// that the DataGrids in the view bind to.
    /// </summary>
    public class SearchRecordsViewModel : INotifyPropertyChanged
    {
        private readonly UserManagementViewModel _userManagement;
        private readonly CardManagementViewModel _cardManagement;
        private readonly TransactionManagementViewModel _transactionManagement;

        private string _statusMessage;

        public ObservableCollection<User> UserResults { get; }
        public ObservableCollection<Card> CardResults { get; }
        public ObservableCollection<Transaction> TransactionResults { get; }

        public SearchRecordsViewModel()
            : this(new UserManagementViewModel(),
                   new CardManagementViewModel(),
                   new TransactionManagementViewModel())
        {
        }

        public SearchRecordsViewModel(
            UserManagementViewModel userManagement,
            CardManagementViewModel cardManagement,
            TransactionManagementViewModel transactionManagement)
        {
            _userManagement = userManagement;
            _cardManagement = cardManagement;
            _transactionManagement = transactionManagement;

            UserResults = new ObservableCollection<User>();
            CardResults = new ObservableCollection<Card>();
            TransactionResults = new ObservableCollection<Transaction>();

            SearchUsersCommand = new RelayCommand(SearchUsers);
            SearchCardsCommand = new RelayCommand(SearchCards);
            SearchTransactionsCommand = new RelayCommand(SearchTransactions);
        }

        public string SearchUserEmail { get; set; }
        public string SearchUserId { get; set; }

        public ICommand SearchUsersCommand { get; }

        private void SearchUsers()
        {
            try
            {
                UserResults.Clear();

                if (!string.IsNullOrWhiteSpace(SearchUserEmail))
                {
                    var byEmail = _userManagement.FindUserByEmail(SearchUserEmail.Trim());
                    if (byEmail != null) UserResults.Add(byEmail);
                }
                else if (!string.IsNullOrWhiteSpace(SearchUserId)
                         && int.TryParse(SearchUserId, out var id))
                {
                    var byId = _userManagement.FindUserById(id);
                    if (byId != null) UserResults.Add(byId);
                }
                else
                {
                    // Empty criteria — show all users
                    foreach (var user in _userManagement.GetAllUsers())
                    {
                        UserResults.Add(user);
                    }
                }

                StatusMessage = $"Users: found {UserResults.Count} match(es).";
            }
            catch (Exception ex)
            {
                StatusMessage = $"User search failed: {ex.Message}";
            }
        }

        public string SearchCardNumber { get; set; }
        public string SearchCardIban { get; set; }

        public ICommand SearchCardsCommand { get; }

        private void SearchCards()
        {
            try
            {
                CardResults.Clear();

                if (!string.IsNullOrWhiteSpace(SearchCardNumber))
                {
                    var byNumber = _cardManagement.FindCardByCardNumber(SearchCardNumber.Trim());
                    if (byNumber != null) CardResults.Add(byNumber);
                }
                else if (!string.IsNullOrWhiteSpace(SearchCardIban))
                {
                    var byIban = _cardManagement.FindCardByIban(SearchCardIban.Trim());
                    if (byIban != null) CardResults.Add(byIban);
                }
                else
                {
                    foreach (var card in _cardManagement.GetAllCards())
                    {
                        CardResults.Add(card);
                    }
                }

                StatusMessage = $"Cards: found {CardResults.Count} match(es).";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Card search failed: {ex.Message}";
            }
        }

        public string SearchTransactionSenderId { get; set; }
        public string SearchTransactionReceiverId { get; set; }

        public ICommand SearchTransactionsCommand { get; }

        private void SearchTransactions()
        {
            try
            {
                TransactionResults.Clear();

                if (!string.IsNullOrWhiteSpace(SearchTransactionSenderId)
                    && int.TryParse(SearchTransactionSenderId, out var senderId))
                {
                    var bySender = _transactionManagement.FindTransactionBySender(senderId);
                    if (bySender != null) TransactionResults.Add(bySender);
                }
                else if (!string.IsNullOrWhiteSpace(SearchTransactionReceiverId)
                         && int.TryParse(SearchTransactionReceiverId, out var receiverId))
                {
                    var byReceiver = _transactionManagement.FindTransactionByReceiver(receiverId);
                    if (byReceiver != null) TransactionResults.Add(byReceiver);
                }
                else
                {
                    foreach (var transaction in _transactionManagement.GetAllTransactions())
                    {
                        TransactionResults.Add(transaction);
                    }
                }

                StatusMessage = $"Transactions: found {TransactionResults.Count} match(es).";
                new HashLogger().Log("INFO", StatusMessage);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Transaction search failed: {ex.Message}";
                new HashLogger().Log("ERROR", StatusMessage);
            }
        }

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
    }
}
