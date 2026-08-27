using BankingApi.DTOs.Account;

namespace BankingApi.Services.Interfaces;

public interface IAccountService
{
    Task<AccountResponse> GetMyAccountAsync(int userId);
}