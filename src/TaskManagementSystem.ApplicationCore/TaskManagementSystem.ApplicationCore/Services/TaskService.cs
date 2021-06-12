using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.ApplicationCore.Constants;
using TaskManagementSystem.ApplicationCore.Interfaces;
using TaskManagementSystem.ApplicationCore.ViewModels;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Infrastructure.Data.Models;

namespace TaskManagementSystem.ApplicationCore.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, IMapper mapper)
        {
            this._taskRepository = taskRepository;
            this._mapper = mapper;
        }
        /// <summary>
        /// Creates a task. Receives the task object from API layer and sends to infra(data) layer to insert record in Database using EFCore.
        /// </summary>
        /// <param name="tasksVM"></param>
        /// <returns></returns>
        public async Task<bool> CreateTask(TasksViewModel tasksVM)
        {
            var tasks = _mapper.Map<Tasks>(tasksVM);

            return await _taskRepository.CreateTask(tasks);
        }
        /// <summary>
        /// Retreives all tasks from Database. Retreives all the tasks from DB (from infrastructure(data) layer).
        /// </summary>
        /// <returns></returns>
        public async Task<IList<TasksViewModel>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllTasks();

            return _mapper.Map<IList<TasksViewModel>>(tasks);
        }
        /// <summary>
        /// Retreives all sub tasks from Database based on the parent id. 
        /// </summary>
        /// <param name="parentTaskId"></param>
        /// <returns></returns>
        public async Task<IList<TasksViewModel>> GetSubTasks(int? parentTaskId)
        {
            var tasks = await _taskRepository.GetSubTasks(parentTaskId);
            
            return _mapper.Map<IList<TasksViewModel>>(tasks);
        }
        /// <summary>
        /// Retreives the specific task from Database based on the task id.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<TasksViewModel> GetTask(int? taskId)
        {
            var task = await _taskRepository.GetTask(taskId);
            
            return _mapper.Map<TasksViewModel>(task);
        }

        /// <summary>
        /// Retreives tasks by Status from Database based on the status code.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<List<TasksViewModel>> GetTasksByStatus(string status)
        {
            var tasks = await _taskRepository.GetTasksByStatus(status);
            return _mapper.Map<List<TasksViewModel>>(tasks);
        }

        /// <summary>
        /// Updates the specific task to database.
        /// </summary>
        /// <param name="tasksVM"></param>
        /// <returns></returns>
        public async Task<string> UpdateTask(TasksViewModel tasksVM)
        {
            string taskValidation = await ValidateTasks(tasksVM);
            if (!string.IsNullOrEmpty(taskValidation))
                return taskValidation;

            var task = _mapper.Map<Tasks>(tasksVM);
            if (task.State == StatusLookUpConstants.Completed)
                task.FinishDate = DateTime.Now.Date;

            await _taskRepository.UpdateTask(task);

            return ValidateTasksConstants.UpdateSuccess;
        }

        /// <summary>
        /// Validates the given task if it has parent id or not. If not, then it means its a parent task and 
        /// the below logic will find the sub tasks for the parent id.
        /// </summary>
        /// <param name="tasksVM"></param>
        /// <returns></returns>
        private async Task<string> ValidateTasks(TasksViewModel tasksVM)
        {
            var task = await _taskRepository.GetTask(tasksVM.Id);

            if(task !=null && task.Id > 0)
            {
                if (task?.ParentTask == 0 || task?.ParentTask == null)
                {
                    return await ValidateSubTasks(task);
                }
                return string.Empty;
            }
            task = null;
            return ValidateTasksConstants.NoTasksFound;
        }

        /// <summary>
        /// Validat sub tasks for the parent task. This will also validate the subtasks if parent task status is changed to Inp
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        private async Task<string> ValidateSubTasks(Tasks tasks)
        {
            var subTasks = await _taskRepository.GetSubTasks(tasks.Id);
            if (subTasks.Count > 0)
            {
                string validationStatus = string.Empty;

                switch (tasks?.State)
                {
                    case StatusLookUpConstants.Completed://Before changing the Parent task to Completed, check if there are any in-progress sub taks.
                        validationStatus = ValidateCompletedSubTasks(subTasks);
                        break;

                    case StatusLookUpConstants.InProgress://Before changing the Parent task to In-progress, check if atleast one sub task is in-progress.
                        validationStatus = ValidateInProgressSubTasks(subTasks);
                        break;

                    case StatusLookUpConstants.Planned://Before changing the Parent task to planned, check if atleast one sub task is in planned status or if sub tasks exists.
                        validationStatus = ValidatePlannedSubTasks(subTasks);
                        break;
                }

                if (!string.IsNullOrEmpty(validationStatus))
                    return validationStatus;
            }
            return string.Empty;
        }

        /// <summary>
        /// Validate the sub tasks having atleast one non completed status before changing the Parent task to Completed.
        /// </summary>
        /// <param name="subTasksCount"></param>
        /// <param name="subTasks"></param>
        /// <returns></returns>
        private string ValidateCompletedSubTasks(IList<Tasks> subTasks)
        {
            var tasks = subTasks.Where(task => task.State != StatusLookUpConstants.Completed).ToList();

            if (tasks == null || (tasks != null && tasks.Count < 1))
                return string.Empty;
            else
                return ValidateTasksConstants.SubTaskInProgress;
        }
        /// <summary>
        /// Validate the sub tasks having atleast one in-progress status before changing the Parent task to In-progress.
        /// </summary>
        /// <param name="subTasks"></param>
        /// <returns></returns>
        private string ValidateInProgressSubTasks(IList<Tasks> subTasks)
        {
            var tasks = subTasks.Where(task => task.State == StatusLookUpConstants.InProgress).ToList();

            if (tasks == null || (tasks != null && tasks.Count < 1))
                return ValidateTasksConstants.NoSubTaskInProgress;
            else
                return string.Empty;
        }

        /// <summary>
        /// Validate the sub tasks having atleast one planned status before changing the Parent task to Planned.
        /// </summary>
        /// <param name="subTasks"></param>
        /// <returns></returns>
        private string ValidatePlannedSubTasks(IList<Tasks> subTasks)
        {
            var tasks = subTasks.Where(task => task.State == StatusLookUpConstants.Planned).ToList();

            if (tasks == null || (tasks != null && tasks.Count < 1))
                return ValidateTasksConstants.NoPlannedTasksFound;
            else
                return string.Empty;
        }

    }
}
