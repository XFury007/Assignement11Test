using MauiBankingExercise.Models;
using MauiBankingExercise.Services;
using MauiBankingExercise.ViewModel;

namespace MauiBankingExercise.View
{
    public partial class CustomerDashboardPage : ContentPage
    {
        public CustomerDashboardPage(Account selectedAccount)
        {
            InitializeComponent();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "banking.db3");
            var dbService = new DatabaseService(dbPath);

            var customer = selectedAccount.Customer;
            var transactions = dbService.GetTransactionsByAccountId(selectedAccount.AccountId);

            BindingContext = new CustomerDashboardViewModel(customer, transactions);
        }
    }
}
