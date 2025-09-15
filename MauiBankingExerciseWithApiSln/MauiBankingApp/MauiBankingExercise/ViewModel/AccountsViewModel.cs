using MauiBankingExercise.Models;
using MauiBankingExercise.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiBankingExercise.ViewModel
{
    public class AccountsViewModel
    {
        private readonly DatabaseService _dbService;

        public ObservableCollection<Account> Accounts { get; set; }

        public AccountsViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            Accounts = new ObservableCollection<Account>(_dbService.GetAccounts());
        }
    }

}
