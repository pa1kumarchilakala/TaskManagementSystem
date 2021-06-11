using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.ApplicationCore.ViewModels
{
    public class TasksViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name should be atleast 1 character and cannot exceed 100 characters.")]
        public string Name { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Description should be atleast 1 character and cannot exceed 1000 characters.")]
        public string Description { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Assigned To should be atleast 1 character and cannot exceed 50 characters.")]
        public string AssignedTo { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        [Required]
        public string State { get; set; }
        public int? ParentTask { get; set; }
    }
}
