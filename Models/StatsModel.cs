using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class StatsModel
    {
        public int Id { get; set; }
        public string? Username {get; set;}
        public int MonstersSlain { get; set; }  = 0;
        public int TasksCompleted { get; set; } =0;
        public int EasyTasks { get; set; } =0;
        public int MedTasks { get; set; } =0;
        public int HardTasks { get; set; } = 0;
        public int Health { get; set; } = 100;

    }
}