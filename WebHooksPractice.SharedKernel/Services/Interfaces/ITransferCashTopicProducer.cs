namespace WebHooks.SharedKernel.Services.Interfaces
{
    public interface ITransferCashTopicProducer
    {
        Task PushTopic(string message);
    }
}