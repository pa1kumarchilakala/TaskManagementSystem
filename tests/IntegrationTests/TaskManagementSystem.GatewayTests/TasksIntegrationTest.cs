using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TaskManagementSystem.APIGateway;
using TaskManagementSystem.ApplicationCore.ViewModels;
using Xunit;

namespace TaskManagementSystem.GatewayTests
{
    public class TasksIntegrationTest
    {
        private readonly WebApplicationFactory<Startup> _webApplicationFactory;
        private readonly HttpClient _baseClient;

        public TasksIntegrationTest()
        {
            _webApplicationFactory = new WebApplicationFactory<Startup>();
            _baseClient = _webApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task Can_Get_Tasks()
        {
            var httpResponse = await _baseClient.GetAsync("/api/Tasks");

            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadAsStringAsync();

            var tasks = JsonConvert.DeserializeObject<IList<TasksViewModel>>(response);

            Assert.Contains(tasks, task => task.Name == "Pavan");
        }
    }
}
