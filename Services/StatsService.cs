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

        public async Task<StatsModel> CreateStats(string username)
        {
            var stats = new StatsModel
            {
                Username = username,
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

        public async Task<StatsModel?> GetStats(string username)
        {
            return await _context.StatsInfo
                .FirstOrDefaultAsync(s => s.Username == username);
        }

        public async Task CompleteTask(string username, TaskDifficulty difficulty)
        {
            var stats = await GetStats(username);
            if (stats == null) return;

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
        }

        public async Task MonsterSlain(string username)
        {
            var stats = await GetStats(username);
            if (stats == null) return;

            stats.MonstersSlain++;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteStats(string username)
        {
            var stats = await GetStats(username);
            if (stats == null) return;

            _context.StatsInfo.Remove(stats);
            await _context.SaveChangesAsync();
        }
    }

    public enum TaskDifficulty
    {
        Easy,
        Medium,
        Hard
    }
}