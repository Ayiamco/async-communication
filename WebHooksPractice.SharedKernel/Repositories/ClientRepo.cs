using DapperHelper;
using DapperHelper.WorkflowCore.Library.Repositories;
using Microsoft.Extensions.Logging;
using WebHooks.SharedKernel.Entities;
using WebHooks.SharedKernel.Repositories.Interfaces;

namespace WebHooks.SharedKernel.Repositories
{
    public class ClientRepo : BaseRepo<ClientRepo>, IClientRepo
    {
        public ClientRepo(ILogger<ClientRepo> logger) : base(logger)
        {
        }

        public async Task<CommandResp> CreateClient(Client request)
        {
            request.Id = Guid.NewGuid();
            var sql = $@"
INSERT INTO clients  (ClientName, HandlerUrl,Id) 
VALUES (@ClientName, @HandlerUrl, @Id)";
            return await RunCommand(sql, request);
        }
    }
}
