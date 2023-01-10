using WebHookPractice.Sender.Worker;
using WebHooks.SharedKernel.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<ITransferCashTopicConsumer, TransferCashTopicConsumer>();
    })
    .Build();

await host.RunAsync();
