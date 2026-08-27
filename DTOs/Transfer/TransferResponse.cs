namespace BankingApi.DTOs.Transfer;

public class TransferResponse
{
    public string Reference { get; set; } = string.Empty;

    public string SenderAccountNumber { get; set; } = string.Empty;

    public string RecipientAccountNumber { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }
}