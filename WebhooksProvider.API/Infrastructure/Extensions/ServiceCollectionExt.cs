using DapperHelper;
using WebHooks.SharedKernel.Repositories;
using WebHooks.SharedKernel.Repositories.Interfaces;

namespace Webhooks.App.Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientRepo, ClientRepo>();
            return services;
        }
    }
}
