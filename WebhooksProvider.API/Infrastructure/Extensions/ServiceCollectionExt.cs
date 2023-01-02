using DapperHelper;
using WebHooks.SharedKernel.Repositories;
using WebHooks.SharedKernel.Repositories.Interfaces;

namespace Webhooks.Provider.Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection AddAppConfigParams(this IServiceCollection services, IConfiguration configuration)
        {
            ConnectionStrings.SqlServerConnection = configuration["ConnectionStrings:SqlServerConnection"];
            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientRepo, ClientRepo>();
            return services;
        }
    }
}
