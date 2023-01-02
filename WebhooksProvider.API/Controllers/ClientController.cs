using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Register(RegisterClient.Request request, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await mediator.Send(request, cancellationToken));
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred: {ex} ");
                return StatusCode(500);
            }

        }
    }
}
