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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<int> CreateTask([FromBody] TasksViewModel task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _tasksService.CreateTask(task);
                    return StatusCodes.Status201Created;
                }
                catch
                {
                    return StatusCodes.Status500InternalServerError;
                }
            }
            else
                return StatusCodes.Status400BadRequest;
        }

        // GET api/<TasksController>/5
        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _tasksService.GetAllTasks();

            return Ok(tasks);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetSubTasks(int id)
        {
            var tasks = await _tasksService.GetSubTasks(id);
            return Ok(tasks);
        }

        // POST api/<TasksController>
        [HttpGet("id")]
        public async Task<IActionResult> GetTask(int id)
        {
            var tasks = await _tasksService.GetTask(id);
            return Ok(tasks);
        }

        // PUT api/<TasksController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TasksViewModel tasksViewModel)
        {
            var task = await _tasksService.GetTask(tasksViewModel.Id);
            
            if (task == null || task.Id < 1)
                return BadRequest(ValidateTasks.NoTasksFound);

            if (tasksViewModel?.ParentTask == 0 || tasksViewModel?.ParentTask == null) //If it is a parent task then check for sub tasks and validate
            {
                string subTasksstatus = await ValidateSubTasks(tasksViewModel);

                if (!string.IsNullOrEmpty(subTasksstatus))
                    return BadRequest(subTasksstatus);
            }
            return NoContent();
        }

        [NonAction]
        public async Task<string> ValidateSubTasks(TasksViewModel tasksViewModel)
        {
            var subTasks = await _tasksService.GetSubTasks(tasksViewModel.Id);
            if (subTasks.Count > 0)
            {
                string validationStatus = string.Empty;

                switch (tasksViewModel?.State)
                {
                    case StatusLookUpConstants.Planned://Validate completed sub tasks
                        validationStatus = ValidatePlannedSubTasks(subTasks);
                        break;

                    case StatusLookUpConstants.InProgress://Validate In-progress sub tasks
                        validationStatus = ValidateInProgressSubTasks(subTasks);
                        break;
                }

                if (!string.IsNullOrEmpty(validationStatus))
                    return validationStatus;
            }
            return string.Empty;
        }

        [NonAction]
        public string ValidateInProgressSubTasks(IList<TasksViewModel> tasksViewModel)
        {
            var tasks = tasksViewModel.Where(task => task.State == StatusLookUpConstants.InProgress).FirstOrDefault();

            if (tasks != null && tasks.Id > 0)
                return ValidateTasks.SubTaskInProgress;
            else
                return string.Empty;
        }

        [NonAction]
        public string ValidatePlannedSubTasks(IList<TasksViewModel> tasksViewModel)
        {
            var tasks = tasksViewModel.Where(task => task.State == StatusLookUpConstants.Planned).FirstOrDefault();

            if (tasks != null && tasks.Id > 0)
                return ValidateTasks.SubTaskPlanned;
            else
                return string.Empty;
        }
    }
}
