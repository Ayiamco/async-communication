using WebHooks.SharedKernel.Services;

namespace WebHookPractice.Sender.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<TransferCashTopicConsumer> _logger;

        public Worker(ILogger<TransferCashTopicConsumer> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var consumer = new TransferCashTopicConsumer(_logger);
                //await Task.Delay(new TimeSpan(0,2,0), stoppingToken);
                await consumer.ConsumeMessage();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}