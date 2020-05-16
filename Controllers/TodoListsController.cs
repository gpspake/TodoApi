using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoListsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoLists
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoLists()
        {
            return await _context.TodoLists.Include((todoList) => todoList.TodoItems).ToListAsync();
        }

        // GET: api/TodoLists/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TodoList>> GetTodoList(long id)
        {
            // model.Item = await db.Items.Include(i => i.ItemVerifications)
            //     .FirstOrDefaultAsync(i => i.Id == id.Value)
            
            var todoList = await _context.TodoLists
                .Include((t) => t.TodoItems)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (todoList == null)
            {
                return NotFound();
            }

            return todoList;
        }

        // PUT: api/TodoLists/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTodoList(long id, TodoList todoList)
        {
            if (id != todoList.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoLists
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TodoList>> PostTodoList(TodoList todoList)
        {
            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoList), new { id = todoList.Id }, todoList);
        }

        // DELETE: api/TodoLists/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<TodoList>> DeleteTodoList(long id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();

            return todoList;
        }

        private bool TodoListExists(long id)
        {
            return _context.TodoLists.Any(e => e.Id == id);
        }
    }
}
