using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.ApplicationCore.Constants;
using TaskManagementSystem.ApplicationCore.Interfaces;
using TaskManagementSystem.ApplicationCore.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        // GET: api/<TasksController>
        //[HttpPost("CreateTask")]
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

        // GET api/<TasksController>/5
        //[HttpGet("GetAllTasks")]
        //[HttpHead("GetAllTasks")]
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _tasksService.GetAllTasks();

            return Ok(tasks);
        }

        //[HttpGet("GetSubTasks/parentTaskId")]
        [HttpGet("{id}/{isParent}")]
        public async Task<IActionResult> GetSubTasks(int id, bool isParent = false)
        {
            if (isParent) return Ok(await _tasksService.GetSubTasks(id));
            
            return Ok(_tasksService.GetTask(id));
        }

        // POST api/<TasksController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var tasks = await _tasksService.GetTask(id);
            return Ok(tasks);
        }

        // PUT api/<TasksController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TasksViewModel tasksViewModel)
        {
            var result = await _tasksService.UpdateTask(tasksViewModel);

            if (result == ValidateTasksConstants.NoTasksFound)
                return NotFound();
            else if (result == ValidateTasksConstants.NoSubTaskInProgress)
                return BadRequest(ValidateTasksConstants.NoSubTaskInProgress);
            else if(result == ValidateTasksConstants.SubTaskPlanned)
                return BadRequest(ValidateTasksConstants.SubTaskPlanned);
            
            return Ok();
        }
        
    }
}
