
using MauiBankingExercise.ViewModel;
using MauiBankingExercise.Services;

namespace MauiBankingExercise.Views
{

    public partial class AccountsPage : ContentPage
    {
        private AccountsViewModel ViewModel => BindingContext as AccountsViewModel;

        public AccountsPage(AccountsViewModel viewModel)   // Injected by DI
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnLoadAccountsClicked(object sender, EventArgs e)
        {
            if (ViewModel != null)
                await ViewModel.LoadAccountsAsync();
            else
                await DisplayAlert("Error", "ViewModel not initialized", "OK");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null && ViewModel.Accounts.Count == 0)
                await ViewModel.LoadAccountsAsync();
        }
    }

}






