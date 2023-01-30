using Dapper.BaseRepository.Config;
using WebHooks.SharedKernel.Entities;

namespace WebHooks.SharedKernel.Repositories.Interfaces
{
    public interface IClientRepo
    {
        Task<CommandResp> CreateClient(Client client);

        Task<Client> GetClient(Guid clientId);
    }
}