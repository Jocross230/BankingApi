namespace BankingApi.Models;

public class Transaction
{
    public long Id { get; set; }

    public int SenderAccountId { get; set; }

    public int RecipientAccountId { get; set; }

    public decimal Amount { get; set; }

    public string TransactionType { get; set; } = string.Empty;

    public string Reference { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}