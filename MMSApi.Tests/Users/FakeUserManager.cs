using DataLibrary.Models;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq;

namespace MMSApi.Tests.Users
{
    public class FakeUserManager : UserManager<User>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<User>>().Object,
                  new IUserValidator<User>[0],
                  new IPasswordValidator<User>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<User>>>().Object)
        { }


        public static FakeUserManager GetFakeUserManager()
        {
            var fakeUserManager = new Mock<FakeUserManager>();

            fakeUserManager.Setup(x => x.Users)
                .Returns(FakeUserRepository.UserList.AsQueryable());

            fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<User>()))
             .ReturnsAsync((User user) =>
             {
                 var u = FakeUserRepository.UserList.Find(usr => usr.Id == user.Id);
                 if (u == null) return IdentityResult.Failed();
                 FakeUserRepository.UserList.Remove(u);
                 return IdentityResult.Success;
             });


            fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback((User user, string pass) => FakeUserRepository.UserList.Add(user));


            fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
           .ReturnsAsync((User user) =>
           {
               var u = FakeUserRepository.UserList.FindIndex(usr => usr.Id == user.Id);
               if (u == -1) return IdentityResult.Failed();
               FakeUserRepository.UserList[u] = user;
               return IdentityResult.Success;
           });

            fakeUserManager.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
           .ReturnsAsync((User user, string token, string newPassword) =>
           {
               var u = FakeUserRepository.UserList.FindIndex(usr => usr.Id == user.Id);
               if (u == -1) return IdentityResult.Failed();
               FakeUserRepository.UserList[u].PasswordHash = newPassword;
               return IdentityResult.Success;
           });

            fakeUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string userName) => 
                            FakeUserRepository.UserList.FirstOrDefault(u => u.UserName.ToLower().Equals(userName.ToLower()))
            );

            fakeUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) =>
                            FakeUserRepository.UserList.FirstOrDefault(u => u.Id.ToLower().Equals(id.ToLower()))
            );

            return fakeUserManager.Object;
        }
    }
}
