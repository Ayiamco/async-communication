using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.SharedKernel.Base
{
    public  class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public string? Message { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
