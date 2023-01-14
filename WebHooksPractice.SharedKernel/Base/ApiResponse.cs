using System.Net;

namespace WebHooks.SharedKernel.Base
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public string? Message { get; set; }

        public string? ErrorMessage { get; set; }
    }

    public class TransferWebHookCallBackPayload : ApiResponse
    {
        public string? TransferReference { get; set; }
    }
}
