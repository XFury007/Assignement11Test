using MauiBankingExercise.Models;
using MauiBankingExercise.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace MauiBankingExercise.ViewModel
{
    public class AccountsViewModel : INotifyPropertyChanged
    {
        private readonly IApiService _apiService;

        private ObservableCollection<Account> _accounts = new();
        private bool _isLoading;
        private string _errorMessage = string.Empty;
        private bool _hasError;
        private bool _apiConnected;

        public AccountsViewModel(IApiService apiService)
        {
            _apiService = apiService;
            ReloadAccountsCommand = new Command(async () => await LoadAccountsAsync());
            TestApiCommand = new Command(async () => await TestApiConnectionAsync());
        }

        public ICommand ReloadAccountsCommand { get; }
        public ICommand TestApiCommand { get; }

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
                HasError = !string.IsNullOrEmpty(value);
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged(nameof(HasError));
            }
        }

        public bool ApiConnected
        {
            get => _apiConnected;
            set
            {
                _apiConnected = value;
                OnPropertyChanged(nameof(ApiConnected));
            }
        }

        public bool HasAccounts => Accounts.Count > 0;

        // Test if API is reachable
        public async Task TestApiConnectionAsync()
        {
            try
            {
                IsLoading = true;
                ApiConnected = false;
                ErrorMessage = string.Empty;

                Debug.WriteLine("Testing API connection...");
                
                bool isConnected = await _apiService.TestApiConnectionAsync();
                ApiConnected = isConnected;
                
                Debug.WriteLine($"API connection test result: {(isConnected ? "Connected" : "Failed")}");
                
                if (!isConnected)
                {
                    ErrorMessage = "Cannot connect to the Banking API. Please make sure it's running.";
                }
            }
            catch (Exception ex)
            {
                ApiConnected = false;
                ErrorMessage = $"Error testing API connection: {ex.Message}";
                Debug.WriteLine($"Exception in TestApiConnectionAsync: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Updated to test API connection first, then load accounts
        public async Task LoadAccountsAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                Debug.WriteLine("AccountsViewModel - Starting to load accounts...");

                // First test if the API is reachable at all
                await TestApiConnectionAsync();
                
                if (!ApiConnected)
                {
                    Debug.WriteLine("API is not connected, aborting account load");
                    return;
                }

                var accounts = await _apiService.GetAllAccountsAsync();

                Debug.WriteLine($"AccountsViewModel - Received {accounts.Count} accounts from API");

                Accounts.Clear();
                foreach (var account in accounts)
                {
                    Accounts.Add(account);
                    Debug.WriteLine($"AccountsViewModel - Added account: {account.AccountNumber} - Balance: {account.AccountBalance:C}");
                }

                OnPropertyChanged(nameof(HasAccounts));
                Debug.WriteLine($"AccountsViewModel - Total accounts in collection: {Accounts.Count}");

                if (Accounts.Count == 0 && ApiConnected)
                {
                    ErrorMessage = "No accounts were found in the database. The API is connected but returned no data.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading accounts: {ex.Message}";
                Debug.WriteLine($"Exception in LoadAccountsAsync: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
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
