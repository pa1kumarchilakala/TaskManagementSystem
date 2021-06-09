using System;
using System.Collections.Generic;

#nullable disable

namespace TaskManagementSystem.Infrastructure.Data.Models
{
    public partial class StatusLookUp
    {
        public StatusLookUp()
        {
            Tasks = new HashSet<Tasks>();
        }

        public string Code { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
