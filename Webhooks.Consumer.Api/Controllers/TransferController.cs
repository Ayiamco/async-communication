using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebHooks.SharedKernel.Base;
using WebHooks.SharedKernel.Commands;
using WebHooks.SharedKernel.Infrastructure;
using WebHooks.SharedKernel.Services.Interfaces;

namespace Webhooks.Subscriber.Api.Controllers
{

    public class TransferController : BaseController
    {
        private readonly IRefitHttpClientFactory<IApiClients> httpClientFactory;

        public TransferController(IRefitHttpClientFactory<IApiClients> httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Index(decimal amt, Guid clientId)
        {
            var apiClient = httpClientFactory.CreateClient("https://localhost:7224/");
            var resp = await apiClient.TransferCash(new TransferCash.TfCommand
            {
                ClientId = clientId,
                SenderBankCode = "06",
                TransactionRef = "ghgvhj",
                ReceiverBankCode = "678",
                SenderAccountNumber = "1234567890",
                ReceiverAccountNumber = "123456790",
                Amount = amt
            });

            return GetResponse(resp);
        }

        [HttpPost("/handler")]
        public async Task<IActionResult> Handler(TransferWebHookCallBackPayload transferHook)
        {
            Console.WriteLine($"Handler recieved webhook: {JsonConvert.SerializeObject(transferHook)}");
            return Ok(new ApiResponse());
        }
    }
}
