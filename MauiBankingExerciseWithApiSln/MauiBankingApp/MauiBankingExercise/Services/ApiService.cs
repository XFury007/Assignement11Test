using System.Text.Json;
using MauiBankingExercise.Models;
using System.Text;
using System.Diagnostics;

namespace MauiBankingExercise.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            // Log the base address for debugging
            Debug.WriteLine($"ApiService initialized with BaseAddress: {_httpClient.BaseAddress}");
        }

        // Test API connectivity
        public async Task<bool> TestApiConnectionAsync()
        {
            try
            {
                Debug.WriteLine("Testing API connection...");
                
                // Hit a known valid endpoint instead of base URL to avoid false 404
                var response = await _httpClient.GetAsync("api/accounts");
                
                Debug.WriteLine($"Ping response: {response.StatusCode}");
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API connection test failed: {ex.Message}");
                return false;
            }
        }

        // Get all accounts by fetching customers and extracting their accounts
        public async Task<List<Account>> GetAllAccountsAsync()
        {
            try
            {
                Debug.WriteLine($"GetAllAccountsAsync - Starting API call to {_httpClient.BaseAddress}api/accounts");
                
                var response = await _httpClient.GetAsync("api/accounts");
                
                Debug.WriteLine($"GetAllAccountsAsync - Response received: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"GetAllAccountsAsync - Response content: {(json.Length > 500 ? json.Substring(0, 500) + "..." : json)}");
                    
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    
                    var accounts = JsonSerializer.Deserialize<List<Account>>(json, options);
                    
                    Debug.WriteLine($"GetAllAccountsAsync - Deserialized {accounts?.Count ?? 0} accounts");
                    
                    // Check for expected properties in the first account
                    if (accounts != null && accounts.Count > 0)
                    {
                        var firstAccount = accounts[0];
                        Debug.WriteLine($"First account details: Id={firstAccount.AccountId}, Number={firstAccount.AccountNumber}, Balance={firstAccount.AccountBalance}");
                        
                        // Check if Customer property is null
                        if (firstAccount.Customer == null)
                        {
                            Debug.WriteLine("WARNING: Customer property is null in the deserialized account");
                        }
                    }

                    return accounts ?? new List<Account>();
                }
                else
                {
                    Debug.WriteLine($"API call failed: {response.StatusCode} - {response.ReasonPhrase}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error content: {errorContent}");
                    return new List<Account>();
                }
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"HTTP Exception in GetAllAccountsAsync: {httpEx.Message}");
                Debug.WriteLine($"HTTP Exception details: {httpEx.InnerException?.Message ?? "No inner exception"}");
                return new List<Account>();
            }
            catch (TaskCanceledException tcEx)
            {
                Debug.WriteLine($"Timeout Exception in GetAllAccountsAsync: {tcEx.Message}");
                return new List<Account>();
            }
            catch (JsonException jsonEx)
            {
                Debug.WriteLine($"JSON Deserialization Exception in GetAllAccountsAsync: {jsonEx.Message}");
                Debug.WriteLine($"JSON Exception path: {jsonEx.Path}");
                return new List<Account>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in GetAllAccountsAsync: {ex.Message}");
                Debug.WriteLine($"Exception type: {ex.GetType().Name}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return new List<Account>();
            }
        }

        // Get accounts for a specific customer
        public async Task<List<Account>> GetAccountsByCustomerIdAsync(int customerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/accounts/customer/{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var accounts = JsonSerializer.Deserialize<List<Account>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return accounts ?? new List<Account>();
                }

                Debug.WriteLine($"API call failed: {response.StatusCode}");
                return new List<Account>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in GetAccountsByCustomerIdAsync: {ex.Message}");
                return new List<Account>();
            }
        }

        // Get all customers with their full details
        public async Task<List<CustomerDisplayModel>> GetCustomersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/customers/display");

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
                Debug.WriteLine($"Exception in GetCustomersAsync: {ex.Message}");
                return new List<CustomerDisplayModel>();
            }
        }

        // Get customer by ID with full details
        public async Task<CustomerDisplayModel?> GetCustomerByIdAsync(int customerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/customers/{customerId}/display");

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
                Debug.WriteLine($"Exception in GetCustomerByIdAsync: {ex.Message}");
                return null;
            }
        }

        // Get all banks
        public async Task<List<Bank>> GetBanksAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/banks");

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
                Debug.WriteLine($"Exception in GetBanksAsync: {ex.Message}");
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

                var response = await _httpClient.PostAsync("api/customers", content);

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
                Debug.WriteLine($"Exception in CreateCustomerAsync: {ex.Message}");
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

                var response = await _httpClient.PostAsync("api/accounts", content);

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
                Debug.WriteLine($"Exception in CreateAccountAsync: {ex.Message}");
                return null;
            }
        }
    }
}
