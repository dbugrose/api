using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Models;
using api.Services.Context;

namespace api.Services
{
    public class HealthService
    {
        private readonly DataContext _context;

        public const int MAX_HP = 100;

        public HealthService(DataContext context)
        {
            _context = context;
        }

        public async Task<HealthModel?> GetHealth(string username)
        {
            return await _context.HealthInfo
                .FirstOrDefaultAsync(h => h.Username == username);
        }

        public async Task<int?> DamageMonster(string username, string difficulty)
        {
            var health = await GetHealth(username);

            if (health == null)
                return null;

            int damage = difficulty switch
            {
                "Easy" => 10,
                "Medium" => 20,
                "Hard" => 35,
                _ => 0
            };

            health.Health -= damage;

            if (health.Health < 0)
                health.Health = 0;

            await _context.SaveChangesAsync();

            return health.Health;
        }

        public async Task ResetHealth(string username)
        {
            var health = await GetHealth(username);

            if (health == null)
                return;

            health.Health = MAX_HP;

            await _context.SaveChangesAsync();
        }
    }
}