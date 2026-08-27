namespace BankingApi.DTOs.Account;

public class AccountResponse
{
    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; set; }
}