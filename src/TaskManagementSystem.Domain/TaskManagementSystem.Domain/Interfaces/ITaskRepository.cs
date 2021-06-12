using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Infrastructure.Data.Models;

namespace TaskManagementSystem.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<bool> CreateTask(Tasks tasks);

        Task<IList<Tasks>> GetAllTasks();

        Task<IList<Tasks>> GetSubTasks(int? parentTaskId);

        Task<Tasks> GetTask(int? taskId);

        Task<List<Tasks>> GetTasksByStatus(string status);

        Task UpdateTask(Tasks task);

    }
}
