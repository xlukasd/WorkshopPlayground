using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tasks.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private static readonly List<TaskItem> Tasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Buy groceries", Description = "Milk, Bread, Cheese", DueDate = DateTime.Now.AddDays(1), IsCompleted = false },
            new TaskItem { Id = 2, Title = "Finish project", Description = "Complete the API refactor", DueDate = DateTime.Now.AddDays(2), IsCompleted = false }
        };

        [HttpGet]
        public IEnumerable<TaskItem> Get()
        {
            return Tasks;
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetById(int id)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            return task;
        }

        [HttpPost]
        public ActionResult<TaskItem> Create(TaskItem task)
        {
            task.Id = Tasks.Count == 0 ? 1 : Tasks.Max(t => t.Id) + 1;
            Tasks.Add(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, TaskItem updatedTask)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.DueDate = updatedTask.DueDate;
            task.IsCompleted = updatedTask.IsCompleted;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            Tasks.Remove(task);
            return NoContent();
        }
    }
}
