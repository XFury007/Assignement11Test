namespace MauiBankingExercise.Models
{
    // --- Customer Model ---
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Optional: accounts for this customer
        public List<Account>? Accounts { get; set; }
    }

    // --- Account Model ---
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal AccountBalance { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateOpened { get; set; }

        // Navigation back to customer
        public Customer? Customer { get; set; }
    }

    // --- DTO for API customer display ---
    public class CustomerDisplayModel
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Accounts belonging to this customer
        public List<Account>? Accounts { get; set; }
    }

    // --- Bank Model ---
    public class Bank
    {
        public int BankId { get; set; }
        public string BankName { get; set; } = string.Empty;
        public string BankAddress { get; set; } = string.Empty;
        public string BranchCode { get; set; } = string.Empty;
        public string ContactPhoneNumber { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string OperatingHours { get; set; } = string.Empty;
    }
}
