using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly StatsService _statsService;

        public StatsController(StatsService statsService)
        {
            _statsService = statsService;
        }

        [HttpPost("create/{username}")]
        public async Task<ActionResult<StatsModel>> CreateStats(string username)
        {
            var stats = await _statsService.CreateStats(username);
            return Ok(stats);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<StatsModel>> GetStats(string username)
        {
            var stats = await _statsService.GetStats(username);

            if (stats == null)
                return NotFound();

            return Ok(stats);
        }

        [HttpPost("CompleteTask/{username}")]
        public async Task<IActionResult> CompleteTask(string username, TaskDifficulty difficulty)
        {
            await _statsService.CompleteTask(username, difficulty);
            return Ok();
        }

        [HttpPost("MonsterSlain/{username}")]
        public async Task<IActionResult> MonsterSlain(string username)
        {
            await _statsService.MonsterSlain(username);
            return Ok();
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteStats(string username)
        {
            await _statsService.DeleteStats(username);
            return Ok();
        }
    }
}