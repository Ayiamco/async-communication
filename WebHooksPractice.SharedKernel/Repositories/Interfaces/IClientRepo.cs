using DapperHelper;
using WebHooks.SharedKernel.Entities;

namespace WebHooks.SharedKernel.Repositories.Interfaces
{
    public interface IClientRepo
    {
        Task<CommandResp> CreateClient(Client client);
    }
}