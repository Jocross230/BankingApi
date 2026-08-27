using BankingApi.DTOs.Transaction;

namespace BankingApi.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<TransactionResponse>> GetByAccountIdAsync(int accountId);
}