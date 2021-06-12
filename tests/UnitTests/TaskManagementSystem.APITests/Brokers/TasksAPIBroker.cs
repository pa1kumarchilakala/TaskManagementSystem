using Microsoft.AspNetCore.Mvc.Testing;
using RESTFulSense.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.API;
using TaskManagementSystem.ApplicationCore.ViewModels;

namespace TaskManagementSystem.APITests.Brokers
{
    public class TasksAPIBroker
    {
        private readonly WebApplicationFactory<Startup> webApplicationFactory;
        private readonly HttpClient baseClient;
        private readonly IRESTFulApiFactoryClient apiFactoryClient;
        private const string TasksUrl = "api/Tasks";
        public TasksAPIBroker()
        {
            this.webApplicationFactory = new WebApplicationFactory<Startup>();
            this.baseClient = this.webApplicationFactory.CreateClient();
            this.apiFactoryClient = new RESTFulApiFactoryClient(this.baseClient);
        }

        public async ValueTask<TasksViewModel> CreateTask(TasksViewModel task)
        {
            return await this.apiFactoryClient.PostContentAsync(TasksUrl, task);
        }

        public async ValueTask<TasksViewModel> GetAllTasks()
        {
            return await this.apiFactoryClient.GetContentAsync<TasksViewModel>($"{TasksUrl}");
        }

        public async ValueTask<IList<TasksViewModel>> GetSubTasks(int id, bool isParent)
        {
            return await this.apiFactoryClient.GetContentAsync<IList<TasksViewModel>>($"{TasksUrl}/{id}/{isParent}");
        }

        public async ValueTask<TasksViewModel> GetTask(int id)
        {
            return await this.apiFactoryClient.GetContentAsync<TasksViewModel>($"{TasksUrl}/{id}/{false}");
        }

        public async ValueTask<TasksViewModel> UpdateTask(TasksViewModel task)
        {
            return await this.apiFactoryClient.PostContentAsync(TasksUrl, task);
        }
    }
}
