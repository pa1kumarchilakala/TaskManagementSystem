using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.ApplicationCore.ViewModels;
using TaskManagementSystem.Infrastructure.Data.Models;

namespace TaskManagementSystem.ApplicationCore.Interfaces
{
    public interface ITaskService
    {
        Task<bool> CreateTask(TasksViewModel tasksVM);

        Task<IList<TasksViewModel>> GetAllTasks();

        Task<IList<TasksViewModel>> GetSubTasks(int parentTaskId);

        Task<TasksViewModel> GetTask(int taskId);

        Task<string> UpdateTask(TasksViewModel tasksVM);
    }
}
