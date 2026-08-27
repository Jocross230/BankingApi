using BankingApi.Models;

namespace BankingApi.Repositories.Interfaces;

public interface ITransferRepository
{
    Task<Transaction> TransferAsync(
        Account senderAccount,
        Account recipientAccount,
        decimal amount,
        string reference);
}