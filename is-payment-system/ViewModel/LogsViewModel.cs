using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using is_payment_system.Model;
using is_payment_system.Repository;

namespace is_payment_system.ViewModel
{
    public class LogsViewModel : INotifyPropertyChanged
    {
        private readonly LogRepository _logRepository;

        private string _statusMessage;

        public ObservableCollection<LogEntry> Logs { get; }

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

        public LogsViewModel()
            : this(new LogRepository())
        {
        }

        public LogsViewModel(LogRepository logRepository)
        {
            _logRepository = logRepository;

            Logs = new ObservableCollection<LogEntry>();

            RefreshCommand = new RelayCommand(LoadLogs);

            LoadLogs();
        }

        private void LoadLogs()
        {
            try
            {
                Logs.Clear();
                foreach (var entry in _logRepository.Logs)
                {
                    Logs.Add(entry);
                }

                StatusMessage = $"Loaded {Logs.Count} log entries.";
            }
            catch (System.Exception ex)
            {
                StatusMessage = $"Failed to load logs: {ex.Message}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
