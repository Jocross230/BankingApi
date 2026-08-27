using BankingApi.DTOs.Transaction;
using BankingApi.Repositories.Interfaces;
using BankingApi.Services.Interfaces;

namespace BankingApi.Services;

public class TransactionService : ITransactionService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<TransactionResponse>>
        GetMyTransactionsAsync(int userId)
    {
        var account =
            await _accountRepository.GetByUserIdAsync(userId);

        if (account is null)
        {
            throw new KeyNotFoundException("Account not found.");
        }

        return await _transactionRepository
            .GetByAccountIdAsync(account.Id);
    }
}