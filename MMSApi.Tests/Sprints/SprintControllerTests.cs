using DataLibrary.DTO;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using MMSAPI;
using MMSAPI.Controllers;
using MMSAPI.Repository;
using MMSAPI.Validations;
using MMSAPI.Validations.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MMSApi.Tests.Sprints
{
    public class SprintControllerTests : IClassFixture<TestFixture<Startup>>
    {
        private SprintController Controller;

        private ISprintRepository fakeRepository;

        private int IntitialSprintCount = 20;

        public SprintControllerTests(TestFixture<Startup> fixture)
        {
            fakeRepository = FakeSprintRepository.GetFakeSprintRepository(IntitialSprintCount);

            var entityUpdate = new Mock<IEntityUpdateHandler>();

            entityUpdate.Setup(x => x.Update<Sprint>(It.IsAny<Sprint>()))
                .Returns((Sprint sprint) =>
                    new EntityHandlerResult<Sprint>(null, sprint));

            Controller = new SprintController(fakeRepository, null, entityUpdate.Object);
        }

        [Fact]
        public void CreateSprint()
        {
            //Arrange
            var newSprint = SprintHelper.GetTestSprint(true);
            //Act
            var actionResult = Controller.Create(SprintDTO.FromModel(newSprint)) as OkObjectResult;
            //Assert
            Assert.NotNull(actionResult);
            var responseObject = actionResult.Value as SprintDTO;
            Assert.NotNull(responseObject);
            Assert.Equal(newSprint.End_Date, responseObject.End_Date);
            Assert.Equal(newSprint.Start_Date, responseObject.Start_Date);
            Assert.Equal(newSprint.Goal, responseObject.Goal);
            Assert.Equal(newSprint.Name, responseObject.Name);
        }

        [Fact]
        public void CreateSprintFromEmptyData()
        {
            //Arrange

            //Act
            var actionResult = Controller.Create(null) as BadRequestResult;
            //Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData("2")]
        [InlineData("1")]
        [InlineData("4")]
        [InlineData("0")]
        public void DeleteSprintById(string id)
        {
            //Arrange
            int initialCount = FakeSprintRepository.Sprints.Count;
            //Act
            var actionResult = Controller.Delete(id) as OkObjectResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.True((bool)actionResult.Value);
            Assert.Equal(FakeSprintRepository.Sprints.Count, initialCount - 1);
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("20000")]
        [InlineData("-21")]
        [InlineData("21")]
        public void DeleteUnexistingSprintById(string id)
        {
            //Arrange
            int initialCount = FakeSprintRepository.Sprints.Count;
            //Act
            Controller.Delete(id);
            //Assert
            Assert.Equal(FakeSprintRepository.Sprints.Count, initialCount);
        }

        [Fact]
        public void DeleteSprintWithNullAsId()
        {
            //Arrange
            int initialCount = FakeSprintRepository.Sprints.Count;
            //Act
            var actionResult = Controller.Delete(null) as BadRequestResult;
            //Assert
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void GetExistingSprintById()
        {
            //Arrange
            var sprint = FakeSprintRepository.Sprints[1];
            //Act
            var actionResult = Controller.GetById(sprint.Id) as OkObjectResult;
            //Assert
            Assert.NotNull(actionResult);
            var response = actionResult.Value as SprintDTO;
            Assert.Equal(sprint.Id, response.Id);
        }

        [Fact]
        public void GetUnexistingSprintById()
        {
            //Arrange
            var id = "someid";
            //Act
            var actionResult = Controller.GetById(id) as OkObjectResult;
            //Assert
            Assert.NotNull(actionResult);
            var response = actionResult.Value as SprintDTO;
            Assert.Null(response);
        }

        [Fact]
        public void GetSprintByIdWithNullId()
        {
            //Act
            var actionResult = Controller.GetById(null) as BadRequestResult;
            //Assert
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void GetAllSprints()
        {
            //Act
            var actionResult = Controller.GetAll() as OkObjectResult;
            //Assert
            Assert.NotNull(actionResult);
            var allSprints = actionResult.Value as List<Sprint>;
            Assert.Equal(FakeSprintRepository.Sprints.Count, allSprints.Count);
        }

        [Fact]
        public void EditSprint()
        {
            //Arrange
            var sprint = SprintHelper.GetTestSprint();
            //Act
            var actionResult = Controller.Update(SprintDTO.FromModel(sprint)) as ObjectResult;
            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal(200, actionResult.StatusCode);
            var response = actionResult.Value as EntityHandlerResult<Sprint>;
            Assert.Equal(sprint.Id, response.SuccessResult.Id);
        }

        [Fact]
        public void EditNullSprint()
        {
            //Act
            var actionResult = Controller.Update(null) as BadRequestResult;
            //Assert
            Assert.NotNull(actionResult);
        }
    }
}
