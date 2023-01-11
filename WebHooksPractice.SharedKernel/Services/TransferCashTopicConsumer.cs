using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebHooks.SharedKernel.Base;
using WebHooks.SharedKernel.Infrastructure;
using WebHooks.SharedKernel.Repositories.Interfaces;
using WebHooks.SharedKernel.Services.Interfaces;
using static WebHooks.SharedKernel.Base.AppConstants;
using static WebHooks.SharedKernel.Commands.TransferCash;

namespace WebHooks.SharedKernel.Services
{
    public class TransferCashTopicConsumer : ITransferCashTopicConsumer
    {
        private readonly ConsumerConfig consumerConfig;
        private readonly ILogger<TransferCashTopicConsumer> logger;
        private readonly IClientRepo clientRepo;
        private readonly IRefitHttpClientFactory<IApiClients> httpClientFactory;

        public TransferCashTopicConsumer(ILogger<TransferCashTopicConsumer> logger,
            IClientRepo clientRepo, IRefitHttpClientFactory<IApiClients> httpClientFactory)
        {
            consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092,localhost:9092",
                GroupId = "foo",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            this.logger = logger;
            this.clientRepo = clientRepo;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task ConsumeMessage(CancellationToken cancellationToken = default)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
            {
                consumer.Subscribe(new List<string> { KafkaTopics.CashTransfer });

                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    var msg = consumeResult?.Message.Value;
                    logger.LogInformation($"result: {msg}");
                    var transferRequest = JsonConvert.DeserializeObject<TfCommand>(msg);
                    var client = await clientRepo.GetClient(transferRequest.ClientId);
                    var httpClient = httpClientFactory.CreateClient(client.HandlerUrl);
                    Random random = new Random();
                    var val = random.Next(0, 1);

                    var webHookPayload = val == 0 ? new ApiResponse { Message = "Transfer was not successful." } : new ApiResponse { Message = "Transfer was successful." };
                    await httpClient.CallHandler(webHookPayload);

                }

                consumer.Close();
            }
        }
    }
}
