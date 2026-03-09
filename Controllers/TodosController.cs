using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using api.Services.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly TodoService _context;

        public TodosController(TodoService context)
        {
            _context = context;
        }
        //Create
    [HttpPost("CreateTodo")]
    public bool CreateTodo(TodoModel todo)
        {
            return _context.CreateTodo(todo);
        }

[HttpGet("GetTodos")]

        public IEnumerable<TodoModel> GetTodos()
        {
            return _context.GetTodos();
        }

[HttpGet("GetIncompleteTodos")]

        public IEnumerable<TodoModel> GetIncompleteTodos()
        {
            return _context.GetIncompleteTodos();
        }
        [HttpPut("UpdateTodo/{id}")]
        public Task<bool> UpdateTodo(int id)
        {
            return _context.UpdateTodo(id);
        }
        [HttpPut("SoftDeleteTodo/{id}")]
        public Task<bool> SoftDeleteTodo(int id)
        {
            return _context.SoftDeleteTodo(id);
        }

        [HttpDelete("HardDeleteTodo/{id}")]
        public bool HardDeleteTodo(int id)
        {
            return _context.HardDeleteTodo(id);
        }
    }
}