using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class FriendListModel
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? FriendId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}