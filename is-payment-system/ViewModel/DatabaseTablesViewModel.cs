using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using is_payment_system.Model;
using is_payment_system.Repository;

namespace is_payment_system.ViewModel
{
    public class DatabaseTablesViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private readonly CardRepository _cardRepository;
        private readonly TransactionRepository _transactionRepository;

        private string _statusMessage;

        public ObservableCollection<User> Users { get; }
        public ObservableCollection<Card> Cards { get; }
        public ObservableCollection<Transaction> Transactions { get; }

        public ICommand RefreshCommand { get; }

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

        public DatabaseTablesViewModel()
            : this(new UserRepository(), new CardRepository(), new TransactionRepository())
        {
        }

        public DatabaseTablesViewModel(
            UserRepository userRepository,
            CardRepository cardRepository,
            TransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _cardRepository = cardRepository;
            _transactionRepository = transactionRepository;

            Users = new ObservableCollection<User>();
            Cards = new ObservableCollection<Card>();
            Transactions = new ObservableCollection<Transaction>();

            RefreshCommand = new RelayCommand(LoadAll);

            LoadAll();
        }

        private void LoadAll()
        {
            try
            {
                ReplaceAll(Users, _userRepository.Users);
                ReplaceAll(Cards, _cardRepository.Cards);
                ReplaceAll(Transactions, _transactionRepository.Transactions);

                StatusMessage = $"Loaded {Users.Count} users, {Cards.Count} cards, {Transactions.Count} transactions.";
            }
            catch (System.Exception ex)
            {
                StatusMessage = $"Failed to load data: {ex.Message}";
            }
        }

        private static void ReplaceAll<T>(ObservableCollection<T> target, System.Collections.Generic.IEnumerable<T> source)
        {
            target.Clear();
            foreach (var item in source)
            {
                target.Add(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
