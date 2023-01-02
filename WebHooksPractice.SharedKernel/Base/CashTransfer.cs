namespace WebHooks.SharedKernel.Models
{
    public record CashTransfer(decimal Amount, string SenderBankCode, string ReceiverBankCode,
        string SenderAccountNumber, string ReceiverAccountNumber, string TransactionRef);

}
