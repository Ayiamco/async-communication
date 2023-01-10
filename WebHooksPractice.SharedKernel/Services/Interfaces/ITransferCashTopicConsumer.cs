namespace WebHooks.SharedKernel.Services.Interfaces
{
    public interface ITransferCashTopicConsumer
    {
        Task ConsumeMessage(CancellationToken cancellationToken = default);
    }
}