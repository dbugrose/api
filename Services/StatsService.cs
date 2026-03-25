using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api.Services.Context;
using api.Models;

namespace api.Services
{
    public class StatsService
    {
        private readonly DataContext _context;

        public StatsService(DataContext context)
        {
            _context = context;
        }

        public async Task<StatsModel> CreateStats(int id)
        {
            var stats = new StatsModel
            {
                Id = id,
                MonstersSlain = 0,
                TasksCompleted = 0,
                EasyTasks = 0,
                MedTasks = 0,
                HardTasks = 0
            };

            _context.StatsInfo.Add(stats);
            await _context.SaveChangesAsync();

            return stats;
        }

        public async Task<StatsModel?> GetStats(int id)
        {
            return await _context.StatsInfo
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<StatsModel> CompleteTask(int id, TaskDifficulty difficulty)
        {
            var stats = await GetStats(id);
            if (stats == null) return null;

            stats.TasksCompleted++;

            switch (difficulty)
            {
                case TaskDifficulty.Easy:
                    stats.EasyTasks++;
                    break;

                case TaskDifficulty.Medium:
                    stats.MedTasks++;
                    break;

                case TaskDifficulty.Hard:
                    stats.HardTasks++;
                    break;
            }

            await _context.SaveChangesAsync();
            return stats;
        }

        public async Task<StatsModel> MonsterSlain(int id)
        {
            var stats = await GetStats(id);
            if (stats == null) return null;

            stats.MonstersSlain++;

            await _context.SaveChangesAsync();
            return stats;
        }

        public async Task DeleteStats(int id)
        {
            var stats = await GetStats(id);
            if (stats == null) return;

            _context.StatsInfo.Remove(stats);
            await _context.SaveChangesAsync();
        }
        public async Task<StatsModel?> DamageMonster(StatsModel health, string difficulty)
        {
            var healthToEdit = await GetStats(health.Id);

            if (healthToEdit == null)
                return null;

            int damage = difficulty switch
            {
                "Easy" => 10,
                "Medium" => 20,
                "Hard" => 30,
                _ => 0
            };

            healthToEdit.Health -= damage;

            health = healthToEdit;

            if (healthToEdit.Health < 0)
            {
                healthToEdit.Health = 0;
            }

            _context.StatsInfo.Update(health);
            await _context.SaveChangesAsync();
            return healthToEdit;
        }


        public async Task<StatsModel?> ResetHealth(StatsModel health)
        {
            {
                var stats = await _context.StatsInfo.FindAsync(health.Id);

                if (stats == null) return null;

                stats.Health = 100;

                await _context.SaveChangesAsync();

                return stats;
            }
        }

    }
            public enum TaskDifficulty
        {
            Easy,
            Medium,
            Hard
        }
}