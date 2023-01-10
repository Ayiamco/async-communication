using WebHookPractice.Sender.Worker;
using WebHooks.SharedKernel.Services;
using WebHooks.SharedKernel.Services.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<ITransferCashTopicConsumer, TransferCashTopicConsumer>();
    })
    .Build();

await host.RunAsync();
