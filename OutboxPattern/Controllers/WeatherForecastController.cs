using MediatR;
using Microsoft.AspNetCore.Mvc;
using OutboxPattern.Application.Commands;

namespace OutboxPattern.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherForecastController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderConnand commend)
        {
            await _mediator.Send(commend);

            return Ok();
        }
    }
}