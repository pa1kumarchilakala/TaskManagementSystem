using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Infrastructure.Data.Models;

namespace TaskManagementSystem.Infrastructure.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementContext _dbContext;

        public TaskRepository(TaskManagementContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<bool> CreateTask(Tasks tasks)
        {
            try
            {
                await _dbContext.Tasks.AddAsync(tasks);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<IList<Tasks>> GetAllTasks()
        {
            return await _dbContext.Tasks.ToListAsync();
        }

        public async Task<IList<Tasks>> GetSubTasks(int? parentTaskId)
        {
            return await _dbContext.Tasks.Where(task => task.ParentTask == parentTaskId).ToListAsync();
        }

        public async Task<Tasks> GetTask(int? taskId)
        {
            return await _dbContext.Tasks.AsNoTracking().Where(task => task.Id == taskId).FirstOrDefaultAsync();
        }

        public async Task<List<Tasks>> GetTasksByStatus(string status)
        {
            return await _dbContext.Tasks.Where(task => task.State == status).ToListAsync();
        }
        public async Task UpdateTask(Tasks task)
        {
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();
        }
    }
}
