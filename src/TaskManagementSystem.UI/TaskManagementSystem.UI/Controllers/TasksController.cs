using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.ApplicationCore.Interfaces;
using TaskManagementSystem.ApplicationCore.ViewModels;
using TaskManagementSystem.UI.Models;

namespace TaskManagementSystem.UI.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            this._taskService = taskService;
        }
        // GET: TasksController
        public async Task<IActionResult> Index()
        {
            var allTasks = await GetAllTasks();
            return View(allTasks);
        }

        // GET: TasksController/Details/5
        public async Task<IActionResult> TaskDetails(int? taskId)
        {
            if (taskId == null)
                return NotFound();

            var task = await _taskService.GetTask(taskId);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // GET: TasksController/Create
        public IActionResult CreateTask()
        {
            return View();
        }

        // POST: TasksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([Bind("Name,Description,AssignedTo,StartDate,FinishDate,State")] TasksViewModel tasksViewModel)
        {
            if (ModelState.IsValid)
            {
                await _taskService.CreateTask(tasksViewModel);
                return RedirectToAction(nameof(Index));
            }
            var allTasks = await GetAllTasks();

            return View(allTasks);
        }

        // GET: TasksController/Edit/5
        public async Task<IActionResult> EditTask(int? taskId)
        {
            if (taskId == null)
            {
                return NotFound();
            }
            var task = await _taskService.GetTask(taskId);
            if(task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: TasksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> EditTask(int taskId, [Bind("Id,Name,Description,AssignedTo,StartDate,FinishDate,State,ParentTask")] TasksViewModel tasksViewModel)
        {
            if (taskId != tasksViewModel.Id)
                return NotFound();

            var task = _taskService.GetTask(taskId);

            if (task == null)
                return NotFound();

            if(ModelState.IsValid)
            {
                try
                {
                   await _taskService.UpdateTask(tasksViewModel);
                }
                catch(Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var allTasks = await GetAllTasks();

            return View(allTasks);
        }

        private async Task<TasksUIVM> GetAllTasks()
        {
            var allTasks = await _taskService.GetAllTasks();

            var tasks = allTasks.Where(task => (task.ParentTask == 0 || task.ParentTask == null)).ToList();
            var subTasks = allTasks.Where(task => (task.ParentTask != null && task.ParentTask > 0)).ToList();

            TasksUIVM tasksVM = new()
            {
                Tasks = tasks,
                SubTasks = subTasks
            };

            return tasksVM;
        }
    }
}
