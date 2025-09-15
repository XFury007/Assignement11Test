using MauiBankingExercise.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;




namespace MauiBankingExercise.Services
{
    public class DatabaseService
    {
        private readonly SQLiteConnection _db; // holds the open SQlite connection for any queries 

        public DatabaseService(string dbPath)   
        {
           _db = new SQLiteConnection(dbPath);
            BankingSeeder.Seed(_db);

        }
        public List<Account> GetAccountsByCustomerId(int id)
        {
            return _db.Table<Account>().Where(x => x.CustomerId == id).ToList();
        }

        public List<Account> GetAccounts()
        {
           
            return _db.GetAllWithChildren<Account>(recursive: true);
        }
        public List<Transaction> GetTransactionsByAccountId(int accountId)
        {
            return _db.Table<Transaction>()
                      .Where(t => t.AccountId == accountId)
                      .ToList();
        }

    }
}
