using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Models;
using Microsoft.AspNetCore.Authorization;


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

        [HttpPost("CreateStats")]
        public async Task<ActionResult<StatsModel>> CreateStats(int id)
        {
            var stats = await _statsService.CreateStats(id);
            return Ok(stats);
        }

        [HttpGet("GetStats/{id}")]
        public async Task<ActionResult<StatsModel>> GetStats(int id)
        {
            var stats = await _statsService.GetStats(id);

            if (stats == null)
                return NotFound();

            return Ok(stats);
        }

        [HttpPut("CompleteTask/{id}/{difficulty}")]
        public async Task<IActionResult> CompleteTask(int id, TaskDifficulty difficulty)
        {
           var completedTasks = await _statsService.CompleteTask(id, difficulty);
            return Ok(completedTasks);
        }

        [HttpPut("MonsterSlain/{id}")]
        public async Task<IActionResult> MonsterSlain(int id)
        {
            var monsterSlain = await _statsService.MonsterSlain(id);
            return Ok(monsterSlain);
        }
        //  [HttpPost("CreateHealthForUser")]
        // public async Task<ActionResult<StatsModel>> CreateHealthForUser(int id)
        // {
        //     var health = await _statsService.CreateHealthForUser(id);
        //     return Ok(health);
        // }
        // [HttpGet("GetHealth")]
        // public async Task<IActionResult> GetHealth()
        // {
        //     var health = await _statsService.GetHealth();

        //     if (health == null)
        //         return NotFound();

        //     return Ok(health);
        // }

        // [HttpGet("GetHealthByUserId/{id}")]
        // public async Task<IActionResult> GetHealthByUserId(int id)
        // {
        //     var health = await _statsService.GetHealthByUserId(id);

        //     if (health == null)
        //         return NotFound();

        //     return Ok(health);
        // }
        [HttpPut("Damage/{difficulty}")]
        public async Task<IActionResult> DamageMonster(StatsModel health, string difficulty)
        {
            var healthToUpdate = await _statsService.DamageMonster(health, difficulty);

            if (healthToUpdate == null)
            {
                return NotFound();
            }

            return Ok(healthToUpdate.Health);
        }

    [HttpPut("ResetHealth")]
    public async Task<IActionResult> ResetHealth(StatsModel health)
        {
            var healthToUpdate = await _statsService.ResetHealth(health);

            if (healthToUpdate == null)
            {
                return NotFound();
            }

            return Ok(healthToUpdate.Health);
        }

        // [HttpDelete("DeleteHealthById")]
        // public async Task<IActionResult> DeleteHealthById(int id)
        // {
        //     var success = await _statsService.DeleteHealthById(id);

        //     if (!success)
        //         return NotFound();

        //     return Ok(success);
        // }
    }
}