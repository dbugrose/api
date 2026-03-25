using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class FriendRequestModel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string? SenderUser {get; set; }
        public int ReceiverId { get; set; }
        public string? ReceiverUser {get; set; }
        public string? Status { get; set; }
    }
}