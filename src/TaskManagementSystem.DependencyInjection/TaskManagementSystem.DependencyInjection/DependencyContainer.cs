using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Infrastructure.Data.Models;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Infrastructure.Data.Repositories;
using TaskManagementSystem.ApplicationCore.Interfaces;
using TaskManagementSystem.ApplicationCore.Services;
using System.Reflection;

namespace TaskManagementSystem.DependencyInjection
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Connection String
            services.AddDbContext<TaskManagementContext>(options =>
                options.UseSqlServer(configuration.GetSection("ConnectionStrings:DefaultConnection").ToString())
            );

            //TaskManagementSystem.Domain Dependencies
            services.AddScoped<ITaskRepository, TaskRepository>();

            //TaskManagementSystem.ApplicationCore Dependencies
            services.AddScoped<ITaskService, TaskService>();

            //Automapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
