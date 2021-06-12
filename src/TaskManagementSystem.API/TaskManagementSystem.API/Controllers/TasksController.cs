using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using TaskManagementSystem.ApplicationCore.Constants;
using TaskManagementSystem.ApplicationCore.Interfaces;
using TaskManagementSystem.ApplicationCore.ViewModels;
using TaskManagementSystem.Infrastructure.Utilities.Utilities;

namespace TaskManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _tasksService;

        public TasksController(ITaskService tasksService)
        {
            this._tasksService = tasksService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTask([FromBody] TasksViewModel task)
        {
            if (ModelState.IsValid)
            {
                await _tasksService.CreateTask(task);
                return CreatedAtAction(nameof(CreateTask), new { id = task.Id }, task);
            }
            else
                return BadRequest(task);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("all")]
        [HttpHead]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _tasksService.GetAllTasks();

            if (tasks == null)
                return NotFound();

            return Ok(tasks);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}/{isParent}")]
        public async Task<IActionResult> GetSubTasks(int id, bool isParent = false)
        {
            if (isParent)
            {
                var tasks = await _tasksService.GetSubTasks(id);
                if (tasks == null)
                    return NotFound();

                return Ok(tasks);
            }

            var task = _tasksService.GetTask(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var tasks = await _tasksService.GetTask(id);
            if (tasks == null)
                return NotFound();

            return Ok(tasks);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetReportByStatus([FromQuery] string status)
        {
            var tasks = await _tasksService.GetTasksByStatus(status);

            if (tasks == null)
                return NotFound();

            Stream dataStream = Convertors.ConvertObjectToCSV(tasks);
            
            if (dataStream == null)
                return NotFound();

            return File(dataStream, "application/csv", "Tasks.csv");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TasksViewModel tasksViewModel)
        {
            var result = await _tasksService.UpdateTask(tasksViewModel);

            if (result == ValidateTasksConstants.NoTasksFound)
                return NotFound();
            else if (result == ValidateTasksConstants.SubTaskInProgress)
                return BadRequest(ValidateTasksConstants.SubTaskInProgress);
            else if (result == ValidateTasksConstants.NoSubTaskInProgress)
                return BadRequest(ValidateTasksConstants.NoSubTaskInProgress);
            else if (result == ValidateTasksConstants.NoPlannedTasksFound)
                return BadRequest(ValidateTasksConstants.NoPlannedTasksFound);

            return Ok();
        }

    }
}
//