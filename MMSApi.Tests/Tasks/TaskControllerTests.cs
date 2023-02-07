using DataLibrary.DTO;
using DataLibrary.StatePattern;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using MMSAPI.Controllers;
using MMSAPI.Repository;
using MMSAPI.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MMSApi.Tests.Tasks
{
    public class TaskControllerTests
    {
        private ITaskRepository _taskRepository;
        private ISprintRepository _sprintRepository;
        private IUserRepository _userRepository;
        private TaskController _taskController;

        public IEntityUpdateHandler _entityUpdateHandler { get; }

        public TaskControllerTests()
        {
            _taskRepository = A.Fake<ITaskRepository>();
            _sprintRepository = A.Fake<ISprintRepository>();
            _userRepository = A.Fake<IUserRepository>();
            _entityUpdateHandler =A.Fake<IEntityUpdateHandler>();
            _taskController = A.Fake<TaskController>();
        }

        [Fact]
        public async Task Create_Task_Response_Ok()
        {
            TaskDTO task = new TaskDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name = "test",
                Description = "DescriptioTest",

                Status = "Status",
                Username = "UsernameTest",
            };
            var response = _taskController.Create(task);
            var httpResponse = response as OkObjectResult;
            Assert.Equal(200, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Create_Task_Response_NotOk()
        {
            TaskDTO task = null;
            var response = _taskController.Create(task);
            var httpResponse = response as BadRequestResult;
            Assert.Equal(400, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Update_Task_Response_Ok()
        {
            TaskDTO task = new TaskDTO
            {
                Id = Guid.NewGuid().ToString(),
                Name = "test",
                Description = "DescriptioTest",

                Status = "Status",
                Username = "UsernameTest",
            };
            var response = _taskController.Update(task);
            Assert.Equal(200, ((Microsoft.AspNetCore.Mvc.ObjectResult)response).StatusCode);
        }

        [Fact]
        public async Task Update_Task_Response_NotOk()
        {
            TaskDTO task = null;
            var response = _taskController.Update(task);
            var httpResponse = response as BadRequestResult;
            Assert.Equal(400, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Delete_Task_Response_Ok()
        {
            var Id = Guid.NewGuid().ToString();
            var response = _taskController.Delete(Id);
            Assert.Equal(200, ((Microsoft.AspNetCore.Mvc.ObjectResult)response).StatusCode);
        }

    

        [Fact]
        public async Task Get_By_Id_Task_Response_Ok()
        {
            var Id = Guid.NewGuid().ToString();
            var response = _taskController.GetById(Id);
            Assert.Equal(200, ((Microsoft.AspNetCore.Mvc.ObjectResult)response).StatusCode);
        }

    

        [Fact]
        public async Task Get_All_Sprint_Task_Response_Ok()
        {
            var sprintId = "123";
            var response = _taskController.GetAll(sprintId);
            Assert.Equal(200, ((Microsoft.AspNetCore.Mvc.ObjectResult)response).StatusCode);
        }

        [Fact]
        public async Task Get_All_Task_Response_Ok()
        {
            var response = _taskController.GetAll();
            Assert.Equal(200, ((Microsoft.AspNetCore.Mvc.ObjectResult)response).StatusCode);
        } 
    }
}
