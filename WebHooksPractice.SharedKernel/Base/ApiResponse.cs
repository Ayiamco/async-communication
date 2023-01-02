using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.SharedKernel.Base
{
    public  class ApiResponse<TResponse>
    {
        public string? Message { get; set; }

        public string? ErrorMessage { get; set; }

        public TResponse? Data { get; set; }
    }
}
