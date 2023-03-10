using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebHooks.SharedKernel.Base;
using WebHooks.SharedKernel.Repositories;
using WebHooks.SharedKernel.Repositories.Interfaces;
using WebHooks.SharedKernel.Services.Interfaces;
using static WebHooks.SharedKernel.Repositories.ClientRepo;

namespace WebHooks.SharedKernel.Commands
{
    public static class TransferCash
    {
        public class Command :IRequest<TfResponse>
        {
            public decimal Amount { get; set; }

            public string SenderBankCode { get; set; }

            public string ReceiverBankCode { get; set; }

            public string SenderAccountNumber { get; set; }

            public string ReceiverAccountNumber { get; set; }

            public string TransactionRef { get; set; }

            public Guid ClientId { get; set; }
        }

        public class TfResponse : ApiResponse { }

        public class Handler : IRequestHandler<Command, TfResponse>
        {
            private readonly ILogger<Handler> logger;
            private readonly IClientRepo clientRepo;
            private readonly ITransferCashTopicProducer producer;

            public Handler(ILogger<Handler> logger, IClientRepo clientRepo , ITransferCashTopicProducer producer)
            {
                this.logger = logger;
                this.clientRepo = clientRepo;
                this.producer = producer;
            }

            public async Task<TfResponse> Handle(Command request, CancellationToken cancellationToken) 
            {
                try
                {
                    await clientRepo.GetClient(request.ClientId);
                    await producer.PushTopic(JsonConvert.SerializeObject(request));
                    return new TfResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        Message = "Transfer request is being proccessed."
                    };
                }
                catch(ClientDoesNotExistException)
                {
                    logger.LogError($"ClientId {request.ClientId} was not found");
                    return new TfResponse
                    { 
                        StatusCode = HttpStatusCode.BadRequest ,
                        ErrorMessage= "ClientId does not exist"
                    };
                }
               
            }
        }
    }
}
