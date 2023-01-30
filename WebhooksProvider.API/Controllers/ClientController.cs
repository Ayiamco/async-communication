using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebHooks.SharedKernel.Base;
using WebHooks.SharedKernel.Commands;

namespace Webhooks.App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : BaseController
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

        [HttpGet]
        public async Task<IActionResult> TransferHook()
        {
            return Ok(new TransferHook
            {
                Status = "The status code for cash transfer request",
            });
        }
    }
}
