using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services.Context;

namespace api.Services
{
    public class TodoService
    {
        private readonly DataContext _context;
        public TodoService(DataContext context)
        {
            _context = context;
        }
        public bool CreateTodo(TodoModel todo)
        {
            bool result;
            _context.Add(todo);
            result = _context.SaveChanges() != 0;
            return result;
        }

        public bool SoftDeleteTodo(int id)
        {
            var todo = _context.TodoInfo.FirstOrDefault(t => t.Id == id);

            if (todo == null)
                return false;

            todo.Deleted = true;
            _context.Update(todo);
            return _context.SaveChanges() != 0;
        }

        public bool HardDeleteTodo(int id)
        {
            var foundItem = _context.TodoInfo.FirstOrDefault(t => t.Id == id);

            if (foundItem == null)
            {
                return false;
            }

            _context.TodoInfo.Remove(foundItem);
            return _context.SaveChanges() > 0;
        }
        public IEnumerable<TodoModel> GetIncompleteTodos()
        {
            return _context.TodoInfo.Where(item => item.Completed == false);
        }

        public IEnumerable<TodoModel> GetTodos()
        {
            return _context.TodoInfo;
        }

        public bool UpdateTodo(int id)
        {
            var todo = _context.TodoInfo.FirstOrDefault(t => t.Id == id);

            if (todo == null)
                return false;

            todo.Completed = true;
            _context.Update(todo);
            return _context.SaveChanges() != 0;
        }
    }
}