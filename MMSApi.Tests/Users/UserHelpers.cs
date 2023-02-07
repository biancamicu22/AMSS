using AutoFixture;
using AutoFixture.Kernel;
using DataLibrary;
using DataLibrary.Models;
using DataLibrary.StatePattern;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MMSAPI.Controllers;
using MMSAPI.Models;
using MMSAPI.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MMSApi.Tests.Users
{
    public static class UserHelpers
    {
        private static UserController _controller;
        private static IUserRepository _fakeUserRepository;
        private static FakeUserManager _fakeUserManager;
        private static FakeSignInManager _fakeSigInManager;
        private static MMSContext _fakeDbContext;

        public static UserController GetUserControllerWithMockedDependencies(int initialUsers)
        {
            _fakeUserRepository = FakeUserRepository.GetFakeUserRepository(initialUsers);
            _fakeUserManager = FakeUserManager.GetFakeUserManager();
            _fakeSigInManager = FakeSignInManager.GetFakeSignInManager();

            var mockedDbContext = new Mock<MMSContext>();
            mockedDbContext.Setup(x => x.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);
            _fakeDbContext = mockedDbContext.Object;

            _controller = new UserController(_fakeDbContext, _fakeUserManager, _fakeSigInManager, null, _fakeUserRepository);
            return _controller;
        }

        public async static Task<UserController> GetControllerWithUserLoggedId(User user = null)
        {
            if (user == null) user = FakeUserRepository.UserList[0];

            var authRequest = new AuthRequest
            {
                username = user.UserName,
                password = user.PasswordHash
            };
            var authResponse = (await _controller.Login(authRequest) as ObjectResult).Value as AuthResponse;

            var newController = new UserController(_fakeDbContext, _fakeUserManager, _fakeSigInManager, null, _fakeUserRepository);

            var token = new JwtSecurityTokenHandler().ReadJwtToken(authResponse.jwt);
            var identity = new ClaimsPrincipal(new ClaimsIdentity(token.Claims));

            newController.ControllerContext = new ControllerContext();
            newController.ControllerContext.HttpContext = new DefaultHttpContext { User = identity };

            return await Task.FromResult(newController);
        }

        public static List<User> GenerateUsersManually(int count)
        {
            var users = new List<User>();
            for (int i = 0; i < count; i++)
            {
                users.Add(new User
                {
                    Id = i.ToString(),
                    FirstName = $"FirstName {i}",
                    LastName = $"LastName {i}",
                    UserName = $"username{i}",
                    Email = $"Email_{i}@email.com",
                    PasswordHash = $"password_{i}",
                    Tasks = new List<AppTask>(),
                    Comments = new List<Comment>(),
                    Roles = "USER,ADMIN"
                });;
            }
            return users;
        }

        public static User GetUserObject(int id = 0)
        {
            if (id > FakeUserRepository.UserList.Count) id = 0;
            return FakeUserRepository.UserList[id];
        }
    }
}
