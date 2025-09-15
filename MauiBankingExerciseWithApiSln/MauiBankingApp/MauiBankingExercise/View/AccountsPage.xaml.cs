using MauiBankingExercise.ViewModel;
using MauiBankingExercise.Services;
using MauiBankingExercise.Models;
using MauiBankingExercise.View;
namespace MauiBankingExercise.Views
{
    public partial class AccountsPage : ContentPage
    {
        public AccountsPage()
        {
            InitializeComponent();

            // Database path setup
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "banking.db3");
            var dbService = new DatabaseService(dbPath);

            // Bind ViewModel
            BindingContext = new AccountsViewModel(dbService);
        }
            private async void OnAccountSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Account selectedAccount)
            {
                // Navigate to Customer Dashboard
                await Navigation.PushAsync(new CustomerDashboardPage(selectedAccount));
            }

    // Clear selection (so you can tap again)
    ((CollectionView)sender).SelectedItem = null;
        }
    
    }
}
