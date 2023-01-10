namespace WebHooks.SharedKernel.Services
{
    public interface ITransferCashTopicProducer
    {
        Task PushTopic(string message);
    }
}