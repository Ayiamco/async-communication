using Refit;
using WebHooks.SharedKernel.Base;
using WebHooks.SharedKernel.Commands;

namespace WebHooks.SharedKernel.Infrastructure
{
    public interface IApiClients
    {
        [Post("/api/client/transfer")]
        Task<TransferCash.TfResponse> TransferCash(TransferCash.TfCommand command);

        [Post("/handler")]
        Task<ApiResponse> CallHandler(TransferWebHookCallBackPayload handlerRequest);
    }
}
