using BankingApi.Data;
using BankingApi.Models;
using BankingApi.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace BankingApi.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly DapperContext _context;

    public TransferRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<Transaction> TransferAsync(
        Account senderAccount,
        Account recipientAccount,
        decimal amount,
        string reference)
    {
        using var connection = _context.CreateConnection();

        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Debit sender
            const string debitSql = """
                UPDATE Accounts
                SET Balance = Balance - @Amount
                WHERE Id = @AccountId;
                """;

            await connection.ExecuteAsync(
                debitSql,
                new
                {
                    Amount = amount,
                    AccountId = senderAccount.Id
                },
                transaction);

            // 2. Credit recipient
            const string creditSql = """
                UPDATE Accounts
                SET Balance = Balance + @Amount
                WHERE Id = @AccountId;
                """;

            await connection.ExecuteAsync(
                creditSql,
                new
                {
                    Amount = amount,
                    AccountId = recipientAccount.Id
                },
                transaction);

            // 3. Create transaction record
            const string transactionSql = """
                INSERT INTO Transactions
                (
                    SenderAccountId,
                    RecipientAccountId,
                    Amount,
                    TransactionType,
                    Reference
                )
                VALUES
                (
                    @SenderAccountId,
                    @RecipientAccountId,
                    @Amount,
                    @TransactionType,
                    @Reference
                );

                SELECT
                    Id,
                    SenderAccountId,
                    RecipientAccountId,
                    Amount,
                    TransactionType,
                    Reference,
                    CreatedAt
                FROM Transactions
                WHERE Id = CAST(SCOPE_IDENTITY() AS BIGINT);
                """;

            var transfer = await connection.QuerySingleAsync<Transaction>(
                transactionSql,
                new
                {
                    SenderAccountId = senderAccount.Id,
                    RecipientAccountId = recipientAccount.Id,
                    Amount = amount,
                    TransactionType = "Transfer",
                    Reference = reference
                },
                transaction);

            // Everything succeeded
            transaction.Commit();

            return transfer;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}