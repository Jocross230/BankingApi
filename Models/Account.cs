namespace BankingApi.Models;

public class Account
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; set; }
}