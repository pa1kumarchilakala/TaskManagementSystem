using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskManagementSystem.ApplicationCore.ViewModels;
using TaskManagementSystem.Infrastructure.Data.Models;

namespace TaskManagementSystem.ApplicationCore.MappingProfiles
{
    public class TaskMappingProfiles : Profile
    {
        public TaskMappingProfiles()
        {
            ////Domain to View
            CreateMap<Tasks, TasksViewModel>();

            ////View to Domain
            CreateMap<TasksViewModel, Tasks>();

            
        }
    }
}
