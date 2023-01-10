namespace WebHooks.SharedKernel.Services
{
    public interface ITransferCashTopicConsumer
    {
        Task ConsumeMessage(CancellationToken cancellationToken = default);
    }
}