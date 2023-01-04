using MediatR;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebHooks.SharedKernel.Base;
using WebHooks.SharedKernel.Commands;

namespace Webhooks.Provider.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<ClientController> logger;

        public ClientController(IMediator mediator, ILogger<ClientController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterClient.Command request, CancellationToken cancellationToken)
        {
            try
            {
                return GetResponse(await mediator.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred: {ex} ");
                return StatusCode(500);
            }

        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferCash.Command command, CancellationToken cancellationToken)
        {
            try
            {
                return GetResponse(await mediator.Send(command, cancellationToken));    
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred: {ex} ");
                return StatusCode(500);
            }
        }

        public IActionResult GetResponse<TResponse>(TResponse response ) where TResponse: ApiResponse
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
