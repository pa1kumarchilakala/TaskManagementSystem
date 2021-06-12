using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.ApplicationCore.ViewModels;

namespace TaskManagementSystem.UI.Models
{
    public class TasksUIVM
    {
        public IList<TasksViewModel> Tasks { get; set; }
        public IList<TasksViewModel> SubTasks { get; set; }
    }
}
