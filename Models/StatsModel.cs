using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class StatsModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public int MonstersSlain { get; set; }  
        public int TasksCompleted { get; set; }
        public int EasyTasks { get; set; }
        public int MedTasks { get; set; }
        public int HardTasks { get; set; }

    }
}