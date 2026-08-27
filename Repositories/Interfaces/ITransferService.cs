using BankingApi.DTOs.Transfer;

namespace BankingApi.Services.Interfaces;

public interface ITransferService
{
    Task<TransferResponse> TransferAsync(
        int senderUserId,
        TransferRequest request);
}