using BankingApi.Data;
using BankingApi.Models;
using BankingApi.Repositories.Interfaces;
using Dapper;

namespace BankingApi.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly DapperContext _context;

    public AccountRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByUserIdAsync(int userId)
    {
        const string sql = """
            SELECT
                Id,
                UserId,
                AccountNumber,
                Balance
            FROM Accounts
            WHERE UserId = @UserId;
            """;

        using var connection = _context.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Account>(
            sql,
            new { UserId = userId });
    }

    public async Task<Account?> GetByAccountNumberAsync(
        string accountNumber)
    {
        const string sql = """
            SELECT
                Id,
                UserId,
                AccountNumber,
                Balance
            FROM Accounts
            WHERE AccountNumber = @AccountNumber;
            """;

        using var connection = _context.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Account>(
            sql,
            new { AccountNumber = accountNumber });
    }

    public async Task<int> CreateAsync(Account account)
    {
        const string sql = """
            INSERT INTO Accounts
            (
                UserId,
                AccountNumber,
                Balance
            )
            VALUES
            (
                @UserId,
                @AccountNumber,
                @Balance
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using var connection = _context.CreateConnection();

        return await connection.QuerySingleAsync<int>(
            sql,
            account);
    }
}