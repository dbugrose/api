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
    [HttpPost("CreateTodo/{username}")]
    public Task<bool> CreateTodo(TodoModel todo)
        {
            return _context.CreateTodo(todo);
        }

[HttpGet("GetTodos/{username}")]

        public Task<TodoModel> GetTodos(string username)
        {
            return _context.GetTodos(username);
        }

[HttpGet("GetIncompleteTodos/{username}")]

        public Task<TodoModel> GetIncompleteTodos(string username)
        {
            return _context.GetIncompleteTodos(username);
        }
        [HttpPut("UpdateTodo/{username}/{id}")]
        public Task<bool> UpdateTodo(string username, int id)
        {
            return _context.UpdateTodo(username, id);
        }
        [HttpPut("SoftDeleteTodo/{username}/{id}")]
        public Task<bool> SoftDeleteTodo(string username, int id)
        {
            return _context.SoftDeleteTodo(username, id);
        }

        [HttpDelete("HardDeleteTodo/{username}/{id}")]
        public Task<bool> HardDeleteTodo(string username, int id)
        {
            return _context.HardDeleteTodo(username, id);
        }
    }
}