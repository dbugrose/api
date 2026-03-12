using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services.Context;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<List<TodosModel>> GetTodosAsync () => await _context.TodosInfo.ToListAsync();
        private async Task<TodosModel> GetTodoByIdAsync(int id)
        {
            return await _context.TodosInfo.FindAsync(id);
        }
        public async Task<bool> AddTodoAsync(TodosModel todo)
        { 
             await _context.TodosInfo.AddAsync(todo);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditTodoAsync(TodosModel todo)
        {
            var todoToEdit = await GetTodoByIdAsync(todo.Id);

            if(todoToEdit == null) return false;

            todoToEdit.Text = todo.Text;
            todoToEdit.Difficulty = todo.Difficulty;
            todoToEdit.Completed = todo.Completed;
            todoToEdit.Deleted = todo.Deleted;
            

            _context.TodosInfo.Update(todoToEdit);
            return await _context.SaveChangesAsync() != 0;
        }

public async Task<bool> HardDeleteTodo(TodosModel todo)
{
    _context.TodosInfo.Remove(todo);
    return await _context.SaveChangesAsync() > 0;
}

// public async Task<bool> HardDeleteUnassignedTodo(int id)
// {
//     var foundItem = await _context.TodosInfo
//         .FirstOrDefaultAsync(t => t.Id == id);

//     if (foundItem == null)
//         return false;

//     _context.TodosInfo.Remove(foundItem);
//     return await _context.SaveChangesAsync() > 0;
// }

        

        public async Task<List<TodosModel>> GetTodoByUserIdAsync(int id) => await _context.TodosInfo.Where(todo => todo.UserId == id).ToListAsync();

        public async Task<List<TodosModel>> GetIncompleteTodosAsync() => await _context.TodosInfo.Where(todo => todo.Completed == false).ToListAsync();
    // }

    }
}