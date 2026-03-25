using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;




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

        [HttpGet("GetTodos")]

        public async Task<IActionResult> GetTodos()
        {
            var todos = await _context.GetTodos();

            if (todos != null) return Ok(todos);

            return NotFound(new { Message = "No Todos " });
        }

        [HttpGet("GetTodosByUserId/{userId}")]
        public async Task<IActionResult> GetAllTodosByUserId(int userId)
        {
            var todos = await _context.GetTodoById(userId);

            if (todos != null) return Ok(new { todos });

            return NotFound(new { Message = "No todos" });
        }
        [HttpPost("CreateTodo")]
        public async Task<IActionResult> CreateTodo(TodosModel todo)
        {
            if (todo == null)
            {
                return BadRequest("Data is required.");
            }
            var success = await _context.AddTodo(todo);

            if (success) return Ok(new { success });

            return BadRequest(new { success });
        }

        [HttpGet("GetIncompleteTodos/")]

        public async Task<IActionResult> GetIncompleteTodos()
        {
            var todos = await _context.GetIncompleteTodos();

            if (todos != null) return Ok(new { todos });

            return BadRequest(new { Message = "No todos " });
        }
        [HttpPut("UpdateTodo")]
        public async Task<IActionResult> UpdateTodo(TodosModel todo)
        {
            var success = await _context.EditTodo(todo);

            if (success) return Ok(new { success });

            return BadRequest(new { success });
        }

        [HttpDelete("HardDeleteTodo/{id}")]
        public async Task<IActionResult> HardDeleteTodo(int id)
        {
            var success = await _context.HardDeleteTodo(id);

            if (!success)
                return NotFound(new { Message = "failed to delete" });

            return Ok(new { success });
        }

        // [HttpDelete("HardDeleteUnassignedTodo/{id}")]
        // public async Task<IActionResult> HardDeleteUnassignedTodo(int id)
        // {
        //     var success = await _context.HardDeleteUnassignedTodo(id);

        //     if (!success)
        //         return NotFound($"Todo with id {id} was not found.");

        //     return Ok("Unassigned todo deleted successfully.");
        // }
    }
}