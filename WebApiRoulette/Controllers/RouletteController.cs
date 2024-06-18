using Microsoft.AspNetCore.Mvc;
using WebApiRoulette.Interfaces;
using WebApiRoulette.Models;

namespace WebApiRoulette.Controllers
{
    [Route("api/roulette")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private readonly IRouletteService _rouletteService;

        public RouletteController(IRouletteService rouletteService)
        {
            _rouletteService = rouletteService;
        }

        [HttpPost, Route("spin")]
        public async Task<ActionResult<Roulette>> SpinRoulette([FromBody] SpinRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Procesar la solicitud y obtener el resultado
            var result = await _rouletteService.SpinRoulette(request.UserName, request.BetType, request.BetValue, request.BetAmount);

            return Ok(result);
        }
    }

        public class SpinRequest
        {
        public string UserName { get; set; }
        public string BetType { get; set; } // "Color", "ParityColor", "NumberColor"
        public string BetValue { get; set; } // "Red", "Black", "Odd", "Even"
        public double BetAmount { get; set; }
    }

}

