using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.APITests.Brokers;
using TaskManagementSystem.ApplicationCore.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace TaskManagementSystem.APITests
{
    [Collection(nameof(APITestCollection))]
    public class TasksUnitTests
    {
        private readonly TasksAPIBroker _tasksAPIBroker;
        private readonly ITestOutputHelper _testOutputHelper;
        public TasksUnitTests(TasksAPIBroker tasksAPIBroker, ITestOutputHelper testOutputHelper)
        {
            this._tasksAPIBroker = tasksAPIBroker;
            this._testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ValidateTask_Fail_Test()
        {
            //Arrange
            TasksViewModel task = ConstructTaskData();
            task.Name = string.Empty;

            //Act
            TasksViewModel actualTask = await this._tasksAPIBroker.CreateTask(task);

            //Assert
            Assert.NotNull(actualTask);
        }

        [Fact]
        public async Task Create_Task_Pass_Test()
        {
            //Arrange
            TasksViewModel task = ConstructTaskData();
            TasksViewModel inputTask = task;
            TasksViewModel expectedTask = inputTask;

            //Act
            TasksViewModel actualTask = await this._tasksAPIBroker.CreateTask(inputTask);


            //Assert
            Assert.Equal(expectedTask.Name, actualTask.Name);
        }


        [Fact]
        public async Task Get_Task_Pass_Test()
        {
            //Arrange
            int taskId = 1;

            //Act
            TasksViewModel task = await this._tasksAPIBroker.GetTask(taskId);

            //Assert
            _testOutputHelper.WriteLine("ID: " + task.Id.ToString());
            Assert.Equal(taskId, task.Id);
        }

        [Fact]
        public async Task Get_Task_Fail_Test()
        {
            //Arrange
            int taskId = 100;

            //Act
            TasksViewModel task = await this._tasksAPIBroker.GetTask(taskId);

            //Assert
            Assert.NotEqual(taskId, task.Id);
        }

        [Fact]
        public async Task Get_SubTasks_Pass_Test()
        {
            //Arrange
            int parentId = 1;

            //Act
            IList<TasksViewModel> tasks = await this._tasksAPIBroker.GetSubTasks(parentId, true);

            //Assert
            Assert.NotEmpty(tasks);
        }
        
        [Fact]
        public async Task Get_SubTasks_Fail_Test()
        {
            //Arrange
            int parentId = 20;

            //Act
            IList<TasksViewModel> tasks = await this._tasksAPIBroker.GetSubTasks(parentId, true);

            //Assert
            Assert.NotEmpty(tasks);
        }
        private TasksViewModel ConstructTaskData()
        {
            TasksViewModel tasksViewModel = new TasksViewModel();
            tasksViewModel.Name = "My Test Task 1";
            tasksViewModel.Description = "This is a testing task.";
            tasksViewModel.StartDate = DateTime.Now;
            tasksViewModel.AssignedTo = "Pavan";
            tasksViewModel.State = "PL";

            return tasksViewModel;
        }
        private TasksViewModel ConstructSubTaskData()
        {
            TasksViewModel tasksViewModel = new TasksViewModel();
            tasksViewModel.Name = "My Test Sub Task 1";
            tasksViewModel.Description = "This is a testing sub task.";
            tasksViewModel.StartDate = DateTime.Now;
            tasksViewModel.AssignedTo = "Kumar";
            tasksViewModel.State = "PL";
            tasksViewModel.ParentTask = 1;

            return tasksViewModel;
        }
    }
}
