using DataLibrary.DTO;
using Microsoft.AspNetCore.Mvc;
using MMSAPI;
using MMSAPI.Controllers;
using MMSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MMSApi.Tests.Users
{
    public class UserControllerTests : IClassFixture<TestFixture<Startup>>
    {

        private UserController Controller { get; }

        private int InitialUsersNumber = 10;

        public UserControllerTests(TestFixture<Startup> fixture)
        {
            Controller = UserHelpers.GetUserControllerWithMockedDependencies(InitialUsersNumber);
        }

        [Fact]
        public void GetAllUsers()
        {
            //Act
            var result = (Controller.GetAll() as OkObjectResult).Value as List<UserDTO>;
            // Assert
            Assert.Equal(result.Count(), FakeUserRepository.UserList.Count);
        }


        [Fact]
        public async void CreateUser()
        {
            var beforeCount = FakeUserRepository.UserList.Count;
            var newUser = new UserDTO
            {
                Email = "test_user2@gmail.com",
                Password = "Password123",
                UserName = "test_user2"
            };
            //Act
            var result = (await Controller.Create(newUser) as OkObjectResult).Value as AuthResponse;
            // Assert
            Assert.Equal(result.email, newUser.Email);
            Assert.Equal(result.username, newUser.UserName);
            Assert.NotNull(result.jwt);
            Assert.Equal(FakeUserRepository.UserList.Count, beforeCount + 1);
        }


        [Fact]
        public async void LoginUserWithInvalidCredentials()
        {
            //Arrange
            var authRequest = new AuthRequest { username = "aUserNameThatDoesntExist", password = "lalal" };
            //Act
            var result = await Controller.Login(authRequest) as ObjectResult;
            // Assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Credentialele nu sunt invalide!", result.Value);
        }

        [Theory]
        [InlineData("username1", "password_1", 200)]
        [InlineData("username3", "password_5", 400)]
        [InlineData("username2", "password_2", 200)]
        [InlineData("username2", "password_3", 400)]
        [InlineData("username3", "password_3", 200)]
        public async void LoginUser(string username, string password, int statusCode)
        {
            //Arrange
            var authRequest = new AuthRequest { username = username, password = password };
            //Act
            var result = await Controller.Login(authRequest) as ObjectResult;
            // Assert
            Assert.Equal(statusCode, result.StatusCode);
        }

        [Fact]
        public async void GetMeAsLoggedUser()
        {
            //Arrange
            var user = UserHelpers.GetUserObject();
            var loggedInController = await UserHelpers.GetControllerWithUserLoggedId(user);
            //Act
            var meUser = (await loggedInController.GetMe() as OkObjectResult).Value as UserDTO;
            // Assert
            Assert.Equal(user.Id, meUser.Id);
        }

        [Fact]
        public async void LogoutUserThatIsSignedIn()
        {
            //Arrange
            var loggedInController = await UserHelpers.GetControllerWithUserLoggedId();
            //Act
            var response = await loggedInController.Logout() as OkResult;
            // Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async void EditUserInformation()
        {
            //Arrange
            var userToUpdate = UserHelpers.GetUserObject(3);
            var loggedInController = await UserHelpers.GetControllerWithUserLoggedId(userToUpdate);
            userToUpdate.Email = "test_userEmail@gmail.com";
            userToUpdate.UserName = "test_userEmail";
            //Act
            var editResult = (await loggedInController.Edit(UserDTO.FromModel(userToUpdate)) as OkObjectResult).Value as AuthResponse;
            // Assert
            Assert.Equal(editResult.email, userToUpdate.Email);
            Assert.Equal(editResult.username, userToUpdate.UserName);
            Assert.NotNull(editResult.jwt);
        }

        [Fact]
        public async void EditUserBadEmailInfo()
        {
            //Arrange
            var userToUpdate = UserHelpers.GetUserObject(3);
            var loggedInController = await UserHelpers.GetControllerWithUserLoggedId(userToUpdate);
            userToUpdate.Email = "";
            userToUpdate.UserName = "test_userEmail";
            //Act
            var editResult = await loggedInController.Edit(UserDTO.FromModel(userToUpdate)) as BadRequestResult;
            // Assert
            Assert.Equal(400, editResult.StatusCode);
        }

        [Fact]
        public async void EditUserWithoutAuth()
        {
            //Arrange
            var userToUpdate = UserHelpers.GetUserObject();
            //Act
            var editResult = await Controller.Edit(UserDTO.FromModel(userToUpdate)) as UnauthorizedResult;
            // Assert
            Assert.Equal(401, editResult.StatusCode);
        }

        [Fact]
        public async void ChangePasswordWithoutBeingAuthenticated()
        {
            //Arrange

            //Act
            var passResult = (await Controller.EditPassword(new UserController.PasswordRequest
            {
                currentPassword = "somePassword",
                newPassword = "someNewPassword"
            })) as BadRequestResult;
            // Assert
            Assert.Equal(400, passResult.StatusCode);
        }

        [Fact]
        public async void ChangePasswordWithInvalidCredentials()
        {
            //Arrange
            var userToUpdate = UserHelpers.GetUserObject(2);
            var loggedInController = await UserHelpers.GetControllerWithUserLoggedId(userToUpdate);

            //Act
            var passResult = (await loggedInController.EditPassword(new UserController.PasswordRequest
            {
                currentPassword = "somePassword",
                newPassword = "someNewPassword"
            })) as BadRequestObjectResult;
            // Assert
            Assert.Equal("Credentialele sunt invalide!", passResult.Value);
            Assert.Equal(400, passResult.StatusCode);
        }

        [Fact]
        public async void ChangePassword()
        {
            //Arrange
            var userToUpdate = UserHelpers.GetUserObject(6);
            var loggedInController = await UserHelpers.GetControllerWithUserLoggedId(userToUpdate);

            //Act
            var passResult = (await loggedInController.EditPassword(new UserController.PasswordRequest
            {
                currentPassword = userToUpdate.PasswordHash,
                newPassword = "someNewPassword"
            })) as OkObjectResult;
            // Assert
            Assert.NotNull(passResult);

            //Check if new passowrd works
            //Arrange
            var authRequest = new AuthRequest { username = userToUpdate.UserName, password = "someNewPassword" };
            //Act
            var result = await Controller.Login(authRequest) as ObjectResult;
            // Assert
            Assert.Equal(200, result.StatusCode);
        }
    }
}