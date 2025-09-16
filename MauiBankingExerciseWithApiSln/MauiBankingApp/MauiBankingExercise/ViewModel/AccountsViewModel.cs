using MauiBankingExercise.Models;
using MauiBankingExercise.Services;


using System.Collections.ObjectModel;


namespace MauiBankingExercise.ViewModel
{
    public class AccountsViewModel
    {

        private readonly ApiService _apiService = new ApiService();

        public ObservableCollection<Account> Accounts { get; set; } = new();

        public async Task LoadAccountsAsync()
        {
            var accounts = await _apiService.GetAccountsAsync();
            Accounts.Clear();
            foreach (var acc in accounts)
                Accounts.Add(acc);
        }


        //private readonly DatabaseService _dbService;

        //public ObservableCollection<Account> Accounts { get; set; }

        //public AccountsViewModel(DatabaseService dbService)
        //{
        //    _dbService = dbService;
        //    Accounts = new ObservableCollection<Account>(_dbService.GetAccounts());
        //}
    }

}
