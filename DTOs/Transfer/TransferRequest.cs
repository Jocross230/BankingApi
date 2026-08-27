using System.ComponentModel.DataAnnotations;

namespace BankingApi.DTOs.Transfer;

public class TransferRequest
{
    [Required]
    public string RecipientAccountNumber { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }
}