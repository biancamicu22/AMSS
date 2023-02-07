using DataLibrary;
using DataLibrary.DTO;
using DataLibrary.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMSApi.Tests.Users;
using MMSAPI;
using MMSAPI.Controllers;
using MMSAPI.Models;
using MMSAPI.Repository;
using MMSAPI.Validations;
using MMSAPI.Validations.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MMSApi.Tests.Comments
{
    public class CommentControllerTests : IClassFixture<TestFixture<Startup>>
    {
        private CommentController Controller { get; }
        private ITaskRepository _taskRepository;
        private ISprintRepository _sprintRepository;
        private IUserRepository _userRepository;
        private ICommentRepository _commentRepository;
        private TaskController _taskController;

        public IEntityUpdateHandler _entityUpdateHandler { get; }

        private List<Comment> Comments = new List<Comment>();
        private List<User> Users = new List<User>();
        private List<AppTask> Tasks = new List<AppTask>();

        private CommentDTO TestComment = new CommentDTO
        {
            Id = Guid.NewGuid().ToString(),
            Description = "Description",
            Date_Posted = DateTime.UtcNow,
            User_Id = Guid.NewGuid().ToString(),
            Task_Id = Guid.NewGuid().ToString(),
        };

        public CommentControllerTests(TestFixture<Startup> fixture)
        {

            _taskRepository = A.Fake<ITaskRepository>();
            _sprintRepository = A.Fake<ISprintRepository>();
            _userRepository = A.Fake<IUserRepository>();
            _entityUpdateHandler = A.Fake<IEntityUpdateHandler>();
            _taskController = A.Fake<TaskController>();

            var commentRepository = new Mock<ICommentRepository>();


            Comments = CommentsHelper.GenerateComments(5);
            Users = UserHelpers.GenerateUsersManually(6);
            Tasks = new List<AppTask>
            {
                new AppTask
                {
                    User = Users[0],
                    Id = "0",
                    Description = "Task 1",
                    Name = "Task Name 1",
                    Sprint = new Sprint()
                }
            };

            var modelComment = TestComment.ToModel();

            commentRepository.Setup(c => c.GetAll()).Returns(Comments);
            commentRepository.Setup(c => c.GetById(It.IsAny<string>())).Returns((string id) => Comments.FirstOrDefault(c => c.Id == id));

            var taskRepository = new Mock<ITaskRepository>();
            taskRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns((string id) => Tasks.FirstOrDefault(u => u.Id == id));

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.GetAll()).Returns(Users);
            userRepository.Setup(x => x.GetById(It.IsAny<string>())).Returns((string id) => Users.FirstOrDefault(u => u.Id == id));
            userRepository.Setup(x => x.Edit(It.IsAny<User>())).Returns((User user) => {
                var usrId = Users.FindIndex(u => u.Id == u.Id);
                Users[usrId] = user;
                return user;
            }
            );

            var dbContext = new Mock<MMSContext>();
            dbContext.Setup(x => x.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

            Controller = new CommentController(commentRepository.Object, userRepository.Object, _entityUpdateHandler, taskRepository.Object);
        }


        [Fact]
        public void GetAllComments()
        {
            //Act
            var result = (Controller.GetAll() as OkObjectResult).Value as List<CommentDTO>;
            // Assert
            Assert.Equal(result.Count(), Comments.Count);
        }

        [Fact]
        public async Task CreateCommentWithInvalidData()
        {
            var beforeCount = Comments.Count;
            var newComment = new CommentDTO
            {
                Description = "Description",
                Date_Posted = DateTime.UtcNow,
                User_Id = Users.FirstOrDefault().Id,
                Task_Id = Guid.NewGuid().ToString(),
            };
            var user = new Mock<ClaimsPrincipal>();

            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user.Object
                }
            };
            //Act
            var response = Controller.Create(newComment);

            Assert.Equal(400, ((Microsoft.AspNetCore.Mvc.ObjectResult)response).StatusCode);
        }

        [Fact]
        public async Task CreateComment()
        {
            var userModel = Users[0];
            var newComment = new CommentDTO
            {
                Description = "Description",
                Date_Posted = DateTime.UtcNow,
                User_Id = userModel.Id,
                Task_Id = "0",
            };
            var user = new Mock<ClaimsPrincipal>();
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userModel.UserName));
            claims.Add(new Claim(ClaimTypes.Email, userModel.Email));
            claims.Add(new Claim("UserID", userModel.Id));


            var identity = new ClaimsPrincipal(new ClaimsIdentity(claims));
            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = identity,
                }
            };
            //Act
            var response = Controller.Create(newComment);
            var result = response as OkObjectResult;
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteCommentWithNullID()
        {
            //Act
            var actionResult = Controller.Delete(null) as BadRequestResult;

            //Assert
            Assert.NotNull(actionResult);   
        }

        [Fact]
        public async Task GetCommentByExistingId()
        {
            //Arrange
            var comment = Comments.First();

            //Act
            var actionResult = Controller.GetById(comment.Id) as OkObjectResult;

            //Assert
            Assert.NotNull(actionResult);

            var response = actionResult.Value as CommentDTO;
            Assert.Equal(comment.Id, response.Id);
        }

        [Fact]
        public async Task UpdateComment()
        {
            //Arrange
            var comment = Comments.First();

            //Act
            var actionResult = Controller.Update(CommentDTO.FromModel(comment)) as ObjectResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal(200, actionResult.StatusCode);
        }
    }
}
