using DapperHelper;
using Refit;
using WebHookPractice.Sender.Worker;
using WebHooks.SharedKernel.Infrastructure;
using WebHooks.SharedKernel.Repositories;
using WebHooks.SharedKernel.Repositories.Interfaces;
using WebHooks.SharedKernel.Services;
using WebHooks.SharedKernel.Services.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.AddConnectionStrings((options) =>
        {
            options.SqlServerConnectionString = builder.Configuration["ConnectionStrings:SqlServerConnection"];
        });
        services.AddSingleton(typeof(IRefitHttpClientFactory<>), typeof(RefitHttpClientFactory<>));
        services.AddRefitClient<IApiClients>();

        services.AddSingleton<IClientRepo, ClientRepo>();
        services.AddSingleton<ITransferCashTopicConsumer, TransferCashTopicConsumer>();
        services.AddHostedService<Worker>();

    })
    .Build();

await host.RunAsync();
