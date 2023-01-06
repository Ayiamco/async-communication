using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using static WebHooks.SharedKernel.Base.AppConstants;

namespace WebHooks.SharedKernel.Services
{
    public class TransferCashTopicConsumer
    {
        private readonly ConsumerConfig consumerConfig;
        private readonly ILogger<TransferCashTopicConsumer> logger;

        public TransferCashTopicConsumer(ILogger<TransferCashTopicConsumer> logger)
        {
            consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092,localhost:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            this.logger = logger;
        }

        public async Task ConsumeMessage(CancellationToken cancellationToken = default)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
            {
                consumer.Subscribe(new List<string> { KafkaTopics.CashTransfer});

                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    logger.LogInformation($"result: {consumeResult?.Message.Value}");
                    // handle consumed message.
                    
                }

                consumer.Close();
            }
        }
    }
}
