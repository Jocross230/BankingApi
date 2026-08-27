using BankingApi.DTOs.Transaction;

namespace BankingApi.Services.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionResponse>> GetMyTransactionsAsync(int userId);
}