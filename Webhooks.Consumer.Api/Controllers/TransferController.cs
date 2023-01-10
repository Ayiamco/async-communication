﻿using Microsoft.AspNetCore.Mvc;
using WebHooks.SharedKernel.Base;
using WebHooks.SharedKernel.Commands;
using WebHooks.SharedKernel.Infrastructure;

namespace Webhooks.Subscriber.Api.Controllers
{

    public class TransferController : BaseController
    {
        private readonly IApiClients apiClients;

        public TransferController(IApiClients apiClients)
        {
            this.apiClients = apiClients;
        }

        [HttpPost]
        public async Task<IActionResult> Index(decimal amt)
        {
            var resp = await apiClients.TransferCash(new TransferCash.TfCommand
            {
                ClientId = new Guid("1C37FC08-96F9-4F82-B041-E50961E45AA5"),
                SenderBankCode = "06",
                TransactionRef = "ghgvhj",
                ReceiverBankCode = "678",
                SenderAccountNumber = "1234567890",
                ReceiverAccountNumber = "123456790",
                Amount = amt
            });

            return GetResponse(resp);
        }
    }
}
