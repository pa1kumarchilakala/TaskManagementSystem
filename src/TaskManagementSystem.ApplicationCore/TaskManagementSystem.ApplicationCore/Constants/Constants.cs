using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.ApplicationCore.Constants
{
    public static class StatusLookUpConstants
    {
        public const string Completed = "CO";
        public const string InProgress = "IP";
        public const string Planned = "PL";
    }

    public static class ValidateTasksConstants
    {
        public const string NoSubTaskInProgress = "No sub tasks are in-progress status. Please make atleast one sub task as in-progress.";
        public const string NoTasksFound = "No tasks found";
        public const string SubTaskPlanned = "One or more sub tasks is still in Planned status.";
        public const string UpdateSuccess = "Task updated successfully";
    }
}
