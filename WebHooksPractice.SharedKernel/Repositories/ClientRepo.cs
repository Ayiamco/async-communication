﻿using Dapper.BaseRepository.Components;
using Dapper.BaseRepository.Config;
using WebHooks.SharedKernel.Entities;
using WebHooks.SharedKernel.Infrastructure;
using WebHooks.SharedKernel.Repositories.Interfaces;

namespace WebHooks.SharedKernel.Repositories
{
    public class ClientRepo : BaseRepository<ClientRepo, AppLogger<ClientRepo>>, IClientRepo
    {
        public ClientRepo(IAppLogger<ClientRepo> logger) : base(logger)
        {
        }

        public async Task<CommandResp> CreateClient(Client request)
        {
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
            var resp = await RunQuery<Client>(sql, new { clientId });

            if (resp == null || !resp.Any())
                throw new ClientDoesNotExistException();

            return resp.First();
        }


        public class ClientDoesNotExistException : Exception { }
    }
}
