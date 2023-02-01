using Dapper.Repository.interfaces;
using Microsoft.Extensions.Logging;

namespace WebHooks.SharedKernel.Infrastructure
{
    public class AppLogger<T> : IBaseRepositoryLogger<T>, IAppLogger<T>
    {
        private readonly ILogger<T> logger;

        public AppLogger(ILogger<T> logger)
        {
            this.logger = logger;
        }
        public void LogError(string message)
        {
            logger.LogError(message);
        }

        public void LogError(string message, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void LogInformation(string message)
        {
            logger.LogInformation(message);
        }

        public void LogInformation(string message, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
