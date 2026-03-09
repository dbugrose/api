using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class TodoModel
    {
    public int Id { get; set; }
    
    public string? Username { get; set; }  
    public string? Text { get; set; }
    public string? Difficulty { get; set; }
    public bool Completed { get; set; }
    public bool Deleted { get; set; }

    }
}