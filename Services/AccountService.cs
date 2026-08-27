using BankingApi.DTOs.Account;
using BankingApi.Repositories.Interfaces;
using BankingApi.Services.Interfaces;

namespace BankingApi.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountResponse> GetMyAccountAsync(int userId)
    {
        var account =
            await _accountRepository.GetByUserIdAsync(userId);

        if (account is null)
        {
            throw new KeyNotFoundException("Account not found.");
        }

        return new AccountResponse
        {
            AccountNumber = account.AccountNumber,
            Balance = account.Balance
        };
    }
}