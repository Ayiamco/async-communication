using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;
using System.Net;
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
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        var msg = consumeResult?.Message.Value;
                        logger.LogInformation($"Consumed Msg value: {msg}");

                        var transferRequest = JsonConvert.DeserializeObject<Command>(msg);
                        var client = await clientRepo.GetClient(transferRequest.ClientId);
                        //var httpClient_ = new HttpClient(new HttpLoggingHandler()) { BaseAddress = new Uri(client.HandlerUrl) };

                        //var httpClient = httpClientFactory.CreateClient(httpClient_);
                        //var resp__ = await httpClient_.PostAsync("/handler", new StringContent("") { });
                        var httpClient = httpClientFactory.CreateClient(client.HandlerUrl);

                        //TODO: Implement actual transfer
                        Random random = new Random();
                        var isSuccesful = random.Next(0, 1);

                        var reqPayload = new TransferWebHookCallBackPayload { TransferReference = transferRequest.TransactionRef };
                        reqPayload.Message = isSuccesful == 0 ? "Transfer was not successful." : "Transfer was successful.";
                        var resp = await httpClient.CallHandler(reqPayload);

                        if (resp == null || resp.StatusCode != HttpStatusCode.OK)
                        {
                            //Push Failure event back to queue
                        }

                        logger.LogInformation("Successfully transfered cash.");
                        //TODO: log event to db
                    }
                    catch (UriFormatException ex)
                    {
                        //send out a mail to the client to update their client handle url
                        logger.LogError(ex, "Client handler Url is invalid.");
                    }
                    catch (ApiException ex)
                    {
                        //Queue unSuccessful handler call for retrial
                        logger.LogError(ex, $"Encountered some error while calling client handleUrl. {ex}");

                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Some error occured");
                    }
                }
                consumer.Close();
            }
        }
    }
}
