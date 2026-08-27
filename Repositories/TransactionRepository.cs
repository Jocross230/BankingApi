using BankingApi.Data;
using BankingApi.DTOs.Transaction;
using BankingApi.Repositories.Interfaces;
using Dapper;

namespace BankingApi.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly DapperContext _context;

    public TransactionRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransactionResponse>> GetByAccountIdAsync(
        int accountId)
    {
        const string sql = """
            SELECT
                t.Reference,
                sender.AccountNumber AS SenderAccountNumber,
                recipient.AccountNumber AS RecipientAccountNumber,
                t.Amount,
                t.TransactionType,
                t.CreatedAt
            FROM Transactions t
            INNER JOIN Accounts sender
                ON sender.Id = t.SenderAccountId
            INNER JOIN Accounts recipient
                ON recipient.Id = t.RecipientAccountId
            WHERE t.SenderAccountId = @AccountId
               OR t.RecipientAccountId = @AccountId
            ORDER BY t.CreatedAt DESC;
            """;

        using var connection = _context.CreateConnection();

        return await connection.QueryAsync<TransactionResponse>(
            sql,
            new { AccountId = accountId });
    }
}