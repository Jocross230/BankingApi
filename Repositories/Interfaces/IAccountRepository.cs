using BankingApi.Models;

namespace BankingApi.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByUserIdAsync(int userId);

    Task<Account?> GetByAccountNumberAsync(string accountNumber);

    Task<int> CreateAsync(Account account);
}