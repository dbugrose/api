using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class BlocksModel
    {
        public int Id { get; set; } 
        public string? BlockerId { get; set; }
        public string? BlockedId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}