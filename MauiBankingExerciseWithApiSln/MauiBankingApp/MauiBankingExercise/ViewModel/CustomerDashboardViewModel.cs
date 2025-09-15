using MauiBankingExercise.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MauiBankingExercise.ViewModel
{
    public class CustomerDashboardViewModel
    {
        public Customer Customer { get; set; }
        public ObservableCollection<Transaction> Transactions { get; set; }

        public CustomerDashboardViewModel(Customer customer, List<Transaction> transactions)
        {
            Customer = customer;
            Transactions = new ObservableCollection<Transaction>(transactions);
        }
    }
}
