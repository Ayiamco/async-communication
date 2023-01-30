using System.Net;

namespace WebHooks.SharedKernel.Base
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public string? Message { get; set; }

        public string? ErrorMessage { get; set; }
    }

    public class TransferWebHookCallBackPayload
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public string? ErrorMessage { get; set; }
        public string? TransferReference { get; set; }
    }
}
