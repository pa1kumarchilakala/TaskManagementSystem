using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Infrastructure.Data.Models;

namespace TaskManagementSystem.Domain.Interfaces
{
    public interface ITaskRepository:IDisposable
    {
        Task<bool> CreateTask(Tasks tasks);

        Task<IList<Tasks>> GetAllTasks();

        Task<IList<Tasks>> GetSubTasks(int parentTaskId);

        Task<Tasks> GetTask(int taskId);

        Task UpdateTask(Tasks task);

    }
}
