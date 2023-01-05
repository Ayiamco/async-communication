using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.SharedKernel.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IActionResult GetResponse<TResponse>(TResponse response) where TResponse : ApiResponse
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response);
                case HttpStatusCode.NotFound:
                    return NotFound(Response);
                case HttpStatusCode.Accepted:
                    return Accepted(response);
                case HttpStatusCode.Created:
                    return Ok(response);
                default:
                    throw new Exception($"Statuscode: {response.StatusCode}  not handled in {nameof(GetResponse)} function.");
            }
        }
    }
}
