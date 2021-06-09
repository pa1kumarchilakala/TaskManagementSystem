using System;
using System.Collections.Generic;

#nullable disable

namespace TaskManagementSystem.Infrastructure.Data.Models
{
    public partial class Tasks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string State { get; set; }
        public int? ParentTask { get; set; }

        public virtual StatusLookUp StateNavigation { get; set; }
    }
}
