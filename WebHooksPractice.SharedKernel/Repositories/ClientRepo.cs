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

        public async Task<Client> GetClient(Guid clientId)
        {
            var sql = $@"
SELECT HandlerUrl,Id FROM clients
WHERE Id = @clientId
";
            var resp= await RunQuery<Client>(sql, new { clientId });

            if (resp == null || !resp.Any()) 
                throw new ClientDoesNotExistException();

            return resp.First();
        }


        public class ClientDoesNotExistException: Exception {}
    }
}
