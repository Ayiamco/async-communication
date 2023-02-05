using Dapper.Repository.interfaces;

namespace WebHooks.SharedKernel.Infrastructure
{
    public interface IAppLogger<TRepo> : IRepositoryLogger<TRepo>
    {
    }
}