using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MediatRExample.MediatRRequest;
using MediatRExample.Publishing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediatRExample.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class MediatRController : Controller
    {
        private readonly IMediator _mediator;

        public MediatRController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Start()
        {
            var result = await _mediator.Send(new Ping("Pong"));
            return Ok(result);
        }

        public async Task<IActionResult> Notification()
        {
            await _mediator.Publish(new PingNotification("Receive Message"));
            return Ok();
        }
    }
}