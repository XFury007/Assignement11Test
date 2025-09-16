using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using MauiBankingExercise.Models;

namespace MauiBankingExercise.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://10.0.2.2:7258/") // match your BankingApi port
            };
        }

        public async Task<List<Account>> GetAccountsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Account>>("api/Accounts")
                       ?? new List<Account>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API Error: {ex.Message}");
                return new List<Account>();
            }
        }
    }
}
