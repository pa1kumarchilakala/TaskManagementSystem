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

        public async Task<bool> CreateTask(TasksViewModel tasksVM)
        {
            var tasks = _mapper.Map<Tasks>(tasksVM);

            return await _taskRepository.CreateTask(tasks);
        }

        public async Task<IList<TasksViewModel>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllTasks();

            return _mapper.Map<IList<TasksViewModel>>(tasks);
        }

        public async Task<IList<TasksViewModel>> GetSubTasks(int parentTaskId)
        {
            var tasks = await _taskRepository.GetSubTasks(parentTaskId);
            
            return _mapper.Map<IList<TasksViewModel>>(tasks);
        }

        public async Task<TasksViewModel> GetTask(int taskId)
        {
            var task = await _taskRepository.GetTask(taskId);
            
            return _mapper.Map<TasksViewModel>(task);
        }

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

        private async Task<string> ValidateSubTasks(Tasks tasks)
        {
            var subTasks = await _taskRepository.GetSubTasks(tasks.Id);
            if (subTasks.Count > 0)
            {
                string validationStatus = string.Empty;

                switch (tasks?.State)
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

        private string ValidateInProgressSubTasks(IList<Tasks> Tasks)
        {
            var tasks = Tasks.Where(task => task.State == StatusLookUpConstants.InProgress).FirstOrDefault();

            if (tasks == null || tasks.Id == 0)
                return ValidateTasksConstants.NoSubTaskInProgress;
            else
                return string.Empty;
        }

        private string ValidatePlannedSubTasks(IList<Tasks> Tasks)
        {
            var tasks = Tasks.Where(task => task.State == StatusLookUpConstants.Planned).FirstOrDefault();

            if (tasks != null && tasks.Id > 0)
                return ValidateTasksConstants.SubTaskPlanned;
            else
                return string.Empty;
        }
    }
}
