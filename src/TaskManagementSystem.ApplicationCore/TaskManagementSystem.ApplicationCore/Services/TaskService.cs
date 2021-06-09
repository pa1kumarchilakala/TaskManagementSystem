using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<bool> UpdateTask(TasksViewModel tasksVM)
        {
            var task = _mapper.Map<Tasks>(tasksVM);

            return await _taskRepository.UpdateTask(task);
        }
    }
}
