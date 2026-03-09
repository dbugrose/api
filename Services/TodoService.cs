using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services.Context;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class TodoService
    {
        private readonly DataContext _context;
        public TodoService(DataContext context)
        {
            _context = context;
        }

        public async Task<TodoModel> GetTodoInfoByUsernameAsync(string username) => await _context.TodoInfo.SingleOrDefaultAsync(user => user.Username == username);
        private async Task<TodoModel> GetTodoByIdAsync(string username, int id)
        {   await GetTodoInfoByUsernameAsync(username);
            return await _context.TodoInfo.FindAsync(id);
        }
        public async Task<bool> CreateTodo(TodoModel todo)
        { 
            bool result;
            _context.Add(todo);
            result = _context.SaveChanges() != 0;
            return result;
        }

        public async Task<bool> SoftDeleteTodo(string username, int id)
        { 
            var todo = await GetTodoByIdAsync(username, id);

            if(todo == null) return false;

            todo.Deleted = true;

            _context.TodoInfo.Update(todo);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> HardDeleteTodo(string username, int id)
        {   await GetTodoInfoByUsernameAsync(username);
            var foundItem = _context.TodoInfo.FirstOrDefault(t => t.Id == id);

            if (foundItem == null)
            {
                return false;
            }

            _context.TodoInfo.Remove(foundItem);
            return _context.SaveChanges() > 0;
        }
        public async Task<TodoModel> GetIncompleteTodos(string username)
        {   await GetTodoInfoByUsernameAsync(username);
            return _context.TodoInfo.FirstOrDefault(t => t.Completed == false);
        }

        public async Task<TodoModel> GetTodos(string username)
        {   
            return _context.TodoInfo.FirstOrDefault(user => user.Username == username);
        }

        public async Task<bool> UpdateTodo(string username, int id)
        {   
            var todo = await GetTodoByIdAsync(username, id);

            if(todo == null) return false;

            todo.Deleted = true;

            _context.TodoInfo.Update(todo);
            return await _context.SaveChangesAsync() != 0;
        }
    }
}