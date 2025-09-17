using System.Text.Json;
using MauiBankingExercise.Models;
using System.Text; 

namespace MauiBankingExercise.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://10.0.2.2:7258"; // Update this to match your API project's URL

        public ApiService()
        {
            _httpClient = new HttpClient();
            // Configure for HTTPS development certificates
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        // Get all accounts by fetching customers and extracting their accounts
       public async Task<List<Account>> GetAllAccountsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/api/accounts");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var accounts = JsonSerializer.Deserialize<List<Account>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return accounts ?? new List<Account>();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"API call failed: {response.StatusCode} - {response.ReasonPhrase}");
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Error content: {errorContent}");
                return new List<Account>();
            }
        }
        catch (HttpRequestException httpEx)
        {
            System.Diagnostics.Debug.WriteLine($"HTTP Exception in GetAllAccountsAsync: {httpEx.Message}");
            return new List<Account>();
        }
        catch (TaskCanceledException tcEx)
        {
            System.Diagnostics.Debug.WriteLine($"Timeout Exception in GetAllAccountsAsync: {tcEx.Message}");
            return new List<Account>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Exception in GetAllAccountsAsync: {ex.Message}");
            return new List<Account>();
        }
    }
        // Get accounts for a specific customer
        public async Task<List<Account>> GetAccountsByCustomerIdAsync(int customerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/api/accounts/customer/{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var accounts = JsonSerializer.Deserialize<List<Account>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return accounts ?? new List<Account>();
                }

                System.Diagnostics.Debug.WriteLine($"API call failed: {response.StatusCode}");
                return new List<Account>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in GetAccountsByCustomerIdAsync: {ex.Message}");
                return new List<Account>();
            }
        }

        // Get all customers with their full details
        public async Task<List<CustomerDisplayModel>> GetCustomersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/api/customers/display");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var customers = JsonSerializer.Deserialize<List<CustomerDisplayModel>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return customers ?? new List<CustomerDisplayModel>();
                }

                return new List<CustomerDisplayModel>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in GetCustomersAsync: {ex.Message}");
                return new List<CustomerDisplayModel>();
            }
        }

        // Get customer by ID with full details
        public async Task<CustomerDisplayModel?> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/api/customers/{customerId}/display");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var customer = JsonSerializer.Deserialize<CustomerDisplayModel>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return customer;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in GetCustomerByIdAsync: {ex.Message}");
                return null;
            }
        }

        // Get all banks
        public async Task<List<Bank>> GetBanksAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/api/banks");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var banks = JsonSerializer.Deserialize<List<Bank>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return banks ?? new List<Bank>();
                }

                return new List<Bank>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in GetBanksAsync: {ex.Message}");
                return new List<Bank>();
            }
        }

        // Create a new customer
        public async Task<Customer?> CreateCustomerAsync(Customer customer)
        {
            try
            {
                var json = JsonSerializer.Serialize(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}/api/customers", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var createdCustomer = JsonSerializer.Deserialize<Customer>(responseJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return createdCustomer;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in CreateCustomerAsync: {ex.Message}");
                return null;
            }
        }

        // Create a new account
        public async Task<Account?> CreateAccountAsync(Account account)
        {
            try
            {
                var json = JsonSerializer.Serialize(account);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseUrl}/api/accounts", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var createdAccount = JsonSerializer.Deserialize<Account>(responseJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return createdAccount;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in CreateAccountAsync: {ex.Message}");
                return null;
            }
        }
    }
}
