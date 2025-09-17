using MauiBankingExercise.Models;
using MauiBankingExercise.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MauiBankingExercise.ViewModel
{
    public class AccountsViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        private ObservableCollection<Account> _accounts = new();
        private bool _isLoading;
        private string _errorMessage = string.Empty;

        public AccountsViewModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public AccountsViewModel() : this(new ApiService()) // Parameterless constructor for XAML
        {
        }

        public ObservableCollection<Account> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        // 🔹 Updated to use GetAllAccountsAsync instead of GetAccountsAsync
        public async Task LoadAccountsAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                System.Diagnostics.Debug.WriteLine("Starting to load accounts...");

                var accounts = await _apiService.GetAllAccountsAsync();

                System.Diagnostics.Debug.WriteLine($"Received {accounts.Count} accounts from API");

                Accounts.Clear();
                foreach (var account in accounts)
                {
                    Accounts.Add(account);
                    System.Diagnostics.Debug.WriteLine($"Added account: {account.AccountNumber} - Balance: {account.AccountBalance:C}");
                }

                System.Diagnostics.Debug.WriteLine($"Total accounts in collection: {Accounts.Count}");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading accounts: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Exception in LoadAccountsAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
