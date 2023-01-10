using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Refit;
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
        public async Task<IActionResult> Index(decimal amt)
        {
            var apiClient =httpClientFactory.CreateClient("https://localhost:7224/");
            var resp = await apiClient.TransferCash(new TransferCash.TfCommand
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

        //[HttpPost]
        //public async Task<IActionResult> Handler(TransferHook transferHook)
        //{
        //    Console.WriteLine($"Handler by handler: {JsonConvert.SerializeObject(transferHook)}");
        //    return Ok();
        //}
    }
}
