using WebHooks.SharedKernel.Services.Interfaces;

namespace WebHookPractice.Sender.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITransferCashTopicConsumer transferCashConsumer;

        public Worker(ILogger<Worker> logger,
            ITransferCashTopicConsumer transferCashConsumer)
        {
            _logger = logger;
            this.transferCashConsumer = transferCashConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(new TimeSpan(0,2,0), stoppingToken);
                await transferCashConsumer.ConsumeMessage();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}