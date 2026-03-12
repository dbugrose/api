using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using api.Services;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthService _service;

        public HealthController(HealthService service)
        {
            _service = service;
        }

        [HttpGet("GetHealth")]
        public async Task<ActionResult<HealthModel>> GetHealth(string username)
        {
            var health = await _service.GetHealth(username);

            if (health == null)
                return NotFound();

            return Ok(health);
        }

        [HttpPut("Damage")]
        public async Task<ActionResult<int>> DamageMonster(string username, string difficulty)
        {
            var health = await _service.DamageMonster(username, difficulty);

            if (health == null)
                return NotFound();

            return Ok(health);
        }

        [HttpPut("ResetHealth")]
        public async Task<IActionResult> ResetHealth(string username)
        {
            await _service.ResetHealth(username);
            return Ok();
        }
    }
}