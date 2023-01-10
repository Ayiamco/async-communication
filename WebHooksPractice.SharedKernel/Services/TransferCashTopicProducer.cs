using Confluent.Kafka;
using WebHooks.SharedKernel.Services.Interfaces;
using static WebHooks.SharedKernel.Base.AppConstants;

namespace WebHooks.SharedKernel.Services
{
    public class TransferCashTopicProducer : ITransferCashTopicProducer
    {
        private readonly ProducerConfig producerConfig;
        public TransferCashTopicProducer()
        {
            producerConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.Leader,
            };
        }

        public async Task PushTopic(string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                var result = await producer.ProduceAsync(KafkaTopics.CashTransfer, new Message<Null, string> { Value = message });
                if (result == null || result.Status != PersistenceStatus.Persisted)
                {
                    Console.WriteLine("Message publishing failed.");
                    return;
                }

                Console.WriteLine($"Message sent: {result.Message.Value}");
            }
        }
    }
}
