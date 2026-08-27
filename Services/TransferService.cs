using BankingApi.DTOs.Transfer;
using BankingApi.Repositories.Interfaces;
using BankingApi.Services.Interfaces;

namespace BankingApi.Services;

public class TransferService : ITransferService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransferRepository _transferRepository;

    public TransferService(
        IAccountRepository accountRepository,
        ITransferRepository transferRepository)
    {
        _accountRepository = accountRepository;
        _transferRepository = transferRepository;
    }

    public async Task<TransferResponse> TransferAsync(
        int senderUserId,
        TransferRequest request)
    {
        // 1. Get sender account from authenticated user
        var senderAccount =
            await _accountRepository.GetByUserIdAsync(senderUserId);

        if (senderAccount is null)
        {
            throw new KeyNotFoundException("Sender account was not found.");
        }

        // 2. Get recipient account
        var recipientAccount =
            await _accountRepository.GetByAccountNumberAsync(
                request.RecipientAccountNumber.Trim());

        if (recipientAccount is null)
        {
            throw new KeyNotFoundException(
                "Recipient account was not found.");
        }

        // 3. Prevent transfer to yourself
        if (senderAccount.Id == recipientAccount.Id)
        {
            throw new InvalidOperationException(
                "You cannot transfer money to your own account.");
        }

        // 4. Validate amount
        if (request.Amount <= 0)
        {
            throw new InvalidOperationException(
                "Transfer amount must be greater than zero.");
        }

        // 5. Check sufficient balance
        if (senderAccount.Balance < request.Amount)
        {
            throw new InvalidOperationException(
                "Insufficient account balance.");
        }

        // 6. Generate unique transfer reference
        var reference =
            $"TRF-{Guid.NewGuid().ToString("N")[..12].ToUpper()}";

        // 7. Perform atomic transfer
        var transaction =
            await _transferRepository.TransferAsync(
                senderAccount,
                recipientAccount,
                request.Amount,
                reference);

        // 8. Return API response
        return new TransferResponse
        {
            Reference = transaction.Reference,
            SenderAccountNumber = senderAccount.AccountNumber,
            RecipientAccountNumber = recipientAccount.AccountNumber,
            Amount = transaction.Amount,
            CreatedAt = transaction.CreatedAt
        };
    }
}